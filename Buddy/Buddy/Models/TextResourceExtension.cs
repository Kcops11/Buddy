using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Buddy.Models
{
    /**TextResourceExtension is a custom extension that allows easy access to a list of
     * restricted words. Should be initialized whenever WordFilter is called. Assembly info
     * at top must still be set.
     *
     */

    public class TextResourceExtension
    {
        public TextResourceExtension()
        {
            GetRepository();
        }

        public string[] badText { get; set; }

        public void GetRepository()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(TextResourceExtension)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Buddy.Data.bad-words.txt");
            var reader = new System.IO.StreamReader(stream);
            badText = new string[1383];

            for (int i = 0; i < 1383; i++)
            {
                try
                {
                    badText[i] = reader.ReadLine().ToLower();
                }
                catch (System.NullReferenceException ex)
                {
                    Debug.WriteLine(ex.ToString());
                    break;
                }
            }
        }
    }
}