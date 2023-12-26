using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using PortfolioManagement_API.Models;

namespace Portfolio_Management.Models
{
    public class ProjectXTechnology
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ProjectDetails")]
        public int ProjectDetailsId { get; set; }
        [ValidateNever]
        public ProjectDetails ProjectDetails { get; set; }

        [ForeignKey("Technology")]
        public int TechnologyId { get; set; }
        [ValidateNever]
        public Technology Technology { get; set; }
    }
}
