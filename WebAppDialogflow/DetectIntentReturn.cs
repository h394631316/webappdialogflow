using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace WebAppDialogflow
{
    public class DetectIntentReturn
    {
        public Google.Cloud.Dialogflow.V2.Intent Intent { get; set; }
        public string IntentId { get; set; }
        public string IntentName { get; set; }
        public decimal IntentDetectionConfidence { get; set; }
        public Dictionary<string,string> Parameters { get; set; }
        public bool AllRequiredParamsPresent { get; set; }
        public string FulfillmentText { get; set; }
        public dynamic AllDetectIntens { get; set; }
        public string ResponseId { get; set; }
    }
}
