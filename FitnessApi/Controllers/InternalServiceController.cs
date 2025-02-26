using FitnessApi.Dto.InternalService;
using FitnessApi.IRepository;
using FitnessApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FitnessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalServiceController : ControllerBase
    {
        private readonly IInternalServiceRepository _internalServiceRepository;
        public InternalServiceController(IInternalServiceRepository internalServiceRepository)
        {
            _internalServiceRepository = internalServiceRepository;
        }

        [HttpPost("create-internal-service")]
        public async Task<IActionResult> CreateInternalService([FromBody] CreateInternalServiceDto createInternalServiceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            try
            {
                var result = await _internalServiceRepository.CreateInternailServiceAsync(createInternalServiceDto);

                return StatusCode(201, new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = "Successfully Created"
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "unexpected error occurred" }
                });
            }

        }

        [HttpGet("get-servicename/{serviceName}")]
        public async Task<IActionResult> GetByServiceNameServiceName([FromRoute] string serviceName)
        {
            try
            {
                var result = await _internalServiceRepository.GetAllAsync(i => i.ServiceName == serviceName);
                
                return Ok(new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.Created,
                    Result = result
                });
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.OK,
                    ErrorMessages = new List<string> { "unexpected error occurred" }
                });
            }
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllServiceName()
        {
            try
            {
                var result = await _internalServiceRepository.GetAllAsync();

                return Ok(new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = result
                });
            }
            catch (Exception)
            {
                return BadRequest(new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string> { "unexpected error occurred" }
                });
            }
        }


    }
}
