using System.Text.Json;
using Microsoft.AspNetCore.Http;
namespace QuaMonBa_Store.Helpers
{
    public static class SessionExtensions
    {
        // Hàm nhét List vào Session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Hàm lấy List từ Session ra
        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
