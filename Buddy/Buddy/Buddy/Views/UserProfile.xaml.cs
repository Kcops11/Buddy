using Buddy.Data;
using Buddy.Models;
using Plugin.Media;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Trigger = Buddy.Models.Trigger;

namespace Buddy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
        private const bool resize = true;

        private bool initalized = false;

        private User user;

        private Models.Trigger trigger = new Models.Trigger();

        protected Member CurrentMember;

        private MemberViewModel MemberView = new MemberViewModel();
        private static ISettings AppSettings =>
             CrossSettings.Current;

        private Buddy.Models.Trigger.TriggerType SelectedTrigger { get; set; }

        private Buddy.Models.Trigger.TriggerReference.ReferenceType SelectedType { get; set; }


        Dictionary<string, Buddy.Models.Trigger.TriggerType> TriggerKeys = new Dictionary<string, Buddy.Models.Trigger.TriggerType>
        {
            { "Switch", Models.Trigger.TriggerType.Switch },
            { "Grounding", Models.Trigger.TriggerType.Grounding },
            { "Trauma", Models.Trigger.TriggerType.Trauma },
        };
        Dictionary<string, Models.Trigger.TriggerReference.ReferenceType> TriggerRefKeys = new Dictionary<string, Buddy.Models.Trigger.TriggerReference.ReferenceType>
        {
            { "Link", Models.Trigger.TriggerReference.ReferenceType.Link },
            { "Image", Models.Trigger.TriggerReference.ReferenceType.Image },
        };


        public UserProfile()
        {
            InitializeComponent();


            user = App.UserDB.GetSpecificUser(App.currentUser); // grab current user // creating new instances of this on every page, maybe go to one instance?

            // home avi needs to have it source changed  

            StyleInit(App.originalStyle);

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
            Name.Text = App.UserDB.GetSpecificUser(1).User_Name;
            BindingContext = MemberView;
            userAV.BindingContext = this;
            user.Members = MemberView.SubMembers;
        }



        private async void SubAviClicked(object sender, System.EventArgs e)
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = resize ? Plugin.Media.Abstractions.PhotoSize.Small : Plugin.Media.Abstractions.PhotoSize.Small,
                    CompressionQuality = 100 //this does nothing
                });

                if (file == null)
                {
                    return;
                }

                if (CurrentMember != null)
                {
                    CurrentMember.AviSource = file.Path;
                }
                else
                {
                    userAV.Source = file.Path;
                    user.UserAvi = file.Path;
                    App.userPath = file.Path;
                }
                SubAviButton.Source = file.Path;
                App.originalAVI = false;
                this.ForceLayout();
            }
        }


        /* Private method section
  */

        /* Sets the style for the current page 
         */
        private void StyleInit(bool OriginalStyle)
        {
            if (!App.originalAVI)
            {
                SubAviButton.Source = App.userPath;
                userAV.Source = App.userPath;
            }
            NameEntry.Text = user.User_Name;
            ProNounsEntry.Text = user.Pro_Nouns;
            BirthdayEntry.Text = user.DoB;
            RoleEntry.Text = "System";
            DescriptEntry.Text = user.SystemDescription;
            if (OriginalStyle)
            {
                Resources["currentStyle"] = Resources["lightTheme"];
                memDisplayGrid.BackgroundColor = Color.White;
                detailsView.BackgroundColor = Color.White;

            }
            else
            {
                Resources["currentStyle"] = Resources["darkTheme"];
                userBoxL.IsVisible = false;
                userBoxD.IsVisible = true;
                memDisplayGrid.BackgroundColor = Color.SlateGray;
                detailsView.BackgroundColor = Color.SlateGray; ;
            }
        }


        private void memDisplayGrid_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CurrentMember = (Member)memDisplayGrid.SelectedItem;
            
            if (CurrentMember.TriggersList == null) // null value here 
            {
                TriggersEntry.Text = "0 Triggers registered";
            }
            else
            {
                TriggersEntry.Text = CurrentMember.TriggersList.Count + " triggers registered.";
            }
            if (CurrentMember.pro_noun == null)
            {
                CurrentMember.pro_noun = "N/A";
            }
            if (CurrentMember.birthday == null)
            {
                CurrentMember.birthday = "N/A";
            }
            if (CurrentMember.role == null)
            {
                CurrentMember.role = "N/A"; //make this a type? some sort of object or setting rather than a string
            }
            if(CurrentMember.AviSource == null)
            {
                SubAviButton.Source = defualtAV.Source;
            }
            else
            {
                SubAviButton.Source = CurrentMember.AviSource;
            }
            NameEntry.Text = CurrentMember.name;
            ProNounsEntry.Text = CurrentMember.pro_noun;
            BirthdayEntry.Text = CurrentMember.birthday;
            DescriptEntry.Text = CurrentMember.description;
            RoleEntry.Text = CurrentMember.role;
            AddTrigger.IsVisible = true;



            if (!App.Current.Properties.ContainsKey("CurrentMember")) // if false create this 
            {
                App.Current.Properties.Add("CurrentMember", CurrentMember);
            }
            else                                                    // else create this
            {
                App.Current.Properties["CurrentMember"] = CurrentMember;
            }
            App.Current.SavePropertiesAsync();                    // save this 
        }

        private void primaryUserP_SizeChanged(object sender, EventArgs e) // wtf is this 
        {

        }

        /** When clicked a entry will appear that allows users 
         *  to register a new trigger. This should then update the num of
         *  registered triggers.
         */
        private void AddTrigger_Clicked(object sender, EventArgs e)
        {
            if (SwitchPicker.Items.Count == 0) // if the entries are blank dont save trigger --> no blank triggers 
            {
                foreach (string triggertype in TriggerKeys.Keys)
                {
                    SwitchPicker.Items.Add(triggertype);
                }
            }
            if (TriggerRefPicker.Items.Count == 0)
            {
                foreach (string triggerRef in TriggerRefKeys.Keys)
                {
                    TriggerRefPicker.Items.Add(triggerRef);
                }
            }

            SwitchPicker.IsVisible = true;
            AddTriggerView.IsVisible = true;
            SelectTrig.IsVisible = true;
            TrigName.IsVisible = true;
            TrigNameEntry.IsVisible = true;
            AddTrigger.IsVisible = false;
            SelectTrig.IsVisible = true;
            TriggerRefPicker.IsVisible = true;
            SelectRef.IsVisible = true;
            RegisterTrigger.IsVisible = true;
        }

        private void TriggerRefPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedType = (Models.Trigger.TriggerReference.ReferenceType)TriggerRefPicker.SelectedIndex;
            if (SelectedType == 0)
            {
                RefURl.IsVisible = true;
                RefImg.IsVisible = false;
            }
            else
            {
                RefImg.IsVisible = true;
                RefURl.IsVisible = false;
            }
        }

        private void SwitchPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTrigger = (Models.Trigger.TriggerType)SwitchPicker.SelectedIndex;
        }

        private void RegisterTrigger_Clicked(object sender, EventArgs e)
        {
            SwitchPicker.IsVisible = false;
            AddTriggerView.IsVisible = false;
            SelectTrig.IsVisible = false;
            TrigName.IsVisible = false;
            TrigNameEntry.IsVisible = false;
            AddTrigger.IsVisible = true;
            SelectTrig.IsVisible = false;
            TriggerRefPicker.IsVisible = false;
            SelectRef.IsVisible = false;
            RegisterTrigger.IsVisible = false;
            RefURl.IsVisible = false;
            RefImg.IsVisible = false; // may need to make a new triggers
            trigger.TrigReference.referenceType = SelectedType;
            trigger.TrigLabel = TrigNameEntry.Text;
            if (SelectedType != Models.Trigger.TriggerReference.ReferenceType.Image)
            {
                trigger.TrigReference.ReferenceSource = RefURl.Text;
            }
            if (CurrentMember.TriggersList != null)
            {
                CurrentMember.TriggersList.Add(trigger);
                TriggersEntry.Text = CurrentMember.TriggersList.Count + " triggers registered.";
            }
            else
            {
                CurrentMember.TriggersList = new List<Trigger>();
                CurrentMember.TriggersList.Add(trigger);
                TriggersEntry.Text = CurrentMember.TriggersList.Count + " triggers registered.";
                trigger.TriggerId = CurrentMember.TriggersList.IndexOf(trigger);
                trigger.Member = CurrentMember;
                App.UserDB.UpdateMemberTrigger(CurrentMember, trigger);
            }
        }

        private async void RefImg_Clicked(object sender, EventArgs e)
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions // this is resetting sometimes, not sure why 
                {
                    PhotoSize = resize ? Plugin.Media.Abstractions.PhotoSize.Small : Plugin.Media.Abstractions.PhotoSize.Small,
                    CompressionQuality = 100 //this does nothing
                });

                if (file == null)
                {
                    return;
                }

                trigger.TrigReference.ReferenceSource = file.Path;

                await App.Current.SavePropertiesAsync();
            }
        }

        private void EditUserButton_Clicked(object sender, EventArgs e)
        {
            SubmitChanges2Sub.IsVisible = true;
            NameEntry.IsEnabled = true;
            BirthdayEntry.IsEnabled = true;
            ProNounsEntry.IsEnabled = true;
            DescriptEntry.IsEnabled = true;
            DescriptEntry.IsEnabled = true;
            SubAviButton.IsEnabled = true;
            if (CurrentMember != null)
            {
                RoleEntry.IsEnabled = true;
            }
            else
            {
                SubAviButton.BackgroundColor = Color.White;
            }
        }

        private void SubmitChanges2Sub_Clicked(object sender, EventArgs e)
        {
            if (CurrentMember == null)
            {
                user.User_Name = NameEntry.Text;
                user.SystemDescription = DescriptEntry.Text;
                user.DoB = BirthdayEntry.Text;
                user.Pro_Nouns = ProNounsEntry.Text;
                user.UserAvi = SubAviButton.Source.ToString();
                App.UserDB.UpdateUser(user);
            }
            else
            {
                CurrentMember.name = NameEntry.Text;
                CurrentMember.description = DescriptEntry.Text;
                CurrentMember.birthday = BirthdayEntry.Text;
                CurrentMember.role = RoleEntry.Text;
                CurrentMember.pro_noun = ProNounsEntry.Text;
                CurrentMember.AviSource = SubAviButton.Source.ToString();
                CurrentMember.user = user; 
                App.UserDB.UpdateUserMembers(user, CurrentMember); // will break, need to set up data table relationships
            }
            SubmitChanges2Sub.IsVisible = false;
            NameEntry.IsEnabled = false;
            BirthdayEntry.IsEnabled = false;
            ProNounsEntry.IsEnabled = false;
            DescriptEntry.IsEnabled = false;
            SubAviButton.IsEnabled = false;
            RoleEntry.IsEnabled = false;
            memDisplayGrid.BeginRefresh();
            //ForceLayout(); // need to try and update the image in the listview // commented this out. Is being updated but dissapears. Is still in list 
        }
    }
}