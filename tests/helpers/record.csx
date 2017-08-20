using System;

public static class Record
{
    public static Exception Exception(Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            return ex;
        }

        return null;
    }
}
