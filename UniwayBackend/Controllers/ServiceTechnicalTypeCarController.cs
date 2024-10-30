using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;
using UniwayBackend.Models.Entities;
using UniwayBackend.Models.Payloads.Base.Response;
using UniwayBackend.Models.Payloads.Core.Request.ServiceTechnicalTypeCar;
using UniwayBackend.Models.Payloads.Core.Response.ServiceTechnicalTypeCar;
using UniwayBackend.Repositories.Base;

namespace UniwayBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceTechnicalTypeCarController : ControllerBoundPropertyDescriptor
    {
        private readonly IBaseRepository<ServiceTechnicalTypeCar, int> _serviceTechnicalTypeCarRepository;
        private readonly IMapper _mapper;
        private readonly UtilitariesResponse<ServiceTechnicalTypeCarResponse> _utilitaries;
        private readonly ILogger<ServiceTechnicalTypeCarController> _logger;

        public ServiceTechnicalTypeCarController(IBaseRepository<ServiceTechnicalTypeCar, int> serviceTechnicalTypeCarRepository,
                                                 IMapper mapper,
                                                 UtilitariesResponse<ServiceTechnicalTypeCarResponse> utilitaries,
                                                 ILogger<ServiceTechnicalTypeCarController> logger)
        {
            _serviceTechnicalTypeCarRepository = serviceTechnicalTypeCarRepository;
            _mapper = mapper;
            _utilitaries = utilitaries;
            _logger = logger;
        }

        [HttpPost("SaveAll")]
        public async Task<ActionResult<MessageResponse<ServiceTechnicalTypeCarResponse>>> SaveAll([FromBody] List<ServiceTechnicalTypeCarRequest> serviceTechnicalTypeCarRequests)
        {
            MessageResponse<ServiceTechnicalTypeCarResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                var servicesTypeCar = _mapper.Map<List<ServiceTechnicalTypeCar>>(serviceTechnicalTypeCarRequests);

                var result = await _serviceTechnicalTypeCarRepository.InsertAll(servicesTypeCar);

                var mapresult = _mapper.Map<List<ServiceTechnicalTypeCarResponse>>(result);

                response = _utilitaries.setResponseBaseForList(mapresult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<ServiceTechnicalTypeCarResponse>()
                    .Code(401).Message(ex.Message).Build();
            }
            return response;
        }

        [HttpPut("Update")]
        public async Task<ActionResult<MessageResponse<ServiceTechnicalTypeCarResponse>>> Update([FromBody] ServiceTechnicalTypeCarRequest request)
        {
            MessageResponse<ServiceTechnicalTypeCarResponse> response;
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().Name);

                if (request.Id == 0) return _utilitaries.setResponseBaseForNotFount();

                var entityMap = _mapper.Map<ServiceTechnicalTypeCar>(request);

                var result = await _serviceTechnicalTypeCarRepository.UpdateAndReturn(entityMap);

                var mapresult = _mapper.Map<ServiceTechnicalTypeCarResponse>(result);

                response = _utilitaries.setResponseBaseForObject(mapresult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response = new MessageResponseBuilder<ServiceTechnicalTypeCarResponse>()
                    .Code(401).Message(ex.Message).Build();
            }
            return response;
        }
    }
}
