using System;
using System.Net;
using System.Threading.Tasks;
using RestApp.Logging;

namespace RestApp
{
    public class RestAppClient : IRestClient
    {
        private readonly IRestClient _restClient;
        private readonly int _retryMaxCount;
        private readonly int _delayMilliseconds;

        private readonly ILogger _logger;

        public RestAppClient(IRestClient restClient, ILogger logger, int retryMaxCount, int delayMilliseconds)
        {
            _restClient = restClient;
            _logger = logger;
            _retryMaxCount = retryMaxCount;
            _delayMilliseconds = delayMilliseconds;
        }

        public async Task<TModel> Get<TModel>(string url)
        {
            return await RetryAsync(() => _restClient.Get<TModel>(url));
        }

        public async Task<TModel> Put<TModel>(string url, TModel model)
        {
            return await RetryAsync(() => _restClient.Put(url, model));
        }

        public async Task<TModel> Post<TModel>(string url, TModel model)
        {
            return await RetryAsync(() => _restClient.Post(url, model));
        }

        public async Task<TModel> Delete<TModel>(int id)
        {
            return await RetryAsync(() => _restClient.Delete<TModel>(id));
        }
        
        private async Task<TModel> RetryAsync<TModel>(Func<Task<TModel>> action)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (WebException ex)
                {
                    if (retryCount >= _retryMaxCount)
                    {
                        throw;
                    }
                    _logger.Error(ex);
                    
                    await Task.Delay(_delayMilliseconds);
                    retryCount++;
                }
            }
        }
    }
}