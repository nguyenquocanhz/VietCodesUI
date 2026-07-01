using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    public enum ModernButtonStyle
    {
        Solid,
        Outline,
        Ghost
    }

    [DefaultEvent("Click")]
    [ToolboxItem(true)]
    [Description("A premium modern button with smooth micro-animations and style configurations.")]
    public class ModernButton : Button
    {
        private int borderRadius = 10;
        private float borderThickness = 1.5f;
        private ModernButtonStyle style = ModernButtonStyle.Solid;
        private Color buttonColor = ModernTheme.AccentViolet;
        private Color textColor = Color.White;
        
        private Color customHoverColor = Color.Empty;
        private Color customClickColor = Color.Empty;
        private Color customBorderColor = Color.Empty;

        // Animations
        private readonly System.Windows.Forms.Timer animationTimer;
        private float hoverProgress = 0f;
        private float clickProgress = 0f;
        private bool isHovered = false;
        private bool isClicked = false;
        private bool isFocused = false;

        private Image? icon = null;
        private ContentAlignment iconAlign = ContentAlignment.MiddleLeft;
        private int iconSpacing = 8;

        [Category("Vietcode UI")]
        [Description("The corner radius of the button.")]
        [DefaultValue(10)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The border thickness.")]
        [DefaultValue(1.5f)]
        public float BorderThickness
        {
            get => borderThickness;
            set
            {
                borderThickness = Math.Max(0f, value);
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The button rendering style.")]
        [DefaultValue(ModernButtonStyle.Solid)]
        public ModernButtonStyle Style
        {
            get => style;
            set
            {
                style = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The primary background color of the button.")]
        public Color ButtonColor
        {
            get => buttonColor;
            set
            {
                buttonColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The text color of the button.")]
        public Color TextColor
        {
            get => textColor;
            set
            {
                textColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("Optional custom hover background color. If empty, it's calculated automatically.")]
        public Color CustomHoverColor
        {
            get => customHoverColor;
            set
            {
                customHoverColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("Optional custom click background color. If empty, it's calculated automatically.")]
        public Color CustomClickColor
        {
            get => customClickColor;
            set
            {
                customClickColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("Optional custom border color. If empty, it uses the button color or theme border.")]
        public Color CustomBorderColor
        {
            get => customBorderColor;
            set
            {
                customBorderColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The icon displayed on the button.")]
        [DefaultValue(null)]
        public Image? Icon
        {
            get => icon;
            set
            {
                icon = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The alignment of the icon.")]
        [DefaultValue(ContentAlignment.MiddleLeft)]
        public ContentAlignment IconAlign
        {
            get => iconAlign;
            set
            {
                iconAlign = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The space between the icon and text in pixels.")]
        [DefaultValue(8)]
        public int IconSpacing
        {
            get => iconSpacing;
            set
            {
                iconSpacing = value;
                Invalidate();
            }
        }

        public ModernButton()
        {
            // Set styles for custom painting and double buffering
            SetStyle(ControlStyles.UserPaint | 
                     ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.ResizeRedraw | 
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(130, 42);
            Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);

            // Initialize Animation Timer
            animationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            animationTimer.Tick += AnimationTimer_Tick;
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            bool needsInvalidate = false;
            float targetHover = isHovered ? 1f : 0f;
            float targetClick = isClicked ? 1f : 0f;

            if (Math.Abs(hoverProgress - targetHover) > 0.05f)
            {
                hoverProgress += (targetHover - hoverProgress) * 0.25f;
                needsInvalidate = true;
            }
            else
            {
                hoverProgress = targetHover;
            }

            if (Math.Abs(clickProgress - targetClick) > 0.05f)
            {
                clickProgress += (targetClick - clickProgress) * 0.25f;
                needsInvalidate = true;
            }
            else
            {
                clickProgress = targetClick;
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

        private void StartAnimation()
        {
            if (!animationTimer.Enabled)
            {
                animationTimer.Start();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            StartAnimation();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            isClicked = false;
            StartAnimation();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                isClicked = true;
                StartAnimation();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isClicked = false;
            StartAnimation();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            isFocused = true;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            isFocused = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Clear control background with parent background color to support rounded transparency
            if (Parent != null)
            {
                using (SolidBrush parentBrush = new SolidBrush(Parent.BackColor))
                {
                    g.FillRectangle(parentBrush, ClientRectangle);
                }
            }

            RectangleF rect = new RectangleF(0, 0, Width, Height);
            // Inset rect slightly to prevent clip cutoff on borders
            float inset = borderThickness / 2f;
            rect.X += inset;
            rect.Y += inset;
            rect.Width -= borderThickness;
            rect.Height -= borderThickness;

            // Define dynamic colors based on states and styles
            Color baseColor = buttonColor;
            Color hoverBg = customHoverColor.IsEmpty ? ControlPaint.Light(baseColor, 0.15f) : customHoverColor;
            Color clickBg = customClickColor.IsEmpty ? ControlPaint.Dark(baseColor, 0.15f) : customClickColor;

            // Opacity-blended background and border colors
            Color currentBg = baseColor;
            Color currentBorder = customBorderColor.IsEmpty ? baseColor : customBorderColor;
            Color currentText = textColor;

            if (style == ModernButtonStyle.Solid)
            {
                // Interpolate base -> hover -> click
                Color blendedHover = ModernTheme.BlendColors(baseColor, hoverBg, hoverProgress);
                currentBg = ModernTheme.BlendColors(blendedHover, clickBg, clickProgress);
                currentBorder = currentBg;
                currentText = textColor;
            }
            else if (style == ModernButtonStyle.Outline)
            {
                Color defaultBg = Color.Transparent;
                Color hoverOutlineBg = Color.FromArgb((int)(25 * hoverProgress), baseColor);
                currentBg = ModernTheme.BlendColors(defaultBg, hoverOutlineBg, hoverProgress);
                if (isClicked)
                {
                    currentBg = Color.FromArgb(40, baseColor);
                }
                currentBorder = baseColor;
                currentText = baseColor;
            }
            else if (style == ModernButtonStyle.Ghost)
            {
                Color defaultBg = Color.Transparent;
                Color hoverGhostBg = Color.FromArgb((int)(20 * hoverProgress), baseColor);
                currentBg = ModernTheme.BlendColors(defaultBg, hoverGhostBg, hoverProgress);
                if (isClicked)
                {
                    currentBg = Color.FromArgb(35, baseColor);
                }
                currentBorder = Color.Transparent;
                currentText = baseColor;
            }

            // Draw Background
            if (currentBg != Color.Transparent)
            {
                using (SolidBrush bgBrush = new SolidBrush(currentBg))
                {
                    ModernTheme.FillRoundedRectangle(g, bgBrush, rect, borderRadius);
                }
            }

            // Draw Border
            if (currentBorder != Color.Transparent && style != ModernButtonStyle.Ghost)
            {
                using (Pen borderPen = new Pen(currentBorder, borderThickness))
                {
                    borderPen.Alignment = PenAlignment.Inset;
                    ModernTheme.DrawRoundedRectangle(g, borderPen, rect, borderRadius);
                }
            }

            // Focus Ring (2026 thin glow inside the button)
            if (isFocused)
            {
                RectangleF focusRect = rect;
                focusRect.Inflate(-2f, -2f);
                using (Pen focusPen = new Pen(Color.FromArgb(120, ModernTheme.AccentBlue), 1f))
                {
                    focusPen.DashStyle = DashStyle.Dot;
                    ModernTheme.DrawRoundedRectangle(g, focusPen, focusRect, Math.Max(0, borderRadius - 2));
                }
            }

            // Draw Text and Icon
            DrawContent(g, currentText);
        }

        private void DrawContent(Graphics g, Color textColor)
        {
            Rectangle textRect = ClientRectangle;
            Rectangle iconRect = Rectangle.Empty;

            if (icon != null)
            {
                int iconWidth = icon.Width;
                int iconHeight = icon.Height;

                // Scale down icon if it's larger than the button height minus padding
                int maxIconSize = Height - 12;
                if (iconWidth > maxIconSize || iconHeight > maxIconSize)
                {
                    float ratio = Math.Min((float)maxIconSize / iconWidth, (float)maxIconSize / iconHeight);
                    iconWidth = (int)(iconWidth * ratio);
                    iconHeight = (int)(iconHeight * ratio);
                }

                // Compute positions based on IconAlign
                int iconX = 12;
                int iconY = (Height - iconHeight) / 2;

                if (iconAlign == ContentAlignment.MiddleRight)
                {
                    iconX = Width - iconWidth - 12;
                    textRect.Width -= (iconWidth + iconSpacing);
                }
                else // Default Left
                {
                    textRect.X += (iconWidth + iconSpacing);
                    textRect.Width -= (iconWidth + iconSpacing);
                }

                iconRect = new Rectangle(iconX, iconY, iconWidth, iconHeight);
                g.DrawImage(icon, iconRect);
            }

            // Draw Text
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            
            // Align text relative to remaining rectangle
            if (icon != null)
            {
                if (iconAlign == ContentAlignment.MiddleLeft)
                {
                    flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
                }
                else
                {
                    flags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
                }
            }

            TextRenderer.DrawText(g, Text, Font, textRect, textColor, flags);
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
