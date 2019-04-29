using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

public class SlideStyle
{
    public SlideStyle(string text)
    {
        Styles.Add(DefaultStyle);
        Colors.Add(DefaultColor);
        Texts.Add(text);
        Backgrounds.Add(DefaultBackground);
    }

    public SlideStyle()
    { }

    private static readonly Color DefaultColor = Color.WhiteSmoke;
    public List<Color> Colors = new List<Color>();

    public List<string> Texts = new List<string>();

    private static readonly FontStyle DefaultStyle = FontStyle.Regular;

    public List<FontStyle> Styles = new List<FontStyle>();

    public List<Color> Backgrounds = new List<Color>();

    public static readonly Color DefaultBackground = Color.Empty;

    public SlideStyle MakeBold(string text)
    {
        this.SetStyle(text, FontStyle.Bold);
        return this;
    }

    public SlideStyle MakeItalic(string text)
    {
        this.SetStyle(text, FontStyle.Italic);
        return this;
    }

    public static bool TryParse(string input, out SlideStyle slideStyle)
    {
        //при усложнении -- стек как в задаче про скобки
        slideStyle = new SlideStyle();
        if (ExtractTag(input, "b", out string text))
        {
            slideStyle.MakeBold(text);
        }
        else if (ExtractTag(input, "i", out text))
        {
            slideStyle.MakeItalic(text);
        }
        slideStyle.ExtractColor(input);

        int plainTextBegins = FindWhereTagEndsInText(input);
        slideStyle.MakeTextPlain(input, plainTextBegins);
        return true;
    }


    private void ExtractColor(string input)
    {
        HtmlDocument htmlSnippet = new HtmlDocument();
        htmlSnippet.LoadHtml(input);
        if (!htmlSnippet.DocumentNode.HasChildNodes)
        {
            return;
        }
        foreach (HtmlNode node in htmlSnippet.DocumentNode.ChildNodes)
        {
            if (node.Name != "span")
            {
                continue;
            }
            var text = node.InnerText;

            string patternRGB = @"rgb.(\d{1,3}), (\d{1,3}), (\d{1,3})";
            Match matchRGB = Regex.Match(node.Attributes[0].Value, patternRGB);

            if (!matchRGB.Success)
            {
                continue;
            }
            var color = Color.FromArgb(
                int.Parse(matchRGB.Groups[1].Value),
                int.Parse(matchRGB.Groups[2].Value),
                int.Parse(matchRGB.Groups[3].Value)
            );

            if (node.Attributes[0].Value.StartsWith("color"))
            {
                this.SetColor(text, color);
            }
            else
            {
                this.SetBackground(text, color);
            }
        }
    }

    private void SetBackground(string text, Color color)
    {
        this.Texts.Add(text);
        this.Backgrounds.Add(color);

        if (this.Styles.Count > 0)
        {
            this.Styles.Add(this.Styles.Last());
        }
        else
        {
            this.Styles.Add(DefaultStyle);
        }

        if (this.Colors.Count > 0)
        {
            this.Colors.Add(this.Colors.Last());
        }
        else
        {
            this.Colors.Add(DefaultColor);
        }

    }

    private void MakeTextPlain(string input, int textBegin)
    {
        if (textBegin < input.Length)
        {
            this.Texts.Add(input.Substring(textBegin));
            this.Styles.Add(DefaultStyle);
            this.Colors.Add(
                DefaultColor
            );
            this.Backgrounds.Add(Color.Empty);
        }
    }

    // при усложнении использовать паттерн Builder
    private void SetColor(string text, Color color)
    {
        this.Texts.Add(text);
        this.Colors.Add(color);
        this.Backgrounds.Add(DefaultBackground);

        if (this.Styles.Count > 0)
        {
            this.Styles.Add(this.Styles.Last());
        }
        else
        {
            this.Styles.Add(DefaultStyle);
        }
    }

    // при усложнении использовать паттерн Builder
    private void SetStyle(string text, FontStyle fontStyle)
    {
        this.Styles.Add(fontStyle);
        this.Texts.Add(text);
        this.Backgrounds.Add(DefaultBackground);
        if (this.Colors.Count > 0)
        {
            this.Colors.Add(this.Colors.Last());
        }
        else
        {
            this.Colors.Add(DefaultColor);
        }
    }

    private static int FindWhereTagEndsInText(string input)
    {
        string[] TagEnd = new[] { "</i>", "</b>", "</span>" };

        int lastTagIndex, textBegin;
        int maxIndex = 0;
        foreach (var tagEnd in TagEnd)
        {
            lastTagIndex = input.IndexOf(tagEnd);
            textBegin = (lastTagIndex != -1) ? lastTagIndex + tagEnd.Length : 0;
            maxIndex = Math.Max(maxIndex, textBegin);
        }
        return maxIndex;
    }

    private static bool ExtractTag(string input, string tag, out string text)
    {
        text = string.Empty;

        HtmlDocument htmlSnippet = new HtmlDocument();
        htmlSnippet.LoadHtml(input);
        if (htmlSnippet.DocumentNode.HasChildNodes)
        {
            var node = htmlSnippet.DocumentNode.FirstChild;
            if (node.Name != tag)
            {
                return false;
            }
            text = node.InnerText;
        }
        return true;
    }
}