using Azure;
using FitnessApi.Dto.UserDetails;
using FitnessApi.IRepository;
using FitnessApi.IService;
using FitnessApi.Mappers.IMappers;
using FitnessApi.Model;
using FitnessApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Formats.Asn1;
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
        private readonly IImageService _imageService;

        public UserDetailsController(IImageService imageService, IUserDetailsRepository userDetails, IMapper<UserDetail, UserDetailsDTO> userDetailsMapper, IMapper<CreateUserDetailsDTO, UserDetail> createUserDetailsMapper, IMapper<UpdateUserDetailsDTO, UserDetail> updateUserDetailsMapper)
        {
            _userDetails = userDetails;
            _userDetailsMapper = userDetailsMapper;
            _createUserDetailsMapper = createUserDetailsMapper;
            _userDetailsMapper = userDetailsMapper;
            _apiResponse = new APIResponse();
            _imageService = imageService;
        }

        [HttpPost("create-user-details")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateUserDetails([FromForm] CreateUserDetailsDTO dto)
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

                var isExistingUser = await _userDetails.GetAsync(u => u.UserId == dto.UserId);

                if(isExistingUser != null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.ErrorMessages.Add($"UserDetail already created");
                    return StatusCode(400, _apiResponse);
                }


                string? imagePath = null;

                if (dto.ProfileImage != null)
                {
                    imagePath = await _imageService.SaveImageAsync(dto.ProfileImage);
                }

                var userDetails = new UserDetail
                {
                    UserId = dto.UserId,
                    FitnessGoal = dto.FitnessGoal,
                    Gender = dto.Gender,
                    Weight = dto.Weight,
                    Height = dto.Height,
                    PreviousFitnessExperience = dto.PreviousFitnessExperience,
                    SpecificDiet = dto.SpecificDiet,
                    DaysCommit = dto.DaysCommit,
                    SpecificExperiencePreferance = dto.SpecificExperiencePreferance,
                    CalorieyGoal = dto.CalorieyGoal,
                    SleepQuality = dto.SleepQuality,
                    ProfileImageUrl = imagePath,
                    Age = dto.Age,
                    Bmi = (Convert.ToDouble(dto.Weight) / Math.Pow(Convert.ToDouble(dto.Height) * 0.3048, 2)).ToString("F2"),
                };

                await _userDetails.CreateAsync(userDetails);
                await _userDetails.SaveAsync();

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = "User details created successfully";
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add($"Error while creating user details: {ex.Message}");
                return StatusCode(500, _apiResponse);
            }
        }

        [HttpGet("get-user-details/{userId}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] string userId)
        {
            try
            {
                var userDetail = await _userDetails.GetAsync(u => u.UserId == userId);

                if (userDetail == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages.Add("User Details Not Found");
                    return NotFound(_apiResponse);
                }
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //_apiResponse.Result = _userDetailsMapper.Map(userDetail);
                _apiResponse.Result = userDetail;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUserDetails([FromForm] UpdateUserDetailsDTO updateUserDetailsDTO, [FromRoute] string userId)
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
                var userDetail = await _userDetails.GetAsync(u => u.UserId == userId);
                if (userDetail == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.ErrorMessages.Add("User Not Found");
                    return NotFound(_apiResponse);
                }

                // Update fields
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
                userDetail.Age = updateUserDetailsDTO.Age;
                userDetail.Bmi = (Convert.ToDouble(updateUserDetailsDTO.Weight) / Math.Pow(Convert.ToDouble(updateUserDetailsDTO.Height) * 0.3048, 2)).ToString("F2");

                // Optional image update
                if (updateUserDetailsDTO.ProfileImage != null)
                {
                    if(userDetail.ProfileImageUrl == null)
                    {
                        string imagePath = await _imageService.SaveImageAsync(updateUserDetailsDTO.ProfileImage);
                        userDetail.ProfileImageUrl = imagePath;
                    }
                    else
                    {
                        //  string imagePath = await _imageService.UpdateImageAsync(userDetail.ProfileImageUrl, updateUserDetailsDTO.ProfileImage);
                        string imagePath = await _imageService.SaveImageAsync(updateUserDetailsDTO.ProfileImage);
                        userDetail.ProfileImageUrl = imagePath;
                    }
                        
                }

                await _userDetails.UpdateUserDetailsAsync(userDetail);

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.Accepted;
                _apiResponse.Result = "Successfully updated user details";
                return StatusCode(202, _apiResponse);
            }
            catch (Exception)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages.Add(SD.UnexpectedError);
                return StatusCode(500, _apiResponse);
            }
        }


    }
}
