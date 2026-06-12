using NevesCS.NonStatic.Services;

using FluentAssertions;

namespace NevesCS.Tests.NonStatic.Services;

public class DebouncerTests
{
    [Fact]
    public async Task Debounce_Action_Is_Called_Once_After_Delay()
    {
        int result = 0;
        var sut = new Debouncer<int>(
            action: (triggeredInput) => result = triggeredInput,
            millisecondDelay: 2);

        sut.Trigger(1);
        sut.Trigger(2);
        sut.Trigger(3);

        await Task.Delay(50);

        result.Should().Be(3);
    }
}
