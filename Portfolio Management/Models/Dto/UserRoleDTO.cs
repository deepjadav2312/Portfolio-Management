using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


    namespace PortfolioManagement_API.Models.Dto
    {
        public class UserRoleDTO
        {
            public string userId { get; set; }
            public List<string> SelectedRoleIds { get; set; }
        }
    }


