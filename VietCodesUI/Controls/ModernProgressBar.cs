using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [ToolboxItem(true)]
    [Description("A premium progress bar with rounded corners, gradient fills, and smooth value transition animations.")]
    public class ModernProgressBar : Control
    {
        private int value = 0;
        private int minimum = 0;
        private int maximum = 100;

        private Color progressColor1 = ModernTheme.AccentViolet;
        private Color progressColor2 = ModernTheme.AccentBlue;
        private Color trackColor = Color.FromArgb(30, 41, 59); // slate-800
        private bool showPercentage = false;

        // Animations
        private readonly System.Windows.Forms.Timer animationTimer;
        private float visualProgress = 0f; // Range: 0 to 1

        [Category("Vietcode UI")]
        [Description("The current progress value.")]
        [DefaultValue(0)]
        public int Value
        {
            get => value;
            set
            {
                this.value = Math.Max(minimum, Math.Min(maximum, value));
                animationTimer.Start();
            }
        }

        [Category("Vietcode UI")]
        [Description("The minimum value.")]
        [DefaultValue(0)]
        public int Minimum
        {
            get => minimum;
            set
            {
                minimum = value;
                if (minimum > maximum) maximum = minimum;
                Value = this.value; // Clamp
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The maximum value.")]
        [DefaultValue(100)]
        public int Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                if (maximum < minimum) minimum = maximum;
                Value = this.value; // Clamp
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The starting color of the progress bar gradient.")]
        public Color ProgressColor1
        {
            get => progressColor1;
            set { progressColor1 = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The ending color of the progress bar gradient.")]
        public Color ProgressColor2
        {
            get => progressColor2;
            set { progressColor2 = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The background color of the track.")]
        public Color TrackColor
        {
            get => trackColor;
            set { trackColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("Display the percentage text on the progress bar.")]
        [DefaultValue(false)]
        public bool ShowPercentage
        {
            get => showPercentage;
            set { showPercentage = value; Invalidate(); }
        }

        public ModernProgressBar()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Size = new Size(200, 16);

            animationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            float target = 0f;
            if (maximum - minimum > 0)
            {
                target = (float)(value - minimum) / (maximum - minimum);
            }

            if (Math.Abs(visualProgress - target) > 0.01f)
            {
                visualProgress += (target - visualProgress) * 0.15f;
                Invalidate();
            }
            else
            {
                visualProgress = target;
                animationTimer.Stop();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear parent background
            if (Parent != null)
            {
                using (SolidBrush parentBrush = new SolidBrush(Parent.BackColor))
                {
                    g.FillRectangle(parentBrush, ClientRectangle);
                }
            }

            RectangleF rect = new RectangleF(0, 0, Width, Height);
            float radius = Height / 2f;

            // Inset path slightly for borders
            rect.X += 0.5f;
            rect.Y += 0.5f;
            rect.Width -= 1f;
            rect.Height -= 1f;

            // Draw track (background)
            Color currentTrackColor = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? trackColor 
                : Color.FromArgb(226, 232, 240); // Soft grey in light mode

            using (SolidBrush trackBrush = new SolidBrush(currentTrackColor))
            {
                ModernTheme.FillRoundedRectangle(g, trackBrush, rect, radius);
            }

            // Draw progress fill
            float fillWidth = rect.Width * visualProgress;
            if (fillWidth > Height) // Need enough width to draw rounded corners correctly
            {
                RectangleF fillRect = new RectangleF(rect.X, rect.Y, fillWidth, rect.Height);
                
                // Create a clip path for the container to ensure the progress fill doesn't overflow
                using (GraphicsPath trackPath = ModernTheme.GetRoundedPath(rect, radius))
                {
                    g.SetClip(trackPath);

                    if (progressColor1 == progressColor2)
                    {
                        using (SolidBrush fillBrush = new SolidBrush(progressColor1))
                        {
                            g.FillRectangle(fillBrush, fillRect);
                        }
                    }
                    else
                    {
                        using (LinearGradientBrush fillBrush = new LinearGradientBrush(
                            rect, progressColor1, progressColor2, LinearGradientMode.Horizontal))
                        {
                            g.FillRectangle(fillBrush, fillRect);
                        }
                    }

                    g.ResetClip();
                }
            }
            else if (fillWidth > 0)
            {
                // For very small fills, draw a simple pill shape at the start
                RectangleF fillRect = new RectangleF(rect.X, rect.Y, Math.Max(fillWidth, 4f), rect.Height);
                using (SolidBrush fillBrush = new SolidBrush(progressColor1))
                {
                    ModernTheme.FillRoundedRectangle(g, fillBrush, fillRect, radius);
                }
            }

            // Optional Percentage Text
            if (showPercentage)
            {
                string text = $"{(int)(visualProgress * 100)}%";
                Color textCol = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                    ? ModernTheme.DarkTextPrimary 
                    : ModernTheme.LightTextPrimary;
                
                using (Font font = new Font("Segoe UI", Height > 14 ? 8.5f : 7f, FontStyle.Bold))
                {
                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    TextRenderer.DrawText(g, text, font, ClientRectangle, textCol, flags);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
