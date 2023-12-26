namespace PortfolioManagement_API.Models.Dto
{
	public class RegisterationRequestDTO
    {
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Address { get; set; }
		public string Email { get; set; }
	}
}
