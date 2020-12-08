using Buddy.Models;
using Xamarin.Forms;

namespace Buddy.Behaviors
{
    /** WordFilterBehavior is intended to prevent users from entering restricted words
     *  in entries that allow raw text.
     *
     *  @author: Dwright@iastate.edu
     */

    public class WordFilterBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }

        /**Creates a new WordFilter Object and scans the passed in text e.
         * Valid is assigned after a call to containsRestricted which just scans the text
         * for restricted words. May need to  be expanded upon at some point to look for
         * characters that may proceed a restricted word.
         */

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            bool IsPositive = false;
            WordFilter filter = new WordFilter();
            IsPositive = filter.ContainsRestricted(e.NewTextValue);
            if (IsPositive)
            {
                ((Entry)sender).Text = string.Empty;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }
    }
}