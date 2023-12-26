using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PortfolioManagement_API.Models.Dto
{
    public class ApplicationUserDTO : IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string PassWord { get; set; }
        public string Role { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
	}
}
