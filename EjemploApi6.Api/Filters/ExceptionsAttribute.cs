using Correlate;
using EjemploApi6.Api.Models;
using EjemploApi6.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Error = EjemploApi6.Api.Models.Error;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;

namespace EjemploApi6.Api.Filters
{
    /// <summary>
    /// ExceptionsAttribute
    /// </summary>
    public class ExceptionsAttribute : Attribute, IExceptionFilter
    {
        private readonly ICorrelationContextAccessor _correlation;

        /// <summary>
        /// ExceptionsAttribute
        /// </summary>
        /// <param name="correlation"></param>
        public ExceptionsAttribute(ICorrelationContextAccessor correlation)
        {
            _correlation = correlation;
        }

        /// <summary>
        /// OnException
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnException(ExceptionContext context)
        {
            SetExceptionType(context);
        }

        private void SetExceptionType(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            switch (exceptionType.Name)
            {
                case nameof(BusinessException):

                    var responseModel = new ErrorDetailModel();
                    var exception = (BusinessException)context.Exception;

                    responseModel.EventId = exception.EventId.Id.ToString();
                    responseModel.Detail = exception.EventId.Name;

                    foreach (var item in exception.Errors)
                    {
                        responseModel.Errors.Add(new Error
                        {
                            Title = item.Title,
                            Code = item.Code,
                            Detail = item.Detail
                        });
                    }

                    responseModel.CorrelationId = _correlation.CorrelationContext?.CorrelationId;
                    context.Result = new ObjectResult(responseModel);
                    context.HttpContext.Response.StatusCode = (int)exception.HttpStatusCode;
                    break;
                default:
                    var exceptionModelResponse = new ErrorDetailModel
                    {
                        EventId = "500",
                        Detail = "Internal Server Error",
                        Errors = new List<Error>{new Error
                            {
                                Title = "Internal Server Error",
                                Code = "500",
                                Detail = context.Exception.Message
                            }
                        },
                        CorrelationId = _correlation.CorrelationContext?.CorrelationId
                    };

                    context.Result = new ObjectResult(exceptionModelResponse);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }

}
