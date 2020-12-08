using Buddy.Models;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace Buddy.Data
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    /** UserDB is the class that takes care of all data management and storage operations for PluralBuddy.
     *  Here we establish data tables for each unique data structure that is managed. 
     */
    public class UserDB
    {
        private readonly SQLiteConnection _SQLiteConnection;

        public UserDB(string dbPath)
        {
            _SQLiteConnection = new SQLiteConnection(dbPath);
            _SQLiteConnection.CreateTable<User>();
            _SQLiteConnection.CreateTable<Member>();
            _SQLiteConnection.CreateTable<Trigger>();
        }

        public IEnumerable<User> GetUsers()
        {
            return (from u in _SQLiteConnection.Table<User>()
                    select u).ToList();
        }

        /** Get specific user returns a user. 
         *  Useful for acessing the user currently logged in 
         */
        public User GetSpecificUser(int id)
        {
            return _SQLiteConnection.Table<User>().FirstOrDefault(t => t.Id == id);
        }

        public void DeleteUser(int id)
        {
            _SQLiteConnection.Delete<User>(id);
        }

        /** Adds user to database.
         *  First looks to see if information is already present in database. 
         */
        public string AddUser(User user)
        {
            var data = _SQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.Email == user.Email && x.User_Name == user.User_Name).FirstOrDefault();
            if (d1 == null)
            {
                _SQLiteConnection.InsertWithChildren(user, true); // originally <int>
                return "y";
            }
            else
            {
                return "f"; // is this being too general
            }
        }

        /** Updates user validation. 
         */
        public bool UpdateUserValidation(string userid)
        {
            try
            {
                var data = _SQLiteConnection.Table<User>();
                var d1 = (from values in data
                          where values.Email == userid
                          select values).Single();
                if (d1 != null)
                {
                    return true;
                }
            }
            catch (System.InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
            }
            return false;
        }

        /** Updates user password.
         *  First searches database to see if user exists. 
         */
        public bool UpdatePassword(string username, string pwd)
        {
            var data = _SQLiteConnection.Table<User>();
            var d1 = (from values in data
                      where values.Email == username
                      select values).Single();
            if (true)
            {
                d1.PassWord = pwd;
                _SQLiteConnection.Update(d1);
                return true;
            }
            else
                return false;
        }

        public bool LoginValidate(string userName1, string pwd1)
        {
            var data = _SQLiteConnection.Table<User>();
            var d1 = data.Where(x => x.Email == userName1 && x.PassWord == pwd1).FirstOrDefault();
            if (d1 != null)
            {
                App.currentUser = d1.Id;
                return true;
            }
            else
                return false;
        }

        /** UpdateUserMembers is called when app is exiting or when a 
         *  significant change has been made. Assumes user var passed in has 
         *  valid array of members. 
         */
        public void UpdateUserMembers(User user, Member member)
        {
            var data = _SQLiteConnection.Table<User>();
            var mem = new object();
            try
            {
                mem = data.Where(x => x.Members == user.Members).FirstOrDefault();
            }
            catch (System.NullReferenceException e) // save triggers when registering members 
            {
            }
            foreach(Member membr in _SQLiteConnection.Table<Member>())
            {
                if(membr.Id == member.Id) // deleting members that are added 
                {
                    _SQLiteConnection.InsertOrReplace(member);
                    _SQLiteConnection.UpdateWithChildren(user);
                    return;
                }
            }

            _SQLiteConnection.Insert(member);

            _SQLiteConnection.UpdateWithChildren(user);
        }

        public void UpdateUser(User user)
        {
            _SQLiteConnection.InsertOrReplace(user);
        }
        public Member GetSpecificMember(int memId, User user)
        {
            var data1 = _SQLiteConnection.Table<Member>();
            try
            {
                Member mem = data1.ElementAtOrDefault(memId);
                if (mem.systemID == user.Id)
                {
                    return mem;
                }
            }
            catch(System.NullReferenceException e) 
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.HResult);
            }

            return null;
        }
        public void UpdateMemberTrigger(Member member, Trigger trigger)
        {
            var data = _SQLiteConnection.Table<Member>();
            var trig = new object();
            try
            {
                trig = data.Where(x => x.TriggersList.Contains(trigger));
                if(trigger.TriggerId == member.systemID)
                {
                    _SQLiteConnection.InsertOrReplace(trigger);
                    _SQLiteConnection.UpdateWithChildren(member);
                    return;
                }
            }
            catch (System.NullReferenceException e)
            {
            }

                _SQLiteConnection.Insert(trigger);
                _SQLiteConnection.UpdateWithChildren(member);
            
        }
        public Trigger GetSpecificTrigger(int trigId, Member member)
        {
            var data = _SQLiteConnection.Table<Trigger>();
            try
            {
                Trigger trig = data.ElementAtOrDefault(trigId);
                if (trig.TriggerId == member.systemID)
                {
                    return trig;
                }
            }
            catch(System.NullReferenceException e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.WriteLine(e.HResult);
            }
            return null;
        }
    }
}