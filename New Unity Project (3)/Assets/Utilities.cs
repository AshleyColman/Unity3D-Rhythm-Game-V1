using System;

public static class Utilities
{
    public static string GetCurrentDate()
    {
        return DateTime.Now.ToString("MM/dd/yyyy h:mm");
    }
}
