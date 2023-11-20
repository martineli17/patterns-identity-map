using Api.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClientController(IClientRepository clientRepository, IUnitOfWork unitOfWork)
        {
            _clientRepository = clientRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientAddDto clientAddDto, CancellationToken cancellationToken)
        {
            var client = new Client(clientAddDto.Name);
            await _clientRepository.AddAsync(client, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            await _clientRepository.GetAllAsync(cancellationToken);
            var result = await _clientRepository.GetAllAsync(cancellationToken);

            return Ok(result);
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName([FromQuery] string name, CancellationToken cancellationToken)
        {
            await _clientRepository.GetByNameAsync(name, cancellationToken);
            var result =  await _clientRepository.GetByNameAsync(name, cancellationToken);

            return Ok(result);
        }
    }
}
