using Microsoft.AspNetCore.Http;

namespace ScotlandsMountains.Api.Functions
{
    public static class Extensions
    {
        public static string GetString(this IQueryCollection queryCollection, string key)
        {
            return queryCollection.ContainsKey(key)
                ? queryCollection[key].ToString()
                : null;
        }

        public static int? GetInt(this IQueryCollection queryCollection, string key)
        {
            var s = queryCollection.GetString(key);

            if (s == null) return null;

            if (int.TryParse(s, out var i))
                return i;

            return null;
        }
    }
}
