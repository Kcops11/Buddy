using Buddy.Data;
using System;
using System.Configuration;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Buddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private User user = new User();

        public RegisterPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            EntryEmail.ReturnCommand = new Command(() => EntryUserName.Focus());
            EntryUserName.ReturnCommand = new Command(() => EntryUserPassword.Focus());
            EntryUserPassword.ReturnCommand = new Command(() => EntryUserPConfirm.Focus());
            if (App.originalStyle)
            {
                Resources["currentStyle"] = Resources["lightTheme"];
            }
            else
            {
                Resources["currentStyle"] = Resources["darkTheme"];
            }
        }

        private async void Registration_Action(object sender, EventArgs e) // need to overhaul this
        {
            if ((string.IsNullOrWhiteSpace(EntryUserName.Text)) || (string.IsNullOrWhiteSpace(EntryEmail.Text)) ||
                (string.IsNullOrWhiteSpace(EntryUserPassword.Text)) || (string.IsNullOrEmpty(EntryUserName.Text)) ||
                (string.IsNullOrEmpty(EntryEmail.Text)) || (string.IsNullOrEmpty(EntryUserPassword.Text)))
            {
                await DisplayAlert("", "Please try again", "←");
            }
            else if (!(string.Equals(EntryUserPassword.Text, EntryUserPConfirm.Text)))
            {
                warningLabel.Text = "Enter same password";
                EntryUserPassword.Text = string.Empty;
                EntryUserPConfirm.Text = string.Empty;
                warningLabel.TextColor = Color.SlateBlue;
                warningLabel.IsVisible = true;
            }
            else
            {
                user.User_Name = EntryUserName.Text;
                user.DoB = DOB.Text;
                user.Email = EntryEmail.Text;
                user.PassWord = EntryUserPassword.Text;
                user.Pro_Nouns = Pronouns.Text; //need to do validation behavior for this?
                try
                {
                    user.MemeberNum = int.Parse(NumUsers.Text);
                    string regString = @"^(0?[1 - 9] | 1[0 - 2]) / ([0 - 2]?[1 - 9] |[1 - 3][01]) /\d{ 4 }$";
                    RegexStringValidator regexStringValidator = new RegexStringValidator(regString);
                    regexStringValidator.Validate(DOB.Text);
                    Console.WriteLine("Validation successfull");
                }
                catch (System.FormatException)
                {
                    Debug.Write("Try entering numbers");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
            try
            {
                var returnval = App.UserDB.AddUser(user);
                if (returnval == "y")
                {
                    await DisplayAlert("", "Welcome to Plural Buddy", "←");
                    await Navigation.PushAsync(new LoginPage());
                }
                else
                {
                    await DisplayAlert("", "Please try again", "←");
                    warningLabel.IsVisible = false;
                    EntryEmail.Text = string.Empty;
                    EntryUserName.Text = string.Empty;
                    EntryUserPassword.Text = string.Empty;
                    EntryUserPConfirm.Text = string.Empty;
                    DOB.Text = string.Empty;
                    NumUsers.Text = string.Empty;
                    Pronouns.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        /**Back command- command issued when user wants
         * to navigate away from the register page
         */

        private void backCommand(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }
    }
}