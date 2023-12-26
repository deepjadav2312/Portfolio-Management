using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using PortfolioManagement_API.Repository.IRepository;
using System.Data;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace PortfolioManagement_API.Controllers.V1
{
    //var claimsIdentity = (ClaimsIdentity)User.Identity;
    //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
    //string ApplicationUserId = userId;

    [Route("api/v{version:apiVersion}/[Controller]/[Action]")]
    [ApiController]
    //[ApiVersion("1.0")]

    public class ApplicationRoleAPIController : ControllerBase
    {
        protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;
 		private readonly IMapper _mapper;
        

        public ApplicationRoleAPIController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _response = new();
        }

        [HttpGet]
        //[MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetApplicationRoles()
        {
            try
            {
                IEnumerable<ApplicationRole> applicationRoleList = await _unitOfWork.ApplicationRole.GetAllAsync(); /*_dbApplicationUser.GetAllAsync();*/
                _response.Result = _mapper.Map<List<ApplicationRoleDTO>>(applicationRoleList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpGet("{Id}", Name = "GetApplicationRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetApplicationRole(string Id)
        {
            try
            {
                //if (Id == 0)
                //{
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    return BadRequest(_response);
                //}
                var applicationRole = await _unitOfWork.ApplicationRole.GetAsync(u => u.Id == Id);
                if (applicationRole == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ApplicationRoleDTO>(applicationRole);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateApplicationRole([FromForm] ApplicationRoleDTO createDTO)
        {

            try
            {

                if (await _unitOfWork.ApplicationRole.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Role already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                //ApplicationRole applicationRole = _mapper.Map<ApplicationRole>(createDTO);
                ApplicationRole applicationRole = new();
                applicationRole.Id = createDTO.Id;
                applicationRole.Name = createDTO.Name;
                applicationRole.NormalizedName = createDTO.Name.ToUpper();

                await _unitOfWork.ApplicationRole.CreateAsync(applicationRole);
                    _response.Result = _mapper.Map<ApplicationRoleDTO>(applicationRole);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetApplicationRole", new { id = applicationRole.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}", Name = "DeleteApplicationRole")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteApplicationRole(string id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var category = await _unitOfWork.ApplicationRole.GetAsync(u => u.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ApplicationRole.RemoveAsync(category);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id}", Name = "UpdateApplicationRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateApplicationRole(string id, [FromForm] ApplicationRoleDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                ApplicationRole model = _mapper.Map<ApplicationRole>(updateDTO);
                await _unitOfWork.ApplicationRole.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                //return Ok(_response);
                return CreatedAtRoute("GetApplicationRole", new { id = updateDTO.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}