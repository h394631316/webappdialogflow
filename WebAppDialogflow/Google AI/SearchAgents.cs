using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public class SearchAgents
    {
        public List<Agent> agents { get; set; }
        public string nextPageToken { get; set; }
    }
}
