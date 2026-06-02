using System.Diagnostics.CodeAnalysis;

namespace NevesCS.NonStatic.Clients.Web3.Solana
{
    public readonly record struct HttpSolanaClientOptions
    {
        public HttpSolanaClientOptions()
        {
        }

        [SetsRequiredMembers]
        public HttpSolanaClientOptions(int transactionConfirmedCheckMaxRetries, TimeSpan transactionConfirmedCheckRetryDelay)
        {
            TransactionConfirmedCheckMaxRetries = transactionConfirmedCheckMaxRetries;
            TransactionConfirmedCheckRetryDelay = transactionConfirmedCheckRetryDelay;
        }

        [SetsRequiredMembers]
        public HttpSolanaClientOptions(TimeSpan transactionConfirmedCheckMaxAwaitDuration, TimeSpan transactionConfirmedCheckRetryDelay)
        {
            TransactionConfirmedCheckRetryDelay = transactionConfirmedCheckRetryDelay;
            TransactionConfirmedCheckMaxRetries = (int)Math.Floor(
                transactionConfirmedCheckMaxAwaitDuration.TotalMilliseconds / transactionConfirmedCheckRetryDelay.TotalMilliseconds);
        }

        public readonly int TransactionConfirmedCheckMaxRetries { get; init; }

        public readonly TimeSpan TransactionConfirmedCheckRetryDelay { get; init; }
    }
}
