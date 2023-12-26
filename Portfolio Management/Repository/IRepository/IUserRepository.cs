

using PortfolioManagement_API.Models.Dto;

namespace PortfolioManagement_API.Repository.IRepository
{
	public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
