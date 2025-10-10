using System.Collections.Generic;

public static class ENSOHelper
{
    // These are years where the ENSO winter occurred (focused on DJF)
    private static readonly HashSet<int> elNinoYears = new() { 1992, 1995, 1998, 2003, 2007, 2010, 2016 };
    private static readonly HashSet<int> laNinaYears = new() { 1999, 2000, 2008, 2011, 2012, 2021, 2022 };

    public static string GetENSOPhase(int year, int month)
    {
        // Month: 1 = January, ..., 12 = December
        // Check for DJF: December of previous year, Jan & Feb of event year
        if (month == 12 && elNinoYears.Contains(year + 1)) return "elnino";
        if ((month == 1 || month == 2) && elNinoYears.Contains(year)) return "elnino";

        if (month == 12 && laNinaYears.Contains(year + 1)) return "lanina";
        if ((month == 1 || month == 2) && laNinaYears.Contains(year)) return "lanina";

        return ""; // Neutral
    }
}
