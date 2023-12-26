using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PortfolioManagement_API.Models.Dto
{
    public class ProjectTypeCreateDTO
    {

        public int Id { get; set; }

        [Required]
        [DisplayName("Project Type")]
        public string ProjectTypes { get; set; }

        public bool IsActive { get; set; }


    }
}
