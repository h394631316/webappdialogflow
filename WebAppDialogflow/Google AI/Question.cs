using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public class Question
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool auto { get; set; }
        public string[] contexts { get; set; }
        public Response[] responses { get; set; }
        public int priority { get; set; }
        public bool webhookUsed { get; set; }
        public bool webhookForSlotFilling { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime lastUpdate { get; set; }
        public bool fallbackIntent { get; set; }
        public string[] events { get; set; }
    }
    public class Response
    {
        public bool resetContexts { get; set; }
        public string[] affectedContexts { get; set; }
        public string[] parameters { get; set; }
        public Message[] messages { get; set; }
        public DefaultResponsePlatforms defaultResponsePlatforms { get; set; }
        public string[] speech { get; set; }
    }
    public class Message
    {
        public int type { get; set; }
        public string lang { get; set; }
        public string speech { get; set; }
    }
    public class DefaultResponsePlatforms
    {

    }
}
