using System;
using System.Runtime.CompilerServices;
using Dominus.Backend.Application;

namespace Dominus.Backend.Data
{
    public class DStringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
    {
        private readonly string _callerPropertyName;

        public string ResourceKey { get; set; }

        public DStringLengthAttribute(string resourceKey, int maximumLength, [CallerMemberName] string propertyName = null)
            : base(maximumLength)
        {
            ResourceKey = resourceKey;
            _callerPropertyName = propertyName;
            ErrorMessage = String.Format(DApp.DefaultLanguage.GetResource("DLCOLUMNMAXLENGTH"), DApp.DefaultLanguage.GetResource(ResourceKey), MaximumLength);
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }

    }

    public class DRequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        private readonly string _callerPropertyName;

        public string ResourceKey { get; set; }

        public DRequiredAttribute(string resourceKey, [CallerMemberName] string propertyName = null)
            : base()
        {
            ResourceKey = resourceKey;
            _callerPropertyName = propertyName;
            ErrorMessage = String.Format(DApp.DefaultLanguage.GetResource("DLCOLUMNISREQUIRED"), DApp.DefaultLanguage.GetResource(ResourceKey));
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }

    }

    public class DDisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        private readonly string _callerPropertyName;

        public string ResourceKey { get; set; }

        public DDisplayNameAttribute(string resourceKey, [CallerMemberName] string propertyName = null)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
            _callerPropertyName = propertyName;
        }

        public override string DisplayName
        {
            get
            {
                string value = null;
                value = DApp.DefaultLanguage.GetResource(ResourceKey);

                if (string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(_callerPropertyName))
                {
                    value = _callerPropertyName;
                }

                return value;
            }
        }

        public string Name
        {
            get { return "RecursoDisplayName"; }
        }
    }

    public class DRequiredFKAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
    {
        private readonly string _callerPropertyName;

        public string ResourceKey { get; set; }

        public DRequiredFKAttribute(string resourceKey, [CallerMemberName] string propertyName = null)
            : base(1,long.MaxValue)
        {
            ResourceKey = resourceKey;
            _callerPropertyName = propertyName;
            ErrorMessage = String.Format(DApp.DefaultLanguage.GetResource("DLCOLUMNFKISREQUIRED"), DApp.DefaultLanguage.GetResource(ResourceKey));
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }

    }
}