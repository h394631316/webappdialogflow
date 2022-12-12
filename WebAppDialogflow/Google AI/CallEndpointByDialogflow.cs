using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public class CallEndpointByDialogflow
    {
        public string id { get; set; }
        public string timestamp { get; set; }
        public string lang { get; set; }
        public Result result { get; set; }
        public Status status { get; set; }
        public string sessionId { get; set; }
    }
    public class Result
    {
        public string source { get; set; }
        public string resolvedQuery { get; set; }
        public string action { get; set; }
        public bool actionIncomplete { get; set; }
        public Parameter parameters { get; set; }
        public Context[] contexts { get; set; }
        public Metadata metadata { get; set; }
        public Fulfillment fulfillment { get; set; }
        public decimal score { get; set; }
    }
    public class Parameter
    {

    }
    public class Context
    {

    }
    public class Metadata
    {
        public string intentId { get; set; }
        public string webhookUsed { get; set; }
        public string webhookForSlotFillingUsed { get; set; }
        public string intentName { get; set; }
    }
    public class Fulfillment
    {
        public string speech { get; set; }
        public Message[] messages { get; set; }
    }
    public class Status
    {
        public int code { get; set; }
        public string errorType { get; set; }
        public bool webhookTimedOut { get; set; }
    }
}
