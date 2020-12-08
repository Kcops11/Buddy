using Buddy.Models;
using Xamarin.Forms;

namespace Buddy.Behaviors
{
    /** BirthdayFormatBehavior is inteded to prevent users from entering anything other
     *  than numbers in the entry that wants bithdate information. This behavior also
     *  automatically formats the entered birthdate.
     */

    public class BirthdateFormatBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            NumberFilter num = new NumberFilter();
            ((Entry)sender).Text = num.letterFilter(e.NewTextValue);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}