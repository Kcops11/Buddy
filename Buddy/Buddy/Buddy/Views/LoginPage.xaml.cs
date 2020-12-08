using Buddy.Data;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Buddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            StyleInit(App.originalStyle);
            Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);
        }

        private void SignupCommand(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }

        private void ForgotPassCommand(object sender, EventArgs e)
        {
            popupLoadingView.IsVisible = true;
            User_Name.IsVisible = false;
            Pass_Word.IsVisible = false;
            loginFrame.IsVisible = false;
            ForgotPass.IsVisible = false;
            backArrow.IsVisible = true;
        }

        private string name;

        private void backCommand(object sender, EventArgs e)
        {
            popupLoadingView.IsVisible = false;
            User_Name.IsVisible = true;
            Pass_Word.IsVisible = true;
            loginFrame.IsVisible = true;
            backArrow.IsVisible = false;
            ForgotPass.IsVisible = true;
        }

        private async void userCheckEmail(object sender, EventArgs e)
        {
            if ((string.IsNullOrWhiteSpace(emailValidationEntry.Text)) || (string.IsNullOrWhiteSpace(emailValidationEntry.Text)))
            {
                await DisplayAlert("", "Enter valid email", "←");
            }
            else
            {
                name = emailValidationEntry.Text;
                var textresult = App.UserDB.UpdateUserValidation(emailValidationEntry.Text); // what is user data here?
                if (textresult)
                {
                    popupLoadingView.IsVisible = false;
                    passwordView.IsVisible = true;
                }
                else
                {
                    await DisplayAlert("", "Email not found, try again", "←");
                }
            }
        }

        private async void Password_ChangeEvent(object sender, EventArgs e)
        {
            if (!(string.Equals(firstPassword.Text, secondPassword.Text)))
            {
                warningLabel.Text = "Enter same password";
                warningLabel.TextColor = Color.SlateBlue;
                warningLabel.IsVisible = true;
            }
            else if ((string.IsNullOrWhiteSpace(firstPassword.Text)) || (string.IsNullOrWhiteSpace(secondPassword.Text)))
            {
                await DisplayAlert("", " Enter Password", "←");
            }
            else
            {
                try
                {
                    var return1 = App.UserDB.UpdatePassword(name, firstPassword.Text);
                    passwordView.IsVisible = false;
                    if (return1)
                    {
                        await DisplayAlert("Password updated", "User data updated", "←");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private async void LoginCommand(object sender, EventArgs e)
        {
            if (User_Name.Text != null && Pass_Word.Text != null)
            {
                var validData = App.UserDB.LoginValidate(User_Name.Text, Pass_Word.Text);
                if (validData)
                {
                    popupLoadingView.IsVisible = false;
                    await Navigation.PushAsync(new HomePage());
                }
                else
                {
                    popupLoadingView.IsVisible = false;
                    await DisplayAlert("Login failed", "Username or Password incorrect", "←");
                }
            }
            else
            {
                await DisplayAlert("", "Enter email and password", "←");
            }
        }
        private void StyleInit(bool OrignalStyle)
        {
            if (OrignalStyle)
            {
                Resources["currentStyle"] = Resources["lightTheme"];
            }
            else
            {
                Resources["currentStyle"] = Resources["darkTheme"];
                loginGrid.BackgroundColor = Color.FromHex("#709fb0");
                signupButton.BackgroundColor = Color.FromHex("#726a95");
            }
        }
    }
}