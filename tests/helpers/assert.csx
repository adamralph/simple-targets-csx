using System;

public static class Assert
{
    public static void HasMessage(Exception exception, string expectedMessage)
    {
        if (exception == null)
        {
            throw new Exception($"Expected an exception with message '{expectedMessage}', but no exception was thrown.");
        }

        if (exception.Message != expectedMessage)
        {
            throw new Exception($"Expected an exception with message '{expectedMessage}', but message was '{exception.Message}'.");
        }
    }

    public static void IsTrue(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception($"Expected true, but was false: {message}");
        }
    }

    public static void Contains(string expectedSubstring, string actualString)
    {
        if (actualString.IndexOf(expectedSubstring, StringComparison.CurrentCulture) < 0)
        {
            throw new Exception($"Expected \"{actualString}\" to contain \"{expectedSubstring}\"");
        }
    }
}
