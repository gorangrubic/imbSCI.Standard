using System.ComponentModel;

namespace Svg
{
    [DefaultProperty("Text")]
    [SvgElement("desc")]
    public class SvgDescription : SvgElement
    {
        private string _text;

        public string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}