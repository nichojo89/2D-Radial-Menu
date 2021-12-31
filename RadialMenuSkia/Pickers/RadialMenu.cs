using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Extended.Iconify;
using SkiaSharp.Views.Forms;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace RadialMenuSkia.Pickers
{
    public sealed class RadialMenu : SKCanvasView
    {
        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(nameof(ItemSource),
            typeof(IReadOnlyList<MenuItem>),
            typeof(RadialMenu),
            new List<MenuItem>());

        public IReadOnlyList<MenuItem> ItemSource
        {
            get => (IReadOnlyList<MenuItem>)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        public RadialMenu()
        {
            EnableTouchEvents = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;
            var info = e.Info;

            canvas.Clear();


            if (ItemSource.Count == 0)
            {
                throw new Exception("Missing menu items for radial wheel");
            }

            const int offset = 90;
            const int selectedArcPadding = 20;

            var sweepingAngle = 360 / ItemSource.Count;

            var borderPaint = new SKPaint
            {
                StrokeWidth = _borderWidth,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                Color = Color.White.ToSKColor()
            };

            var gradient = SKShader.CreateRadialGradient(
                new SKPoint(info.Rect.MidX, info.Rect.MidY),
                _arcLength,
                new SKColor[]{Color.Orange.ToSKColor(), Color.DarkOrange.ToSKColor()},
                null,
                SKShaderTileMode.Clamp);

            var innerPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Shader = gradient
            };

            _arcSegments.Clear();

            for (var i = 0; i < ItemSource.Count; i++)
            {
                var path = new SKPath();

                var degrees = i * sweepingAngle;

                var length = _selectedArcSegmentIndex == i
                    ? _arcLength + selectedArcPadding
                    : _arcLength;

                var startX = info.Rect.MidX + length * Math.Cos((degrees - offset) * (Math.PI / 180));
                var startY = info.Rect.MidY + length * Math.Sin((degrees - offset) * (Math.PI / 180));

                path.MoveTo(new SKPoint((float)startX, (float)startY));
                path.ArcTo(new SKRect(
                    info.Rect.MidX - length,
                    info.Rect.MidY - length,
                    info.Rect.MidX + length,
                    info.Rect.MidY + length
                ), degrees - offset, sweepingAngle, true);
                path.LineTo(new SKPoint(info.Rect.MidX, info.Rect.MidY));
                path.Close();

                _arcSegments.Add(path);

                canvas.DrawPath(path, innerPaint);
                canvas.DrawPath(path, borderPaint);

                using (var textPaint = new SKPaint
                {
                    IsAntialias = true,
                    TextSize = 60,
                    TextAlign = SKTextAlign.Center
                })
                {
                    const float lengthRatio = 0.75f;

                    var iconX = (float)(info.Rect.MidX
                        + (_arcLength
                        * lengthRatio)
                        * Math.Cos((degrees + (sweepingAngle / 2) - offset)
                        * (Math.PI / 180)));

                    var iconY = (float)(info.Rect.MidY +
                        (_arcLength * lengthRatio)
                        * Math.Sin((degrees + (sweepingAngle / 2) - offset)
                        * (Math.PI / 180)))
                        //vertical text alignment offset
                        + (textPaint.TextSize / 2);

                    //canvas.DrawCircle(new SKPoint(iconX, iconY), 10, borderPaint);
                    canvas.DrawIconifiedText($"{{{{{ItemSource[i].Icon} color=FFFFFF}}}}", (float)iconX, (float)iconY,
                        textPaint);
                }
            }

            _circlePath.Reset();
            _circlePath.AddCircle(info.Rect.MidX, info.Rect.MidY, _diameter);
            _circlePath.Close();

            canvas.DrawPath(_circlePath, borderPaint);

            canvas.DrawCircle(
                new SKPoint(info.Rect.MidX, info.Rect.MidY),
                _diameter - (_borderWidth / 2), innerPaint);
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            base.OnTouch(e);

            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    _selectedArcSegmentIndex = -1;

                    for (var i = 0; i < _arcSegments.Count; i++)
                    {
                        if (_arcSegments[i].Contains(e.Location.X, e.Location.Y)
                            && !_circlePath.Contains(e.Location.X, e.Location.Y))
                        {
                            _selectedArcSegmentIndex = i;

                            this.DisplayToastAsync(ItemSource[i].Name, 1000);
                        }
                    }

                    InvalidateSurface();
                    break;
            }

            e.Handled = true;
        }

        private int _diameter = 80;
        private int _borderWidth = 6;
        private const int _arcLength = 200;
        private int _selectedArcSegmentIndex = -1;
        private SKPath _circlePath = new SKPath();
        private List<SKPath> _arcSegments = new List<SKPath>();
    }
}