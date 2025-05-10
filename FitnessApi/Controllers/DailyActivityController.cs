using FitnessApi.Dto.DailyActivity;
using FitnessApi.IRepository;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;
using FitnessApi.Repository;
using FitnessApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyActivityController : ControllerBase
    {
        private readonly IDailyActivityRepository _activityRepo;
        private readonly IMapper<CreateDailyActivityDTO, DailyActivity> _createMapper;
        private readonly IMapper<UpdateDailyActivityDTO, DailyActivity> _updateMapper;
        private readonly IMapper<DailyActivity, DailyActivityDTO> _dtoMapper;
        private readonly APIResponse _response;

        public DailyActivityController(
            IDailyActivityRepository activityRepo,
            IMapper<CreateDailyActivityDTO, DailyActivity> createMapper,
            IMapper<UpdateDailyActivityDTO, DailyActivity> updateMapper,
            IMapper<DailyActivity, DailyActivityDTO> dtoMapper)
        {
            _activityRepo = activityRepo;
            _createMapper = createMapper;
            _updateMapper = updateMapper;
            _dtoMapper = dtoMapper;
            _response = new APIResponse();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDailyActivityDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                           .Select(e => e.ErrorMessage)
                                                           .ToList();
                return BadRequest(_response);
            }

            try
            {

                var existing = (await _activityRepo
    .GetAllAsync(a => a.UserId == dto.UserId))
    .FirstOrDefault(a => a.Date.Date == dto.Date.Date);

                if (existing != null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.Conflict;
                    _response.ErrorMessages.Add("A record for this user on the same date already exists.");
                    return Conflict(_response);
                }

                await _activityRepo.CreateAsync(_createMapper.Map(dto));
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully inserted";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add($"An error occurred while creating activity: {ex.Message}");
                return StatusCode(500, _response);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDailyActivityDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                           .Select(e => e.ErrorMessage)
                                                           .ToList();
                return BadRequest(_response);
            }

            try
            {
                await _activityRepo.UpdateActivityAsync(_updateMapper.Map(dto));
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully updated";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add($"An error occurred while updating activity: {ex.Message}");
                return StatusCode(500, _response);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            try
            {
                var activities = await _activityRepo.GetAllAsync(a => a.UserId == userId);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = activities.Select(_dtoMapper.Map);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add($"An error occurred while fetching activities: {ex.Message}");
                return StatusCode(500, _response);
            }
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var record = await _activityRepo.GetAsync(a => a.Id == id);
                if (record == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = "Activity not found";
                    return NotFound(_response);
                }

                await _activityRepo.RemoveAsync(record);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully deleted";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add($"An error occurred while deleting activity: {ex.Message}");
                return StatusCode(500, _response);
            }
        }
    }
}