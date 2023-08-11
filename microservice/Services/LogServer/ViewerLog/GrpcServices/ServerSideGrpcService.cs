using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1.Protos;
using static GrpcService1.Protos.ServerSideProtoService;
namespace ViewerLog.GrpcServices
{
    public class ServerSideGrpcService
    {

        #region constructor

        private readonly ServerSideProtoService.ServerSideProtoServiceClient _discountProtoService;

        public ServerSideGrpcService(ServerSideProtoService.ServerSideProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        #endregion

        #region get discount

        public async Task GetAllLogs()
        {
            try
            {
                using var channel = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new ServerSideProtoService.ServerSideProtoServiceClient(channel);
                var getAllRequest = new GetAllRequest();
               // await GetAllProducts(client);
                // var getAllResponse = await _discountProtoService.GetAll();
               // return;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        
        #endregion
    }
}
