using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
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
	}
}
