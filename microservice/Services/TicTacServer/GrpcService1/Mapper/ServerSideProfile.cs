using AutoMapper;
using GrpcService1.Protos;
using GrpcService1.Models;

namespace GrpcService1.Mapper
{
    public class ServerSideProfile : Profile
    {
        public ServerSideProfile()
        {
            CreateMap<LogMessageReciver, LoggerModel>().ReverseMap();
        }
    }
}
