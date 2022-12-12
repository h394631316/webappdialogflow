using System;

namespace WebAppDialogflow
{
    public class Chatbot
    {
        public Guid Id { get; set; }
        public string LanguageId { get; set; }
        public string DialogFlowBotId { get; set; }
        public EnumBotType EngineType { get; set; }
    }
}
