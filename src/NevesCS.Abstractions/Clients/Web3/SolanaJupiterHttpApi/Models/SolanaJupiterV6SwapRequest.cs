using System.Diagnostics.CodeAnalysis;

namespace NevesCS.Abstractions.Clients.Web3.SolanaJupiterHttpApi.Models
{
    [ExcludeFromCodeCoverage]
    public readonly record struct SolanaJupiterV6SwapRequest
    {
        public required readonly string BaseAssetAddress { get; init; }

        public required readonly string QuoteAssetAddress { get; init; }

        public required readonly JupiterOrderType OrderType { get; init; }

        /// <summary>
        /// The amount of input token is the amount of token that goes out of the account into the Swap Program.
        /// The token we trade for the one we want to get in the current swap. <br/>
        /// <br/>
        ///
        /// IF JupiterOrderType.Buy => InputToken IS QuoteAssetAddress <br/>
        /// IF JupiterOrderType.Sell => InputToken IS BaseAssetAddress <br/>
        /// </summary>
        public required readonly decimal AmountOfInputToken { get; init; }

        /// <summary>
        /// E.g. 0.5%, 1%, etc.
        ///
        /// </summary>
        public required readonly float SlippagePercentage { get; init; }
    }

    public enum JupiterOrderType
    {
        Buy,
        Sell
    }
}
