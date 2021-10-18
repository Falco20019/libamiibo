using System;

namespace LibAmiibo.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppIDAttribute : Attribute
    {
        public uint AppID { get; }

        public AppIDAttribute(uint appId)
        {
            AppID = appId;
        }
    }
}
