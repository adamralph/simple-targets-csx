public void AssertThrowsWithMessage(string expectedMessage, Action action)
{
    try
    {
        action();

        throw new Exception($"Expected an exception with message '{expectedMessage}', but none was found.");
    }
    catch (Exception exception)
    {
        if (exception.Message != expectedMessage)
        {
            throw new Exception($@"Expected exception with message '{expectedMessage}' but message was '{exception.Message}'.");
        }
    }
}
