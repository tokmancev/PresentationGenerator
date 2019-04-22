using NUnit.Framework;
using Presentation_Generator.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void SmokeTest()
        {
            var slideStyle = new SlideStyle("Hello");
            Assert.Pass();
        }

        [Test]
        public void Should_have_default_style()
        {
            var slideStyle = new SlideStyle("Hello");
            Assert.AreEqual(SlideStyle.TextStyle.Regular, slideStyle.Styles[0]);            
        }

        [Test]
        public void Should_have_bold_style()
        {
            var slideStyle = new SlideStyle("Hello");
            slideStyle.MakeBold();
            Assert.AreEqual(SlideStyle.TextStyle.Bold, slideStyle.Styles[0]);
        }

        [Test]
        public void Should_have_white_color_by_default()
        {
            var slideStyle = new SlideStyle("Hello");
            Assert.AreEqual(Color.WhiteSmoke, slideStyle.Colors[0]);
        }

        [Test]
        public void Should_have_bold_style_when_text_tag()
        {
            SlideStyle.TryParse("[bold]test[/bold]", out var slideStyle);
            Assert.AreEqual(SlideStyle.TextStyle.Bold, slideStyle.Styles[0]);
        }

        [Test]
        public void Make_italic_from_tag()
        {
            SlideStyle.TryParse("[italic]test[/italic]", out var slideStyle);
            Assert.AreEqual(SlideStyle.TextStyle.Italic, slideStyle.Styles[0]);
        }
        private bool ColorsAreEqual(Color color1, Color color2)
        {
            return color1.R == color2.R
                && color1.G == color2.G
                && color1.B == color2.B
                && color1.A == color2.A;
        }
        [Test]
        public void Should_change_color_by_tag()
        {
            SlideStyle.TryParse("[rgb(255,0,0)]test[/rgb]", out var slideStyle);
            Assert.IsTrue(ColorsAreEqual(Color.Red, slideStyle.Colors[0]));
        }

        [Test]
        public void Should_be_blue_by_tag()
        {
            SlideStyle.TryParse("[rgb(0,0,255)]test[/rgb]", out var slideStyle);
            Assert.IsTrue(ColorsAreEqual(Color.Blue, slideStyle.Colors[0]));
        }

        [Test]
        public void Should_be_two_colors_by_tag()
        {
            SlideStyle.TryParse("[rgb(255,0,0)]My[/rgb] [rgb(0,0,255)]test[/rgb]", out var slideStyle);

            var expectedColor = new Color[] { Color.Red, Color.Blue };
            var textElements = new string[] { "My", "test" };

            var colorIterator = slideStyle.Colors.GetEnumerator();
            var textIterator = slideStyle.Texts.GetEnumerator();

            for (int i = 0; i < expectedColor.Length; ++i)
            {
                Assert.IsTrue(colorIterator.MoveNext());
                Assert.IsTrue(ColorsAreEqual(expectedColor[i], colorIterator.Current));
                Assert.IsTrue(textIterator.MoveNext());
                Assert.AreEqual(textElements[i], textIterator.Current);
            }
        }

    }

    public class SlideStyle
    {
        public SlideStyle(string text)
        {
            Styles.Add(TextStyle.Regular);
            Colors.Add(Color.WhiteSmoke);
            Texts.Add(text);
        }

        private SlideStyle()
        {}
        public List<Color> Colors = new List<Color>();
        
        public List<string> Texts = new List<string>();

        public enum TextStyle
        {
            Regular,
            Bold,
            Italic
        }

        public List<TextStyle> Styles = new List<TextStyle>();
        

        public SlideStyle MakeBold()
        {
            Styles[0] = TextStyle.Bold;
            return this;
        }

        public SlideStyle MakeItalic()
        {
            Styles[0] = TextStyle.Italic;
            return this;
        }

        public static bool TryParse(string input, out SlideStyle slideStyle)
        {
            slideStyle = new SlideStyle();
            if (ExtractTag(input, "bold", out string text))
            {
                slideStyle.Styles.Add(TextStyle.Bold);
                slideStyle.Texts.Add(text);
            }

            if (ExtractTag(input, "italic", out text))
            {
                slideStyle.Styles.Add(TextStyle.Italic);
                slideStyle.Texts.Add(text);
            }

            string patternRGB = @".rgb.(\d{1,3}),(\d{1,3}),(\d{1,3})..(.+?)./rgb.";
            Match matchRGB = Regex.Match(input, patternRGB);

            while (matchRGB.Success)
            {
                Color color = Color.FromArgb(
                    int.Parse(matchRGB.Groups[1].Value),
                    int.Parse(matchRGB.Groups[2].Value),
                    int.Parse(matchRGB.Groups[3].Value)
                );

                slideStyle.Colors.Add(
                    color
                );

                slideStyle.Texts.Add(
                    matchRGB.Groups[4].Value
                );

                matchRGB = matchRGB.NextMatch();
            }

            return true;
        }

        private static bool ExtractTag(string input, string tag, out string text)
        {
            string patternBold = @"." + tag + ".(.+?)./" + tag + ".";
            Match match = Regex.Match(input, patternBold);

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