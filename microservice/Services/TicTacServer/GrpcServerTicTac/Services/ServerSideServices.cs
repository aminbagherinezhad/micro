using AutoMapper;
using Grpc.Core;
using GrpcServerTicTac.Models;
using GrpcServerTicTac.Protos;

namespace GrpcServerTicTac.Services
{
    public class ServerSideServices : ServerSideProtoService.ServerSideProtoServiceBase
    {
        #region constructor
        private LoggerDbContext _loggerDb;
        private readonly IMapper _mapper;
        public ServerSideServices(IMapper mapper, LoggerDbContext loggerDb)
        {
            _mapper = mapper;
            _loggerDb = loggerDb;
        }
        #endregion

        #region get serverside
        //public override async Task<LoggerModel> GetLog(GetLogRequest request, ServerCallContext context)
        //{
        //	var logger = request.Id;
        //	if (_logger.IsEnabled(LogLevel.Debug))
        //		throw new RpcException(new Status(StatusCode.NotFound, $"ServerSide with id {request.Id} is Not Found"));
        //	_logger.LogInformation("ServerSide is Retrived for Id");
        //	return logger;
        //}


        public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
        {
            // Here, you should implement the logic to retrieve a list of LoggerModel instances.
            // This could involve fetching data from a database or any other data source.

            List<LoggerModel> allLogs = new List<LoggerModel>
        {
            // Sample LoggerModel instances, replace with your actual data retrieval logic.
            new LoggerModel { Id = 1, MessageLogs = "Log 1" },
            new LoggerModel { Id = 2, MessageLogs = "Log 2" }
        };

            var response = new GetAllResponse();
            response.Loggers.AddRange(allLogs);
            return response;
        }

        #endregion
    }
}
