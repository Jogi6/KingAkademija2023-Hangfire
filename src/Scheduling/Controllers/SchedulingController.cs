using Microsoft.AspNetCore.Mvc;
using Scheduling.Services;

namespace Scheduling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulingController : Controller
    {
        #region
        #endregion

        #region Fields
        private readonly ISchedulingService schedulingService;
        #endregion

        #region Constructor
        public SchedulingController(ISchedulingService schedulingService)
        {
            this.schedulingService = schedulingService ?? throw new ArgumentNullException(nameof(schedulingService));
        }
        #endregion

        #region Actions
        [HttpGet]
        public IActionResult ExchangeRateImportFromDate([FromQuery] DateTime date, CancellationToken cancellationToken)
        {

            bool isCompleted = schedulingService.GetExchangeRate(date, cancellationToken);

            if (!isCompleted)
            {
                return BadRequest();
            }

            return Ok(isCompleted);
        }
        #endregion
        

    }
}
