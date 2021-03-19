namespace PakAnas.Commons.Models
{
    public class TextValue
    {
        public string Text { set; get; }
        public object Value { set; get; }

        public TextValue()
        {

        }

        public TextValue(string text, object value)
        {
            this.Text = text;
            this.Value = value;
        }
    }
}