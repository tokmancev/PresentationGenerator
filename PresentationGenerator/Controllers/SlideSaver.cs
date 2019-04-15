using System;
using System.IO;
using Presentation_Generator.Models;
using SkiaSharp;

namespace Presentation_Generator.Controllers
{
    public static class SlideSaver
    {
        public static void SaveSlideAsJpeg(Slide slide, string outputPath)
        {
            LoadBackgroundFromFile(slide.PathToBackgroundPicture, out SKBitmap backgroundPicture);
            const int fontSize = 26;
            DrawWord(
                backgroundPicture, 
                "Hello!",
                fontSize,
                new Offset(100, 150, fontSize, 400)
            );
            //PlaceTitleOnPicture(slide, backgroundPicture);
            //PlaceTextOnPicture(slide, backgroundPicture);
            SaveBackgroundToFile(outputPath, backgroundPicture);
        }

        private static void DrawWord(SKBitmap backgroundPicture, string word, int fontSize, Offset offset)
        {
            //var graphics = Graphics.FromImage(backgroundPicture);
            //var wordPosition = GetWordPosition(offset);
            //graphics.DrawString(word,
            //    wordStyle.Font,
            //    wordStyle.SolidBrush, wordPosition,
            //    new StringFormat(StringFormatFlags.NoClip));
            //offset.MoveRight(graphics.MeasureString(word, wordStyle.Font).Width);
            using (var canvas = new SKCanvas(backgroundPicture))
            {
                using (var paint = new SKPaint())
                {
                    
                    paint.TextSize = fontSize;
                    paint.IsAntialias = true;
                    paint.Color = new SKColor(0xE6, 0xB8, 0x9C);
                    paint.IsStroke = false;
                    var wordPosition = GetWordPosition(offset);

                    canvas.DrawText(word, wordPosition.Left, wordPosition.Bottom, paint);
                }
            }

        }

        private static bool SaveBackgroundToFile(string outputPath, SKBitmap backgroundPicture)
        {
            bool result = false;
            using (Stream fileStream = File.OpenWrite(outputPath))
            {
                using (SKManagedWStream wstream = new SKManagedWStream(fileStream))
                {
                    //backgroundPicture.Save(outputPath);
                    result = backgroundPicture.Encode(wstream, SKEncodedImageFormat.Jpeg, 10);//10???
                }
            }
            return result;
        }

        public static bool LoadBackgroundFromFile(string pathToFile, out SKBitmap background)
        {
            Stream fileStream = File.OpenRead(pathToFile);
            using (var stream = new SKManagedStream(fileStream))
                background = SKBitmap.Decode(stream);

            return background.ScalePixels(background, SKFilterQuality.Medium);
        }

        private static SKRect GetWordPosition(Offset offset)
        {
            return new SKRect(offset.StartPosX + offset.X,
                offset.StartPosY + offset.Y,
                800, 50);
        }

        //private static void PlaceTextOnPicture(Slide slide, SKBitmap backgroundPicture)
        //{
        //    var offset = new Offset(100, 150, WordStyles.CommonFontSize,400);
        //    var words = ExtractWordsFromText(slide);
        //    var wordStyle = WordStyles.CommonTextStyle;
        //    for (var i = 0; i < words.Length; i++)
        //    {
        //        var word = words[i];
        //        if (words[i].Contains("<"))
        //        {
        //            wordStyle = WordStyles.GetWordStyle(words[i], WordStyles.CommonFontSize);
        //            word = words[i].Remove(0, words[i].IndexOf('>') + 1);
        //        }
        //        word = word.Replace("$]", "");
        //        if (word.Contains('\n'))
        //        {
        //            word = word.Replace("\n", "");
        //            offset.NewLine();
        //        }

        //        DrawWord(backgroundPicture, word, wordStyle, offset);
        //        if (words[i].Contains("$]")) wordStyle = WordStyles.CommonTextStyle;
        //        offset.TryMakeNewLine();
        //    }
        //}



        //private static string[] ExtractWordsFromText(Slide slide)
        //{
        //    var words = slide.Text
        //        .Split(new[] { " ", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        //    words[0] = words[0].Replace("\n", " ");
        //    return words;
        //}

        //private static void PlaceTitleOnPicture(Slide slide, SKBitmap backgroundPicture)
        //{
        //    var titleText = slide.Title;
        //    var titleStyle = WordStyles.TitleStyle;
        //    var titleGraphic = Graphics.FromImage(backgroundPicture);
        //    var titlePosition = GetTitleTextPosition(titleGraphic, titleText, titleStyle);

        //    DrawTitleText(titleGraphic, titleText, titleStyle, titlePosition);
        //}

        //private static void DrawTitleText(SKCanvas canvas, string title, SKRect titlePosition)
        //{
        //    //titleGraphic.DrawString(title,
        //    //    titleStyle.Font,
        //    //    titleStyle.SolidBrush, titlePosition,
        //    //    new StringFormat(StringFormatFlags.NoClip));
        //    canvas.Clear(SKColors.White);
        //    using (var paint = new SKPaint())
        //    {
        //        paint.TextSize = 64.0f;
        //        paint.IsAntialias = true;
        //        paint.Color = new SKColor(0xE6, 0xB8, 0x9C);
        //        paint.IsStroke = false;

        //        canvas.DrawText(title, titlePosition.Left, titlePosition.Bottom, paint);
        //    }
        //}

        //private static SKRect GetTitleTextPosition(SKCanvas canvas, string title,
        //    WordStyle titleStyle)
        //{
        //    var titleMeasurement = canvas.MeasureString(title, titleStyle.Font).Width;
        //    float titleStartPosX = 400 - (titleMeasurement / 2);
        //    float titleStartPosY = 50;
        //    var titlePosition = new SKRect(titleStartPosX, titleStartPosY, 600, 100);
        //    return titlePosition;
        //}
    }
}