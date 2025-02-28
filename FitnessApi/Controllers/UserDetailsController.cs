using FitnessApi.Dto.UserDetails;
using FitnessApi.IRepository;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;
using FitnessApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsRepository _userDetails;
        private readonly IMapper<UserDetail, UserDetailsDTO> _userDetailsMapper;
        private readonly IMapper<CreateUserDetailsDTO, UserDetail> _createUserDetailsMapper;
        private readonly IMapper<UpdateUserDetailsDTO, UserDetail> _updateUserDetailsMapper;
        private APIResponse _apiResponse;
        public UserDetailsController(IUserDetailsRepository userDetails, IMapper<UserDetail, UserDetailsDTO> userDetailsMapper, IMapper<CreateUserDetailsDTO, UserDetail> createUserDetailsMapper, IMapper<UpdateUserDetailsDTO, UserDetail> updateUserDetailsMapper)
        {
            _userDetails = userDetails;
            _userDetailsMapper = userDetailsMapper;
            _createUserDetailsMapper = createUserDetailsMapper;
            _userDetailsMapper = userDetailsMapper;
            _apiResponse = new APIResponse();
        }

        [HttpPost("create-user-details")]
        public async Task<IActionResult> CreateUserDetails([FromBody] CreateUserDetailsDTO userDetailsDTO)
        {

            if (!ModelState.IsValid)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(_apiResponse);
            }

            try
            {
               await _userDetails.CreateAsync(_createUserDetailsMapper.Map(userDetailsDTO));
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = "Successfully Created UserDetails";

                return StatusCode(201, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add(SD.UnexpectedError);
                return BadRequest(_apiResponse);
            }
        }

        [HttpGet("get-user-details/{userId}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] string userId)
        {
            try
            {
                UserDetail userDetail = await _userDetails.GetAsync(u => u.UserId == userId);
                if (userDetail == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages.Add("User Not Found");
                    return NotFound(_apiResponse);
                }
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = _userDetailsMapper.Map(userDetail);
                return Ok(_apiResponse);
            }
            catch (Exception)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add(SD.UnexpectedError);
                return BadRequest(_apiResponse);
            }

        }

        [HttpPost("update-user-details/{userId}")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserDetailsDTO updateUserDetailsDTO, [FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(_apiResponse);
            }

            try
            {
                UserDetail userDetail = await _userDetails.GetAsync(u => u.UserId == userId);
                if(userDetail == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages.Add("User Not Found");
                    return NotFound(_apiResponse);
                }

                userDetail.FitnessGoal = updateUserDetailsDTO.FitnessGoal;
                userDetail.Gender = updateUserDetailsDTO.Gender;
                userDetail.Weight = updateUserDetailsDTO.Weight;
                userDetail.Height = updateUserDetailsDTO.Height;
                userDetail.PreviousFitnessExperience = updateUserDetailsDTO.PreviousFitnessExperience;
                userDetail.SpecificDiet = updateUserDetailsDTO.SpecificDiet;
                userDetail.DaysCommit = updateUserDetailsDTO.DaysCommit;
                userDetail.SpecificExperiencePreferance = updateUserDetailsDTO.SpecificExperiencePreferance;
                userDetail.CalorieyGoal = updateUserDetailsDTO.CalorieyGoal;
                userDetail.SleepQuality = updateUserDetailsDTO.SleepQuality;


                await _userDetails.UpdateUserDetailsAsync(userDetail);

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Result = "Successfully updated userDetails";

                return StatusCode(202, _apiResponse);

            }
            catch (Exception)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add(SD.UnexpectedError);
                return BadRequest(_apiResponse);
            }

        }



    }
}
