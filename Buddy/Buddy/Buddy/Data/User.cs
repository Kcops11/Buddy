using Buddy.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Buddy.Data
{
    public class User
    {
        [PrimaryKey, AutoIncrement]

        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; } // not sure why im using this 

        public string User_Name { get; set; }

        [MaxLength(20)]
        public string Email { get; set; } 

        [MaxLength(12)]
        public string PassWord { get; set; }

        [MaxLength(16)]
        public string DoB { get; set; }

        public string UserAvi { get; set; } = null;

        public string Pro_Nouns { get; set; }
        public int MemeberNum { get; set; }

        public string SystemDescription { get; set; }


        [OneToMany]
        public ObservableCollection<Member> Members { get; set; }
    }
}