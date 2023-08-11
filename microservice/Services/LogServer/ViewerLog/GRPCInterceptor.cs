using Grpc.Core.Interceptors;
using Grpc.Core;

namespace ViewerLog
{
    public class GRPCInterceptor : Interceptor
    {
        private readonly ILogger logger;

        public GRPCInterceptor(ILogger logger)
        {
            this.logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            logger.LogDebug($"starting call");

            var response = await base.UnaryServerHandler(request, context, continuation);

            logger.LogDebug($"finished call");

            return response;
        }
    }
}
