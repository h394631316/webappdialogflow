using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public class Usersay
    {
        public string id { get; set; }
        public Data[] data { get; set; }
        public bool isTemplate { get; set; }
        public int count { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime updated { get; set; }
    }
    public class Data
    {
        public string text { get; set; }
        public bool userDefined { get; set; }
    }

    
}
