namespace LibAmiibo.Helper
{
    public class GroupName
    {
        public string FullName { get; private set; }
        public string ShortName { get; private set; }

        public GroupName(string fullName, string shortName)
        {
            this.FullName = fullName;
            this.ShortName = shortName;
        }
    }
}
