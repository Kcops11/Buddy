
using Buddy.Data;
using Buddy.Views;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using rootnamespace.Helpers;
using System;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;

namespace Buddy
{
    public partial class App : Application
    {
        private static UserDB userDB;

        public string IsFirstTime
        {
            get { return Settings.GeneralSettings; }
            set
            {
                if (Settings.GeneralSettings == value)
                    return;
                Settings.GeneralSettings = value;
                OnPropertyChanged();
            }
        }
        public static bool originalStyle
        {
            get => AppSettings.GetValueOrDefault(nameof(originalStyle), true);
            set => AppSettings.AddOrUpdateValue(nameof(originalStyle), value);
        }

        public static bool originalAVI
        {
            get => AppSettings.GetValueOrDefault(nameof(originalAVI), true);
            set => AppSettings.AddOrUpdateValue(nameof(originalAVI), value);
        }

        public static bool isAnimating
        {
            get => AppSettings.GetValueOrDefault(nameof(isAnimating), false);
            set => AppSettings.AddOrUpdateValue(nameof(isAnimating), value);
        }

        public static bool animationEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(animationEnabled), true);
            set => AppSettings.AddOrUpdateValue(nameof(animationEnabled), value);
        }

        public static string userPath
        {
            get => AppSettings.GetValueOrDefault(nameof(userPath), string.Empty); 
            set => AppSettings.AddOrUpdateValue(nameof(userPath), value);
        }
        public static int currentUser
        {
            get => App.AppSettings.GetValueOrDefault(nameof(currentUser), 0);
            set => App.AppSettings.AddOrUpdateValue(nameof(currentUser), value);
        }
        private static ISettings AppSettings =>
              CrossSettings.Current;

        public static UserDB UserDB
        {
            get
            {
                if (userDB == null)
                {
                    userDB = new UserDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.db3"));
                }
                return userDB;
            }
        }

        public App()
        {
            InitializeComponent();
           if(IsFirstTime == "yes")
            {
                AppSettings.Clear();
                IsFirstTime = "no";
                // add a value in user that tells whether or not its their first time using the app and pull value from there to here
                MainPage = new NavigationPage(new RegisterPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage()) { BackgroundColor = Color.Transparent };
            }
         
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}