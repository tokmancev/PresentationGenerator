using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

public class SlideStyle
{
    public SlideStyle(string text)
    {
        Styles.Add(FontStyle.Regular);
        Colors.Add(Color.WhiteSmoke);
        Texts.Add(text);
    }

    private SlideStyle()
    { }

    private static readonly Color DefaultColor = Color.WhiteSmoke;
    public List<Color> Colors = new List<Color>();

    public List<string> Texts = new List<string>();

    private static readonly FontStyle DefaultStyle = FontStyle.Regular;

    public List<FontStyle> Styles = new List<FontStyle>();
    

    public SlideStyle MakeBold()
    {
        Styles[0] = FontStyle.Bold;
        return this;
    }

    public SlideStyle MakeItalic()
    {
        Styles[0] = FontStyle.Italic;
        return this;
    }

    public static bool TryParse(string input, out SlideStyle slideStyle)
    {
        //при усложнении -- стек как в задаче про скобки
        slideStyle = new SlideStyle();
        if (ExtractTag(input, "b", out string text))
        {
            slideStyle.Styles.Add(FontStyle.Bold);
            slideStyle.Texts.Add(text);
            if (slideStyle.Colors.Count > 0)
            {
                slideStyle.Colors.Add(slideStyle.Colors.Last());
            }
            else
            {
                slideStyle.Colors.Add(DefaultColor);
            }
        }
        else if (ExtractTag(input, "i", out text))
        {
            slideStyle.Styles.Add(FontStyle.Italic);
            slideStyle.Texts.Add(text);
            if (slideStyle.Colors.Count > 0)
            {
                slideStyle.Colors.Add(slideStyle.Colors.Last());
            }
            else
            {
                slideStyle.Colors.Add(DefaultColor);
            }
        }
        string patternRGB = @"\[rgb.(\d{1,3}), (\d{1,3}), (\d{1,3}).\]";
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

            //string coloredText = matchRGB.Groups[4].Value;

            int finishIndex = input.IndexOf("[/rgb]", matchRGB.Index);

            int startIndex = matchRGB.Index + matchRGB.Length;
            //string coloredText = matchRGB.Groups[4].Value;
            string coloredText = input.Substring(startIndex, finishIndex-startIndex);
            slideStyle.Texts.Add(
                coloredText
            );
            if (slideStyle.Styles.Count > 0)
            {
                slideStyle.Styles.Add(slideStyle.Styles.Last());
            }
            else
            {
                slideStyle.Styles.Add(DefaultStyle);
            }
            matchRGB = matchRGB.NextMatch();
        }

        //TODO: add color tag handling
        var lastTagIndex = input.IndexOf("</i>");

        var textBegin = (lastTagIndex != -1) ? lastTagIndex + "</i>".Length : 0;

        var lastTagIndex1 = input.IndexOf("</b>");

        var textBegin1 = (lastTagIndex1 != -1) ? lastTagIndex1 + "</b>".Length : 0;

        textBegin = Math.Max(textBegin, textBegin1);

        var lastTagIndex2 = input.IndexOf("[/rgb]");

        var textBegin2 = (lastTagIndex2 != -1) ? lastTagIndex2 + "[/rgb]".Length : 0;

        textBegin = Math.Max(textBegin, textBegin2);

        if (textBegin < input.Length)
        {
            slideStyle.Texts.Add(input.Substring(textBegin));
            slideStyle.Styles.Add(DefaultStyle);
            slideStyle.Colors.Add(
                DefaultColor
            );
        }
        return true;
    }

    private static bool ExtractTag(string input, string tag, out string text)
    {
        //string patternBold = @"<" + tag + @">(\W*.+?\W*)</" + tag + ">";
        //Match match = Regex.Match(input, patternBold);

        //if (match.Success)
        //{
        //    text = match.Groups[1].ToString();
        //    return true;
        //}
        text = string.Empty;
        var startIndex = input.IndexOf("<" + tag + ">");
        if(startIndex == -1)
        {
            return false;
        }
        startIndex += 1 + tag.Length + 1;
        var finishIndex = input.IndexOf("</" + tag + ">");
        if (finishIndex == -1)
        {
            finishIndex = input.Length - 1;
        }
        text = input.Substring(startIndex, finishIndex - startIndex);
        return true;
    }
}