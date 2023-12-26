using AutoMapper;using Azure;using Microsoft.AspNetCore.Authorization;using Microsoft.AspNetCore.Http;using Microsoft.AspNetCore.Identity;using Microsoft.AspNetCore.JsonPatch;using Microsoft.AspNetCore.Mvc;using Microsoft.EntityFrameworkCore;using System.Data;using System.Net;using System.Security.Claims;using System.Text.Json;using static System.Runtime.InteropServices.JavaScript.JSType;using Microsoft.AspNetCore.Mvc;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using PortfolioManagement_API.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Text.Json;using PortfolioManagement_API.Data;

namespace PortfolioManagement_API.Controllers.V1{    [Route("api/v{version:apiVersion}/[Controller]/[Action]")]    [ApiController]    [ApiVersion("1.0")]    public class ApplicationUserAPIController : ControllerBase    {        protected APIResponse _response;        private readonly IUnitOfWork _unitOfWork;        private readonly IMapper _mapper;        private readonly ApplicationDbContext _db;        public ApplicationUserAPIController(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db)        {            _unitOfWork = unitOfWork;            _mapper = mapper;            _response = new();            _db = db;        }        [HttpGet(Name = "GetApplicationUsers")]        [MapToApiVersion("1.0")]        [ProducesResponseType(StatusCodes.Status200OK)]        public async Task<ActionResult<APIResponse>> GetApplicationUsers()        {            try            {                IEnumerable<ApplicationUser> applicationUserList = await _unitOfWork.ApplicationUser.GetAllAsync();                _response.Result = _mapper.Map<List<ApplicationUserDTO>>(applicationUserList);                _response.StatusCode = HttpStatusCode.OK;                return Ok(_response);            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages                     = new List<string>() { ex.ToString() };            }            return _response;        }


        [HttpGet("{Id}", Name = "GetApplicationUser")]        [ProducesResponseType(StatusCodes.Status200OK)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        [ProducesResponseType(StatusCodes.Status404NotFound)]        public async Task<ActionResult<APIResponse>> GetApplicationUser(string Id)        {            try            {                var applicationUser = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == Id);                if (applicationUser == null)                {                    _response.StatusCode = HttpStatusCode.NotFound;                    return NotFound(_response);                }                _response.Result = _mapper.Map<ApplicationUserDTO>(applicationUser);                _response.StatusCode = HttpStatusCode.OK;                return Ok(_response);            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages                     = new List<string>() { ex.ToString() };            }            return _response;        }



        //[HttpGet("{companyId:int}", Name = "GetUserByRole")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> GetUserByRole(int roleId)
        //{
        //    try
        //    {
        //        if (roleId == 0)
        //        {
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }
        //        var states = _db.ApplicationUsers.Include(u => u.Role).Include(u => u.UserName).Where(u => u.Id == roleId).ToList();

        //        if (states == null || states.Count() == 0)
        //        {
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }

        //        response.Result = mapper.Map<List<ApplicationUserDTO>>(states);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
        //    }

        //    return _response;
        //}

        [HttpPost(Name = "CreateApplicationUser")]        [ProducesResponseType(StatusCodes.Status201Created)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        public async Task<ActionResult<APIResponse>> CreateApplicationUser([FromForm] ApplicationUserDTO createDTO)        {            try            {                if (await _unitOfWork.ApplicationUser.GetAsync(u => u.UserName.ToLower() == createDTO.UserName.ToLower()) != null)                {                    ModelState.AddModelError("ErrorMessages", "User already Exists!");                    return BadRequest(ModelState);                }                if (createDTO == null)                {                    return BadRequest(createDTO);                }                ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(createDTO);                await _unitOfWork.ApplicationUser.CreateAsync(applicationUser);                _response.Result = _mapper.Map<ApplicationUserDTO>(applicationUser);                _response.StatusCode = HttpStatusCode.Created;                return CreatedAtRoute("GetApplicationUser", new { id = applicationUser.Id }, _response);            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages                     = new List<string>() { ex.ToString() };            }            return _response;        }        [HttpPut(Name = "UpdateApplicationUser")]        [ProducesResponseType(StatusCodes.Status204NoContent)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        [ProducesResponseType(StatusCodes.Status201Created)]        [ProducesResponseType(StatusCodes.Status400BadRequest)]        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        public async Task<ActionResult<APIResponse>> UpdateApplicationUser([FromForm] UserRoleDTO updateDTO)        {            try            {                await _unitOfWork.ApplicationUserRole.RemoveRangeAsync(x => x.UserId == updateDTO.userId, false);                foreach (var roleList in updateDTO.SelectedRoleIds)                {                    ApplicationUserRole applicationUserRole = new();                    applicationUserRole.UserId = updateDTO.userId;                    applicationUserRole.RoleId = roleList;                    await _unitOfWork.ApplicationUserRole.CreateAsync(applicationUserRole);                }                _response.StatusCode = HttpStatusCode.Created;                return _response;            }            catch (Exception ex)            {                _response.IsSuccess = false;                _response.ErrorMessages = new List<string>() { ex.ToString() };            }            return _response;        }


        //[HttpPut(Name = "UpdateApplicationUser")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<APIResponse>> UpdateApplicationUser([FromForm] UserVM updateDTO)

        //{
        //    try
        //    {
        //        List<ApplicationRoleDTO> RoleList = _mapper.Map<List<ApplicationRoleDTO>>(updateDTO.ApplicationRoleList);
        //        // CarXColor carxcolor = _mapper.Map<CarXColor>(createDTO.CarXColor);
        //        ApplicationUserDTO User = _mapper.Map<ApplicationUserDTO>(updateDTO.ApplicationUser);

        //        await _unitOfWork.ApplicationUserRole.RemoveRangeAsync(x => x.UserId == User.Id, false);

        //        foreach (var roleList in RoleList)
        //        {
        //            if (roleList.IsChecked == true)
        //            {
        //                ApplicationUserRole applicationUserRole = new();
        //                applicationUserRole.UserId = User.Id;
        //                applicationUserRole.RoleId = roleList.Id;
        //                await _unitOfWork.ApplicationUserRole.CreateAsync(applicationUserRole);
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //        _response.StatusCode = HttpStatusCode.Created;
        //        return _response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages
        //             = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}
    }}