using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Uplift.ExtensionMethods
{
    public static class SessionExtensionMethods
    {
        public static void SaveObjectInSession(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T RetrieveFromSession<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
