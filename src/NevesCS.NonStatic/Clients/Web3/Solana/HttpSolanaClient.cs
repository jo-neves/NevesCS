using NevesCS.Abstractions.Clients.Web3.Solana;
using NevesCS.Abstractions.Clients.Web3.Solana.Exceptions;
using NevesCS.Abstractions.Services;
using NevesCS.Static.Constants.Web3.Solana;

using Solnet.Rpc;

namespace NevesCS.NonStatic.Clients.Web3.Solana
{
    public sealed class HttpSolanaClient : ISolanaClient
    {
        private readonly HttpSolanaClientOptions Options;

        private readonly IRpcClient RpcClient;

        private readonly IJsonParser JsonParser;

        private readonly CancellationToken CancellationToken;

        public HttpSolanaClient(
            HttpSolanaClientOptions options,
            IRpcClient rpcClient,
            IJsonParser jsonParser,
            CancellationToken cancellationToken = default)
        {
            Options = options;
            RpcClient = rpcClient;
            JsonParser = jsonParser;
            CancellationToken = cancellationToken;
        }

        public async Task<bool> CheckForTransactionConfirmedAsync(string transactionSignature)
        {
            var retryCount = 0;

            while (retryCount <= Options.TransactionConfirmedCheckMaxRetries)
            {
                var result = await RpcClient.GetSignatureStatusesAsync([transactionSignature], true);

                if (!result.WasSuccessful)
                {
                    throw new SolanaRpcException(JsonParser.SerializeObject(result));
                }
                else if (result.Result?.Value[0]?.ConfirmationStatus == SolanaTransactionConfirmationStatus.Finalized)
                {
                    return true;
                }

                ++retryCount;
                await Task.Delay(Options.TransactionConfirmedCheckRetryDelay, CancellationToken);
            }

            return false;
        }
    }
}
