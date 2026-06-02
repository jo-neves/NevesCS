using NevesCS.Abstractions.Interfaces;
using NevesCS.Static.Constants.Values;

namespace NevesCS.NonStatic.Statistics
{
    public class SimpleMovingAverage : IStatisticalIndicator
    {
        private readonly Queue<decimal> Data;

        private decimal Total = Ints.Zero;

        private readonly uint SampleSize;

        public SimpleMovingAverage(uint sampleSize)
        {
            SampleSize = sampleSize;

            Data = new((int)sampleSize);
        }

        public void AddSample(decimal item)
        {
            if (Data.Count == SampleSize)
            {
                Total -= Data.Dequeue();
            }

            Total += item;
            Data.Enqueue(item);
        }

        public decimal Compute(decimal newSample)
        {
            AddSample(newSample);

            return Compute();
        }

        public decimal Compute()
        {
            return Total / Data.Count;
        }
    }
}
