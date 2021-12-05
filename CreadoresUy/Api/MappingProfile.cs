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

            CreateMap<DefaultPlan, DefaultPlanBODto>();
            CreateMap<DefaultPlanBODto, DefaultPlan>();

            CreateMap<DefaultBenefit, BenefitBODto>();
            CreateMap<BenefitBODto, DefaultBenefit>();

            CreateMap<Benefit, BenefitBODto>();
            CreateMap<BenefitBODto, Benefit>();

            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();

            CreateMap<Tag, AdminBODto>();
            CreateMap<AdminBODto, Tag>();

            CreateMap<Category, CategoryBODto>();
            CreateMap<CategoryBODto, Category>();
            
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<User, AdminDto>();
            CreateMap<AdminDto, User>();

            CreateMap<User, AdminBODto>();
            CreateMap<AdminBODto, User>();

            CreateMap<User, LoginDto>();
            CreateMap<LoginDto, User>();

            CreateMap<Creator, CreatorRawDto>();
            CreateMap<CreatorRawDto, Creator>();

            CreateMap<Creator, CreatorBODto>();
            CreateMap<CreatorBODto, Creator>();

            CreateMap<Creator, CreadorSearchDto>();
            CreateMap<CreadorSearchDto, Creator>();


            CreateMap<Creator, CreatorDto>();
            CreateMap<CreatorDto, Creator>();

            CreateMap<Creator, CreatorProfileDto>();
            CreateMap<CreatorProfileDto, Creator>();

            CreateMap<Creator, CreadorDatabaseDto>();
            CreateMap<CreadorDatabaseDto, Creator>();

            CreateMap<User, AdminDto>();
            CreateMap<AdminDto, User>();

            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Benefit, BenefitDTO>();
            CreateMap<BenefitDTO, Benefit>();

            CreateMap<User, UserBODto>();
            CreateMap<UserBODto, User>();

            CreateMap<Plan, BasePlanDto>();
            CreateMap<BasePlanDto, Plan>();

            CreateMap<UpdatePlanAndBenefitsDto, Plan>();
            CreateMap<Plan, UpdatePlanAndBenefitsDto>();

            CreateMap<Plan, BasicPlanDto>();
            CreateMap<BasicPlanDto, Plan>();
        }
    }
}
