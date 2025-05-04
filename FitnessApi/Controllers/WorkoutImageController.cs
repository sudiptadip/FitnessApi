using Azure;
using FitnessApi.Data;
using FitnessApi.Dto.UserDetails;
using FitnessApi.Dto.WorkoutImage;
using FitnessApi.IService;
using FitnessApi.Model;
using FitnessApi.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutImageController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IImageService _imageService;
        private APIResponse _response;
        public WorkoutImageController(ApplicationDbContext db, IImageService imageService)
        {
            _db = db;
            _imageService = imageService;
            _response = new APIResponse();
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateWorkoutImageDto createWorkoutImageDto)
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
                var imagePath = await _imageService.SaveImageAsync(createWorkoutImageDto.Image);
                await _db.WorkoutImages.AddAsync(new Model.WorkoutImage
                {
                    ImageName = createWorkoutImageDto.ImageName!,
                    Type = createWorkoutImageDto.Type,
                    ImageUrl = imagePath
                });

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully insert";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(SD.UnexpectedError + ex.Message);
                return StatusCode(500, _response);
            }

        }



        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateWorkoutImageDto updateDto)
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
                var workoutImage = await _db.WorkoutImages.FindAsync(updateDto.Id);
                if (workoutImage == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Image not found.");
                    return NotFound(_response);
                }

                if (updateDto.Image != null)
                {
                    // workoutImage.ImageUrl = await _imageService.UpdateImageAsync(workoutImage.ImageUrl, updateDto.Image);
                    workoutImage.ImageUrl = await _imageService.SaveImageAsync(updateDto.Image);
                }

                workoutImage.ImageName = updateDto.ImageName.ToString();
                workoutImage.Type = updateDto.Type;

                _db.WorkoutImages.Update(workoutImage);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "Successfully updated";
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


        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var workoutImage = await _db.WorkoutImages.FindAsync(id);
                if (workoutImage == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages.Add("Image not found.");
                    return NotFound(_response);
                }

              //  var imageDeleted = await _imageService.DeleteImageAsync(workoutImage.ImageUrl);

                _db.WorkoutImages.Remove(workoutImage);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
               // _response.Result = imageDeleted ? "Image and record deleted." : "Record deleted, but image not found.";
                _response.Result = "Image deleted Successfully.";
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


        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var images = _db.WorkoutImages
                .Select(w => new
                {
                    w.Id,
                    w.ImageName,
                    w.Type,
                    w.ImageUrl
                })
                .ToList();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = images;
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
