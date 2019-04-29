using System.Drawing;

namespace Presentation_Generator.Controllers.Fonts
{

    public class WordStyle
    {
        public Font Font { get; }
        public SolidBrush SolidBrush { get; }
        public SolidBrush backgroundBrush { get; }

        public WordStyle(Font font, SolidBrush textBrush, SolidBrush backBrush = null)
        {
            Font = font;
            SolidBrush = textBrush;
            backgroundBrush = backBrush;
        }

        public override bool Equals(object obj)
        {
            var objAsWordStyle = obj as WordStyle;
            return objAsWordStyle != null &&
                   objAsWordStyle.Font.Equals(Font) &&
                   objAsWordStyle.SolidBrush.Color.Equals(SolidBrush.Color);
        }

        public override int GetHashCode()
        {
            return Font.GetHashCode() * SolidBrush.GetHashCode();
        }
    }
}