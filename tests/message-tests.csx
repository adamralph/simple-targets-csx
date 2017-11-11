#load "../src/internal/util.csx"
#load "helpers/assert.csx"

using static SimpleTargetsUtil;

TestMessage(0.000_001D,     "1 ns");
TestMessage(0.001D,         "1 \u00B5s"); // µs
TestMessage(1D,             "1 ms");
TestMessage(1_000D,         "1 s");
TestMessage(119_000D,       "1 min 59 s");
TestMessage(1_000_000D,     "16 min 40 s");
TestMessage(1_000_000_000D, "16,667 min");

void TestMessage(double elapsedMilliseconds, string expectedSubstring)
{
    var message = SuccessMessage("foo", false, false, elapsedMilliseconds);
    Assert.Contains(expectedSubstring, message);
}
