namespace VehicleHotSpotBackend.Core
{
    public class InMemoryStorage
    {
        private IDictionary<string, Guid> _tokens;

        public InMemoryStorage()
        {
            _tokens = new Dictionary<string, Guid>();
        }

        public void AddToken(string token, Guid userId)
        {
            lock (_tokens)
            {
                if (_tokens.ContainsKey(token))
                {
                    _tokens[token] = userId;
                }
                else
                {
                    _tokens.Add(token, userId);
                }
            }
        }

        public Guid GetUserId(string token)
        {
            var userId = Guid.Empty;
            lock (_tokens)
            {
                _tokens.TryGetValue(token, out userId);
            }

            return userId;
        }
    }
}