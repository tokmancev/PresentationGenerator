using NUnit.Framework;
using Presentation_Generator.Controllers;
using Presentation_Generator.Models;
using System.Drawing;


namespace Tests
{
    public class Tests
    {
        [Test]
        public void SmokeTest()
        {
            var slideStyle = new SlideStyle("Hello");
        }

        [Test]
        public void Should_have_default_style()
        {
            var slideStyle = new SlideStyle("Hello");
            Assert.AreEqual(FontStyle.Regular, slideStyle.Styles[0]);            
        }

        [Test]
        public void Should_have_bold_style()
        {
            var slideStyle = new SlideStyle("Hello");
            slideStyle.MakeBold();
            Assert.AreEqual(FontStyle.Bold, slideStyle.Styles[0]);
        }

        [Test]
        public void Should_have_white_color_by_default()
        {
            var slideStyle = new SlideStyle("Hello");
            Assert.AreEqual(Color.WhiteSmoke, slideStyle.Colors[0]);
        }


        [Test]
        public void Should_have_regular_style_by_default()
        {
            SlideStyle.TryParse("test", out var slideStyle);
            Assert.AreEqual(Color.WhiteSmoke, slideStyle.Colors[0]);
            Assert.AreEqual(FontStyle.Regular, slideStyle.Styles[0]);
            Assert.AreEqual("test", slideStyle.Texts[0]);
        }

        [Test]
        public void Should_have_bold_style_when_text_tag()
        {
            SlideStyle.TryParse("<b>\r\n\"test\"</b>", out var slideStyle);
            Assert.AreEqual(FontStyle.Bold, slideStyle.Styles[0]);
            Assert.AreEqual("\r\n\"test\"", slideStyle.Texts[0]);
        }

        [Test]
        public void Make_italic_from_tag()
        {
            SlideStyle.TryParse("<i>test</i>", out var slideStyle);
            Assert.AreEqual(FontStyle.Italic, slideStyle.Styles[0]);
        }

        [Test]
        public void Should_have_two_parts_of_text()
        {
            SlideStyle.TryParse("<i>test</i> styles", out var slideStyle);
            Assert.AreEqual(2, slideStyle.Texts.Count);
            Assert.AreEqual(" styles", slideStyle.Texts[1]);
            Assert.AreEqual(2, slideStyle.Colors.Count);
        }

        [Test]
        public void Should_have_two_styles_of_text()
        {
            SlideStyle.TryParse("<b>test</b> styles", out var slideStyle);
            Assert.AreEqual(2, slideStyle.Texts.Count);
            Assert.AreEqual(" styles", slideStyle.Texts[1]);
            Assert.AreEqual(2, slideStyle.Colors.Count);
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
            SlideStyle.TryParse("[rgb(255, 0, 0)]test[/rgb]", out var slideStyle);
            Assert.IsTrue(ColorsAreEqual(Color.Red, slideStyle.Colors[0]));
        }

        [Test]
        public void Should_be_blue_by_tag()
        {
            SlideStyle.TryParse("[rgb(0, 0, 255)]test[/rgb]", out var slideStyle);
            Assert.IsTrue(ColorsAreEqual(Color.Blue, slideStyle.Colors[0]));
        }

        [Test]
        public void Should_be_two_colors_by_tag()
        {
            SlideStyle.TryParse("[rgb(255, 0, 0)]My[/rgb] [rgb(0, 0, 255)]test[/rgb]", out var slideStyle);

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
        [Test]
        public void Should_be_red_text_on_picture()
        {
            Slide slide = new Slide();
            slide.Text = "[rgb(255, 0, 0)]test[/rgb]";
            slide.PathToBackgroundPicture = "background.jpg";
            SlideSaver.SaveSlideAsJpeg(slide, "0.jpg");
        }

        [Test]
        public void Should_be_two_colors_on_picture()
        {
            Slide slide = new Slide();
            slide.Text = "[rgb(255, 0, 0)]My[/rgb] [rgb(0, 0, 255)]test[/rgb]";
            slide.PathToBackgroundPicture = "background.jpg";
            SlideSaver.SaveSlideAsJpeg(slide, "1.jpg");
        }
    }
}