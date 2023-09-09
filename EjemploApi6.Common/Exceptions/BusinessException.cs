using Microsoft.Extensions.Logging;
using System.Net;

namespace EjemploApi6.Common.Exceptions
{
    public sealed class BusinessException : Exception
    {
        public EventId EventId { get; }
        public List<Error> Errors { get; }
        public HttpStatusCode HttpStatusCode { get; set; }

        public BusinessException(EventId eventId
            , List<Error> errors
            , HttpStatusCode httpStatusCode = HttpStatusCode.UnprocessableEntity)
        {
            EventId = eventId;
            Errors = new List<Error>();
            Errors.AddRange(errors);
            Errors = errors;
            HttpStatusCode = httpStatusCode;
        }

        public BusinessException(EventId eventId
            , Error error
            , HttpStatusCode httpStatusCode = HttpStatusCode.UnprocessableEntity)
        {
            EventId = eventId;
            Errors = new List<Error> { error };
            HttpStatusCode = httpStatusCode;
        }
    }
}
