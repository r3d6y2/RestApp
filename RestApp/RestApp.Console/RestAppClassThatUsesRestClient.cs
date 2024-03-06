using System.Threading.Tasks;
using RestApp.Logging;

namespace RestApp.Console
{
    public class RestAppClassThatUsesRestClient
    {
        private IRestClient _3rdPartyRestClient;
        private ILogger _logger;

        public RestAppClassThatUsesRestClient(IRestClient rdPartyRestClient)
        {
            _3rdPartyRestClient = rdPartyRestClient;
        }
        
        public Task<TModel> GetSomething<TModel>(string url)
        {
            return _3rdPartyRestClient.Get<TModel>(url);
        }
    }
}