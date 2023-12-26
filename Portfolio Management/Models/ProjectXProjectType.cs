using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace PortfolioManagement_API.Models
{
    public class ProjectXProjectType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ProjectDetails")]
        public int ProjectDetailsId { get; set; }
        [ValidateNever]
        public ProjectDetails ProjectDetails { get; set; }

        [ForeignKey("ProjectType")]
        public int ProjectTypeId { get; set; }
        [ValidateNever]
        public ProjectType ProjectType { get; set; }
    }
}
