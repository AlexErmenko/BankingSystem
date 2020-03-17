using System;
using System.Text.Json;

using Microsoft.AspNetCore.Http;

namespace Web.Extension
{
	public static class SessionExtension
	{
		public static void Set<T>(this ISession session, string key, T value) { session.SetString(key: key, value: JsonSerializer.Serialize(value: value)); }

		public static T Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key: key);
			return value == null ? default : JsonSerializer.Deserialize<T>(json: value);
		}
	}
}
