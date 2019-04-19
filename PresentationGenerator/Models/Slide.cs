using System.Runtime.Serialization;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Presentation_Generator.Models
{
    [DataContract]
    public class Slide
    {
        [DataMember]
        public string PathToBackgroundPicture { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Text { get; set; }

        public enum TextStyle
        {
            Default,
            Bold,
            Italic
        }

        public TextStyle Style { get; set;} = TextStyle.Default;
        public Color Color { get; set; } = Color.WhiteSmoke;

        public Slide MakeBold()
        {
            Style = TextStyle.Bold;
            return this;
        }

        public Slide MakeItalic()
        {
            Style = TextStyle.Italic;
            return this;
        }

        public static bool TryParse(string input, out Slide slide)
        {
            slide = new Slide();
            if (ExtractTag(input, "bold", out string text))
            {
                slide.MakeBold();
                slide.Text = text;
            }

            if (ExtractTag(input, "italic", out text))
            {
                slide.MakeItalic();
                slide.Text = text;
            }

            string patternRGB = @".rgb\((\d{1,3}),(\d{1,3}),(\d{1,3})\).(.+)./rgb.";
            Regex regExprRGB = new Regex(patternRGB, RegexOptions.Singleline);
            Match matchRGB = regExprRGB.Match(input);

            if (matchRGB.Success)
            {
                slide.Color = Color.FromArgb(
                    int.Parse(matchRGB.Groups[1].ToString()),
                    int.Parse(matchRGB.Groups[2].ToString()),
                    int.Parse(matchRGB.Groups[3].ToString())
                );
                slide.Text = text;
            }

            return true;
        }

        private static bool ExtractTag(string input, string tag, out string text)
        {
            string patternBold = @"." + tag + ".(.+)./" + tag + ".";
            Regex regExpr = new Regex(patternBold, RegexOptions.Singleline);
            Match match = regExpr.Match(input);

            if (match.Success)
            {
                text = match.Groups[1].ToString();
                return true;
            }
            text = string.Empty;
            return false;
        }
    }
}