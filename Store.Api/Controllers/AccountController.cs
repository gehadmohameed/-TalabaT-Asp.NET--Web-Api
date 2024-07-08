using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Api.DTOs;
using Store.Api.Extensions;
using Store.Api.Response;
using Store.Core.Entites.identity;
using Store.Core.Services;
using System.ComponentModel;
using System.Security.Claims;

namespace Store.Api.Controllers
{
   
    public class AccountController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManger;
        private readonly SignInManager<AppUser> _signInManger;
        private readonly ItokenServices _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger ,
            ItokenServices tokenService , IMapper mapper)
        {
            _userManger = userManager;
            _signInManger = signInManger;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task <ActionResult<UserDto>> Register(RegisterDto model )
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "This Email is Already is use"));
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber

            };
         var Result =  await  _userManger.CreateAsync(User, model.Password);
            if(!Result.Succeeded) return BadRequest(new ApiResponse(400));
            var reuturendUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token =  await _tokenService.CreateTakenAsync(User, _userManger)

            };
            return Ok(reuturendUser);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model )
        {
            var User = await _userManger.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
          var Result =  await  _signInManger.CheckPasswordSignInAsync(User, model.Password, false);
            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDto()
            {

                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTakenAsync(User, _userManger)
            });
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task <ActionResult <UserDto>> GetCurrentUser()
        {
            var Email =  User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManger.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTakenAsync(user , _userManger)
            };
            return Ok(ReturnedUser);
        }

        [Authorize]
        [HttpGet("CurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddres()
        {
            var user = await _userManger.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task <ActionResult<AddressDto>> UpdateAddress(AddressDto UpdateAddress)
        {
            
            var user = await _userManger.FindUserWithAddressAsync(User);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var address = _mapper.Map<AddressDto, Address>(UpdateAddress);
            address.Id = user.Address.Id;
            user.Address = address;
            var Result = await  _userManger.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(UpdateAddress);
        }
        [HttpGet("emailExists")]
        public async Task <ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManger.FindByEmailAsync(email) is not null;
        }
    }   
}
