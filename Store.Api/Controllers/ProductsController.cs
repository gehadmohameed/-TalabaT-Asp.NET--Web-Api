using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Api.DTOs;
using Store.Api.Helpers;
using Store.Api.Response;
using Store.Core;
using Store.Core.Entites;
using Store.Core.Repositories;
using Store.Core.Specifications;

namespace Store.Api.Controllers
{
    public class ProductsController : APIBaseController
    {
        
        private readonly IMapper _mapper;
        private readonly IUniteOfWork _uniteOfWork;

        public ProductsController( IMapper mapper, 
            IUniteOfWork uniteOfWork)
        {
           
            _mapper = mapper;
           _uniteOfWork = uniteOfWork;
        }
        //Get All products 
        // [Authorize]
        [ChachedAttribute(300)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(Params);
            var Products = await _uniteOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
           var MappedProducts = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFiltrationForCountAsync(Params);
            var Count = await _uniteOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize , MappedProducts , Count));
            #region another ways


            //OkObjectResult Result = new OkObjectResult(Products);
            //return Result;
            //****//
            //var ReturnedObject = new Pagination<ProductToReturnDto>()
            //{
            //    PageIndex = Params.PageIndex,
            //PageSize = Params.PageSize,
            //Data = MappedProducts
            //};
            #endregion

        }
        //Get Product ById
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound )]
        public async Task <ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecification(id);
            var Product = await _uniteOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<Product, ProductToReturnDto>(Product);
            if (Product is null) return NotFound(new ApiResponse(404));
            return Ok(MappedProducts);
         }

        // get all types 
        [HttpGet("Types")]
        public async Task <ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types =  await _uniteOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }




      // get all brands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _uniteOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }



    }
}
