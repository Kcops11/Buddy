namespace Buddy.Models
{
    internal class NumFilter
    {
        public string letterFilter(string s)
        {
            string temp = "";
            for (int j = 0; j < s.Length; j++)
            {
                if (!(char.IsNumber(s[j])))
                {
                    if (j > 0)
                    {
                        return temp;
                    }
                    else
                    {
                        temp = s.Substring(0, j);
                    }
                }
            }
            if (temp == "")
            {
                return s;
            }
            else
            {
                return temp;
            }
        }
    }
}