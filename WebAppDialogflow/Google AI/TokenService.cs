using AttackPrevent.Business;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace intent_library.Google_AI
{
    public interface ITokenService
    {
        //string GetRefreshToken(string code,string clientId,string clientSecret,string redirectUri);
        //string GetAccessToken(string clientId, string clientSecret, string refreshToken);
        Task<string> GetToken(string key);
    }
    public class TokenService: ITokenService
    {
        private ConcurrentDictionary<string, string> credentialJsonDic = new ConcurrentDictionary<string, string>();
        private string GetRefreshToken(string code, string clientId, string clientSecret, string redirectUri)
        {
            string tokenUrl = string.Format("https://accounts.google.com/o/oauth2/token");
            WebRequest request = WebRequest.Create(tokenUrl);
            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";
            var post = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code",
                                      code,
                                      clientId,
                                      clientSecret,
                                      redirectUri);

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(post);
            }
            var resonseJson = "";
            using (var response = request.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    resonseJson = sr.ReadToEnd();
                }
            }
            string refreshToken = JsonConvert.DeserializeAnonymousType(resonseJson, new { refresh_token = "" }).refresh_token;

            return refreshToken;

        }
        private string GetAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            string tokenUrl = string.Format("https://accounts.google.com/o/oauth2/token");
            WebRequest request = WebRequest.Create(tokenUrl);
            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";
            var post = string.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token",
                                      clientId,
                                      clientSecret,
                                      refreshToken
                                      );

            using (var sw = new StreamWriter(request.GetRequestStream()))
            {
                sw.Write(post);
            }
            var resonseJson = "";
            using (var response = request.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    resonseJson = sr.ReadToEnd();
                }
            }
            string accessToken = JsonConvert.DeserializeAnonymousType(resonseJson, new { access_token = "" }).access_token;

            return accessToken;
        }
        public async Task<string> GetToken(string key)
        {
            //GoogleCredential credential;
            //using (var stream = new FileStream(@"dialogflowkey\Comm100Bot008-60929b4ebabb.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = GoogleCredential.FromStream(stream)
            //        .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            //}

            //return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            if (!credentialJsonDic.ContainsKey(key))
            {
                //string value = "{\"type\": \"service_account\",  \"project_id\": \"test-469dc\",  \"private_key_id\": \"b5a6d4d04f059b1b043d0c0cb557782b1704bdd9\",\"private_key\": \"-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDSO6c4Ph3UKcHz\ncxBGchthTvmPerFvgbMNOTu2KXBdLA+CPkZG83zLn3segVfFecudHRnzNjCvo8zK\npDGrDmCx+XL1z0SATv95jkrAAnrQdR+aYrsZ7TvUMVIEYw38P9zxixyCBhMM7ZII\n0GnUQ3r6CRpDKtFpKHL/VuZ2kVj81LDLsxUTfaFF7qUsUbcg81A7VNiYBUTvLR2R\npXwSOsBvdZeTm/G9v0EYea78vbMm3c3VfJpRBijI8HH1/cl0ck16X7WedKbTU3AO\nL/JWAgKh0Z07OW5bI76nU8TTcatoU/kr+nAEoYw8RKfVECa9rB9/ceaWjO2iCflG\ndASBSrMjAgMBAAECggEALO11o3XW3w/bdIDrAfsBse50M77nK2q+zn0kLx409pxj\nZhST5TbEvSlEYPUIQlnwcw+Ui57Rkb+CFO449RLaBD7IXBKi037OZvZLMDR3dZQK\nIG6oCHbj2cl+exSGGRm2yGHPwlO0/4bHth1/+E5Beiy20ZLVdNEhnkK+dqlCKKmR\nzLxTWt77Vlc+XtngW2fmdiK0yhUM2uD1V617adFfOH+tMGluWjcYTXsvshaB0zim\nPY713A32URY20xyBmRbgDkNzX2xnIfMOsAujMejkpFiApLSIXUC0kXRqm31i+AC1\n3hqrS+4cWQDAMjiKCr2L2ReaGDyf16Kb/hYzpeSs3QKBgQD/ra4buwgI3cX3CMiY\nMC8mdUR6vP/5cdcYmH2FEVuNnJf4ynswq8ymZ8H9Q9oe7pK7mtIg+Qau1prq9p8I\nJdIRw1nmTHx+Se1hGJ33fhpnUz9dnDq0AxHC0vLj7KO8gRZCAfn4cnn6aDQnidMs\nidmKMnrXeY1hGaLDg78GpPet7wKBgQDSf1dYNSFDdevRZ8+B9+roLjZCZ7H5LGuc\niVAfAndNeeWWFV2SODSimCkZo8q2buHvDF8yAvEqp0gogC6bCg3yj8v+Ctxfuz/4\n/JChyhyovxUIGKKtCopjPyJtxE3GJz/NNhxVoo+t2OgOZp/QT+M+Eqz9smAj8owo\nYIngn1UCDQKBgQD7tfkaw979G5ixw6v4aDeDHR/11ewLn8+rHzUztAr2N5xGCtGM\n8RXhHXUV6Z7rpORAXgNRAtlZd128xF98nf9aDYCMAfDuQ8m3NY0PAs7rukE/33bL\nkdoCe7s0ONxZ0JwhD+EQf2Mm6z+tTTrhNjU8oMXfLIPLfHmJEkMWP4K3cQKBgGTy\nRqv5hsM4WOfqtdvpTdEc4E6GNhZ90/mU7ESCxT31eEG6a05FABD9kbBHGV7V3Cns\nr/Zm7Mr8wCEUH8WgU8IAbd6dzMNoJCy+yomcSJQwVuC0F8eOZ4Fi9JrZakbd6b36\n9imnNOo1PEwzsO6QcTUCdc2QboSuu8JdUaHUJ0s9AoGAaE1/vFt76gLs4S1TP3zz\ngzBbeF1M1bc5+A4pGdO7EEm/K77spBKEeCfsdzb5WgwIlTafp5U9Wulzac1JIIfC\nMMqeajdgXMN8fDxLtattkYpSff4ITsce+Neh8mzP/Et4c+xXSnc1GnwfOVRZaNN1\nGxZdceQ5Btlj08ys6ent8IA=\n-----END PRIVATE KEY-----\n\",  \"client_email\": \"test-18@test-469dc.iam.gserviceaccount.com\",  \"client_id\": \"116091150968188969033\",  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",  \"token_uri\": \"https://accounts.google.com/o/oauth2/token\",  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/test-18%40test-469dc.iam.gserviceaccount.com\"}";
                //string value = "{  \"type\": \"service_account\",  \"project_id\": \"test456-4faa7\",  \"private_key_id\": \"3e357d61becdc6fe6bbfbc9105c0afe0edf50846\",\"private_key\": \"-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDN+lNkE8rO47rW\nSbKaW55ijfK5yvMUY5RLrHtQUG0ApStiRCgmLQfWMxEO4s+XQdtQBr0NCe9e5jYo\nm0o7oPLwDgDekqT2PZVSOHKRLp8ZyWEjjjK2m7hdwMbR3j2FaMgmt72JYg8hCmos\n4iY5k3Qk+q5U/F1AjG2EDzRgWqyCMibEeoiDJyi0FGjyCpt6Jku36DxIpvgBnaJ+\nz2k1QVS3UATlU3AxhK72L4TMk0Zc3Wr1MeF9qw7Pmsv+2nDgNs0lRiT6BxxVNncX\nlJvboFBb4TAjM55a9iUyKoeSOzFS0K+TJJXaomOldRzn2+2SqneQzK3hSfl97nae\nyuWYwOGrAgMBAAECggEAKJZ1LDlCM3ZocKVhDk2TbjQuajH1qETfCfJY8kCG2iOx\n4tEvYCZrbuftKpPC+Dk/8AsD4bfddUPD1EiVXh6fAHKh9TnMS8Fi1SrOWJFpRHmM\nLKduktEoiUDdbKlwUgONvSNjs9igxo/40BNYGOxXbJoBFDoOXhPN2MHuPjZf29+P\nhCtqVzBRgUT7ve0nxO7TJuRHQ3ufIIhFVvOTN7E9UJHfUXAdjx8rKj62UiIZmHEE\n7h2Ax8kmOBQCI8zmPYOPq42J1CQrO7TcTXj9a+rLrseYKpm1o16rJGVksxGA+rg7\nzT4MPfaG3/FMxEWj0s7w3w6D7QiOD37IPWDEH0ObcQKBgQDqGjZtZsfxzLr/1HO6\ngW9GvWF2DM7RsyxF6eInAIRGCyM58QnbHh/U1qqhrlDAiGNn+d6mUfjRMiqD6mLy\ntkCTpC/vg2b8PydQNSGD+LyJ3+S2IxoecL/Zg0L2uOkRFA/U2lwV3BrlqoaFrdei\nLEjf3PXFSLCZM41kWVzzVRLp5QKBgQDhPqVZgMjR7mWP6W+YQeQs8sRXyEGPAsTS\nJnPRG6IJrkrY7HLVwUVTu1V1UbaTGdizbXW+7nF1Is5mItaGj5Srd4wrrUxxca5p\nzUrLWYxw70syUJ/6Kh3nE/QxA2HTPxYLk8GIR73pIu6n7SxL3ebF2f3w+HSuWrJu\nmwv9IISkTwKBgED453Qzm3iblqbcQwZXWBftBbiq01fV/4qj3/VtKkj6XFt/b3nW\nAYWj0pWu+JCHCffYnJfSllkRMEWObsnoFUIkxn0WHiYSLQoQxO2IwmMNtb9rPJuI\nNWbcDGDHb0AjLkWoGPikLv7g9hbx6bAD1HH1NGz9wK20ZiCcwLR0PxyVAoGBAMww\nsU6xC9wbxKAwm0SovwPsVAfL93qApRJpX2a6KSujgCCkQPn8Ci/8zFzdZah2rtcB\nrNMYb2wj0H4QqO81LC5A4qYZpQnWZ6lsPJiZplIe68lCA7SSB6ealCz2hvEs6Hw4\nRyKW/tdhHn7G5/giwnirxKR1Z7lDaSwRjE288+HLAoGATHxTJFA0bOIKxH7j76fW\nbxqa61IW5xeTPAGLVA4yJOiTcBAxvzfaRac5goJwpNi4ECzNeFcJsuAlHJ2lOcvL\nlSJOYdYM1kvMoLaEfkikWx5lieYKGm+lpyMEvIgLA8YSioEjc/6HBVLROyG/415i\n8teoV/ZkGuX0TezIhwR71y0=\n-----END PRIVATE KEY-----\n\",\"client_email\": \"test456-4faa7@appspot.gserviceaccount.com\",\"client_id\": \"110865071412740333488\",\"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\"token_uri\": \"https://accounts.google.com/o/oauth2/token\",\"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/test456-4faa7%40appspot.gserviceaccount.com\"}";
                //string value = Utils.GetFileContext("dialogflowkey/test456-3df99d51602a.json");
                string[] arr = key.Split('_');
                string siteId = arr[0];

                string value = Utils.GetFileContext(string.Format("dialogflowkey/{0}.json", key));
                //string value = Utils.GetResourceContext(this.GetType(), string.Format("VPO.{0}.json", siteId));
                credentialJsonDic.TryAdd(key, value);
            }
            string json = credentialJsonDic[key];

            GoogleCredential credential = GoogleCredential.FromJson(json).CreateScoped("https://www.googleapis.com/auth/cloud-platform");

            string strResult = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            return strResult;
        }
    }
//    public class SerTokenService : ITokenService
//    {
//        public async Task<string> GetToken(string key)
//        {
//            try
//            {
//                string[] scopes = new string[] {
//"https://www.googleapis.com/auth/cloud-platform"
// };

//                var certificate = new X509Certificate2("test456-4afc95ce4a98.p12", "notasecret", X509KeyStorageFlags.Exportable);

//                ServiceAccountCredential credential = new ServiceAccountCredential(
//                     new ServiceAccountCredential.Initializer("116827338551944877416")
//                     {
//                         Scopes = scopes,
                         
//                     }.FromCertificate(certificate));
//                return await credential.GetAccessTokenForRequestAsync();

//            }
//            catch (Exception e)
//            {

//            }
//            return "";
//        }
//    }
}
