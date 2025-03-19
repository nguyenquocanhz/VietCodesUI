using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VietcodeUI.Controls
{
    public partial class RoundedButton : Button
    {
        private int borderRadius = 20;
        private Color borderColor = Color.Silver;
        private float borderThickness = 1.75f;
        private Color buttonColor = Color.MediumSlateBlue;
        private Color textColor = Color.White;
        private Color hoverColor;
        private Color clickColor;
        private Color focusBorderColor = Color.DodgerBlue;

        private bool isHovered = false;
        private bool isClicked = false;
        private bool isFocused = false;

        [Category("Vietcode UI")]
        [Description("The border radius of the button.")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The color of the button border.")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The thickness of the button border.")]
        public float BorderThickness
        {
            get { return borderThickness; }
            set
            {
                borderThickness = Math.Max(0, value);
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The background color of the button.")]
        public Color ButtonColor
        {
            get { return buttonColor; }
            set
            {
                buttonColor = value;
                hoverColor = ControlPaint.Light(buttonColor, 0.2f);
                clickColor = ControlPaint.Dark(buttonColor, 0.2f);
                BackColor = buttonColor;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The text color of the button.")]
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                ForeColor = textColor;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The focus border color of the button.")]
        public Color FocusBorderColor
        {
            get { return focusBorderColor; }
            set
            {
                focusBorderColor = value;
                Invalidate();
            }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(150, 40);
            ButtonColor = buttonColor;
            ForeColor = textColor;

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        }

        private GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            float m = 2.75F;
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();

            GraphPath.AddArc(Rect.X + m, Rect.Y + m, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2 + m, Rect.Y + m, Rect.Width - r2 - m, Rect.Y + m);
            GraphPath.AddArc(Rect.X + Rect.Width - radius - m, Rect.Y + m, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width - m, Rect.Y + r2, Rect.Width - m, Rect.Height - r2 - m);
            GraphPath.AddArc(Rect.X + Rect.Width - radius - m, Rect.Y + Rect.Height - radius - m, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2 - m, Rect.Height - m, Rect.X + r2 - m, Rect.Height - m);
            GraphPath.AddArc(Rect.X + m, Rect.Y + Rect.Height - radius - m, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X + m, Rect.Height - r2 - m, Rect.X + m, Rect.Y + r2 + m);

            GraphPath.CloseFigure();
            return GraphPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF Rect = new RectangleF(0, 0, Width, Height);
            GraphicsPath GraphPath = GetRoundPath(Rect, borderRadius);

            Region = new Region(GraphPath);
            Color currentColor = isClicked ? clickColor : isHovered ? hoverColor : buttonColor;
            e.Graphics.FillPath(new SolidBrush(currentColor), GraphPath);

            using (Pen pen = new Pen(isFocused ? focusBorderColor : borderColor, borderThickness))
            {
                pen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(pen, GraphPath);
            }

            // Vẽ lại chữ
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(e.Graphics, Text, Font, Rectangle.Round(Rect), textColor, flags);
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
            isClicked = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isClicked = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isClicked = false;
            Invalidate();
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
    }
}
