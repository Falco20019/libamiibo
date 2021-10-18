using System;

namespace LibAmiibo.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CheatAttribute : Attribute
    {
        public enum Type
        {
            Undefined,
            TextBox,
            CheckBox,
            MultiDropDown,
            DropDown,
            NumberSpinner
        }

        public Type DisplayType { get; }
        public string Section { get; }
        public string Name { get; }
        public string Description { get; set; }
        public uint Min { get;set; }
        public uint Max { get;set; }

        public CheatAttribute(Type displayType, string section, string name)
        {
            DisplayType = displayType;
            Section = section;
            Name = name;
        }
    }
}
