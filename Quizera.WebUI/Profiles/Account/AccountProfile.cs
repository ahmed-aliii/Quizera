using AutoMapper;
using Quizera.Domain;

namespace Quizera.WebUI
{
    public class AccountProfile : Profile
    {
        public AccountProfile() 
        { 
            //VM => Entity || Entity => VM 
            CreateMap<ApplicationUser , SignupVM>().ReverseMap();

        }
    }
}
