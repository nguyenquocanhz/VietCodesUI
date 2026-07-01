using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [ToolboxItem(true)]
    [Description("A premium TabControl with flat headers, active tab indicators, and custom styling.")]
    public class ModernTabControl : TabControl
    {
        private Color activeColor = ModernTheme.AccentViolet;
        private int hoveredIndex = -1;

        [Category("Vietcode UI")]
        [Description("The color of the selected tab indicator and accent text.")]
        public Color ActiveColor
        {
            get => activeColor;
            set { activeColor = value; Invalidate(); }
        }

        public ModernTabControl()
        {
            // Enable user paint to fully customize TabControl appearance and remove ugly retro border
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer, true);

            DoubleBuffered = true;
            ItemSize = new Size(100, 36);
            SizeMode = TabSizeMode.Fixed;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHoveredIndex = -1;
            for (int i = 0; i < TabCount; i++)
            {
                if (GetTabRect(i).Contains(e.Location))
                {
                    newHoveredIndex = i;
                    break;
                }
            }

            if (hoveredIndex != newHoveredIndex)
            {
                hoveredIndex = newHoveredIndex;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hoveredIndex = -1;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Color bgCol = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? ModernTheme.DarkBackground 
                : ModernTheme.LightBackground;

            Color cardCol = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? ModernTheme.DarkCardBackground 
                : ModernTheme.LightCardBackground;

            Color textPrimary = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? ModernTheme.DarkTextPrimary 
                : ModernTheme.LightTextPrimary;

            Color textSecondary = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? ModernTheme.DarkTextSecondary 
                : ModernTheme.LightTextSecondary;

            Color borderCol = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? ModernTheme.DarkBorder 
                : ModernTheme.LightBorder;

            // Draw total control background
            using (SolidBrush bgBrush = new SolidBrush(bgCol))
            {
                g.FillRectangle(bgBrush, ClientRectangle);
            }

            // Draw the tab content background area
            if (TabCount > 0 && SelectedIndex >= 0)
            {
                Rectangle headerRect = GetTabRect(SelectedIndex);
                Rectangle contentRect = ClientRectangle;
                contentRect.Y = ItemSize.Height + 2; // Offset for headers
                contentRect.Height -= (ItemSize.Height + 2);

                using (SolidBrush contentBrush = new SolidBrush(cardCol))
                {
                    g.FillRectangle(contentBrush, contentRect);
                }

                // Draw a clean border separator between header and content
                using (Pen borderPen = new Pen(borderCol, 1f))
                {
                    g.DrawLine(borderPen, 0, ItemSize.Height + 1, Width, ItemSize.Height + 1);
                }
            }

            // Draw tab headers
            for (int i = 0; i < TabCount; i++)
            {
                Rectangle tabRect = GetTabRect(i);
                bool isSelected = i == SelectedIndex;
                bool isHovered = i == hoveredIndex;

                // Subtle background for states
                if (isSelected)
                {
                    using (SolidBrush selBrush = new SolidBrush(cardCol))
                    {
                        g.FillRectangle(selBrush, tabRect);
                    }

                    // Active indicator bar (underline)
                    using (SolidBrush indicatorBrush = new SolidBrush(activeColor))
                    {
                        g.FillRectangle(indicatorBrush, tabRect.X, tabRect.Bottom - 2, tabRect.Width, 3);
                    }
                }
                else if (isHovered)
                {
                    Color hoverCol = Color.FromArgb(15, activeColor);
                    using (SolidBrush hovBrush = new SolidBrush(hoverCol))
                    {
                        g.FillRectangle(hovBrush, tabRect);
                    }
                }

                // Draw Tab Text
                string title = TabPages[i].Text;
                Color txtColor = isSelected ? activeColor : (isHovered ? textPrimary : textSecondary);
                using (Font font = new Font(Font.FontFamily, isSelected ? 9.5f : 9f, isSelected ? FontStyle.Bold : FontStyle.Regular))
                {
                    TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
                    TextRenderer.DrawText(g, title, font, tabRect, txtColor, flags);
                }
            }
        }
    }
}
