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

    public class ProjectXTechnologyAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;

        public ProjectXTechnologyAPIController(IUnitOfWork unitOfWork, IMapper mapper,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
            _db = db;
        }


        [HttpGet(Name= "GetProjectXTechnologys")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjectXTechnologys([FromQuery(Name = "filterDisplayOrder")] int? Id,
           [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<ProjectXTechnology> projectXTechnologyList;

                if (Id > 0)
                {

                    projectXTechnologyList = await _unitOfWork.ProjectXTechnology.GetAllAsync(u => u.Id == Id, includeProperties: "ProjectDetails,Technology", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                else
                {
                    projectXTechnologyList = await _unitOfWork.ProjectXTechnology.GetAllAsync(includeProperties: "ProjectDetails,Technology", pageSize: pageSize,
                        pageNumber: pageNumber);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    projectXTechnologyList = projectXTechnologyList.Where(u => u.ProjectDetails.ProjectName.ToLower().Contains(search) ||
                                                 u.Technology.TechnologyName.ToLower().Contains(search));

                }
                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<ProjectXTechnologyDTO>>(projectXTechnologyList);
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



        [HttpGet("{id:int}", Name = "GetProjectXTechnology")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(CategoryDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetProjectXTechnology(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var projectXTech = await _unitOfWork.ProjectXTechnology.GetAsync(u => u.Id == id, includeProperties: "ProjectDetails,Technology");
                if (projectXTech == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProjectXTechnologyDTO>(projectXTech);
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


        [HttpPost(Name = "CreateProjectXTechnology")]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProjectXTechnology([FromForm] CxtDTO createDTO)
        {
            try
            {

                await _unitOfWork.ProjectXTechnology.RemoveRangeAsync(u => u.ProjectDetailsId == createDTO.ProjectDetailsId, false);

                foreach (var technologyId in createDTO.SelectedTechnologyIds)
                {
                    ProjectXTechnology projectXTechnology = new();
                    projectXTechnology.ProjectDetailsId = createDTO.ProjectDetailsId;
                    projectXTechnology.TechnologyId = Convert.ToInt32(technologyId);
                    await _unitOfWork.ProjectXTechnology.CreateAsync(projectXTechnology);
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

        [HttpGet("{projectDetailsId:int}", Name = "GetProjectXTechnologyByProjectDetailsId")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProjectXTechnologyByProjectDetailsId(int projectDetailsId)
        {
            try
            {
                if (projectDetailsId == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var states = _db.ProjectXTechnologies.Include(u => u.ProjectDetails).Include(u => u.Technology).Where(u => u.ProjectDetailsId == projectDetailsId).ToList();

                if (states == null || states.Count() == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<List<ProjectXTechnologyDTO>>(states);
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




        //[HttpPost(Name = "CreateProjectXTechnology")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<APIResponse>> CreateProjectXTechnology([FromForm] YourModel createDTO)
        //{
        //    try
        //    {
        //        await _unitOfWork.ProjectXTechnology.RemoveRangeAsync(u => u.CompanyId == createDTO.CompanyId, false);

        //        foreach (var colorid in createDTO.SelectedAmenityIds)
        //        {
        //            // bool combinationExists = carXColorList.Any(c => c.CarId == createDTO.CarId && c.ColorId == Convert.ToInt32(colorid));
        //            //if (combinationExists)
        //            //{
        //            ProjectXTechnology carXColor = new();
        //            carXColor.CompanyId = createDTO.CompanyId;
        //            carXColor.AmenityId = Convert.ToInt32(colorid);
        //            await _unitOfWork.ProjectXTechnology.CreateAsync(carXColor);

        //            //}
        //            //else
        //            //{
        //            //    CarXColor carXColor = new();
        //            //    carXColor.CarId = createDTO.CarId;
        //            //    carXColor.ColorId = Convert.ToInt32(colorid);
        //            //    await _unitOfWork.CarXColor.CreateAsync(carXColor);

        //            //}
        //        }
        //        _response.StatusCode = HttpStatusCode.Created;
        //        return _response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}


        //[HttpGet("{firstCategoryId:int}", Name = "GetAmenityByFirstCategoryId")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> GetAmenityByFirstCategoryId(int firstCategoryId)
        //{
        //    try
        //    {
        //        if (firstCategoryId == 0)
        //        {
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }
        //        var states = _db.Amenities.Include(u => u.FirstCategory).Where(u => u.FirstCategoryId == firstCategoryId).ToList();

        //        if (states == null || states.Count() == 0)
        //        {
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }

        //        _response.Result = _mapper.Map<List<AmenityDTO>>(states);
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




        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteProjectXTechnology")]

        public async Task<ActionResult<APIResponse>> DeleteProjectXTechnology(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var projectXTech = await _unitOfWork.ProjectXTechnology.GetAsync(u => u.Id == id);
                if (projectXTech == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ProjectXTechnology.RemoveAsync(projectXTech);
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

