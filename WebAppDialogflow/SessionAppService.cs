using Google.Cloud.Dialogflow.V2;
using intent_library.Google_AI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using intent_library;
using System.Collections.Generic;

namespace WebAppDialogflow
{
    public class SessionAppService : ISessionAppService
    {
        private readonly IDialogFlowService _dialogFlowService;
        public SessionAppService(IDialogFlowService dialogFlowService)
        {
            _dialogFlowService = dialogFlowService;
        }

        public async Task<DetectIntentReturn> GetIntentByQuestion(GetIntentByQuestionInput input, int siteId)
        {
            string authKey = string.Format("{0}_{1}_CertJson", siteId, input.Bot.Id);
            string projectId = input.Bot.DialogFlowBotId;

            string question = input.Question;
            string language = input.Bot.LanguageId;

            intent_library.Google_AI.DetectIntentRequest detectIntentRequest = new intent_library.Google_AI.DetectIntentRequest
            {
                queryInput = new intent_library.Google_AI.QueryInput
                {
                    text = new Text
                    {
                        text = question,
                        languageCode = language,
                    }
                },
            };

            DetectIntentResponse detectIntentResponse = await _dialogFlowService.detectIntent(authKey, projectId, detectIntentRequest);

            var result = new DetectIntentReturn();

            if (detectIntentResponse.QueryResult.Intent != null
                && detectIntentResponse.ResponseId != null)
            {
                result.IntentDetectionConfidence = (decimal)detectIntentResponse.QueryResult.IntentDetectionConfidence;
                result.ResponseId = detectIntentResponse.ResponseId;
                result.Intent = detectIntentResponse.QueryResult.Intent;
                result.IntentName = detectIntentResponse.QueryResult.Intent.DisplayName;
                result.AllRequiredParamsPresent = detectIntentResponse.QueryResult.AllRequiredParamsPresent;

                //追问
                result.FulfillmentText = detectIntentResponse.QueryResult.FulfillmentText;
                string name = detectIntentResponse.QueryResult.Intent.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    string[] splitName = name.Split('/');
                    if (splitName.Length > 4)
                    {
                        result.IntentId = splitName[4];
                    }
                }
            }

            //prompt fields
            if (detectIntentResponse.QueryResult.Parameters != null)
            {
                Dictionary<string, string> parameterDic = new Dictionary<string, string>();
                foreach(var item in detectIntentResponse.QueryResult.Parameters.Fields)
                {
                    string key = item.Key;
                    string value = item.Value.ToString();
                    if (!parameterDic.ContainsKey(key))
                    {
                        parameterDic.Add(key,value);
                    }
                }

                result.Parameters = parameterDic;
            }

            return result;
        }
    }
}
