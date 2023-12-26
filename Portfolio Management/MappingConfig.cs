using AutoMapper.Features;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Metrics;
using System.Drawing;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using Portfolio_Management.Models;

namespace PortfolioManagement_API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //    CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            //    CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            //    CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            //    CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserDTO>().ReverseMap();
            CreateMap<ApplicationUser, IdentityUser>().ReverseMap();

            CreateMap<ApplicationRole, ApplicationRoleDTO>().ReverseMap();
            CreateMap<ApplicationRole, IdentityRole>().ReverseMap();

            CreateMap<ApplicationUserRole, ApplicationUserRoleDTO>().ReverseMap();
            CreateMap<ApplicationUserRole, IdentityUserRole<string>>().ReverseMap();


            
            CreateMap<ProjectType, ProjectTypeDTO>().ReverseMap();
            CreateMap<ProjectType, ProjectTypeCreateDTO>().ReverseMap();
            CreateMap<ProjectType, ProjectTypeUpdateDTO>().ReverseMap();


            CreateMap<ProjectDetails, ProjectDetailsDTO>().ReverseMap();
            CreateMap<ProjectDetails, ProjectDetailsCreateDTO>().ReverseMap();
            CreateMap<ProjectDetails, ProjectDetailsUpdateDTO>().ReverseMap();

            CreateMap<ProjectXTechnology, ProjectXTechnologyDTO>().ReverseMap();
            CreateMap<ProjectXTechnology, ProjectXTechnologyCreateDTO>().ReverseMap();
            CreateMap<ProjectXTechnology, ProjectXTechnologyUpdateDTO>().ReverseMap();

            CreateMap<ProjectXProjectType, ProjectXProjectTypeDTO>().ReverseMap();
            CreateMap<ProjectXProjectType, ProjectXProjectTypeCreateDTO>().ReverseMap();
            CreateMap<ProjectXProjectType, ProjectXProjectTypeUpdateDTO>().ReverseMap();


            CreateMap<Technology, TechnologyDTO>().ReverseMap();
            CreateMap<Technology, TechnologyCreateDTO>().ReverseMap();
            CreateMap<Technology, TechnologyUpdateDTO>().ReverseMap();




        }
    }
}
