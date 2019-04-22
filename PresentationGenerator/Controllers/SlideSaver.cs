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
            var offset = new Offset(100, 150, WordStyles.CommonFontSize,400);
            //TODO: fix tags in Regex
            slide.Text = slide.Text.Replace("<span style=\"color: ", "[");
            slide.Text = slide.Text.Replace(");\">", ")]");
            slide.Text = slide.Text.Replace("</span>", "[/rgb]");
            SlideStyle.TryParse(slide.Text, out var slideStyle);
            
            for (int i = 0; i < slideStyle.Texts.Count; ++i)
            {
                WordStyle wordStyle = WordStyles.GetWordStyle(
                    slideStyle.Styles[i],
                    slideStyle.Colors[i],
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

                //DrawWord(resizedBackground, slideStyle.Texts[i], wordStyle, offset);
            }
        }

        private static string[] ExtractWordsFromText(string text)
        {
            var words = text
                .Split(new[] { " ", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            words[0] = words[0].Replace("\n", " ");
            return words;
        }


        /*private static void PlaceTextOnPicture(Slide slide, Bitmap resizedBackground)
        {
            var offset = new Offset(100, 150, WordStyles.CommonFontSize,400);
            var words = ExtractWordsFromText(slide);
            var wordStyle = WordStyles.CommonTextStyle;
            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (words[i].Contains("<"))
                {
                    wordStyle = WordStyles.GetWordStyle(words[i], WordStyles.CommonFontSize);
                    word = words[i].Remove(0, words[i].IndexOf('>') + 1);
                }
                word = word.Replace("$]", "");
                if (word.Contains('\n'))
                {
                    word = word.Replace("\n", "");
                    offset.NewLine();
                }
                             
                DrawWord(resizedBackground, word, wordStyle, offset);
                if (words[i].Contains("$]")) wordStyle = WordStyles.CommonTextStyle;
                offset.TryMakeNewLine();
            }
        }
        */

        private static void DrawWord(Bitmap resizedBackground, string word, WordStyle wordStyle, Offset offset)
        {
            var graphics = Graphics.FromImage(resizedBackground);
            var wordPosition = GetWordPosition(offset);
            graphics.DrawString(word,
                wordStyle.Font,
                wordStyle.SolidBrush, wordPosition,
                new StringFormat(StringFormatFlags.NoClip));
            offset.MoveRight(graphics.MeasureString(word, wordStyle.Font).Width);
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