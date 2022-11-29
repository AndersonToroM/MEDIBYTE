namespace Dominus.Backend.HttpClient
{
    public class ProxyBusinessLogic
    {
        public string token = "";

        public string serviceAddress = "http://localhost:5000";

        public ProxyBusinessLogic(string token, string serviceAddress = "")
        {
            this.token = token;
            this.serviceAddress = serviceAddress;
        }
    }
}
