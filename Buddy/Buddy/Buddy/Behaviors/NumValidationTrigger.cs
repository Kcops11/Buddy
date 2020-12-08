using Xamarin.Forms;

namespace Buddy.Behaviors
{
    public class NumValidationTrigger : TriggerAction<Entry>
    {
        private string _prevValue = string.Empty;

        protected override void Invoke(Entry entry)
        {
            int n;
            var isNumeric = int.TryParse(entry.Text, out n);

            if (!string.IsNullOrWhiteSpace(entry.Text) && (entry.Text.Length > 8 || !isNumeric))
            {
                entry.Text = _prevValue;
                return;
            }

            _prevValue = entry.Text;
        }
    }
}