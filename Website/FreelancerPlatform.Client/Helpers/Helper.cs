namespace FreelancerPlatform.Client.Helpers
{
    public static class Helper
    {
        public static string CutString(string str)
        {
            if (str.Length > 150)
            {
                var teampString = str.Substring(0, 149);
                teampString += " ...";
                return teampString;
            }

            return str.Trim();
        }
    }
}
