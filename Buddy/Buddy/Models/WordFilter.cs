namespace Buddy.Models
{
    /**WordFilter is the class called when a user inputs raw text. This class contains
     * one method, ContainsRestricted, which scans for restricted words via querying an
     * estblished list of resrticted words.
     */

    public class WordFilter
    {
        private TextResourceExtension rWD = new TextResourceExtension();

        public bool ContainsRestricted(string s)
        {
            bool restrictedPresent = false;

            s = s.ToLower();
            string[] incomingString = s.Split(new char[] { '/', ' ' });

            for (int i = 0; i < incomingString.Length; i++)
            {
                for (int j = 0; j < rWD.badText.Length; j++)
                {
                    if (incomingString[i] == rWD.badText[j])
                    {
                        restrictedPresent = true;
                        break;
                    }
                }
                if (restrictedPresent) { break; }
            }
            return restrictedPresent;
        }
    }
}