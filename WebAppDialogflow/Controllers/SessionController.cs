using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAppDialogflow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessionController : Controller
    {
        private readonly ISessionAppService _sessionAppService;
        public SessionController(ISessionAppService sessionAppService)
        {
            _sessionAppService = sessionAppService;
        }

        [HttpPost]
        [Route("getIntentByQuestion")]
        public async Task<DetectIntentReturn> GetIntentByQuestion([FromBody] GetIntentByQuestionInput input, [FromQuery] int siteId)
        {
            return await _sessionAppService.GetIntentByQuestion(input, siteId);
        }
    }
}
