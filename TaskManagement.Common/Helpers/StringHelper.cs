using System.Globalization;
using System.Text.RegularExpressions;

namespace TaskManagement.Common.Helpers;
public static class StringHelper
{

    public static bool PhoneValid(this string? value)
    {
        if (value.IsNullParameter())
            return false;

        if (value!.Count() < 11 || value!.Count() > 11)
            return false;

        if (!NumberValid(value!))
            return false;

        return true;
    }

    public static string? PersianDate(DateTime? dateTime)
    {
        if (dateTime.IsNullParameter())
            return null;

        PersianCalendar PersianCalendar1 = new PersianCalendar();
        return string.Format(@"{0}/{1}/{2}",
            PersianCalendar1.GetYear(dateTime!.Value),
            PersianCalendar1.GetMonth(dateTime!.Value),
            PersianCalendar1.GetDayOfMonth(dateTime!.Value));
    }

    public static DateTime? StringToDate(this string? strDate)
    {
        if (strDate.IsNullParameter())
            return null;

        strDate = Fa2En(strDate!);
        PersianCalendar pc = new PersianCalendar();
        string PersianDate1 = strDate!;
        string[] parts = PersianDate1.Split('/', '-');
        return pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), 0, 0, 0, 0);
    }

    public static bool NumberValid(this string? value)
    {
        if (value.IsNullParameter())
            return false;

        value = Fa2En(value);
        for (int i = 0; i < value!.Count(); i++)
        {
            int ew = value![i];
            if (ew < 48 || ew > 57)
            {
                return false;
            }
        }
        return true;
    }

    public static bool HasValue(this string? value, bool ignoreWhiteSpace = true)
        => ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);

    public static int? ToInt(this string? value)
    {
        if (value.IsNullParameter())
            return null;

        return Convert.ToInt32(value!.Fa2En());
    }

    public static decimal? ToDecimal(this string? value)
    {
        if (value.IsNullParameter())
            return null;

        return Convert.ToDecimal(value!.Fa2En());
    }

    public static string? ToNumeric(this int? value)
    {
        if (value.IsNullParameter())
            return null;

        return value!.Value.ToString("N0");
    }

    public static string? ToNumeric(this decimal? value)
    {
        if (value.IsNullParameter())
            return null;

        return value!.Value.ToString("N0");
    }

    public static string? ToCurrency(this int? value)
    {
        if (value.IsNullParameter())
            return null;

        //fa-IR => current culture currency symbol => ریال
        //123456 => "123,123ریال"
        return value!.Value.ToString("C0");
    }

    public static string? ToCurrency(this decimal? value)
    {
        if (value.IsNullParameter())
            return null;

        return value!.Value.ToString("C0");
    }

    public static string? En2Fa(this string? str)
    {
        if (str.IsNullParameter())
            return null;

        return str!.Replace("0", "۰")
            .Replace("1", "۱")
            .Replace("2", "۲")
            .Replace("3", "۳")
            .Replace("4", "۴")
            .Replace("5", "۵")
            .Replace("6", "۶")
            .Replace("7", "۷")
            .Replace("8", "۸")
            .Replace("9", "۹");
    }

    public static string? Fa2En(this string? str)
    {
        if (str.IsNullParameter())
            return null;

        return str!.Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9")
            //iphone numeric
            .Replace("٠", "0")
            .Replace("١", "1")
            .Replace("٢", "2")
            .Replace("٣", "3")
            .Replace("٤", "4")
            .Replace("٥", "5")
            .Replace("٦", "6")
            .Replace("٧", "7")
            .Replace("٨", "8")
            .Replace("٩", "9");
    }

    public static string? FixPersianChars(this string? str)
    {
        if (str.IsNullParameter())
            return null;

        return str!.Replace("ﮎ", "ک")
            .Replace("ﮏ", "ک")
            .Replace("ك", "ک")
            .Replace("ﮐ", "ک")
            .Replace("ﮑ", "ک")
            .Replace("ك", "ک")
            .Replace("ي", "ی")
            .Replace(" ", " ")
            .Replace("‌", " ")
            .Replace("ھ", "ه");//.Replace("ئ", "ی");
    }

    public static string? FixPersianCharsFull(this string? str)
    {
        if (str.IsNullParameter())
            return null;

        return str!.Trim()!.FixPersianChars()!.Fa2En()!.Replace("آ", "ا");
    }

    public static bool IsNullParameter(this object? param)
    {
        if (param is string strParam)
            return string.IsNullOrEmpty(strParam) || string.IsNullOrWhiteSpace(strParam);

        return param is null;
    }

}
