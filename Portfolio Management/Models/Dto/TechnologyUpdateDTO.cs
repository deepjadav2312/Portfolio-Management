using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PortfolioManagement_API.Models.Dto
{
    public class TechnologyUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Technology Name")]
        public string TechnologyName { get; set; }
        public string Version { get; set; }

        public bool IsActive { get; set; }


    }
}
