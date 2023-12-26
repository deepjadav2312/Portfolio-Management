using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PortfolioManagement_API.Models.Dto
{
    public class ProjectXProjectTypeDTO
    {
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
