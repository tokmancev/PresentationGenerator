using NUnit.Framework;
using Presentation_Generator.Models;
using System.Drawing;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void SmokeTest()
        {
            var slide = new Slide();
            Assert.Pass();
        }

        [Test]
        public void Should_have_default_style()
        {
            var slide = new Slide();
            Assert.AreEqual(Slide.TextStyle.Default, slide.Style);            
        }

        [Test]
        public void Should_have_bold_style()
        {
            var slide = new Slide();
            slide.MakeBold();
            Assert.AreEqual(Slide.TextStyle.Bold, slide.Style);
        }

        [Test]
        public void Should_have_white_color_by_default()
        {
            var slide = new Slide();
            Assert.AreEqual(Color.WhiteSmoke, slide.Color);
        }

        [Test]
        public void Should_have_bold_style_when_text_tag()
        {
            Slide.TryParse("[bold]test[/bold]", out var slide);
            Assert.AreEqual(Slide.TextStyle.Bold, slide.Style);
        }

        [Test]
        public void Make_italic_from_tag()
        {
            Slide.TryParse("[italic]test[/italic]", out var slide);
            Assert.AreEqual(Slide.TextStyle.Italic, slide.Style);
        }
        [Test]
        public void Should_change_color_by_tag()
        {
            Slide.TryParse("[rgb(255,0,0)]test[/rgb]", out var slide);
            Assert.AreEqual(Color.Red.R, slide.Color.R);
            Assert.AreEqual(Color.Red.G, slide.Color.G);
            Assert.AreEqual(Color.Red.B, slide.Color.B);
            Assert.AreEqual(Color.Red.A, slide.Color.A);
        }

        [Test]
        public void Should_be_blue_by_tag()
        {
            Slide.TryParse("[rgb(0,0,255)]test[/rgb]", out var slide);
            Assert.AreEqual(Color.Blue.R, slide.Color.R);
            Assert.AreEqual(Color.Blue.G, slide.Color.G);
            Assert.AreEqual(Color.Blue.B, slide.Color.B);
            Assert.AreEqual(Color.Blue.A, slide.Color.A);
        }
    }
}