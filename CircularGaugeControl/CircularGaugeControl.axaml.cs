/*Copyright (c) 2009 T.Evelyn (evescode@gmail.com) 

All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1.Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2.Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in 
 the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 

THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS 

BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 

GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 

LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 

DAMAGE.*/


using Avalonia.Controls.Metadata;
using Avalonia.Controls.Shapes;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Path = Avalonia.Controls.Shapes.Path;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Animation;
using System;
using Avalonia.Styling;
using Avalonia.Animation.Easings;

namespace CircularGaugeControl
{
    /// <summary>
    /// Represents a Circular Gauge control
    /// </summary>
    [TemplatePart(Name = "PART_LayoutRoot", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_Pointer", Type = typeof(Path))]
    [TemplatePart(Name = "PART_RangeIndicatorLight", Type = typeof(Ellipse))]
    [TemplatePart(Name = "PART_PointerCap", Type = typeof(Ellipse))]
    public class CircularGaugeControl : TemplatedControl
    {
        #region Private variables

        //Private variables
        private Grid? rootGrid;
        private Path? rangeIndicator;
        private Path? pointer;
        private Ellipse? pointerCap;
        private Ellipse? lightIndicator;
        private bool isInitialValueSet = false;
        private double arcradius1;
        private double arcradius2;
        private readonly int animatingSpeedFactor = 6;

        #endregion

        #region Dependency properties

        /// <summary>
        /// Dependency property to Get/Set the OuterFrameStrokeThickness 
        /// </summary>
        public static readonly StyledProperty<double> OuterFrameStrokeThicknessProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(OuterFrameStrokeThickness), 16);

        /// <summary>
        /// Dependency property to Get/Set the Easing 
        /// </summary>
        public static readonly StyledProperty<Easing> EasingValueProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Easing>(nameof(EasingValue), new SplineEasing());

        /// <summary>
        /// Dependency property to Get/Set the current value 
        /// </summary>
        public static readonly StyledProperty<double> CurrentValueProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(CurrentValue), double.MinValue);

        /// <summary>
        /// Dependency property to Get/Set the Minimum Value 
        /// </summary>
        public static readonly StyledProperty<double> MinValueProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(MinValue));

        /// <summary>
        /// Dependency property to Get/Set the Maximum Value 
        /// </summary>
        public static readonly StyledProperty<double> MaxValueProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(MaxValue));


        /// <summary>
        /// Dependency property to Get/Set the Radius of the gauge
        /// </summary>
        public static readonly StyledProperty<double> RadiusProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(Radius));

        /// <summary>
        /// Dependency property to Get/Set the Pointer cap Radius
        /// </summary>
        public static readonly StyledProperty<double> PointerCapRadiusProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(PointerCapRadius));

        /// <summary>
        /// Dependency property to Get/Set the pointer length
        /// </summary>
        public static readonly StyledProperty<double> PointerLengthProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(PointerLength));

        /// <summary>
        /// Dependency property to Get/Set the scale Radius
        /// </summary>
        public static readonly StyledProperty<double> ScaleRadiusProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(ScaleRadius));

        /// <summary>
        /// Dependency property to Get/Set the starting angle of scale
        /// </summary>
        public static readonly StyledProperty<double> ScaleStartAngleProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(ScaleStartAngle));

        /// <summary>
        /// Dependency property to Get/Set the sweep angle of scale
        /// </summary>
        public static readonly StyledProperty<double> ScaleSweepAngleProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(ScaleSweepAngle));

        /// <summary>
        /// Dependency property to Get/Set the number of major divisions on the scale
        /// </summary>
        public static readonly StyledProperty<double> MajorDivisionsCountProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(MajorDivisionsCount));

        /// <summary>
        /// Dependency property to Get/Set the number of minor divisions on the scale
        /// </summary>
        public static readonly StyledProperty<double> MinorDivisionsCountProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(MinorDivisionsCount));

        /// <summary>
        /// Dependency property to Get/Set Optimal Range End Value
        /// </summary>
        public static readonly StyledProperty<double> OptimalRangeEndValueProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(OptimalRangeEndValueProperty));

        /// <summary>
        /// Dependency property to Get/Set Optimal Range Start Value
        /// </summary>
        public static readonly StyledProperty<double> OptimalRangeStartValueProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(OptimalRangeStartValue));

        /// <summary>
        /// Dependency property to Get/Set the image source
        /// </summary>
        public static readonly StyledProperty<IImage> ImageSourceProperty =
            AvaloniaProperty.Register<CircularGaugeControl, IImage>(nameof(ImageSource));

        /// <summary>
        /// Dependency property to Get/Set the image offset
        /// </summary>
        public static readonly StyledProperty<double> ImageOffsetProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(ImageOffset));

        /// <summary>
        /// Dependency property to Get/Set the range indicator light offset
        /// </summary>
        public static readonly StyledProperty<double> RangeIndicatorLightOffsetProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(RangeIndicatorLightOffset));

        /// <summary>
        /// Dependency property to Get/Set the image Size
        /// </summary>

        public static readonly StyledProperty<Size> ImageSizeProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Size>(nameof(ImageSizeProperty));

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Radius
        /// </summary>
        public static readonly StyledProperty<double> RangeIndicatorRadiusProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(RangeIndicatorRadiusProperty));

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator Thickness
        /// </summary>
        public static readonly StyledProperty<double> RangeIndicatorThicknessProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(RangeIndicatorThickness));

        /// <summary>
        /// Dependency property to Get/Set the scale label Radius
        /// </summary>
        public static readonly StyledProperty<double> ScaleLabelRadiusProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(ScaleLabelRadius));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Size
        /// </summary>
        public static readonly StyledProperty<Size> ScaleLabelSizeProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Size>(nameof(ScaleLabelSize));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label FontSize
        /// </summary>
        public static readonly StyledProperty<double> ScaleLabelFontSizeProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(ScaleLabelFontSize));

        /// <summary>
        /// Dependency property to Get/Set the Scale Label Foreground
        /// </summary>
        public static readonly StyledProperty<Color> ScaleLabelForegroundProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(ScaleLabelForegroundProperty));

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Size
        /// </summary>
        public static readonly StyledProperty<Size> MajorTickSizeProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Size>(nameof(MajorTickSizeProperty));

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Size
        /// </summary>
        public static readonly StyledProperty<Size> MinorTickSizeProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Size>(nameof(MinorTickSize));

        /// <summary>
        /// Dependency property to Get/Set the Major Tick Color
        /// </summary>
        public static readonly StyledProperty<Color> MajorTickColorProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(MajorTickColor));

        /// <summary>
        /// Dependency property to Get/Set the Minor Tick Color
        /// </summary>
        public static readonly StyledProperty<Color> MinorTickColorProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(MinorTickColor));

        /// <summary>
        /// Dependency property to Get/Set the Gauge Background Color
        /// </summary>
        public static readonly StyledProperty<Color> GaugeBackgroundColorProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(GaugeBackgroundColor));

        /// <summary>
        /// Dependency property to Get/Set the Pointer Thickness
        /// </summary>
        public static readonly StyledProperty<double> PointerThicknessProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(PointerThicknessProperty));

        /// <summary>
        /// Dependency property to Get/Set the an option to reset the pointer on start up to the minimum value
        /// </summary>
        public static readonly StyledProperty<bool> ResetPointerOnStartUpProperty =
            AvaloniaProperty.Register<CircularGaugeControl, bool>(nameof(ResetPointerOnStartUp));

        /// <summary>
        /// Dependency property to Get/Set the Scale Value Precision
        /// </summary>
        public static readonly StyledProperty<int> ScaleValuePrecisionProperty =
            AvaloniaProperty.Register<CircularGaugeControl, int>(nameof(ScaleValuePrecision));

        /// <summary>
        /// Dependency property to Get/Set the Below Optimal Range Color
        /// </summary>
        public static readonly StyledProperty<Color> BelowOptimalRangeColorProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(BelowOptimalRangeColor));

        /// <summary>
        /// Dependency property to Get/Set the Optimal Range Color
        /// </summary>
        public static readonly StyledProperty<Color> OptimalRangeColorProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(OptimalRangeColor));

        /// <summary>
        /// Dependency property to Get/Set the Above Optimal Range Color
        /// </summary>
        public static readonly StyledProperty<Color> AboveOptimalRangeColorProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(AboveOptimalRangeColor));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text
        /// </summary>
        public static readonly StyledProperty<string> DialTextProperty =
            AvaloniaProperty.Register<CircularGaugeControl, string>(nameof(DialText));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Color
        /// </summary>
        public static readonly StyledProperty<Color> DialTextColorProperty =
            AvaloniaProperty.Register<CircularGaugeControl, Color>(nameof(DialTextColor));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Font Size
        /// </summary>
        public static readonly StyledProperty<int> DialTextFontSizeProperty =
            AvaloniaProperty.Register<CircularGaugeControl, int>(nameof(DialTextFontSize));

        /// <summary>
        /// Dependency property to Get/Set the Dial Text Offset
        /// </summary>
        public static readonly StyledProperty<double> DialTextOffsetProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(DialTextOffset));

        /// <summary>
        /// Dependency property to Get/Set the Range Indicator light Radius
        /// </summary>
        public static readonly StyledProperty<double> RangeIndicatorLightRadiusProperty =
            AvaloniaProperty.Register<CircularGaugeControl, double>(nameof(RangeIndicatorLightRadius));


        #endregion

        #region Wrapper properties

        /// <summary>
        /// Gets/Sets the StrokeThickness of the OuterFrame
        /// </summary>
        public double OuterFrameStrokeThickness
        {
            get => GetValue(OuterFrameStrokeThicknessProperty);
            set => SetValue(OuterFrameStrokeThicknessProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Easing value
        /// </summary>
        public Easing EasingValue
        {
            get => GetValue(EasingValueProperty);
            set => SetValue(EasingValueProperty, value);
        }

        /// <summary>
        /// Gets/Sets the current value
        /// </summary>
        public double CurrentValue
        {
            get => GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Minimum Value
        /// </summary>
        public double MinValue
        {
            get => GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Maximum Value
        /// </summary>
        public double MaxValue
        {
            get => GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Minimum Value
        /// </summary>
        public double Radius
        {
            get => GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Pointer cap radius
        /// </summary>
        public double PointerCapRadius
        {
            get => GetValue(PointerCapRadiusProperty);
            set => SetValue(PointerCapRadiusProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Pointer Length
        /// </summary>
        public double PointerLength
        {
            get => GetValue(PointerLengthProperty);
            set => SetValue(PointerLengthProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Pointer Thickness
        /// </summary>
        public double PointerThickness
        {
            get => GetValue(PointerThicknessProperty);
            set => SetValue(PointerThicknessProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Scale radius
        /// </summary>
        public double ScaleRadius
        {
            get => GetValue(ScaleRadiusProperty);
            set => SetValue(ScaleRadiusProperty, value);
        }

        /// <summary>
        /// Gets/Sets the scale start angle
        /// </summary>
        public double ScaleStartAngle
        {
            get => GetValue(ScaleStartAngleProperty);
            set => SetValue(ScaleStartAngleProperty, value);
        }

        /// <summary>
        /// Gets/Sets the scale sweep angle
        /// </summary>
        public double ScaleSweepAngle
        {
            get => GetValue(ScaleSweepAngleProperty);
            set => SetValue(ScaleSweepAngleProperty, value);
        }

        /// <summary>
        /// Gets/Sets the number of major divisions on the scale
        /// </summary>
        public double MajorDivisionsCount
        {
            get => GetValue(MajorDivisionsCountProperty);
            set => SetValue(MajorDivisionsCountProperty, value);
        }

        /// <summary>
        /// Gets/Sets the number of minor divisions on the scale
        /// </summary>
        public double MinorDivisionsCount
        {
            get => GetValue(MinorDivisionsCountProperty);
            set => SetValue(MinorDivisionsCountProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Optimal range end value
        /// </summary>
        public double OptimalRangeEndValue
        {
            get => GetValue(OptimalRangeEndValueProperty);
            set => SetValue(OptimalRangeEndValueProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Optimal Range Start Value
        /// </summary>
        public double OptimalRangeStartValue
        {
            get => GetValue(OptimalRangeStartValueProperty);
            set => SetValue(OptimalRangeStartValueProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Gauge image source
        /// </summary>
        public IImage ImageSource
        {
            get => GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }


        /// <summary>
        /// Gets/Sets the Image offset
        /// </summary>
        public double ImageOffset
        {
            get => GetValue(ImageOffsetProperty);
            set => SetValue(ImageOffsetProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Range Indicator Light offset
        /// </summary>
        public double RangeIndicatorLightOffset
        {
            get => GetValue(RangeIndicatorLightOffsetProperty);
            set => SetValue(RangeIndicatorLightOffsetProperty, value);
        }


        /// <summary>
        /// Gets/Sets the Image width and height
        /// </summary>
        public Size ImageSize
        {
            get => GetValue(ImageSizeProperty);
            set => SetValue(ImageSizeProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Range Indicator Radius 
        /// </summary>
        public double RangeIndicatorRadius
        {
            get => GetValue(RangeIndicatorRadiusProperty);
            set => SetValue(RangeIndicatorRadiusProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Range Indicator Thickness 
        /// </summary>
        public double RangeIndicatorThickness
        {
            get => GetValue(RangeIndicatorThicknessProperty);
            set => SetValue(RangeIndicatorThicknessProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Scale Label Radius 
        /// </summary>
        public double ScaleLabelRadius
        {
            get => GetValue(ScaleLabelRadiusProperty);
            set => SetValue(ScaleLabelRadiusProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Scale Label Size 
        /// </summary>
        public Size ScaleLabelSize
        {
            get => GetValue(ScaleLabelSizeProperty);
            set => SetValue(ScaleLabelSizeProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Scale Label Font Size 
        /// </summary>
        public double ScaleLabelFontSize
        {
            get => GetValue(ScaleLabelFontSizeProperty);
            set => SetValue(ScaleLabelFontSizeProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Scale Label Foreground 
        /// </summary>
        public Color ScaleLabelForeground
        {
            get => GetValue(ScaleLabelForegroundProperty);
            set => SetValue(ScaleLabelForegroundProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Major Tick Size 
        /// </summary>
        public Size MajorTickSize
        {
            get => GetValue(MajorTickSizeProperty);
            set => SetValue(MajorTickSizeProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Minor Tick Size 
        /// </summary>
        public Size MinorTickSize
        {
            get => GetValue(MinorTickSizeProperty);
            set => SetValue(MinorTickSizeProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Major Tick Color 
        /// </summary>
        public Color MajorTickColor
        {
            get => GetValue(MajorTickColorProperty);
            set => SetValue(MajorTickColorProperty, value);
        }
        /// <summary>
        /// Gets/Sets the Minor Tick Color 
        /// </summary>
        public Color MinorTickColor
        {
            get => GetValue(MinorTickColorProperty);
            set => SetValue(MinorTickColorProperty, value);
        }

        /// <summary>
        /// Gets/Sets the Gauge Background color
        /// </summary>
        public Color GaugeBackgroundColor
        {
            get => GetValue(GaugeBackgroundColorProperty);
            set => SetValue(GaugeBackgroundColorProperty, value);
        }


        /// <summary>
        /// Gets/Sets option to reset the pointer to minimum on start up, Default is true
        /// </summary>
        public bool ResetPointerOnStartUp
        {
            get => GetValue(ResetPointerOnStartUpProperty);
            set => SetValue(ResetPointerOnStartUpProperty, value);
        }

        /// <summary>
        /// Gets/Sets scale value precision 
        /// </summary>
        public int ScaleValuePrecision
        {
            get => GetValue(ScaleValuePrecisionProperty);
            set => SetValue(ScaleValuePrecisionProperty, value);
        }
        /// <summary>
        /// Gets/Sets Below Optimal Range Color
        /// </summary>
        public Color BelowOptimalRangeColor
        {
            get => GetValue(BelowOptimalRangeColorProperty);
            set => SetValue(BelowOptimalRangeColorProperty, value);
        }

        /// <summary>
        /// Gets/Sets Optimal Range Color
        /// </summary>
        public Color OptimalRangeColor
        {
            get => GetValue(OptimalRangeColorProperty);
            set => SetValue(OptimalRangeColorProperty, value);
        }

        /// <summary>
        /// Gets/Sets Above Optimal Range Color
        /// </summary>
        public Color AboveOptimalRangeColor
        {
            get => GetValue(AboveOptimalRangeColorProperty);
            set => SetValue(AboveOptimalRangeColorProperty, value);
        }
        /// <summary>
        /// Gets/Sets Dial Text
        /// </summary>
        public string DialText
        {
            get => GetValue(DialTextProperty);
            set => SetValue(DialTextProperty, value);
        }
        /// <summary>
        /// Gets/Sets Dial Text Color
        /// </summary>
        public Color DialTextColor
        {
            get => GetValue(DialTextColorProperty);
            set => SetValue(DialTextColorProperty, value);
        }
        /// <summary>
        /// Gets/Sets Dial Text Font Size
        /// </summary>
        public int DialTextFontSize
        {
            get => GetValue(DialTextFontSizeProperty);
            set => SetValue(DialTextFontSizeProperty, value);
        }
        /// <summary>
        /// Gets/Sets Dial Text Offset
        /// </summary>
        public double DialTextOffset
        {
            get => GetValue(DialTextOffsetProperty);
            set => SetValue(DialTextOffsetProperty, value);
        }

        /// <summary>
        /// Gets/Sets Range Indicator Light Radius
        /// </summary>
        public double RangeIndicatorLightRadius
        {
            get => GetValue(RangeIndicatorLightRadiusProperty);
            set => SetValue(RangeIndicatorLightRadiusProperty, value);
        }

        #endregion

        #region Constructor
        static CircularGaugeControl()
        {
            CurrentValueProperty.Changed.AddClassHandler<CircularGaugeControl>(OnCurrentValuePropertyChanged);
            OptimalRangeEndValueProperty.Changed.AddClassHandler<CircularGaugeControl>(OnOptimalRangeEndValuePropertyChanged);
            OptimalRangeStartValueProperty.Changed.AddClassHandler<CircularGaugeControl>(OnOptimalRangeStartValuePropertyChanged);
        }
        #endregion

        #region Methods
        private static void OnCurrentValuePropertyChanged(CircularGaugeControl gauge, AvaloniaPropertyChangedEventArgs e)
        {
            gauge.OnCurrentValueChanged(e);
        }

        private static void OnOptimalRangeEndValuePropertyChanged(CircularGaugeControl gauge, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            if ((double)e.NewValue > gauge.MaxValue)
            {
                gauge.OptimalRangeEndValue = gauge.MaxValue;
            }

        }
        private static void OnOptimalRangeStartValuePropertyChanged(CircularGaugeControl gauge, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            if ((double)e.NewValue < gauge.MinValue)
            {
                gauge.OptimalRangeStartValue = gauge.MinValue;
            }
        }

        public virtual void OnCurrentValueChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            if (e.OldValue == null) return;
            //Validate and set the new value
            double newValue = (double)e.NewValue;
            double oldValue = (double)e.OldValue;
            AnimatePointerFromValues(newValue, oldValue);

        }

        private void AnimatePointerFromValues(double newValue, double oldValue)
        {
            if (newValue > MaxValue)
            {
                newValue = MaxValue;
            }
            else if (newValue < MinValue)
            {
                newValue = MinValue;
            }

            if (oldValue > MaxValue)
            {
                oldValue = MaxValue;
            }
            else if (oldValue < MinValue)
            {
                oldValue = MinValue;
            }

            if (pointer != null)
            {
                double realworldunit = ScaleSweepAngle / (MaxValue - MinValue);
                //Resetting the old value to min value the very first time.
                if (oldValue == 0 && !isInitialValueSet)
                {
                    oldValue = MinValue;
                    isInitialValueSet = true;

                }
                double db = oldValue - MinValue;
                double oldcurr_realworldunit = db * realworldunit;

                db = newValue - MinValue;
                double newcurr_realworldunit = db * realworldunit;

                double oldcurrentvalueAngle = ScaleStartAngle + oldcurr_realworldunit;
                double newcurrentvalueAngle = ScaleStartAngle + newcurr_realworldunit;

                //Animate the pointer from the old value to the new value
                AnimatePointer(oldcurrentvalueAngle, newcurrentvalueAngle);
            }
        }

        /// <summary>
        /// Animates the pointer to the current value to the new one
        /// </summary>
        /// <param name="oldcurrentvalueAngle"></param>
        /// <param name="newcurrentvalueAngle"></param>
        void AnimatePointer(double oldcurrentvalueAngle, double newcurrentvalueAngle)
        {
            if (pointer != null)
            {
                var rotateTransformAngle = RotateTransform.AngleProperty;

                var animation = new Animation()
                {
                    FillMode = FillMode.Forward,
                    Easing = EasingValue,
                    Duration = TimeSpan.FromMilliseconds(Math.Abs(oldcurrentvalueAngle - newcurrentvalueAngle) * animatingSpeedFactor),
                    Children =
                    {
                        new KeyFrame
                        {
                            Setters = {
                                new Setter
                                {
                                    Property = rotateTransformAngle,
                                    Value = oldcurrentvalueAngle
                                }
                            },
                            Cue = new Cue(0),
                        },
                        new KeyFrame
                        {
                            Setters = {
                                new Setter
                                {
                                    Property = rotateTransformAngle,
                                    Value = newcurrentvalueAngle
                                }
                            },
                            Cue = new Cue(1)
                        },
                    }
                };
                animation.RunAsync(pointer);

                Animation_Completed();
            }
        }



        /// <summary>
        /// Move pointer without animating
        /// </summary>
        /// <param name="angleValue"></param>
        void MovePointer(double angleValue)
        {
            if (pointer != null)
            {
                if (pointer.RenderTransform is TransformGroup tg)
                {
                    if (tg.Children[0] is RotateTransform rt)
                    {
                        rt.Angle = angleValue;
                    }

                    Animation_Completed();
                }
            }
        }

        /// <summary>
        /// Switch on the Range indicator light after the pointer completes animating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Animation_Completed()
        {
            if (lightIndicator != null)
            {
                if (this.CurrentValue > OptimalRangeEndValue)
                {
                    lightIndicator.Fill = GetRangeIndicatorGradEffect(AboveOptimalRangeColor);

                }
                else if (this.CurrentValue <= OptimalRangeEndValue && this.CurrentValue >= OptimalRangeStartValue)
                {
                    lightIndicator.Fill = GetRangeIndicatorGradEffect(OptimalRangeColor);

                }
                else if (this.CurrentValue < OptimalRangeStartValue)
                {
                    lightIndicator.Fill = GetRangeIndicatorGradEffect(BelowOptimalRangeColor);
                }
            }
        }

        /// <summary>
        /// Get gradient brush effect for the range indicator light
        /// </summary>
        /// <param name="gradientColor"></param>
        /// <returns></returns>
        private static LinearGradientBrush GetRangeIndicatorGradEffect(Color gradientColor)
        {

            var gradient = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(new Point(0, 0), RelativeUnit.Relative),
                EndPoint = new RelativePoint(new Point(1, 1), RelativeUnit.Relative)
            };
            var color1 = new GradientStop();
            if (gradientColor == Colors.Transparent)
            {
                color1.Color = gradientColor;
            }
            else
            {
                color1.Color = Colors.LightGray;
            }

            color1.Offset = 0.2;
            gradient.GradientStops.Add(color1);
            var color2 = new GradientStop
            {
                Color = gradientColor,
                Offset = 0.5
            };
            gradient.GradientStops.Add(color2);
            var color3 = new GradientStop
            {
                Color = gradientColor,
                Offset = 0.8
            };
            gradient.GradientStops.Add(color3);
            return gradient;
        }


        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            //Get reference to known elements on the control template
            rootGrid = e.NameScope.Find<Grid>("PART_LayoutRoot");
            pointer = e.NameScope.Find<Path>("PART_Pointer");
            pointerCap = e.NameScope.Find<Ellipse>("PART_PointerCap");
            lightIndicator = e.NameScope.Find<Ellipse>("PART_RangeIndicatorLight");

            //Draw scale and range indicator
            DrawScale();
            DrawRangeIndicator();

            //Set Zindex of pointer and pointer cap to a really high number so that it stays on top of the 
            //scale and the range indicator
            pointer?.SetValue(ZIndexProperty, 100000);
            pointerCap?.SetValue(ZIndexProperty, 100000);

            if (ResetPointerOnStartUp)
            {
                //Reset Pointer
                MovePointer(ScaleStartAngle);
            }
            else
            {
                AnimatePointerFromValues(CurrentValue, 0);
            }
        }


        //Drawing the scale with the Scale Radius
        private void DrawScale()
        {
            //Calculate one major tick angle 
            double majorTickUnitAngle = ScaleSweepAngle / MajorDivisionsCount;

            //Obtaining One major ticks value
            double majorTicksUnitValue = (MaxValue - MinValue) / MajorDivisionsCount;
            majorTicksUnitValue = Math.Round(majorTicksUnitValue, ScaleValuePrecision);

            double minvalue = MinValue; ;

            // Drawing Major scale ticks
            for (double i = ScaleStartAngle; i <= (ScaleStartAngle + ScaleSweepAngle); i += majorTickUnitAngle)
            {

                //Majortick is drawn as a rectangle 
                var majortickrect = new Rectangle
                {
                    Height = MajorTickSize.Height,
                    Width = MajorTickSize.Width,
                    Fill = new SolidColorBrush(MajorTickColor),
                    RenderTransformOrigin = new RelativePoint(new Point(0.5, 0.5), RelativeUnit.Relative),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var majortickgp = new TransformGroup();
                var majortickrt = new RotateTransform();

                //Obtaining the angle in radians for calulating the points
                double i_radian = i * Math.PI / 180;
                majortickrt.Angle = i;
                majortickgp.Children.Add(majortickrt);
                var majorticktt = new TranslateTransform
                {
                    //Finding the point on the Scale where the major ticks are drawn
                    //here drawing the points with center as (0,0)
                    X = (int)(ScaleRadius * Math.Cos(i_radian)),
                    Y = (int)(ScaleRadius * Math.Sin(i_radian))
                };

                //Points for the textblock which hold the scale value
                var majorscalevaluett = new TranslateTransform
                {
                    //here drawing the points with center as (0,0)
                    X = (int)(ScaleLabelRadius * Math.Cos(i_radian)),
                    Y = (int)(ScaleLabelRadius * Math.Sin(i_radian))
                };

                //Defining the properties of the scale value textbox
                var tb = new TextBlock
                {
                    Height = ScaleLabelSize.Height,
                    Width = ScaleLabelSize.Width,
                    FontSize = ScaleLabelFontSize,
                    Foreground = new SolidColorBrush(ScaleLabelForeground),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                //Writing and appending the scale value

                //checking minvalue < maxvalue w.r.t scale precion value
                if (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision))
                {
                    minvalue = Math.Round(minvalue, ScaleValuePrecision);
                    tb.Text = minvalue.ToString();
                    minvalue += majorTicksUnitValue;
                }
                else
                {
                    break;
                }
                majortickgp.Children.Add(majorticktt);
                majortickrect.RenderTransform = majortickgp;
                tb.RenderTransform = majorscalevaluett;
                rootGrid?.Children.Add(majortickrect);
                rootGrid?.Children.Add(tb);

                //Drawing the minor axis ticks
                double onedegree = majorTickUnitAngle / MinorDivisionsCount;

                if ((i < (ScaleStartAngle + ScaleSweepAngle)) && (Math.Round(minvalue, ScaleValuePrecision) <= Math.Round(MaxValue, ScaleValuePrecision)))
                {
                    //Drawing the minor scale
                    for (double mi = i + onedegree; mi < (i + majorTickUnitAngle); mi += onedegree)
                    {
                        //here the minortick is drawn as a rectangle 
                        var mr = new Rectangle
                        {
                            Height = MinorTickSize.Height,
                            Width = MinorTickSize.Width,
                            Fill = new SolidColorBrush(MinorTickColor),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            RenderTransformOrigin = new RelativePoint(new Point(0.5, 0.5), RelativeUnit.Relative)
                        };

                        var minortickgp = new TransformGroup();
                        var minortickrt = new RotateTransform
                        {
                            Angle = mi
                        };
                        minortickgp.Children.Add(minortickrt);
                        var minorticktt = new TranslateTransform();

                        //Obtaining the angle in radians for calulating the points
                        double mi_radian = mi * Math.PI / 180;
                        //Finding the point on the Scale where the minor ticks are drawn
                        minorticktt.X = (int)(ScaleRadius * Math.Cos(mi_radian));
                        minorticktt.Y = (int)(ScaleRadius * Math.Sin(mi_radian));

                        minortickgp.Children.Add(minorticktt);
                        mr.RenderTransform = minortickgp;
                        rootGrid?.Children.Add(mr);
                    }
                }
            }
        }

        /// <summary>
        /// Obtaining the Point (x,y) in the circumference 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Point GetCircumferencePoint(double angle, double radius)
        {
            double angle_radian = angle * Math.PI / 180;
            //Radius-- is the Radius of the gauge
            double X = Radius + radius * Math.Cos(angle_radian);
            double Y = Radius + radius * Math.Sin(angle_radian);
            var p = new Point(X, Y);
            return p;
        }

        /// <summary>
        /// Draw the range indicator
        /// </summary>
        private void DrawRangeIndicator()
        {
            double realworldunit = ScaleSweepAngle / (MaxValue - MinValue);
            double optimalStartAngle;
            double optimalEndAngle;
            double db;

            db = OptimalRangeStartValue - MinValue;
            optimalStartAngle = db * realworldunit;

            db = OptimalRangeEndValue - MinValue;
            optimalEndAngle = db * realworldunit;
            // calculating the angle for optimal Start value

            double optimalStartAngleFromStart = ScaleStartAngle + optimalStartAngle;

            // calculating the angle for optimal End value

            double optimalEndAngleFromStart = ScaleStartAngle + optimalEndAngle;

            //Calculating the Radius of the two arc for segment 
            arcradius1 = (RangeIndicatorRadius + RangeIndicatorThickness);
            arcradius2 = RangeIndicatorRadius;

            double endAngle = ScaleStartAngle + ScaleSweepAngle;

            // Calculating the Points for the below Optimal Range segment from the center of the gauge

            Point A = GetCircumferencePoint(ScaleStartAngle, arcradius1);
            Point B = GetCircumferencePoint(ScaleStartAngle, arcradius2);
            Point C = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point D = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);

            bool isReflexAngle = Math.Abs(optimalStartAngleFromStart - ScaleStartAngle) > 180.0;
            DrawSegment(A, B, C, D, isReflexAngle, BelowOptimalRangeColor);

            // Calculating the Points for the Optimal Range segment from the center of the gauge

            Point A1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius1);
            Point B1 = GetCircumferencePoint(optimalStartAngleFromStart, arcradius2);
            Point C1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point D1 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            bool isReflexAngle1 = Math.Abs(optimalEndAngleFromStart - optimalStartAngleFromStart) > 180.0;
            DrawSegment(A1, B1, C1, D1, isReflexAngle1, OptimalRangeColor);

            // Calculating the Points for the Above Optimal Range segment from the center of the gauge

            Point A2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius1);
            Point B2 = GetCircumferencePoint(optimalEndAngleFromStart, arcradius2);
            Point C2 = GetCircumferencePoint(endAngle, arcradius2);
            Point D2 = GetCircumferencePoint(endAngle, arcradius1);
            bool isReflexAngle2 = Math.Abs(endAngle - optimalEndAngleFromStart) > 180.0;
            DrawSegment(A2, B2, C2, D2, isReflexAngle2, AboveOptimalRangeColor);
        }

        //Drawing the segment with two arc and two line

        private void DrawSegment(Point p1, Point p2, Point p3, Point p4, bool reflexangle, Color clr)
        {

            // Segment Geometry
            var segments = new PathSegments
            {
                // First line segment from pt p1 - pt p2
                new LineSegment() { Point = p2 },

                //Arc drawn from pt p2 - pt p3 with the RangeIndicatorRadius 
                new ArcSegment()
                {
                    Size = new Size(arcradius2, arcradius2),
                    Point = p3,
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = reflexangle

                },

                // Second line segment from pt p3 - pt p4
                new LineSegment() { Point = p4 },

                //Arc drawn from pt p4 - pt p1 with the Radius of arcradius1 
                new ArcSegment()
                {
                    Size = new Size(arcradius1, arcradius1),
                    Point = p1,
                    SweepDirection = SweepDirection.CounterClockwise,
                    IsLargeArc = reflexangle

                }
            };

            // Defining the segment path properties
            Color rangestrokecolor;
            if (clr == Colors.Transparent)
            {
                rangestrokecolor = clr;
            }
            else
            {
                rangestrokecolor = Colors.White;
            }

            rangeIndicator = new Path()
            {
                StrokeJoin = PenLineJoin.Round,
                Stroke = new SolidColorBrush(rangestrokecolor),
                //Color.FromArgb(0xFF, 0xF5, 0x9A, 0x86)
                Fill = new SolidColorBrush(clr),
                Opacity = 0.65,
                StrokeThickness = 0.25,
                Data = new PathGeometry()
                {
                    Figures = new PathFigures()
                     {
                        new PathFigure()
                        {
                            IsClosed = true,
                            StartPoint = p1,
                            Segments = segments
                        }
                    }
                }
            };

            //Set Z index of range indicator
            rangeIndicator.SetValue(Canvas.ZIndexProperty, 150);
            // Adding the segment to the root grid 
            rootGrid?.Children.Add(rangeIndicator);

        }

        #endregion
    }
}
