using Polly;
using Polly.Retry;

namespace NevesCS.Static.Utils.Vendor;

public static class PollyRetryPolicyUtils
{
    public static AsyncRetryPolicy GetHttpAsyncRetryPolicy(int retryCount, int startMillisecondsDuration)
    {
        return Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(GetSleepDurationPolicy(retryCount, startMillisecondsDuration));
    }

    public static IEnumerable<TimeSpan> GetSleepDurationPolicy(int numberOfRetries, int startMillisecondsDuration)
    {
        for (var i = 1; i <= numberOfRetries; ++i)
        {
            yield return GetSleepDuration(startMillisecondsDuration, i);
        }
    }

    public static TimeSpan GetSleepDuration(int startMillisecondsDuration, int retryAttempt)
    {
        return TimeSpan.FromSeconds(startMillisecondsDuration * retryAttempt);
    }
}
