using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace Buddy.Models
{
    class MemberViewModel 
    {
        public ObservableCollection<Member> SubMembers { get; set; }

        public MemberViewModel()
        {
            SubMembersInit(); //  check if this is 
        }
        public void RemoveImage(int index)
        {
            SubMembers.ElementAt(index).AviSource = string.Empty;
        }
        public void UpdateImage( int index, string newPath)
        {
            SubMembers.ElementAt(index).AviSource = newPath;
        }
        private void SubMembersInit()
        {
            SubMembers = new ObservableCollection<Member>();

            for (int i = 0; i < App.UserDB.GetSpecificUser(App.currentUser).MemeberNum; i++)
            {
                var mem = App.UserDB.GetSpecificMember(i, App.UserDB.GetSpecificUser(App.currentUser));

                if(mem == null)
                {
                    SubMembers.Add(new Member
                    {
                        Id = i,
                        name = "Member " + (i + 1)
                    });
                }
                else
                {
                    if(mem.AviSource.Contains("File: "))
                    {
                        mem.AviSource = mem.AviSource.Replace("File: ", "");
                    }
                    mem.user = App.UserDB.GetSpecificUser(App.currentUser);
                    SubMembers.Add(mem);
                }
            }
        }
    }
}
