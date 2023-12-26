using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using PortfolioManagement_API.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace PortfolioManagement_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper,
            RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

             bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            //bool isValid;
            //if (user.PassWord == loginRequestDTO.Password)
            //{
            //    isValid = true;
            //}
            //else
            //{
            //    isValid = false;
            //}

            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    ApplicationUser = null
                };
            }

            //if user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //token ma value set kari
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                  new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                ApplicationUser = _mapper.Map<ApplicationUserDTO>(user),
            };
            return loginResponseDTO;
        }

        public async Task<ApplicationUserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerationRequestDTO.Email,
                Email = registerationRequestDTO.Email,
                NormalizedEmail = registerationRequestDTO.Email.ToUpper(),
                FirstName = registerationRequestDTO.FirstName,
                LastName = registerationRequestDTO.LastName,
                Address = registerationRequestDTO.Address,
                PhoneNumber = registerationRequestDTO.PhoneNumber,
                PassWord = registerationRequestDTO.Password
            };

			try
			{
				var role = await _unitOfWork.ApplicationRole.GetAsync(u => u.Name == "Customer");
				var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
				if (result.Succeeded)
				{
					// below code are used when user interface doesnot create.
					//if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
					//{
					//     await _roleManager.CreateAsync(new IdentityRole("admin"));
					//    await _roleManager.CreateAsync(new IdentityRole("customer"));
					//}
					ApplicationUserRole applicationUserRole = new();
					applicationUserRole.UserId = user.Id;
					applicationUserRole.RoleId = role.Id;
					await _unitOfWork.ApplicationUserRole.CreateAsync(applicationUserRole);

					// await _userManager.AddToRoleAsync(user, "Customer");
					var userToReturn = _db.ApplicationUsers
					.FirstOrDefault(u => u.UserName == registerationRequestDTO.Email);
					return _mapper.Map<ApplicationUserDTO>(userToReturn);
				}
			}
			catch (Exception e)
            {

            }

            return new ApplicationUserDTO();
        }
    }
}


