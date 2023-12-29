

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PortfolioManagement_API.Models.VM
{
	public class ProjectXTechnologyVM
    {
		//public CompanyXPaymentVM()
		//{
		//	CompanyXPayment = new CompanyXPaymentCreateDTO();
		//	Company = new CompanyCreateDTO();
		//}
		//public CompanyXPaymentCreateDTO CompanyXPayment { get; set; }

		//public CompanyCreateDTO Company { get; set; }

		//[ValidateNever]
		//public List<CompanyXPaymentCreateDTO> CompanyXPaymentlist { get; set; }

		//      [ValidateNever]
		//public IEnumerable<SelectListItem> CompanyList { get; set; }
		//public int CompanyId { get; set; }
		//[ValidateNever]
		//public List<PaymentDTO> Paymentlist { get; set; }
		    public int ProjectDetailsId { get; set; }
           public List<int> TechnologyIds { get; set; }
	}
}
