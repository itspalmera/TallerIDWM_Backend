using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.RequestHelpers;

using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace TallerIDWM_Backend.Src.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationMetaData metadata)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            response.Headers.Append("Pagination", JsonSerializer.Serialize(metadata, options));
            response.Headers.Append(HeaderNames.AccessControlExposeHeaders, "Pagination");

        }
    }
}