namespace Dominus.Backend.HttpClient
{
    public class BaseProxy
    {
        protected string token = "";

        //Production
        protected string serviceAddress = "";

        public BaseProxy(string serviceAddress, string token)
        {
            this.serviceAddress = serviceAddress;
            this.token = token;
        }
    }
}
