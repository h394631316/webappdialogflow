using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAppDialogflow
{
    public interface ISessionAppService
    {
        Task<DetectIntentReturn> GetIntentByQuestion(GetIntentByQuestionInput input, int siteId);
    }
}
