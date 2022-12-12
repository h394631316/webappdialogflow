using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public class DetectIntentRequest
    {
        public QueryInput queryInput { get; set; }
    }
    public class QueryInput
    {
        public Text text { get; set; }
        public Event Event { get; set; }
    }
    public class Text
    {
        public string text { get; set; }
        public string languageCode { get; set; }
    }
    public class Event
    {
        public string name { get; set; }
        public string languageCode { get; set; }
    }
}
