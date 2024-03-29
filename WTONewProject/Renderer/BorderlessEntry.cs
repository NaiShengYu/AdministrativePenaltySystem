﻿using Xamarin.Forms;

namespace WTONewProject.Renderer
{
    public class BorderlessEntry : Entry
    {
        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(BorderlessEntry), false);

        public static readonly BindableProperty XAlignProperty =
           BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(BorderlessEntry),
           TextAlignment.Start);

        public static readonly BindableProperty YAlignProperty =
           BindableProperty.Create("YAlign", typeof(TextAlignment), typeof(BorderlessEntry),
           TextAlignment.Center);


        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }
        /// <summary>
        /// 对齐方式
        /// </summary>
        public TextAlignment XAlign
        {
            get { return (TextAlignment)GetValue(XAlignProperty); }
            set { SetValue(XAlignProperty, value); }
        }

        public TextAlignment YAlign
        {
            get { return (TextAlignment)GetValue(YAlignProperty); }
            set { SetValue(YAlignProperty, value); }
        }

    }
}
