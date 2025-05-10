using FitnessApi.Dto.DailyStepActivity;
using FitnessApi.IRepository;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace FitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyStepActivityController : ControllerBase
    {
        private readonly IDailyStepActivityRepository _repository;
        private readonly IDailyActivityRepository _activityRepo;
        private readonly IMapper<DailyStepActivity, DailyStepActivityDTO> _dtoMapper;
        private readonly IMapper<CreateDailyStepActivityDTO, DailyStepActivity> _createMapper;
        private readonly APIResponse _apiResponse;

        public DailyStepActivityController(IDailyStepActivityRepository repository,
            IMapper<DailyStepActivity, DailyStepActivityDTO> dtoMapper,
            IMapper<CreateDailyStepActivityDTO, DailyStepActivity> createMapper, IDailyActivityRepository activityRepo)
        {
            _repository = repository;

            _dtoMapper = dtoMapper;
            _createMapper = createMapper;
            _apiResponse = new APIResponse();
            _activityRepo = activityRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateDailyStepActivityDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                               .Select(e => e.ErrorMessage)
                                                               .ToList();
                return BadRequest(_apiResponse);
            }

            try
            {
                var targetDate = dto.Date.Value.Date;
                var nextDate = targetDate.AddDays(1);

                var existing = (await _repository
    .GetAllAsync(a => a.UserId == dto.UserId))
    .FirstOrDefault(a => a.Date?.Date == dto.Date?.Date);

                if (existing != null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.Conflict;
                    _apiResponse.ErrorMessages.Add("A record for this user on the same date already exists.");
                    return Conflict(_apiResponse);
                }

                // Proceed to create
                DailyStepActivity entity = _createMapper.Map(dto);
                await _repository.CreateAsync(entity);
                await _repository.SaveAsync();

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = "Activity created successfully";
                return StatusCode(201, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, _apiResponse);
            }
        }

        
        [HttpGet("get-by-user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            try
            {
                var data = await _repository.GetAllAsync(a => a.UserId == userId);

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = data.Select(_dtoMapper.Map).ToList();
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, _apiResponse);
            }
        }


        class GetStepAndActivityByUserVM
        {
           public List<DailyStepActivity> DailyStepActivityList { get; set; }
           public List<DailyActivity> DailyActivityList { get; set; }
        }


        [HttpGet("GetStepAndActivityByUser/{userId}")]
        public async Task<IActionResult> GetStepAndActivityByUser(string userId)
        {
            try
            {
                var data = await _repository.GetAllAsync(a => a.UserId == userId);
                var data2 = await _activityRepo.GetAllAsync(a => a.UserId == userId);

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                GetStepAndActivityByUserVM vm = new GetStepAndActivityByUserVM
                {
                    DailyStepActivityList = data,
                    DailyActivityList = data2
                };

                _apiResponse.Result = vm;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add($"Error: {ex.Message}");
                return StatusCode(500, _apiResponse);
            }
        }



        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDailyStepActivityDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                                              .Select(e => e.ErrorMessage)
                                                              .ToList();
                return BadRequest(_apiResponse);
            }

            try
            {
                var existingActivity = await _repository.GetAsync(a => a.Id == id);

                if (existingActivity == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages.Add("Activity not found");
                    return NotFound(_apiResponse);
                }

                // Update properties
                existingActivity.Step = dto.Step;
                existingActivity.Kcal = dto.Kcal;
                existingActivity.Km = dto.Km;
                existingActivity.Minutes = dto.Minutes;

                await _repository.UpdateAsync(existingActivity);
                await _repository.SaveAsync();

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Accepted;
                _apiResponse.Result = "Activity updated successfully";
                return StatusCode(202, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add($"Error while updating: {ex.Message}");
                return StatusCode(500, _apiResponse);
            }
        }


        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var activity = await _repository.GetAsync(a => a.Id == id);
                if (activity == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages.Add("Activity not found");
                    return NotFound(_apiResponse);
                }

                await _repository.RemoveAsync(activity);
                await _repository.SaveAsync();

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = "Activity deleted successfully";
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add($"Error while deleting: {ex.Message}");
                return StatusCode(500, _apiResponse);
            }
        }


    }
}
