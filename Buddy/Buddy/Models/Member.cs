using Buddy.Data;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using Xamarin.Forms;


namespace Buddy.Models 
{
    /** Member is intended to be the class utilized by sub-members of a given system. 
     *  This is different that User, who represents the overall system and interacts with the 
     *  database more directly. 
     */
    public class Member : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

            private string Name;
            public string name 
            {
                get { return Name; }
                set
                {
                    Name = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("name"));
                }
             }
            // I dont think these should be public 
            public string pro_noun { get; set; } 
            public string birthday { get; set; }
            public string role { get; set; }

            [ForeignKey(typeof(User))]
            public int systemID { get; set; }

            public string description { get; set; }

            private string AVI = null;
            public string AviSource
            {
                get { return AVI; }
                set
                {
                    AVI = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AviSource"));
                } 
            } 


        public override string ToString()
        {
            return "" + name + " /" + pro_noun + " /" + role + " /" + systemID;
        }

        [OneToMany]
        public List <Trigger> TriggersList { get; set; } // need to make sure only strings are


        [ManyToOne]
        public User user { get; set; } // may not want to expose in this way 
    }
}
