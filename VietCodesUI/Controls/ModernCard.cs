using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [ToolboxItem(true)]
    [Description("A premium panel with rounded corners, custom borders, and soft shadow simulation.")]
    public class ModernCard : Panel
    {
        private int borderRadius = 12;
        private Color cardColor = Color.Empty;
        private Color borderColor = Color.Empty;
        private float borderThickness = 1f;
        private bool showShadow = true;
        private int shadowDepth = 4;

        [Category("Vietcode UI")]
        [Description("The corner radius of the card.")]
        [DefaultValue(12)]
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
        [Description("The background color of the card. If empty, uses the active theme card color.")]
        public Color CardColor
        {
            get => cardColor;
            set
            {
                cardColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The border color of the card. If empty, uses the active theme border color.")]
        public Color BorderColor
        {
            get => borderColor;
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The thickness of the card border.")]
        [DefaultValue(1f)]
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
        [Description("Determines if a soft shadow is drawn around the card.")]
        [DefaultValue(true)]
        public bool ShowShadow
        {
            get => showShadow;
            set
            {
                showShadow = value;
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The size/depth of the simulated soft shadow.")]
        [DefaultValue(4)]
        public int ShadowDepth
        {
            get => shadowDepth;
            set
            {
                shadowDepth = Math.Max(0, value);
                Invalidate();
            }
        }

        public ModernCard()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            Size = new Size(200, 150);
            Padding = new Padding(12);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear with parent background to keep rounded transparency clean
            if (Parent != null)
            {
                using (SolidBrush parentBrush = new SolidBrush(Parent.BackColor))
                {
                    g.FillRectangle(parentBrush, ClientRectangle);
                }
            }

            // Determine colors
            Color bgCol = cardColor.IsEmpty ? ModernTheme.CardBackground : cardColor;
            Color borderCol = borderColor.IsEmpty ? ModernTheme.Border : borderColor;

            // Calculate card boundaries, insetting for shadow if enabled
            RectangleF cardRect = new RectangleF(0, 0, Width, Height);
            if (showShadow && shadowDepth > 0)
            {
                cardRect.Width -= shadowDepth;
                cardRect.Height -= shadowDepth;
            }

            // Inset slightly for border thickness to avoid clipping
            float inset = borderThickness / 2f;
            cardRect.X += inset;
            cardRect.Y += inset;
            cardRect.Width -= borderThickness;
            cardRect.Height -= borderThickness;

            // Draw shadow simulation
            if (showShadow && shadowDepth > 0)
            {
                // Multi-layered soft alpha rings
                int maxAlpha = 20;
                int layers = shadowDepth;
                for (int i = 1; i <= layers; i++)
                {
                    int alpha = maxAlpha / (i + 1);
                    Color shadowColor = Color.FromArgb(alpha, Color.Black);
                    using (Pen shadowPen = new Pen(shadowColor, i * 1.5f))
                    {
                        shadowPen.Alignment = PenAlignment.Outset;
                        // Draw offset shadow path
                        RectangleF shadowRect = cardRect;
                        shadowRect.X += i * 0.75f;
                        shadowRect.Y += i * 0.75f;
                        ModernTheme.DrawRoundedRectangle(g, shadowPen, shadowRect, borderRadius);
                    }
                }
            }

            // Fill card background
            using (SolidBrush bgBrush = new SolidBrush(bgCol))
            {
                ModernTheme.FillRoundedRectangle(g, bgBrush, cardRect, borderRadius);
            }

            // Draw border
            if (borderThickness > 0 && borderCol != Color.Transparent)
            {
                using (Pen borderPen = new Pen(borderCol, borderThickness))
                {
                    borderPen.Alignment = PenAlignment.Inset;
                    ModernTheme.DrawRoundedRectangle(g, borderPen, cardRect, borderRadius);
                }
            }
        }
    }
}
