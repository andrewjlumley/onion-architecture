using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Http.ODataPackage;

namespace Common.Http
{
	public interface IODataPackage
	{
		ODataPagination? Pagination { get; }
		String GetQueryString();
	}

	public class ODataPackage : IODataPackage
	{
		internal ODataPackage() { }

		internal ODataPackage(HttpRequest request)
		{
			if (new[] {"$top", "$skip" }.Any(f => request.Query.ContainsKey(f)))
				Pagination = new ODataPagination(request);
		}

		public String GetQueryString()
		{
			StringBuilder query = new StringBuilder();
			query.Append("?");

			if (Pagination != null)
				query.Append($"$top={Pagination.Top}&$skip={Pagination.Skip}");

			return query.ToString();
		}

		public ODataPagination? Pagination { get; internal set; }

		public sealed class ODataPagination
		{
			public ODataPagination() { }

			public ODataPagination(HttpRequest request)
			{
				if (int.TryParse(request.Query["$top"], out int top))
					Top = top;

				if (int.TryParse(request.Query["$skip"], out int skip))
					Skip = skip;
			}

			public int Top { get; internal set; }
			public int Skip { get; internal set; }
		}
	}

	public static class ODataHelper
	{
		public static IODataPackage Create() => new ODataPackage();

		public static bool TryQuery(HttpRequest request, out IODataPackage? package)
		{
			package = request.Query.Any() ? new ODataPackage(request) : (IODataPackage?)null;
			return package != null;
		}

		public static IODataPackage AssignPagination(this IODataPackage package, int top, int skip)
		{
			((ODataPackage)package).Pagination = new ODataPagination();
			package.Pagination!.Top = top;
			package.Pagination!.Skip = skip;
			return package;
		}
	}
}
