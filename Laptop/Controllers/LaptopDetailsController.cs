using Laptop.Interface;
using Laptop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Laptop.Controllers
{
    [EnableRateLimiting("Fixedwindow")]
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopDetailsController : ControllerBase
    {
        private readonly ILaptopRepository _laptopRepository;
        private readonly ILogger <LaptopDetailsController>_logger;

        public LaptopDetailsController(ILaptopRepository laptopRepository, ILogger<LaptopDetailsController> logger)
        {
            _laptopRepository = laptopRepository ?? throw new ArgumentNullException(nameof(laptopRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("GetAllEquipment")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Create Begins");
                var data = await _laptopRepository.GetLaptopDetails();
                if (data == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }

        [HttpGet("GetEquipmentByName/{Name}")]
        public async Task<IActionResult> GetById(string Name)
        {
            var data = await _laptopRepository.GetEquipmentByName(Name);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [HttpGet("GetEquipmentById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _laptopRepository.GetLaptopById(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [HttpPost("AddEquipment")]
        public async Task<IActionResult> Add(EquipmentModel model)
        {
            var data = await _laptopRepository.Create(model);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [HttpPut("UpdateEquipment")]
        public async Task<IActionResult> Updates(EquipmentModel model, int id)
        {
            var data = await _laptopRepository.Update(model, id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [HttpDelete("DeleteEquipment")]
        public async Task<IActionResult> Remove(int id )
        {
            var data = await _laptopRepository.Delete(id);
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }

        [HttpGet("GetAllBanks")]
        public async Task<IActionResult> GetBanks()
        {
            var data = await _laptopRepository.GetAllBanks();
            if (data == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
        }
    }
}
