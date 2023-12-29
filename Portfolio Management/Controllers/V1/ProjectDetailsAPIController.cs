using AutoMapper;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using PortfolioManagement_API.Repository.IRepository;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text.Json;

namespace PortfolioManagement_API.Controllers.V1
{
	//[Route("api/[controller]")]
	[Route("api/v{version:apiVersion}/[Controller]/[Action]")]
	[ApiController]
    [ApiVersion("1.0")]
    public class ProjectDetailsAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProjectDetailsAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }


		[HttpGet(Name = "GetProjectDetailss")]
		[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjectDetailss([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {

                IEnumerable<ProjectDetails> projectDetailsList = await _unitOfWork.ProjectDetails.GetAllAsync();

             
                if (!string.IsNullOrEmpty(search))
                {
                    string datasearch = search.ToLower();
                    projectDetailsList = projectDetailsList.Where(u => u.ProjectName.ToLower().Contains(datasearch));
                }
                // Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                if (pageNumber > 0)
                {
                    projectDetailsList = projectDetailsList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(projectDetailsList));
                _response.Result = _mapper.Map<List<ProjectDetailsDTO>>(projectDetailsList);
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


        //[HttpGet(Name = "ProjectDetailsByPagination")]
        //[ResponseCache(CacheProfileName = "Default30")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> ProjectDetailsByPagination(string term, string orderBy, int currentPage = 1)
        //{
        //    try
        //    {
        //        term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

        //        ProjectDetailsIndexVM projectDetailsIndexVM = new ProjectDetailsIndexVM();
        //        IEnumerable<ProjectDetails> list = await _unitOfWork.ProjectDetails.GetAllAsync();
        //        list.OrderBy(a => a.ProjectDetailss).ToList();

        //        var List = _mapper.Map<List<ProjectDetailsDTO>>(list);

        //        projectDetailsIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "projectSizes_desc" : "";

        //        if (!string.IsNullOrEmpty(term))
        //        {
        //            List = List.Where(u => u.ProjectDetailss.ToLower().Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        //        }

        //        switch (orderBy)
        //        {
        //            case "projectSizes_desc":
        //                List = List.OrderByDescending(a => a.ProjectDetailss).ToList();
        //                break;

        //            default:
        //                List = List.OrderBy(a => a.ProjectDetailss).ToList();
        //                break;
        //        }
        //        int totalRecords = List.Count();
        //        int pageSize = 10;
        //        int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        //        List = List.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        //        // current=1, skip= (1-1=0), take=5 
        //        // currentPage=2, skip (2-1)*5 = 5, take=5 ,
        //        projectDetailsIndexVM.hourlyRates = List;
        //        projectDetailsIndexVM.CurrentPage = currentPage;
        //        projectDetailsIndexVM.TotalPages = totalPages;
        //        projectDetailsIndexVM.Term = term;
        //        projectDetailsIndexVM.PageSize = pageSize;
        //        projectDetailsIndexVM.OrderBy = orderBy;

        //        _response.Result = _mapper.Map<ProjectDetailsIndexVM>(projectDetailsIndexVM);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages
        //             = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}

        [HttpGet("{id:int}", Name = "GetProjectDetails")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(CategoryDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetProjectDetails(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var projectDetails = await _unitOfWork.ProjectDetails.GetAsync(u => u.Id == id);
                if (projectDetails == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProjectDetailsDTO>(projectDetails);
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

        [HttpGet(Name = "ProjectDetailsByLazyLoading")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> ProjectDetailsByLazyLoading(int pageNum)
        {
            try
            {
                const int RecordsPerPage = 10;

                IEnumerable<ProjectDetails> projectDetailsList = await _unitOfWork.ProjectDetails.GetAllAsync();


                int skip = pageNum * RecordsPerPage;
                var tempList = projectDetailsList.Skip(skip).Take(RecordsPerPage).ToList();

                _response.Result = _mapper.Map<List<ProjectDetailsDTO>>(tempList);

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


        [HttpPost(Name = "CreateProjectDetails")]
       // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProjectDetails([FromForm] ProjectDetailsCreateDTO createDTO)
        {

            try
            {
                if (await _unitOfWork.ProjectDetails.GetAsync(u => u.ProjectName.Trim().ToLower() == createDTO.ProjectName.Trim().ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "ProjectDetails name already Exists!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
              
                ProjectDetails projectDetails = _mapper.Map<ProjectDetails>(createDTO);
            
                await _unitOfWork.ProjectDetails.CreateAsync(projectDetails);
                _response.Result = _mapper.Map<ProjectDetailsDTO>(projectDetails);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProjectDetails", new { id = projectDetails.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteProjectDetails")]
     //   [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteProjectDetails(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var projectDetails = await _unitOfWork.ProjectDetails.GetAsync(u => u.Id == id);
                if (projectDetails == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ProjectDetails.RemoveAsync(projectDetails);
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
    //    [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateProjectDetails")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProjectDetails(int id, [FromForm] ProjectDetailsUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _unitOfWork.ProjectDetails.GetAsync(u => u.ProjectName.ToLower() == updateDTO.ProjectName.ToLower() && u.Id != updateDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "ProjectDetailsName already Exists!");
                    return BadRequest(ModelState);
                }

                ProjectDetails model = _mapper.Map<ProjectDetails>(updateDTO);
                await _unitOfWork.ProjectDetails.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialProjectDetails")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialProjectDetails(int id, JsonPatchDocument<ProjectDetailsUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var projectDetails = await _unitOfWork.ProjectDetails.GetAsync(u => u.Id == id, tracked: false);

            ProjectDetailsUpdateDTO projectDetailsDTO = _mapper.Map<ProjectDetailsUpdateDTO>(projectDetails);


            if (projectDetails == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(projectDetailsDTO, ModelState);
            ProjectDetails model = _mapper.Map<ProjectDetails>(projectDetailsDTO);

            await _unitOfWork.ProjectDetails.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpGet(Name = "ProjectDetailsSearchByLazyLoading")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> ProjectDetailsSearchByLazyLoading([FromQuery] int pageNum, string? search)
        {



            try
            {
                const int RecordsPerPage = 3;

                IEnumerable<ProjectDetails> companyDTOList = await _unitOfWork.ProjectDetails.GetAllAsync();

                if (!string.IsNullOrEmpty(search))
                {
                    companyDTOList = companyDTOList.Where(x => x.ProjectName.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                int skip = pageNum * RecordsPerPage;
                var tempList = companyDTOList.Skip(skip).Take(RecordsPerPage).ToList();

                if (pageNum == 0 && tempList.Count == 0)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages
                         = new List<string>() { "Data does not exists" };
                }
                else
                {
                    _response.Result = _mapper.Map<List<ProjectDetailsDTO>>(tempList);

                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }

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
