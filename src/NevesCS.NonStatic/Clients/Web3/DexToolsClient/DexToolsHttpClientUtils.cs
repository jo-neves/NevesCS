using NevesCS.Abstractions.Clients.Web3.DexTools.Exceptions;
using NevesCS.Static.Constants;
using NevesCS.Static.Utils;

using System.Net.Http.Json;

namespace NevesCS.NonStatic.Clients.Web3.DexToolsClient
{
    internal static class DexToolsHttpClientUtils
    {
        public static async Task<TResult?> TryGetOrThrowAsync<TResult>(
            HttpClient httpClient,
            string requestUri,
            CancellationToken cancellationToken)
        {
            return await FuncUtils.TryCatchAsync(
                async () => await httpClient.GetFromJsonAsync<TResult>(requestUri, cancellationToken),
                (ex) => throw new DexToolsApiHttpException(HttpMethods.Get, requestUri, requestContent: null, ex));
        }
    }
}
