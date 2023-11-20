using Microsoft.Extensions.Logging;

namespace Data.Base
{
    public class IdentityMapQueryCommand : IIdentityMapQueryCommand
    {
        private readonly IIdentityMapManager _identityMap;
        private readonly ILogger<IdentityMapQueryCommand> _logger;
        public IdentityMapQueryCommand(IIdentityMapManager identityMap, ILogger<IdentityMapQueryCommand> logger)
        {
            _identityMap = identityMap;
            _logger = logger;
        }

        public async Task<TReturn> ExecuteAsync<TReturn>(string key, Func<Task<TReturn>> func)
        {
            if (_identityMap.HasObject(key))
            {
                _logger.LogInformation("Getting the data {0} from Identity Map", key);
                return _identityMap.GetObjet<TReturn>(key);
            }

            var result = await func();
            _logger.LogInformation("Getting the data {0} from DataBase", key);
            _identityMap.MapObject(key, result);

            return result;
        }
    }

    public interface IIdentityMapQueryCommand
    {
        Task<TReturn> ExecuteAsync<TReturn>(string key, Func<Task<TReturn>> func);
    }
}
