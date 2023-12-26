

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortfolioManagement_API.Models.Dto;

namespace PortfolioManagement_API.Models.VM
{
    public class UserVM
    {
        public UserVM()
        {
            ApplicationUser = new ApplicationUserDTO();

            ApplicationUserRole = new ApplicationUserRoleDTO();
        }
        public ApplicationUserDTO ApplicationUser { get; set; }
        public ApplicationUserRoleDTO ApplicationUserRole { get; set; }


        [ValidateNever]
        public List<ApplicationUserRoleDTO> ApplicationUserRoleList { get; set; }

        [ValidateNever]
        public List<ApplicationRoleDTO> ApplicationRoleList { get; set; }
    }
}
