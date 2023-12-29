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
    public class ProjectTypeAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProjectTypeAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }


		[HttpGet(Name = "GetProjectTypes")]
		[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProjectTypes([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string search, int pageSize = 0, int pageNumber = 0)
        {
            try
            {
                IEnumerable<ProjectType> projectTypeList = await _unitOfWork.ProjectType.GetAllAsync();


                if (!string.IsNullOrEmpty(search))
                {
                    string datasearch = search.ToLower();
                    projectTypeList = projectTypeList.Where(u => u.ProjectTypes.ToLower().Contains(datasearch));
                }
               // Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
               if(pageNumber > 0)
                {
                projectTypeList = projectTypeList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(projectTypeList));
                _response.Result = _mapper.Map<List<ProjectTypeDTO>>(projectTypeList);
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


        //[HttpGet(Name = "ProjectTypeByPagination")]
        //[ResponseCache(CacheProfileName = "Default30")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> ProjectTypeByPagination(string term, string orderBy, int currentPage = 1)
        //{
        //    try
        //    {
        //        term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

        //        ProjectTypeIndexVM projectTypeIndexVM = new ProjectTypeIndexVM();
        //        IEnumerable<ProjectType> list = await _unitOfWork.ProjectType.GetAllAsync();
        //        list.OrderBy(a => a.ProjectTypeName).ToList();

        //        var List = _mapper.Map<List<ProjectTypeDTO>>(list);

        //        projectTypeIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "projectSizes_desc" : "";

        //        if (!string.IsNullOrEmpty(term))
        //        {
        //            List = List.Where(u => u.ProjectTypeName.ToLower().Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        //        }

        //        switch (orderBy)
        //        {
        //            case "projectSizes_desc":
        //                List = List.OrderByDescending(a => a.ProjectTypeName).ToList();
        //                break;

        //            default:
        //                List = List.OrderBy(a => a.ProjectTypeName).ToList();
        //                break;
        //        }
        //        int totalRecords = List.Count();
        //        int pageSize = 10;
        //        int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        //        List = List.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        //        // current=1, skip= (1-1=0), take=5 
        //        // currentPage=2, skip (2-1)*5 = 5, take=5 ,
        //        projectTypeIndexVM.languages = List;
        //        projectTypeIndexVM.CurrentPage = currentPage;
        //        projectTypeIndexVM.TotalPages = totalPages;
        //        projectTypeIndexVM.Term = term;
        //        projectTypeIndexVM.PageSize = pageSize;
        //        projectTypeIndexVM.OrderBy = orderBy;

        //        _response.Result = _mapper.Map<ProjectTypeIndexVM>(projectTypeIndexVM);
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

        [HttpGet("{id:int}", Name = "GetProjectType")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type =typeof(CategoryDTO))]
        //        [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
        public async Task<ActionResult<APIResponse>> GetProjectType(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var projectType = await _unitOfWork.ProjectType.GetAsync(u => u.Id == id);
                if (projectType == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProjectTypeDTO>(projectType);
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

        [HttpPost(Name = "CreateProjectType")]
       // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProjectType([FromForm] ProjectTypeCreateDTO createDTO)
        {

            try
            {
                if (await _unitOfWork.ProjectType.GetAsync(u => u.ProjectTypes.Trim().ToLower() == createDTO.ProjectTypes.Trim().ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "ProjectType name already Exists!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
              
                ProjectType projectType = _mapper.Map<ProjectType>(createDTO);
            
                await _unitOfWork.ProjectType.CreateAsync(projectType);
                _response.Result = _mapper.Map<ProjectTypeDTO>(projectType);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProjectType", new { id = projectType.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteProjectType")]
     //   [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteProjectType(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var projectType = await _unitOfWork.ProjectType.GetAsync(u => u.Id == id);
                if (projectType == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ProjectType.RemoveAsync(projectType);
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
        [HttpPut("{id:int}", Name = "UpdateProjectType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProjectType(int id, [FromForm] ProjectTypeUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _unitOfWork.ProjectType.GetAsync(u => u.ProjectTypes.ToLower() == updateDTO.ProjectTypes.ToLower() && u.Id != updateDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "ProjectTypeName already Exists!");
                    return BadRequest(ModelState);
                }

                ProjectType model = _mapper.Map<ProjectType>(updateDTO);
                await _unitOfWork.ProjectType.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialProjectType")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialProjectType(int id, JsonPatchDocument<ProjectTypeUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var projectType = await _unitOfWork.ProjectType.GetAsync(u => u.Id == id, tracked: false);

            ProjectTypeUpdateDTO projectTypeDTO = _mapper.Map<ProjectTypeUpdateDTO>(projectType);


            if (projectType == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(projectTypeDTO, ModelState);
            ProjectType model = _mapper.Map<ProjectType>(projectTypeDTO);

            await _unitOfWork.ProjectType.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
