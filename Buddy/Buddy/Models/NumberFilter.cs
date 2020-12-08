namespace Buddy.Models
{
    /**NumberFilter is the class called when a user inputs numbers. This class contains
     * one method, ContainsLetter, which scans for letters.
     */

    public class NumberFilter
    {
        public string letterFilter(string s)
        {
            string temp = "";
            for (int j = 0; j < s.Length; j++)
            {
                if (!(char.IsNumber(s[j])) && (s[j] != '/'))
                {
                    temp = s.Substring(0, j);
                }
                if ((j == 2 && s[j] != '/') || (j == 5 && s[j] != '/'))
                {
                    temp += s.Substring(0, j) + "/" + s.Substring(j);
                }
                if (j > 9)
                {
                    temp = s.Substring(0, 10);
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