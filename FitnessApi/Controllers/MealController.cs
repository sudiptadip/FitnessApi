using FitnessApi.Dto.Meal;
using FitnessApi.Dto.UserDetails;
using FitnessApi.IRepository;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Mappers.MealMapper;
using FitnessApi.Model;
using FitnessApi.Repository;
using FitnessApi.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealRepository _mealRepository;
        private APIResponse _response;
        private readonly IMapper<CreateMealDTO, Meal> _createMealMapper;
        private readonly IMapper<UpdateMealDTO, Meal> _updateMealMapper;
        private readonly IMapper<Meal, MealDTO> _mealMapper;

        public MealController(IMealRepository mealRepository, IMapper<CreateMealDTO, Meal> createMealMapper, IMapper<UpdateMealDTO, Meal> updateMealMapper, IMapper<Meal, MealDTO> mealMapper)
        {
            _mealRepository = mealRepository;
            _response = new APIResponse();
            _createMealMapper = createMealMapper;
            _updateMealMapper = updateMealMapper;
            _mealMapper = mealMapper;
        }

        [HttpPost("create-meal")]
        public async Task<IActionResult> CreateMeal([FromBody] CreateMealDTO UpdateMealDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(_response);
            }

            try
            {
                await _mealRepository.CreateAsync(_createMealMapper.Map(UpdateMealDTO));
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully insert";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(SD.UnexpectedError);
                return StatusCode(500, _response);
            }
        }

        [HttpPut("update-meal")]
        public async Task<IActionResult> UpdateMeal([FromBody] UpdateMealDTO UpdateMealDTO)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(_response);
            }

            try
            {
                await _mealRepository.UpdateMealAsync(_updateMealMapper.Map(UpdateMealDTO));
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully update";
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(SD.UnexpectedError);
                return StatusCode(500, _response);
            }
        }

        [HttpGet("get-meal/{userId}")]
        public async Task<IActionResult> GetMealByUserId([FromRoute] string userId)
        {
            try
            {
                var mealList = await _mealRepository.GetAllAsync(u => u.UserId == userId, includeProperties: "MealItems");
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = mealList.Select(x => _mealMapper.Map(x));
                return Ok(_response);
            }
            catch (Exception)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(SD.UnexpectedError);
                return StatusCode(500, _response);
            }
        }

        [HttpDelete("delete-meal/{id:int}")]
        public async Task<IActionResult> DeleteMeal([FromRoute] int id)
        {
            try
            {
                var meal = await _mealRepository.GetAsync(u => u.Id == id, includeProperties: "MealItems");
                if (meal == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.Result = "Meal Not Found";
                    return NotFound(_response);
                }

                await _mealRepository.RemoveAsync(meal);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully Deleted";
                return Ok(_response);
            }
            catch (Exception)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(SD.UnexpectedError);
                return StatusCode(500, _response);
            }
        }

    }
}
