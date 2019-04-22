﻿using System;
using System.Drawing;

namespace Presentation_Generator.Controllers.Fonts
{
    public static class WordStyles
    {
        private static readonly Lazy<WordStyle> _titleStyleLazy;
        private static readonly Lazy<WordStyle> _commonTextStyleLazy;
        private static WordStyle _titleStyle;
        private static WordStyle _commonTextStyle;

        static WordStyles()
        {
            _titleStyleLazy = new Lazy<WordStyle>(InitTitleStyle);
            _commonTextStyleLazy=new Lazy<WordStyle>(InitCommonTextStyle);
            InitFontSizesByDefault();
        }

        #region public
        public static WordStyle TitleStyle => GetTitleStyle();
        public static WordStyle CommonTextStyle => GetCommonTextStyle();

        public static int TitleFontSize { get; set; }
        public static int CommonFontSize { get; set; }

        public static WordStyle GetWordStyle(FontStyle style, Color color, int fontSize)
        {
            var brush = new SolidBrush(color);
            return new WordStyle(new Font("Arial", fontSize, style), brush);
        }
        #endregion

        private static void InitFontSizesByDefault()
        {
            TitleFontSize = 30;
            CommonFontSize = 26;
        }

        #region InterationWithPrivateFields
        private static WordStyle GetTitleStyle()
        {
            if (!_titleStyleLazy.IsValueCreated)
                _titleStyle = _titleStyleLazy.Value;
            return _titleStyle;
        }

        private static WordStyle GetCommonTextStyle()
        {
            if (!_commonTextStyleLazy.IsValueCreated)
                _commonTextStyle = _commonTextStyleLazy.Value;
            return _commonTextStyle;
        } 
        #endregion

        #region LaziesInit
        private static WordStyle InitTitleStyle()
        {
            return new WordStyle(new Font("Arial", TitleFontSize, FontStyle.Bold),
                new SolidBrush(Color.WhiteSmoke));
        }

        private static WordStyle InitCommonTextStyle()
        {
            return new WordStyle(new Font("Arial", CommonFontSize, FontStyle.Regular),
                new SolidBrush(Color.WhiteSmoke));
        } 
        #endregion
    }
}