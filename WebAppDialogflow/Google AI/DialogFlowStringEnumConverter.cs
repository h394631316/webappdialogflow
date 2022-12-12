using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Cloud.Dialogflow.V2.Agent.Types;

namespace intent_library.Google_AI
{
    public class MatchModeStringEnumConverter : Newtonsoft.Json.Converters.StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is MatchMode)
            {
                string correctEnumString = "MATCH_MODE_HYBRID";
                switch (value)
                {
                    case MatchMode.Hybrid: correctEnumString = "MATCH_MODE_HYBRID"; break;
                    case MatchMode.MlOnly: correctEnumString = "MATCH_MODE_MLONLY"; break;
                    case MatchMode.Unspecified: correctEnumString = "MATCH_MODE_UNSPECIFIED"; break;
                }
                writer.WriteValue(correctEnumString);// or something else
                return;
            }

            base.WriteJson(writer, value, serializer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(MatchMode))
            {
                var enumString = reader.Value.ToString();
                string correctEnumString = enumString;
                switch (enumString)
                {
                    case "MATCH_MODE_HYBRID": correctEnumString = MatchMode.Hybrid.ToString(); break;
                    case "MATCH_MODE_ML_ONLY": correctEnumString = MatchMode.MlOnly.ToString(); break;
                    case "MATCH_MODE_UNSPECIFIED": correctEnumString = MatchMode.Unspecified.ToString(); break;
                }
                return (MatchMode)Enum.Parse(typeof(MatchMode), correctEnumString);
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
