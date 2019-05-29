using System;
using System.ComponentModel;
using UIKit;
using WTONewProject.iOS.Renderer;
using WTONewProject.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]

namespace WTONewProject.iOS.Renderer
{
    public class BorderlessEntryRenderer:EntryRenderer
    {
        public static void Init() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            var view = e.NewElement as BorderlessEntry;
            SetBorder(view);
            SetTextAlignment(view);

        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (BorderlessEntry)Element;

            if (e.PropertyName == BorderlessPicker.HasBorderProperty.PropertyName)
                SetBorder(view);
            if (e.PropertyName == BorderlessEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
        }

        private void SetBorder(BorderlessEntry view)
        {
            if (view == null || Control == null)
            {
                return;
            }
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }


        private void SetTextAlignment(BorderlessEntry view)
        {
            if (view == null || Control == null)
            {
                return;
            }
            switch (view.XAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

    }
}
