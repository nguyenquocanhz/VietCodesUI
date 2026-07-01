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
    [Description("A premium modern CheckBox with smooth hover glow and checked state animations.")]
    public class ModernCheckBox : CheckBox
    {
        private Color checkedColor = ModernTheme.AccentViolet;
        private Color uncheckedColor = Color.FromArgb(148, 163, 184); // slate-400
        private int boxSize = 18;

        // Animations
        private readonly System.Windows.Forms.Timer animationTimer;
        private float checkProgress = 0f;
        private float hoverProgress = 0f;
        private bool isHovered = false;

        [Category("Vietcode UI")]
        [Description("The color of the box when checked.")]
        public Color CheckedColor
        {
            get => checkedColor;
            set { checkedColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The border color of the box when unchecked.")]
        public Color UncheckedColor
        {
            get => uncheckedColor;
            set { uncheckedColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The size of the check box in pixels.")]
        [DefaultValue(18)]
        public int BoxSize
        {
            get => boxSize;
            set
            {
                boxSize = Math.Max(12, value);
                Invalidate();
            }
        }

        public ModernCheckBox()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            ForeColor = ModernTheme.DarkTextPrimary;
            Font = new Font("Segoe UI", 9.5f);

            animationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            bool needsInvalidate = false;
            float targetCheck = Checked ? 1f : 0f;
            float targetHover = isHovered ? 1f : 0f;

            if (Math.Abs(checkProgress - targetCheck) > 0.05f)
            {
                checkProgress += (targetCheck - checkProgress) * 0.25f;
                needsInvalidate = true;
            }
            else
            {
                checkProgress = targetCheck;
            }

            if (Math.Abs(hoverProgress - targetHover) > 0.05f)
            {
                hoverProgress += (targetHover - hoverProgress) * 0.25f;
                needsInvalidate = true;
            }
            else
            {
                hoverProgress = targetHover;
            }

            if (needsInvalidate)
            {
                Invalidate();
            }
            else
            {
                animationTimer.Stop();
            }
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            animationTimer.Start();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            animationTimer.Start();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            animationTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear parent background
            if (Parent != null)
            {
                using (SolidBrush parentBrush = new SolidBrush(Parent.BackColor))
                {
                    g.FillRectangle(parentBrush, ClientRectangle);
                }
            }

            // Sync Text Color with theme
            Color currentTextCol = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? ModernTheme.DarkTextPrimary 
                : ModernTheme.LightTextPrimary;

            // Box Bounds
            float boxX = 2f;
            float boxY = (Height - boxSize) / 2f;
            RectangleF boxRect = new RectangleF(boxX, boxY, boxSize, boxSize);

            // Compute border and background colors
            Color hoverBorder = ControlPaint.Light(uncheckedColor, 0.15f);
            Color borderCol = ModernTheme.BlendColors(uncheckedColor, hoverBorder, hoverProgress);
            Color finalBorderColor = ModernTheme.BlendColors(borderCol, checkedColor, checkProgress);
            Color fillBgColor = Color.FromArgb((int)(255 * checkProgress), checkedColor);

            // Fill check background if checked
            if (checkProgress > 0)
            {
                using (SolidBrush fillBrush = new SolidBrush(fillBgColor))
                {
                    ModernTheme.FillRoundedRectangle(g, fillBrush, boxRect, 4f);
                }
            }

            // Draw box border
            using (Pen borderPen = new Pen(finalBorderColor, 1.5f))
            {
                borderPen.Alignment = PenAlignment.Inset;
                ModernTheme.DrawRoundedRectangle(g, borderPen, boxRect, 4f);
            }

            // Draw Vector Checkmark
            if (checkProgress > 0)
            {
                using (Pen checkPen = new Pen(Color.FromArgb((int)(255 * checkProgress), Color.White), 2f))
                {
                    checkPen.StartCap = LineCap.Round;
                    checkPen.EndCap = LineCap.Round;
                    checkPen.LineJoin = LineJoin.Round;

                    // Compute points relative to boxRect
                    PointF pt1 = new PointF(boxRect.X + boxSize * 0.25f, boxRect.Y + boxSize * 0.5f);
                    PointF pt2 = new PointF(boxRect.X + boxSize * 0.45f, boxRect.Y + boxSize * 0.7f);
                    PointF pt3 = new PointF(boxRect.X + boxSize * 0.75f, boxRect.Y + boxSize * 0.3f);

                    // Interpolate check drawing path for premium feel
                    if (checkProgress < 0.5f)
                    {
                        float p = checkProgress / 0.5f;
                        float x2 = pt1.X + (pt2.X - pt1.X) * p;
                        float y2 = pt1.Y + (pt2.Y - pt1.Y) * p;
                        g.DrawLine(checkPen, pt1, new PointF(x2, y2));
                    }
                    else
                    {
                        g.DrawLine(checkPen, pt1, pt2);
                        float p = (checkProgress - 0.5f) / 0.5f;
                        float x3 = pt2.X + (pt3.X - pt2.X) * p;
                        float y3 = pt2.Y + (pt3.Y - pt2.Y) * p;
                        g.DrawLine(checkPen, pt2, new PointF(x3, y3));
                    }
                }
            }

            // Draw Text
            Rectangle textRect = new Rectangle(boxSize + 8, 0, Width - boxSize - 8, Height);
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(g, Text, Font, textRect, currentTextCol, flags);
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
