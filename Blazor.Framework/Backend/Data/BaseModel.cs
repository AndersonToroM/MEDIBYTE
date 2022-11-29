using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Dominus.Backend.Data
{
    public enum ColapseClass
    {
        [Description("collapse multi-collapse")]
        Collapse,
        [Description("visible")]
        Visible
    }

    public class KeyValue
    {
        public string Id { get; set; }

        public string Value { get; set; }

        public int Key { get; set; }

    }

    public class BaseModel
    {
        public string VisibleFindPanel { get; set; }

        public string VisibleNewPanel { get; set; }

        public BaseModel()
        {
            VisibleFindPanel = ColapseClass.Collapse.GetDescription();
            VisibleNewPanel = ColapseClass.Collapse.GetDescription();
        }

    }

    public static class StringExtentions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return string.Empty;
        }
    }
}