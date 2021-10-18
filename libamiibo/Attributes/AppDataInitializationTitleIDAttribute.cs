using System;
using LibAmiibo.Data.Settings.AppData;

namespace LibAmiibo.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AppDataInitializationTitleIDAttribute : Attribute
    {
        public Title TitleID { get; }

        public AppDataInitializationTitleIDAttribute(string titleId)
        {
            TitleID = Title.FromTitleID(titleId);
        }
    }
}
