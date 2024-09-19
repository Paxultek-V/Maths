
public static class FormatIdleNumber
{
    public static string FormatNumber(this float a)
    {
        int count = 0;
        string unit = "";

        if (a < 1000)
            return a.ToString("F0");
        
        while (a >= 1000f)
        {
            a /= 1000;
            count++;
        }

        switch (count)
        {
            case 1:
                unit = "K";
                break;
            case 2:
                unit = "M";
                break;
            default:
                break;
        }

        string number = "";

        
        if (a > 100)
            number = a.ToString("F0");
        else if (a > 10)
            number = a.ToString("F1");
        else
            number = a.ToString("F2");


        return number + unit;
    }
}