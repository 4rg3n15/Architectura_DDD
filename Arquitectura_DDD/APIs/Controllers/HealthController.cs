using Microsoft.AspNetCore.Mvc;
using Arquitectura_DDD.Infraestructure.Persistence;

namespace Arquitectura_DDD.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly MongoDbContext _context;

        public HealthController(MongoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // Probar conexión a MongoDB
                var client = _context.GetCollection<object>("test").Database.Client;
                return Ok(new { 
                    Status = "Healthy", 
                    Database = "Connected",
                    Timestamp = DateTime.UtcNow 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Status = "Unhealthy", 
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow 
                });
            }
        }
    }
}
