using System.Collections.Generic;
using System.Drawing;
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
        }
        else if (ExtractTag(input, "i", out text))
        {
            slideStyle.Styles.Add(FontStyle.Italic);
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

            string coloredText = matchRGB.Groups[4].Value;
            slideStyle.Texts.Add(
                coloredText
            );

            slideStyle.Styles.Add(DefaultStyle);
            matchRGB = matchRGB.NextMatch();
        }

        if (slideStyle.Styles.Count == 0)
        {
            slideStyle.Styles.Add(DefaultStyle);
        }

        if (slideStyle.Texts.Count == 0)
        {
            slideStyle.Texts.Add(input);
        }

        if (slideStyle.Colors.Count == 0)
        {
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