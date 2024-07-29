using Newtonsoft.Json;

namespace TiklaYe_CQRS.Models
{
    //  SetObject ve GetObject metotları ile JSON kullanarak objeleri saklamamızı ve geri almamızı sağlar.
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
