using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PortfolioManagement_API.Models;
using PortfolioManagement_API.Models.Dto;
using PortfolioManagement_API.Repository.IRepository;
using System.Data;
using System.Net;
using System.Text.Json;


namespace PortfolioManagement_API.Controllers.V1
{
	//[Route("api/[controller]")]
	[Route("api/v{version:apiVersion}/[Controller]/[Action]")]
	[ApiController]
    [ApiVersion("1.0")]
    public class TechnologyAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TechnologyAPIController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }


		[HttpGet(Name = "GetTechnologys")]
		[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetTechnologys([FromQuery(Name = "filterDisplayOrder")] int? Id,
            [FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Technology> paymentList = await _unitOfWork.Technology.GetAllAsync();
             
                if (!string.IsNullOrEmpty(search))
                {
                    string datasearch = search.ToLower();
                    paymentList = paymentList.Where(u => u.TechnologyName.ToLower().Contains(datasearch));
                }
                //Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };
                if (pageNumber > 0)
                {
                    paymentList = paymentList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                }
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paymentList));
                _response.Result = _mapper.Map<List<TechnologyDTO>>(paymentList);
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


        //[HttpGet(Name = "TechnologyByPagination")]
        //[ResponseCache(CacheProfileName = "Default30")]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<APIResponse>> TechnologyByPagination(string term, string orderBy, int currentPage = 1)
        //{
        //    try
        //    {
        //        term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

        //        TechnologyIndexVM paymentIndexVM = new TechnologyIndexVM();
        //        IEnumerable<Technology> list = await _unitOfWork.Technology.GetAllAsync();
        //        list.OrderBy(a => a.TechnologyName).ToList();

        //        var List = _mapper.Map<List<TechnologyDTO>>(list);

        //        paymentIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "projectSizes_desc" : "";

        //        if (!string.IsNullOrEmpty(term))
        //        {
        //            List = List.Where(u => u.TechnologyName.ToLower().Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
        //        }

        //        switch (orderBy)
        //        {
        //            case "projectSizes_desc":
        //                List = List.OrderByDescending(a => a.Technologys).ToList();
        //                break;

        //            default:
        //                List = List.OrderBy(a => a.Technologys).ToList();
        //                break;
        //        }
        //        int totalRecords = List.Count();
        //        int pageSize = 10;
        //        int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        //        List = List.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        //        // current=1, skip= (1-1=0), take=5 
        //        // currentPage=2, skip (2-1)*5 = 5, take=5 ,
        //        paymentIndexVM.languages = List;
        //        paymentIndexVM.CurrentPage = currentPage;
        //        paymentIndexVM.TotalPages = totalPages;
        //        paymentIndexVM.Term = term;
        //        paymentIndexVM.PageSize = pageSize;
        //        paymentIndexVM.OrderBy = orderBy;

        //        _response.Result = _mapper.Map<TechnologyIndexVM>(paymentIndexVM);
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

        [HttpGet("{id:int}", Name = "GetTechnology")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<ActionResult<APIResponse>> GetTechnology(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var payment = await _unitOfWork.Technology.GetAsync(u => u.Id == id);
                if (payment == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<TechnologyDTO>(payment);
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

        [HttpPost(Name = "CreateTechnology")]
       // [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTechnology([FromForm] TechnologyCreateDTO createDTO)
        {

            try
            {
                if (await _unitOfWork.Technology.GetAsync(u => u.TechnologyName.Trim().ToLower() == createDTO.TechnologyName.Trim().ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Technology name already Exists!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
              
                Technology payment = _mapper.Map<Technology>(createDTO);
            
                await _unitOfWork.Technology.CreateAsync(payment);
                _response.Result = _mapper.Map<TechnologyDTO>(payment);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetTechnology", new { id = payment.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteTechnology")]
     //   [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteTechnology(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var payment = await _unitOfWork.Technology.GetAsync(u => u.Id == id);
                if (payment == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Technology.RemoveAsync(payment);
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
        [HttpPut("{id:int}", Name = "UpdateTechnology")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateTechnology(int id, [FromForm] TechnologyUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                if (await _unitOfWork.Technology.GetAsync(u => u.TechnologyName.ToLower() == updateDTO.TechnologyName.ToLower() && u.Id != updateDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "TechnologyName already Exists!");
                    return BadRequest(ModelState);
                }

                Technology model = _mapper.Map<Technology>(updateDTO);
                await _unitOfWork.Technology.UpdateAsync(model);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialTechnology")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialTechnology(int id, JsonPatchDocument<TechnologyUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var payment = await _unitOfWork.Technology.GetAsync(u => u.Id == id, tracked: false);

            TechnologyUpdateDTO paymentDTO = _mapper.Map<TechnologyUpdateDTO>(payment);


            if (payment == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(paymentDTO, ModelState);
            Technology model = _mapper.Map<Technology>(paymentDTO);

            await _unitOfWork.Technology.UpdateAsync(model);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }


    }
}
