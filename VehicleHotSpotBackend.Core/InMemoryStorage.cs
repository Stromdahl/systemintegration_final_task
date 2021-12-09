using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleHotSpotBackend.Core
{
    public class InMemoryStorage
    {

        public InMemoryStorage()
        {
            _tokens = new Dictionary<string, Guid>();
        }
        private IDictionary<string, Guid> _tokens;

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

        public string GetUserToken(Guid userId)
        {
            var token = "";
            lock(_tokens)
            {
                token = _tokens.FirstOrDefault(x => x.Value == userId).Key;
            }
            return token;
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
