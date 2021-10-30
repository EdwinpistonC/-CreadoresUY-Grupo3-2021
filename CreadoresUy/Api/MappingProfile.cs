using AutoMapper;
using Share.Dtos;
using Share.Entities;

namespace Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Content, ContentDto>();
            CreateMap<ContentDto, Content>();

            CreateMap<Plan, PlanDto>();
            CreateMap<PlanDto, Plan>();

            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<User, LoginDto>();
            CreateMap<LoginDto, User>();

            CreateMap<Creator, CreatorRawDto>();
            CreateMap<CreatorRawDto, Creator>();

            CreateMap<Creator, CreatorDto>();
            CreateMap<CreatorDto, Creator>();

            CreateMap<Creator, CreadorDatabaseDto>();
            CreateMap<CreadorDatabaseDto, Creator>();

            CreateMap<User, AdminDto>();
            CreateMap<AdminDto, User>();

            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Plan, BasePlanDto>();
            CreateMap<BasePlanDto, Plan>();
        }
    }
}
