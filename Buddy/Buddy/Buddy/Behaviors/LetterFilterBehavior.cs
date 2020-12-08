using Buddy.Models;
using Xamarin.Forms;

namespace Buddy.Behaviors
{
    /** LetterFilterBehavior is intended to filter out letters in entries asking for
     *  numerical imput.
     */

    internal class LetterFilterBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            NumFilter num = new NumFilter();
            ((Entry)sender).Text = num.letterFilter(e.NewTextValue);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}