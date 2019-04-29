using System;
using System.Drawing;
using System.Drawing.Imaging;
using Presentation_Generator.Controllers.Fonts;
using Presentation_Generator.Models;

namespace Presentation_Generator.Controllers
{
    public static class SlideSaver
    {
        public static void SaveSlideAsJpeg(Slide slide, string outputPath)
        {
            var backgroundPicture = Image.FromFile(slide.PathToBackgroundPicture);
            var resizedBackground = new Bitmap(backgroundPicture, 800, 600);
            PlaceTitleOnPicture(slide, resizedBackground);
            PlaceTextOnPicture(slide, resizedBackground);
            resizedBackground.Save(outputPath, ImageFormat.Jpeg);
        }

        private static void PlaceTextOnPicture(Slide slide, Bitmap resizedBackground)
        {
            var offset = new Offset(100, 150, WordStyles.CommonFontSize, 400);
            SlideStyle.TryParse(slide.Text, out var slideStyle);

            for (int i = 0; i < slideStyle.Texts.Count; ++i)
            {
                WordStyle wordStyle = WordStyles.GetWordStyle(
                    slideStyle.Styles[i],
                    slideStyle.Colors[i],
                    slideStyle.Backgrounds[i],
                    WordStyles.CommonFontSize);

                var words = ExtractWordsFromText(slideStyle.Texts[i]);
                for (var j = 0; j < words.Length; j++)
                {
                    var word = words[j];
                    if (word.Contains('\n'))
                    {
                        word = word.Replace("\n", "");
                        offset.NewLine();
                    }

                    DrawWord(resizedBackground, word, wordStyle, offset);
                    offset.TryMakeNewLine();
                }
            }
        }

        private static string[] ExtractWordsFromText(string text)
        {
            var words = text
                .Split(new[] { " ", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            words[0] = words[0].Replace("\n", " ");
            return words;
        }

        private static void DrawWord(
            Bitmap resizedBackground, 
            string word, 
            WordStyle wordStyle, 
            Offset offset
        ){
            var graphics = Graphics.FromImage(resizedBackground);
            var wordPosition = GetWordPosition(offset);
            if (wordStyle.backgroundBrush != null)
            {
                graphics.FillRectangle(wordStyle.backgroundBrush, wordPosition);
            }

            graphics.DrawString(
                word,
                wordStyle.Font,
                wordStyle.SolidBrush, 
                wordPosition,
                new StringFormat(StringFormatFlags.NoClip)
            );

            offset.MoveRight(graphics.MeasureString(word, wordStyle.Font).Width);
            //Example, assuming g is your Graphics object, image is your Image object, and color is your Color object:
            //g.FillRectangle(new SolidBrush(color), new Rectangle(Point.Empty, image.Size));
        }

        //???дубль
        private static void DrawTitleText(Graphics titleGraphic, string title,
            WordStyle titleStyle, RectangleF titlePosition)
        {
            titleGraphic.DrawString(title,
                titleStyle.Font,
                titleStyle.SolidBrush, titlePosition,
                new StringFormat(StringFormatFlags.NoClip));
        }

        private static RectangleF GetWordPosition(Offset offset)
        {
            return new RectangleF(offset.StartPosX + offset.X,
                offset.StartPosY + offset.Y,
                800, 50);
        }

        private static void PlaceTitleOnPicture(Slide slide, Bitmap resizedBackground)
        {
            var titleText = slide.Title;
            var titleStyle = WordStyles.TitleStyle;
            var titleGraphic = Graphics.FromImage(resizedBackground);
            var titlePosition = GetTitleTextPosition(titleGraphic, titleText, titleStyle);

            DrawTitleText(titleGraphic, titleText, titleStyle, titlePosition);
        }

        private static RectangleF GetTitleTextPosition(Graphics titleGraphic, string title,
            WordStyle titleStyle)
        {
            var titleMeasurement = titleGraphic.MeasureString(title, titleStyle.Font).Width;
            float titleStartPosX = 400 - (titleMeasurement / 2);
            float titleStartPosY = 50;
            var titlePosition = new RectangleF(titleStartPosX, titleStartPosY, 600, 100);
            return titlePosition;
        }
    }
}