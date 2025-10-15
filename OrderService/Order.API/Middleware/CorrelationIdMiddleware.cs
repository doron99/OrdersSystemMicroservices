using Serilog.Context;

namespace Order.API.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers["x-correlation-id"].ToString();

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Response.Headers.Add("x-correlation-id", correlationId);
            }

            var traceParent = context.Request.Headers["traceparent"].ToString();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("TraceParent", traceParent))
            {
                _logger.LogInformation("CorrelationId: "+correlationId);
                await _next(context);
            }
        }
    }
}
