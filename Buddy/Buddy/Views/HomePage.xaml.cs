
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Buddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class HomePage : ContentPage
    {
        private double PostX;

        private static ISettings AppSettings =>
            CrossSettings.Current;
        public HomePage()
        {
            InitializeComponent();

            StyleInit(App.originalStyle);

            if (!App.originalAVI) { userAvi.Source = App.userPath; }

            NavigationPage.SetHasBackButton(this, false);

        }

        private async void Settings_Clicked(object sender, System.EventArgs e)
        {
            if (App.animationEnabled) // may remove and generalize
            {
                if (!(App.isAnimating))
                {
                    if (!(moreView.IsVisible))
                    {
                        App.isAnimating = true;
                        //anim in
                        new Animation
                    {{0, 1, new Animation (v => moreView.TranslationX = v, 0, 224 ) },
                     {0, 1, new Animation(v => divider.TranslationX = v, 0,23 ) },
                     {0, 1, new Animation ( v => settingsGroup.TranslationX = v, -160 , 50 ) }
                    }
                        .Commit(this, "slideAnimation", length: 350, easing: Easing.Linear, finished: (x, y) =>
                          App.isAnimating = false);

                        moreView.IsVisible = !moreView.IsVisible;
                        divider.IsVisible = !divider.IsVisible;
                        settingsGroup.IsVisible = !settingsGroup.IsVisible;
                    }
                    else
                    {
                        App.isAnimating = true;
                        //anim out
                        new Animation
                    {{0, 1, new Animation (v => moreView.TranslationX = v, 224, 0 ) },
                     {0, 1, new Animation (v => divider.TranslationX = v, 23,0 ) },
                     {0, 1, new Animation (v => settingsGroup.TranslationX = v, 0 , -160 ) }
                    }
                        .Commit(this, "slideAnimation", length: 350, easing: Easing.Linear, finished: (x, y) =>
                          App.isAnimating = false);

                        await Task.Delay(350);
                        moreView.IsVisible = !moreView.IsVisible;
                        divider.IsVisible = !divider.IsVisible;
                        settingsGroup.IsVisible = !settingsGroup.IsVisible;
                    }
                }
            }
            else
            {
                moreView.IsVisible = !moreView.IsVisible;
            }
        }

        private void settings_Clicked_2(object sender, System.EventArgs e)
        {
            // push new page with settings
            // probably gonna leave this blank for the time being until I know what I want in here
            // where is this in homepage?
        }

        private async void logout_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private void darkModeToggle_Clicked(object sender, System.EventArgs e)
        {
            if (App.originalStyle) { StyleInit(false); }
            AppSettings.AddOrUpdateValue("originalStyle", false);
        }

        private void lightModeToggle_Clicked(object sender, System.EventArgs e)
        {
            if (!(App.originalStyle)) { StyleInit(true); }
            AppSettings.AddOrUpdateValue("originalStyle", true);
        }

        private async void userAvi_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new UserProfile());
        }

        private void journal_Clicked(object sender, System.EventArgs e)
        {
        }

        private void stats_Clicked(object sender, System.EventArgs e)
        {
        }

        private void addMemb_Clicked(object sender, System.EventArgs e)
        {
        }

        private void common_Clicked(object sender, System.EventArgs e)
        {
        }

        /* Private method section
         */

        /* Sets the style for the current page 
         */
        private void StyleInit(bool OriginalStyle)
        {
            if (OriginalStyle)
            {
                Resources["currentStyle"] = Resources["lightTheme"];

                App.Current.Properties["originalStyle"] = true;
                popupBlack.IsVisible = false;
                popupWhite.IsVisible = true;
                darkModeToggle.IsVisible = true;
                lightModeToggle.IsVisible = false;
                persistentD.IsVisible = false;
                persistentL.IsVisible = true;
                accentViewL.IsVisible = true;
                accentViewD.IsVisible = false;
                buttonBorder.Color = Color.White;
                divider.Color = Color.FromHex("#f3f3f3");// can I do this in a better way?

            }
            else
            {
                Resources["currentStyle"] = Resources["darkTheme"];

                App.Current.Properties["originalStyle"] = false;
                darkModeToggle.IsVisible = false;
                lightModeToggle.IsVisible = true;
                persistentL.IsVisible = false;
                persistentD.IsVisible = true;
                popupBlack.IsVisible = true;
                popupWhite.IsVisible = false;
                accentViewD.IsVisible = true;
                accentViewL.IsVisible = false;
                buttonBorder.Color = Color.SlateGray;
                divider.Color = Color.FromHex("#1d3318");
            }
            App.originalStyle = OriginalStyle;
            App.Current.SavePropertiesAsync();
        }

        private void cV_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this.Width > 100)
            {

                if (Settings.X > 0)
                {

                    PostX = Application.Current.MainPage.Width - 250; // not setting properly when app is larger than expected 

                    Constraint newX = Constraint.Constant(PostX);

                    RelativeLayout.SetXConstraint(aviGroup, newX);

                }

            }
        }
    }
}