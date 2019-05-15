using System;
using CoreAnimation;
using CoreGraphics;
using WTONewProject.iOS.Renderer;
using WTONewProject.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]

namespace WTONewProject.iOS.Renderer
{
    public class GradientColorStackRenderer : VisualElementRenderer<StackLayout>
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            var stack = this.Element as GradientColorStack;
            CGColor startColor = stack.StartColor.ToCGColor();
            CGColor endColor = stack.EndColor.ToCGColor();
            #region for Vertical Gradient
            var gradientLayer = new CAGradientLayer();
            #endregion
            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] { startColor, endColor
        };
            gradientLayer.StartPoint = new CGPoint(0, 0);
            gradientLayer.EndPoint =new CGPoint(1, 0);
            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}
