using System;

namespace WebAppDialogflow
{
    public class GetIntentByQuestionInput
    {
        public Chatbot Bot { get; set; }
        public Guid SessionId { get; set; }
        public string Question { get; set; }
        public string IntentName { get; set; }
        public bool IsPrompt { get; set; }
    }
}
