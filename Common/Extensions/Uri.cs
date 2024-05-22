using Common.Exceptions;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Common.Extensions
{
	public static class UriExtensionMethods
	{
		public static bool IsNullOrWhiteSpace(this Uri uri)
		{
			return uri == null || String.IsNullOrWhiteSpace(uri.AbsolutePath?.TrimEnd('/'));
		}

		public static bool IsSchema(this Uri uri, String schema)
		{
			return IsNullOrWhiteSpace(uri) == false && uri.Scheme == schema;
		}

		public static bool IsValidEmailAddress(this Uri uri)
		{
			if (uri.IsSchema(Uri.UriSchemeMailto) == false)
				return false;

			return Regex.IsMatch(uri.AbsolutePath.TrimStart('/'), @"^([\w-'\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,7}|[0-9]{1,3})(\]?)$");
		}

        public static async Task<(HttpStatusCode Status, T Value, IDictionary<String, String[]> Headers)> JsonRequest<T>(this Uri uri, IDictionary<String, String>? headers = null, TimeSpan? timeout = null)
        {
            return await SendJsonRequest<T>(uri, HttpMethod.Get, null, headers, timeout);
        }

        public static async Task<(HttpStatusCode Status, T Value, IDictionary<String, String[]> Headers)> JsonRequest<T, U>(this Uri uri, HttpMethod method, U json, IDictionary<String, String>? headers = null, TimeSpan? timeout = null)
        {
            return await SendJsonRequest<T>(uri, method, json, headers, timeout);
        }

        private static async Task<(HttpStatusCode, T, IDictionary<String, String[]>)> SendJsonRequest<T>(this Uri uri, HttpMethod method, object? json = null, IDictionary<String, String>? headers = null, TimeSpan? timeout = null)
        {
            using (var httpClient = new HttpClient())
            {
                if (uri.Scheme == "https")
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                if (timeout != null)
                    httpClient.Timeout = timeout.Value;

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                foreach (var header in headers ?? new Dictionary<String, String>())
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var requestMessage = new HttpRequestMessage(method, uri);

                if (json != null)
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.SendAsync(requestMessage);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();

                    var responseHeaders = new Dictionary<String, String[]>();
                    foreach (var header in response.Headers)
                    {
                        responseHeaders[header.Key] = header.Value.ToArray();
                    }

                    if (typeof(T) == typeof(String))
                        return (response.StatusCode, (T)Convert.ChangeType(responseBody, typeof(T)), responseHeaders);
                    else
                        return (response.StatusCode, JsonConvert.DeserializeObject<T>(responseBody)!, responseHeaders);
                }

                catch (HttpRequestException ex)
                {
                    throw new InfrastructureException(ex.Message, ex.InnerException);
                }
            }
        }
    }
}
