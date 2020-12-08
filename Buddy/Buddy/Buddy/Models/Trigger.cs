using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Buddy.Models
{
    /** Trigger class is inteded to streamline the process in which a user registers a trigger. 
     *  For now triggers only have 3 types and allows for two types of references to that trigger. 
     *  Distinguising whether or not the reference is a link or image will help the app decide 
     *  how to best handle the reference.
     */
    public class Trigger
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Member))]
        public int TriggerId { get; set; }

        public string TrigLabel;

        public TriggerReference TrigReference;

        public Trigger()
        {

        }

        public enum TriggerType
         {
           Switch,
           Grounding,
           Trauma
         }

         public struct TriggerReference
        {
            public enum ReferenceType
            {
                Link,
                Image
            }

            public ReferenceType referenceType { get; set; }

            public string ReferenceSource { get; set; }
        }

        [ManyToOne]
        public Member Member { get; set; }
    }
}
