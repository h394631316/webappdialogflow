using Google.Cloud.Dialogflow.V2;
using Google.LongRunning;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public interface IDialogFlowService:IHttpWebRequestMethod
    {
        Task<Agent> getAgentAsync(string key,string projectId);
        Task<Operation> agentExportAsync(string key,string projectId);
        Task<Operation> agentImportAsync(string key, string projectId, string agentContent);
        Task<Operation> agentRestoreAsync(string key, string projectId, string agentContent);
        Task<SearchAgents> agentSearchAsync(string key, string projectId, string pageToken);
        Task<Operation> agentTrainAsync(string key, string projectId);

        Task<Operation> entityTypesEntitiesBatchCreateAsync(string key, string projectId, string json);
        Task<Operation> entityTypesEntitiesBatchDeleteAsync(string key, string projectId, string json);
        Task<Operation> entityTypesEntitiesBatchUpdateAsync(string projectId);

        Task<Operation> agentEntityTypesBatchDeleteAsync(string key, string projectId, string json);
        Task<Operation> agentEntityTypesBatchUpdateAsync(string projectId);
        Task<EntityType> agentEntityTypesCreateAsync(string projectId);
        Task<string> agentEntityTypesDeleteAsync(string projectId);
        Task<EntityType> agentEntityTypesGetAsync(string projectId);
        Task<string> agentEntityTypesListAsync(string key, string projectId, string pageToken = "");
        Task<EntityType> agentEntityTypesPatchAsync(string projectId);

        Task<Operation> projectsOperationsGet(string key, string name);

        Task<DetectIntentResponse> detectIntent(string key, string name, Google_AI.DetectIntentRequest detectIntentRequest);

        Task<string> agentIntentsListAsync(string key, string projectId, string pageToken = "");
    }
    public class DialogFlowService : HttpWebRequestMethod, IDialogFlowService
    {
        public DialogFlowService(ITokenService tokenService)
            :base(tokenService)
        {
        }

        public async Task<Agent> getAgentAsync(string key, string projectId)
        {
            //GET https://dialogflow.googleapis.com/v2/{parent=projects/*}/agent
            Agent agent = new Agent();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent";
            uriString = string.Format(uriString, projectId);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage() {
                 RequestUri=new Uri(uriString)
            };
            HttpResponseMessage httpResponseMessage = await GetAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
                agent = JsonConvert.DeserializeObject<Agent>(strResult, new MatchModeStringEnumConverter());
            }
            return agent;
        }
        public async Task<Operation> agentExportAsync(string key, string projectId)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*}/agent:export
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:export";
            uriString = string.Format(uriString, projectId);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString)
            };
            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
                dynamic dynamicObj = JsonConvert.DeserializeObject<dynamic>(strResult);
                // var byteStr = dynamicObj.response.agentContent;
                // byte[] buff = Convert.FromBase64String(byteStr);
                // string agentContent = Convert.ToBase64String(buff);

                ByteString byteString = ByteString.FromBase64(dynamicObj.response.agentContent.ToString());
                FileStream fs = new FileStream(@"d:\file_export_ok.zip", FileMode.OpenOrCreate);
                byteString.WriteTo(fs);
                fs.Close();


                operation = JsonConvert.DeserializeObject<Operation>(strResult);

            }
            return operation;
        }
        public async Task<Operation> agentImportAsync(string key, string projectId, string agentContent)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*}/agent:import
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:import";
            uriString = string.Format(uriString, projectId);
            string json = JsonConvert.SerializeObject(new { agentContent });
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key, 120);
            string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
            operation = JsonConvert.DeserializeObject<Operation>(strResult);

            return operation;
        }
        public async Task<Operation> agentRestoreAsync(string key, string projectId, string agentContent)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*}/agent:restore
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:restore";
            uriString = string.Format(uriString, projectId);
            string json = JsonConvert.SerializeObject(new { agentContent });
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key, 120);
            string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
            operation = JsonConvert.DeserializeObject<Operation>(strResult);

            return operation;
        }
        public async Task<SearchAgents> agentSearchAsync(string key, string projectId, string pageToken)
        {
            //GET https://dialogflow.googleapis.com/v2/{parent=projects/*}/agent:search
            SearchAgents searchAgents = new SearchAgents();
            string uriString = "";
            if (!string.IsNullOrEmpty(pageToken))
            {
                uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:search?pageSize=500pageToken={1}";
                uriString = string.Format(uriString, projectId, pageToken);
            }
            else
            {
                uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:search?pageSize=500";
                uriString = string.Format(uriString, projectId);
            }
    
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString)
            };
            HttpResponseMessage httpResponseMessage = await GetAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
                searchAgents = JsonConvert.DeserializeObject<SearchAgents>(strResult, new MatchModeStringEnumConverter());
            }
            return searchAgents;
        }
        public async Task<Operation> agentTrainAsync(string key, string projectId)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*}/agent:train
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:train";
            uriString = string.Format(uriString, projectId);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString)
            };

            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key, 120);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();

                operation = JsonConvert.DeserializeObject<Operation>(strResult);

            }
            return operation;
        }

        public async Task<Operation> entityTypesEntitiesBatchCreateAsync(string key, string projectId, string json)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*/agent/entityTypes/*}/entities:batchCreate
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent:train";
            uriString = string.Format(uriString, projectId);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString),
                Content=new StringContent(json, Encoding.UTF8, "application/json") 
            };

            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();

                operation = JsonConvert.DeserializeObject<Operation>(strResult);

            }
            return operation;
        }

        public async Task<Operation> entityTypesEntitiesBatchDeleteAsync(string key, string projectId, string json)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*/agent/entityTypes/*}/entities:batchDelete
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/entityTypes/entities:batchDelete";
            uriString = string.Format(uriString, projectId);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key);
           
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();

                operation = JsonConvert.DeserializeObject<Operation>(strResult);

            return operation;
        }
        public async Task<Operation> entityTypesEntitiesBatchUpdateAsync(string projectId)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*/agent/entityTypes/*}/entities:batchUpdate
            Operation operation = new Operation();
            return operation;
        }

        public async Task<Operation> agentEntityTypesBatchDeleteAsync(string key, string projectId, string json)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*/agent}/entityTypes:batchDelete
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/entityTypes:batchDelete";
            uriString = string.Format(uriString, projectId);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key, 120);
       
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();

                operation = JsonConvert.DeserializeObject<Operation>(strResult);

            
            return operation;
        }
        public async Task<Operation> agentEntityTypesBatchUpdateAsync(string projectId)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*/agent}/entityTypes:batchUpdate
            Operation operation = new Operation();
            return operation;
        }
        public async Task<EntityType> agentEntityTypesCreateAsync(string projectId)
        {
            //POST https://dialogflow.googleapis.com/v2/{parent=projects/*/agent}/entityTypes
            EntityType entityType = new EntityType();
            return entityType;
        }
        public async Task<string> agentEntityTypesDeleteAsync(string projectId)
        {
            //DELETE https://dialogflow.googleapis.com/v2/{name=projects/*/agent/entityTypes/*}
            string strResult = "";
            return strResult;
        }
        public async Task<EntityType> agentEntityTypesGetAsync(string projectId)
        {
            //GET https://dialogflow.googleapis.com/v2/{name=projects/*/agent/entityTypes/*}
            EntityType entityType = new EntityType();
            return entityType;
        }
        public async Task<string> agentEntityTypesListAsync(string key, string projectId, string pageToken="")
        {
            List<EntityType> entityTypeList = new List<EntityType>();
            string json = "";
            //GET https://dialogflow.googleapis.com/v2/projects/{0}/agent/entityTypes?pageSize=500
            string uriString = "";
            if (!string.IsNullOrEmpty(pageToken))
            {
                uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/entityTypes?pageSize=500&pageToken={1}";
                uriString = string.Format(uriString, projectId, pageToken);
            }
            else
            {
                uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/entityTypes?pageSize=500";
                uriString = string.Format(uriString, projectId);
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString)
            };
            HttpResponseMessage httpResponseMessage = await GetAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
                //entityTypeList = JsonConvert.DeserializeObject<List<EntityType>>(strResult, new MatchModeStringEnumConverter());
                json = strResult;
            }  
            return json;
        }
        public async Task<EntityType> agentEntityTypesPatchAsync(string projectId)
        {
            //PATCH https://dialogflow.googleapis.com/v2/{entityType.name=projects/*/agent/entityTypes/*}
            EntityType entityType = new EntityType();
            return entityType;
        }

        public async Task<Operation> projectsOperationsGet(string key, string name)
        {
            //GET https://dialogflow.googleapis.com/v2/{name=projects/*/operations/*}
            Operation operation = new Operation();
            string uriString = "https://dialogflow.googleapis.com/v2/{0}";
            uriString = string.Format(uriString, name);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString)
            };
            HttpResponseMessage httpResponseMessage = await GetAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
                operation = JsonConvert.DeserializeObject<Operation>(strResult, new MatchModeStringEnumConverter());
            }
            return operation;
        }

        public async Task<DetectIntentResponse> detectIntent(string key, string projectId, Google_AI.DetectIntentRequest detectIntentRequest)
        {
            DetectIntentResponse detectIntentResponse = new DetectIntentResponse();
            string uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/sessions/{1}:detectIntent";
            uriString = string.Format(uriString, projectId,Guid.NewGuid().ToString());
            //string json = JsonConvert.SerializeObject(detectIntentRequest);
            string json = JsonConvert.SerializeObject(detectIntentRequest, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            HttpResponseMessage httpResponseMessage = await PostAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
                detectIntentResponse = JsonConvert.DeserializeObject<DetectIntentResponse>(strResult);
            }
            return detectIntentResponse;
        }

        public async Task<string> agentIntentsListAsync(string key, string projectId, string pageToken = "")
        {
            List<EntityType> entityTypeList = new List<EntityType>();
            string json = "";
            //GET https://dialogflow.googleapis.com/v2/projects/{0}/agent/intents?intentView=1&pageSize=500
            string uriString = "";
            if (!string.IsNullOrEmpty(pageToken))
            {
                uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/intents?intentView=1&pageSize=500&pageToken={1}";
                uriString = string.Format(uriString, projectId, pageToken);
            }
            else
            {
                uriString = "https://dialogflow.googleapis.com/v2/projects/{0}/agent/intents?intentView=1&pageSize=500";
                uriString = string.Format(uriString, projectId);
            }

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriString)
            };
            HttpResponseMessage httpResponseMessage = await GetAsync(httpRequestMessage, key);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strResult = await httpResponseMessage.Content.ReadAsStringAsync();
                //entityTypeList = JsonConvert.DeserializeObject<List<EntityType>>(strResult, new MatchModeStringEnumConverter());
                json = strResult;
            }
            return json;
        }
    }
}
