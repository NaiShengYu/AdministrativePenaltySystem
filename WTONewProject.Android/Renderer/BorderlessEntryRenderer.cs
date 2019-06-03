using System;
using System.ComponentModel;
using Android.Content;
using WTONewProject.Droid.Renderer;
using WTONewProject.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]

namespace WTONewProject.Droid.Renderer
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            BorderlessEntry view = e.NewElement as BorderlessEntry;
            SetBorder(view);
            SetTextXAlignment(view);
            SetTextYAlignment(view);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (BorderlessEntry)Element;

            if (e.PropertyName == BorderlessEntry.HasBorderProperty.PropertyName)
                SetBorder(view);
            if (e.PropertyName == BorderlessEntry.XAlignProperty.PropertyName)
                SetTextXAlignment(view);
            if (e.PropertyName == BorderlessEntry.YAlignProperty.PropertyName)
                SetTextYAlignment(view);

        }


        private void SetBorder(BorderlessEntry view)
        {
            if (view == null || Control == null)
            {
                return;
            }
            if (!view.HasBorder)
            {
                Control.Background = null;
                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }
        }


        private void SetTextXAlignment(BorderlessEntry view)
        {
            if (view == null || Control == null)
            {
                return;
            }
            switch (view.XAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = Android.Views.GravityFlags.CenterHorizontal;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = Android.Views.GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = Android.Views.GravityFlags.Start;
                    break;
            }
        }


        private void SetTextYAlignment(BorderlessEntry view)
        {
            if (view == null || Control == null)
            {
                return;
            }
            switch (view.YAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = Android.Views.GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = Android.Views.GravityFlags.Start;
                    break;
            }
        }
    }
}
