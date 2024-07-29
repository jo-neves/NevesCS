namespace NevesCS.Abstractions.Clients.Web3.Solana
{
    public interface ISolanaClient
    {
        public Task<bool> CheckForTransactionConfirmedAsync(string transactionSignature);
    }
}
