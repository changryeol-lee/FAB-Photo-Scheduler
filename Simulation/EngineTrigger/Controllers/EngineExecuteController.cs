using FabSchedulerModel.ModelConfig;
using Microsoft.AspNetCore.Mvc;

namespace EngineTrigger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EngineExecuteController : ControllerBase
    {
        [HttpPost("execute-engine")]
        public IActionResult RunSimulation([FromBody] PhotoSimulationOption options)
        {
            SimulationRunner runner = new SimulationRunner();
            runner.RunSimulation(options); 

            return Ok("Simulation Completed Successfully");
        }

    }
}