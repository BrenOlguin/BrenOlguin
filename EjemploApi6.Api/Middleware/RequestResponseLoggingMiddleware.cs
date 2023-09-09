using Microsoft.IO;

namespace EjemploApi6.Api.Middleware
{
    /// <summary>
    /// RequestResponseLoggingMiddleware
    /// </summary>
    public class RequestResponseLoggingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        /// <summary>
        /// RequestResponseLoggingMiddleware
        /// </summary>
        /// <param name="loggerFactory"></param>
        public RequestResponseLoggingMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await LogRequest(context);
            await LogResponse(context, next);
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private async Task LogRequest(HttpContext context)
        {
            var request = context.Request;
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            _logger.LogDebug("REQUEST: {Schema} {@Host} {@Path} {@QueryString} {@Headers} {Body}", request.Scheme,
                request.Host, request.Path, request.QueryString, request.Headers, ReadStreamInChunks(requestStream));

            context.Request.Body.Position = 0;
        }

        private async Task LogResponse(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;
            var response = context.Response;

            var originalBodyStream = context.Response.Body;

            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogDebug("RESPONSE: {Schema} {@Host} {@Path} {@QueryString} {@Headers} {StatusCode} {Body}",
                request.Scheme, request.Host, request.Path, request.QueryString, request.Headers, response.StatusCode,
                body);

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
