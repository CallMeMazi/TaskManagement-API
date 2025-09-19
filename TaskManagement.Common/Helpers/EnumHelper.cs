using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TaskManagement.Common.Helpers;
public static class EnumHelper
{
    public static IEnumerable<T> GetEnumValues<T>(this T input)
        where T : struct, Enum
    {
        return Enum.GetValues(input.GetType()).Cast<T>();
    }

    public static IEnumerable<T> GetEnumFlags<T>(this T input) 
        where T : struct, Enum
    {
        foreach (var value in Enum.GetValues(input.GetType()))
            if (input.HasFlag(value as Enum))
                yield return (T)value;
    }

    public static string ToDisplay(this Enum value, DisplayProperty property = DisplayProperty.Name)
    {
        Assert.NotNull(value, nameof(value));

        var attribute = value.GetType().GetField(value.ToString())!
            .GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();

        if (attribute == null)
            return value.ToString();

        return attribute.GetType()!.GetProperty(property.ToString())!.GetValue(attribute, null)!.ToString() ?? value.ToString();
    }

    public static Dictionary<int, string> ToDictionary<T>()
        where T : struct, Enum
    {
        return Enum.GetValues(typeof(T)).Cast<Enum>().ToDictionary(p => Convert.ToInt32(p), q => ToDisplay(q));
    }
}

public enum DisplayProperty
{
    Description,
    GroupName,
    Name,
    Prompt,
    ShortName,
    Order
}
