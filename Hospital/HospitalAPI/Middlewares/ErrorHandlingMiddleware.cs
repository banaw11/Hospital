using HospitalAPI.Middlewares.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(BadRequestException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Bad request at {context.Request.Path} with error : \n {e.Message}");
            }
            catch(NotFoundException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync($"Not found content for request at {context.Request.Path} \n Details : {e.Message}");
            }
            catch(ForbidException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync($"Access denied, {e.Message}");
            }
            catch(Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Something went wrong. \n Check error message : \n {e.Message}");
            }
        }
    }
}
