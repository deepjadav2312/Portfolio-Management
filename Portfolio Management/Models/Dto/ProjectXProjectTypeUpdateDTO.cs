using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PortfolioManagement_API.Models.Dto
{
    public class ProjectXProjectTypeUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        public int ProjectDetailsId { get; set; }

        public int ProjectTypeId { get; set; }
  



    }
}
