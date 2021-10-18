using System;

namespace LibAmiibo.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SupportedGameAttribute : Attribute
    {
        public Type SupportedGameType { get; }

        public SupportedGameAttribute(Type type)
        {
            this.SupportedGameType = type;
        }
    }
}
