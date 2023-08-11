using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewerLog.GrpcServices;

namespace ViewerLog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ServerSideGrpcService _discountService;
        private readonly ServerSideProtoService.ServerSideProtoServiceClient _serverSideClient;
        public LogController(ServerSideGrpcService discountService, ServerSideProtoService.ServerSideProtoServiceClient serverSideClient)
        {
            _serverSideClient = serverSideClient;
            _discountService = discountService;
        }
        [HttpGet("all-logs")]
        public async Task<IActionResult> GetAllLogs()
        {

            //var grpcChannel = GrpcChannel.ForAddress("http://localhost:5022");
            //var serverSideClient = new ServerSideProtoService.ServerSideProtoServiceClient(grpcChannel);

            //var getAllRequest = new GetAllRequest();
            //await _discountService.GetAllLogs();

            try
            {
                var channel = GrpcChannel.ForAddress("http://localhost:5001"); // Replace with your server's address
                var client = new ServerSideProtoService.ServerSideProtoServiceClient(channel);

                var getAllRequest = new GetAllRequest();
                var getAllResponse = await client.GetAllAsync(getAllRequest);

                foreach (var loggerModel in getAllResponse.Loggers)
                {
                    Console.WriteLine($"ID: {loggerModel.Id}, Message: {loggerModel.MessageLogs}");
                }
            
                // Return the loggerModels as JSON or in any other appropriate format
                return Ok();
            }
            catch (RpcException ex)
            {
                // Handle gRPC-related exceptions here
                return StatusCode((int)ex.StatusCode, ex.Status.Detail);
            }


        }
    }
}