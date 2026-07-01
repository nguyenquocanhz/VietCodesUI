using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [DefaultEvent("CheckedChanged")]
    [ToolboxItem(true)]
    [Description("A Windows 11 style sliding toggle switch with smooth animations.")]
    public class ModernToggleButton : Control
    {
        private bool isChecked = false;
        private Color onColor = ModernTheme.AccentViolet;
        private Color offColor = Color.FromArgb(71, 85, 105); // slate-600
        private Color thumbColor = Color.White;

        // Animations
        private readonly System.Windows.Forms.Timer animationTimer;
        private float checkProgress = 0f;
        private bool isHovered = false;

        public event EventHandler? CheckedChanged;

        [Category("Vietcode UI")]
        [Description("The state of the toggle switch.")]
        [DefaultValue(false)]
        public bool Checked
        {
            get => isChecked;
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    CheckedChanged?.Invoke(this, EventArgs.Empty);
                    animationTimer.Start();
                }
            }
        }

        [Category("Vietcode UI")]
        [Description("The background color when checked.")]
        public Color OnColor
        {
            get => onColor;
            set { onColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The background color when unchecked.")]
        public Color OffColor
        {
            get => offColor;
            set { offColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The color of the sliding switch knob.")]
        public Color ThumbColor
        {
            get => thumbColor;
            set { thumbColor = value; Invalidate(); }
        }

        public ModernToggleButton()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Size = new Size(45, 22);

            animationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            float target = isChecked ? 1f : 0f;
            if (Math.Abs(checkProgress - target) > 0.05f)
            {
                checkProgress += (target - checkProgress) * 0.25f;
                Invalidate();
            }
            else
            {
                checkProgress = target;
                animationTimer.Stop();
                Invalidate();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            Checked = !Checked;
            base.OnClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            Invalidate();
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
            rect.X += 1f;
            rect.Y += 1f;
            rect.Width -= 2f;
            rect.Height -= 2f;

            // Calculate colors
            Color activeOffColor = isHovered ? ControlPaint.Light(offColor, 0.15f) : offColor;
            Color activeOnColor = isHovered ? ControlPaint.Light(onColor, 0.15f) : onColor;
            Color currentBg = ModernTheme.BlendColors(activeOffColor, activeOnColor, checkProgress);

            // Draw track (rounded pill)
            using (SolidBrush trackBrush = new SolidBrush(currentBg))
            {
                ModernTheme.FillRoundedRectangle(g, trackBrush, rect, radius);
            }

            // Draw sliding thumb (circle)
            float thumbSize = Height - 6f;
            float minX = 3f;
            float maxX = Width - thumbSize - 3f;
            float currentThumbX = minX + (maxX - minX) * checkProgress;
            float currentThumbY = 3f;

            RectangleF thumbRect = new RectangleF(currentThumbX, currentThumbY, thumbSize, thumbSize);
            using (SolidBrush thumbBrush = new SolidBrush(thumbColor))
            {
                g.FillEllipse(thumbBrush, thumbRect);
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
