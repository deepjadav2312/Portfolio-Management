using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PortfolioManagement_API.Models.Dto
{
    public class ApplicationUserRoleDTO : IdentityUserRole<string>
    {

    }
}
