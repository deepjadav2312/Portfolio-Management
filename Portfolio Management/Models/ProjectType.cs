using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PortfolioManagement_API.Models
{
    public class ProjectType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Project Type")]
        public string ProjectTypes { get; set; }

        public bool IsActive { get; set; }


    }
}
