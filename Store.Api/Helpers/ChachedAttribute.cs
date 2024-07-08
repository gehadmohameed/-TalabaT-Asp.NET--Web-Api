using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Core.Services;
using System.Text;

namespace Store.Api.Helpers
{
    public class ChachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSeconds;

        public ChachedAttribute(int EXpireTimeInSeconds )
        {
            _expireTimeInSeconds = EXpireTimeInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService = context.HttpContext.RequestServices.GetRequiredService<IresponseCacheService>();
            var CacheKey = GenerteCacheKeyFromRequest(context.HttpContext.Request);
            var CachedResponse =   await   CacheService.GetCachedResponse(CacheKey);
            if (!string.IsNullOrEmpty(CachedResponse))
            {
                var ContentResult = new ContentResult
                {
                    Content = CachedResponse,
                    ContentType = "Application/Json",
                    StatusCode = 200,
                };
                context.Result = ContentResult;
            }
      var ExecutedEndPointContext =    await  next.Invoke(); //Will Execute EndPoint
            if( ExecutedEndPointContext.Result is OkObjectResult result) 
            {
          await CacheService.CasheResponseAsync(CacheKey, result.Value, TimeSpan.FromSeconds(_expireTimeInSeconds));
            
            }
        }

        private string GenerteCacheKeyFromRequest(HttpRequest request)
        {

            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach (var (Key, Value) in request.Query.OrderBy(X=>X.Key))

            {
                KeyBuilder.Append($"|{Key}-{Value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
