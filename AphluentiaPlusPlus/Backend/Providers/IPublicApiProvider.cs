namespace Backend.Providers
{
    public interface IPublicApiProvider
    {
        public Task<(bool, string?)> Get(string endpoint);
        public Task<(bool, string?)> Post(string endpoint, object body);
        public Task<(bool, string?)> Put(string endpoint, object body);
        public Task<(bool, string?)> Delete(string endpoint);
    }
}
