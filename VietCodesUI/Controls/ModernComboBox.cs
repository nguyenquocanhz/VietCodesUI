using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [ToolboxItem(true)]
    [Description("A premium modern ComboBox with flat styling, custom arrow, and themed drop-down items.")]
    public class ModernComboBox : ComboBox
    {
        private Color borderColor = ModernTheme.DarkBorder;
        private Color focusBorderColor = ModernTheme.AccentViolet;
        private Color arrowColor = ModernTheme.AccentViolet;
        private int borderRadius = 8;

        private bool isFocused = false;
        private bool isHovered = false;

        [Category("Vietcode UI")]
        [Description("The border color of the ComboBox.")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The border color on focus.")]
        public Color FocusBorderColor
        {
            get => focusBorderColor;
            set { focusBorderColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The color of the dropdown arrow.")]
        public Color ArrowColor
        {
            get => arrowColor;
            set { arrowColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        [Description("The corner radius of the ComboBox.")]
        [DefaultValue(8)]
        public int BorderRadius
        {
            get => borderRadius;
            set
            {
                borderRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        public ModernComboBox()
        {
            // Set styles to support custom painting
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            Size = new Size(200, 38);
            Font = new Font("Segoe UI", 10f);
            
            // Adjust default padding / height
            ItemHeight = 24;
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

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Define themed colors
            Color bgCol = ModernTheme.CardBackground;
            Color textCol = ModernTheme.TextPrimary;
            Color selectBgCol = Color.FromArgb(40, ModernTheme.AccentViolet);
            Color selectTextCol = ModernTheme.AccentViolet;

            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // Draw Item Background
            using (SolidBrush bgBrush = new SolidBrush(isSelected ? selectBgCol : bgCol))
            {
                g.FillRectangle(bgBrush, e.Bounds);
            }

            // Draw Item Text
            string text = Items[e.Index]?.ToString() ?? "";
            using (Font itemFont = new Font("Segoe UI", 9.5f, isSelected ? FontStyle.Bold : FontStyle.Regular))
            {
                TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                Rectangle textBounds = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 16, e.Bounds.Height);
                TextRenderer.DrawText(g, text, itemFont, textBounds, isSelected ? selectTextCol : textCol, flags);
            }

            e.DrawFocusRectangle();
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

            // Theme values
            Color bgCol = ModernTheme.CardBackground;
            Color textCol = ModernTheme.TextPrimary;
            Color activeBorder = isFocused ? focusBorderColor : (isHovered ? ControlPaint.Light(borderColor, 0.2f) : borderColor);

            RectangleF rect = new RectangleF(0, 0, Width, Height);
            rect.X += 0.75f;
            rect.Y += 0.75f;
            rect.Width -= 1.5f;
            rect.Height -= 1.5f;

            // Draw Background
            using (SolidBrush bgBrush = new SolidBrush(bgCol))
            {
                ModernTheme.FillRoundedRectangle(g, bgBrush, rect, borderRadius);
            }

            // Draw Border
            using (Pen borderPen = new Pen(activeBorder, 1.5f))
            {
                borderPen.Alignment = PenAlignment.Inset;
                ModernTheme.DrawRoundedRectangle(g, borderPen, rect, borderRadius);
            }

            // Draw Arrow (Caret pointing down)
            int arrowSize = 6;
            float arrowX = Width - 20;
            float arrowY = (Height - arrowSize) / 2f + 1;

            using (Pen arrowPen = new Pen(arrowColor, 2f))
            {
                arrowPen.StartCap = LineCap.Round;
                arrowPen.EndCap = LineCap.Round;
                g.DrawLine(arrowPen, arrowX - arrowSize, arrowY, arrowX, arrowY + arrowSize);
                g.DrawLine(arrowPen, arrowX, arrowY + arrowSize, arrowX + arrowSize, arrowY);
            }

            // Draw Selected Text
            string selectedText = SelectedItem != null ? SelectedItem.ToString() ?? "" : Text;
            Rectangle textRect = new Rectangle(12, 0, Width - 40, Height);
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(g, selectedText, Font, textRect, textCol, flags);
        }
    }
}
