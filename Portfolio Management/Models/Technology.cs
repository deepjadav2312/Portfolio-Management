using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PortfolioManagement_API.Models
{
    public class Technology
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Technology Name")]
        public string TechnologyName { get; set; }
        public string Version { get; set; }

        public bool IsActive { get; set; }


    }
}
