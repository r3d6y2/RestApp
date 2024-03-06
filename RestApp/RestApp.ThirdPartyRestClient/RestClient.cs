using System.Threading.Tasks;

namespace RestApp
{
    public class RestClient : IRestClient
    {
        public Task<TModel> Get<TModel>(string url)
        {
            throw new System.NotImplementedException();
        }

        public Task<TModel> Put<TModel>(string url, TModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<TModel> Post<TModel>(string url, TModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<TModel> Delete<TModel>(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}