
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using PortfolioManagement_API.Repository.IRepository;
using System.Net;


namespace PortfolioManagement_API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[Controller]/[Action]")]
    [ApiController]
    [ApiVersionNeutral]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        protected APIResponse _response;
        public UsersController(IUserRepository userRepo, IUnitOfWork unitOfWork)
        {
            _userRepo = userRepo;
            _response = new();
            _unitOfWork = unitOfWork;
        }

        [HttpPost(Name = "login")]
        public async Task<IActionResult> Login([FromForm] LoginRequestDTO model)
        {
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.ApplicationUser == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }


        [HttpPost(Name = "register")]
        public async Task<IActionResult> Register([FromForm] RegisterationRequestDTO registerationRequestDTO)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(registerationRequestDTO.Email);
            if (!ifUserNameUnique)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            //ApplicationUser user = new()
            //{
            //    UserName = registerationRequestDTO.Email,
            //    Email = registerationRequestDTO.Email,
            //    NormalizedEmail = registerationRequestDTO.Email.ToUpper(),
            //    FirstName = registerationRequestDTO.FirstName,
            //    LastName = registerationRequestDTO.LastName,
            //    Address = registerationRequestDTO.Address,
            //    PassWord = registerationRequestDTO.Password
            //};

            // await _unitOfWork.ApplicationUser.CreateAsync(user);

            var user = await _userRepo.Register(registerationRequestDTO);
            if (user == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Error while registering");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }


    }
}
