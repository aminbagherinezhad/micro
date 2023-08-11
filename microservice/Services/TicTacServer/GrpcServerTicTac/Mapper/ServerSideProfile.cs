using AutoMapper;
using GrpcServerTicTac.Protos;
using GrpcServerTicTac.Models;

namespace GrpcServerTicTac.Mapper
{
    public class ServerSideProfile : Profile
    {
        public ServerSideProfile()
        {
            CreateMap<LogMessageReciver, LoggerModel>().ReverseMap();
        }
    }
}
