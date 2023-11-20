namespace Data.Base
{
    public class IdentityMapManager : IIdentityMapManager
    {
        private readonly Dictionary<string, object> _objectsMapped;

        public IdentityMapManager()
        {
            _objectsMapped = new();
        }

        public TReturn GetObjet<TReturn>(string key)
        {
            return (TReturn)_objectsMapped[key];
        }

        public void MapObject(string key, object value)
        {
            if (!_objectsMapped.ContainsKey(key))
                _objectsMapped[key] = value;
        }

        public bool HasObject(string key)
        {
            return _objectsMapped.ContainsKey(key);
        }
    }

    public interface IIdentityMapManager
    {
        void MapObject(string key, object value);
        bool HasObject(string key);
        TReturn GetObjet<TReturn>(string key);
    }
}
