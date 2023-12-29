using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio_Management.Models;
using PortfolioManagement_API.Data;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using PortfolioManagement_API.Repository.IRepository;
using System.Data;
using System.Drawing;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace HelpingHands_API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[Controller]/[Action]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class ProjectXProjectTypeAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;

        public ProjectXProjectTypeAPIController(IUnitOfWork unitOfWork, IMapper mapper,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            _db = db;
        }


        [HttpGet(Name= "GetProjectXProjectTypes")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjectXProjectTypes([FromQuery(Name = "filterDisplayOrder")] int? Id,
           [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<ProjectXProjectType> projectXProjectTypeList;

                if (Id > 0)
                {

                    projectXProjectTypeList = await _unitOfWork.ProjectXProjectType.GetAllAsync(u => u.Id == Id, includeProperties: "ProjectDetails", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    projectXProjectTypeList = await _unitOfWork.ProjectXProjectType.GetAllAsync(includeProperties: "ProjectDetails", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    projectXProjectTypeList = projectXProjectTypeList.Where(u => u.ProjectDetails.ProjectName.ToLower().Contains(search) ||
                                                 u.ProjectType.ProjectTypes.ToLower().Contains(search));

                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<ProjectXProjectTypeDTO>>(projectXProjectTypeList);
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



        [HttpGet("{id:int}", Name = "GetProjectXProjectType")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(CategoryDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetProjectXProjectType(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var projectXProject = await _unitOfWork.ProjectXProjectType.GetAsync(u => u.Id == id, includeProperties: "ProjectDetails");
                if (projectXProject == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProjectXProjectTypeDTO>(projectXProject);
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


        [HttpPost(Name = "CreateProjectXProjectType")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProjectXProjectType([FromForm] CxpDTO createDTO)
        {
            try
            {
                await _unitOfWork.ProjectXProjectType.RemoveRangeAsync(u => u.ProjectDetailsId == createDTO.ProjectDetailsId, false);

                foreach (var projectDetailsId in createDTO.SelectedProjectTypeIds)
                {
                    ProjectXProjectType projectXProjectType = new();
                    projectXProjectType.ProjectDetailsId = createDTO.ProjectDetailsId;
                    projectXProjectType.ProjectTypeId = Convert.ToInt32(projectDetailsId);
                    await _unitOfWork.ProjectXProjectType.CreateAsync(projectXProjectType);
                }
                _response.StatusCode = HttpStatusCode.Created;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("{projectDetailsId:int}", Name = "GetProjectXProjectTypeByProjectDetailsId")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProjectXProjectTypeByProjectDetailsId(int projectDetailsId)
        {
            try
            {
                if (projectDetailsId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var states = _db.ProjectXProjectTypes.Include(u => u.ProjectDetails).Include(u => u.ProjectType).Where(u => u.ProjectDetailsId == projectDetailsId).ToList();

                if (states == null || states.Count() == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<List<ProjectXProjectTypeDTO>>(states);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }




     



        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteProjectXProjectType")]

        public async Task<ActionResult<APIResponse>> DeleteProjectXProjectType(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var projectXProject = await _unitOfWork.ProjectXProjectType.GetAsync(u => u.Id == id);
                if (projectXProject == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ProjectXProjectType.RemoveAsync(projectXProject);
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
    }
}

