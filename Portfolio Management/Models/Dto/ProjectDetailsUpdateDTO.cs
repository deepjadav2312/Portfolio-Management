using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PortfolioManagement_API.Models.Dto
{
    public class ProjectDetailsUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }
        public string ClientName { get; set; }

        public string ClientEmail { get; set; }

        public string Budgent { get; set; }

        public string Duration { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }



    }
}
