using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [DefaultEvent("TextChanged")]
    [ToolboxItem(true)]
    [Description("A premium modern text box with rounded borders, focus glow animations, and placeholder text.")]
    public class ModernTextBox : UserControl
    {
        private Color borderColor = ModernTheme.DarkBorder;
        private Color borderFocusColor = ModernTheme.AccentViolet;
        private int borderRadius = 8;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private Color placeholderColor = Color.DarkGray;
        private string placeholderText = "";
        private bool isPasswordChar = false;
        private bool isPlaceholder = false;

        // Components
        private readonly TextBox textBox1;
        private readonly System.Windows.Forms.Timer animationTimer;
        private float focusProgress = 0f;
        private bool isFocused = false;

        [Category("Vietcode UI")]
        public Color BorderColor
        {
            get => borderColor;
            set { borderColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        public Color BorderFocusColor
        {
            get => borderFocusColor;
            set { borderFocusColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        public int BorderRadius
        {
            get => borderRadius;
            set { borderRadius = Math.Max(0, value); Invalidate(); }
        }

        [Category("Vietcode UI")]
        public int BorderSize
        {
            get => borderSize;
            set { borderSize = Math.Max(1, value); Invalidate(); }
        }

        [Category("Vietcode UI")]
        public bool UnderlinedStyle
        {
            get => underlinedStyle;
            set { underlinedStyle = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        public bool PasswordChar
        {
            get => isPasswordChar;
            set
            {
                isPasswordChar = value;
                textBox1.UseSystemPasswordChar = value;
            }
        }

        [Category("Vietcode UI")]
        public bool Multiline
        {
            get => textBox1.Multiline;
            set
            {
                textBox1.Multiline = value;
                UpdateControlHeight();
            }
        }

        [Category("Vietcode UI")]
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                textBox1.BackColor = value;
            }
        }

        [Category("Vietcode UI")]
        public override Color ForeColor
        {
            get => base.ForeColor;
            set
            {
                base.ForeColor = value;
                textBox1.ForeColor = value;
            }
        }

        [Category("Vietcode UI")]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                textBox1.Font = value;
                if (DesignMode) UpdateControlHeight();
            }
        }

        [Category("Vietcode UI")]
        public string PlaceholderText
        {
            get => placeholderText;
            set
            {
                placeholderText = value;
                SetPlaceholder();
            }
        }

        [Category("Vietcode UI")]
        public Color PlaceholderColor
        {
            get => placeholderColor;
            set { placeholderColor = value; Invalidate(); }
        }

        [Category("Vietcode UI")]
        public override string Text
        {
            get => isPlaceholder ? "" : textBox1.Text;
            set
            {
                textBox1.Text = value;
                SetPlaceholder();
            }
        }

        public new event EventHandler? TextChanged;

        public ModernTextBox()
        {
            // Initialize TextBox
            textBox1 = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Multiline = false
            };
            
            textBox1.TextChanged += TextBox1_TextChanged;
            textBox1.Enter += TextBox1_Enter;
            textBox1.Leave += TextBox1_Leave;
            textBox1.Click += TextBox1_Click;
            textBox1.KeyDown += TextBox1_KeyDown;

            // Set UserControl Properties
            Padding = new Padding(12, 10, 12, 10);
            Size = new Size(250, 40);
            BackColor = ModernTheme.DarkCardBackground;
            ForeColor = ModernTheme.DarkTextPrimary;
            Font = new Font("Segoe UI", 10F);
            
            textBox1.BackColor = BackColor;
            textBox1.ForeColor = ForeColor;
            textBox1.Font = Font;
            textBox1.Location = new Point(12, (Height - textBox1.Height) / 2);
            textBox1.Width = Width - 24;

            Controls.Add(textBox1);

            // Animation Timer
            animationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            animationTimer.Tick += AnimationTimer_Tick;

            UpdateControlHeight();
        }

        private void TextBox1_TextChanged(object? sender, EventArgs e)
        {
            if (isPlaceholder && textBox1.Text != placeholderText)
            {
                isPlaceholder = false;
                textBox1.UseSystemPasswordChar = isPasswordChar;
            }
            TextChanged?.Invoke(this, e);
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            float target = isFocused ? 1f : 0f;
            if (Math.Abs(focusProgress - target) > 0.05f)
            {
                focusProgress += (target - focusProgress) * 0.25f;
                Invalidate();
            }
            else
            {
                focusProgress = target;
                animationTimer.Stop();
                Invalidate();
            }
        }

        private void TextBox1_Enter(object? sender, EventArgs e)
        {
            isFocused = true;
            if (isPlaceholder)
            {
                isPlaceholder = false;
                textBox1.Text = "";
                textBox1.ForeColor = ForeColor;
                textBox1.UseSystemPasswordChar = isPasswordChar;
            }
            animationTimer.Start();
        }

        private void TextBox1_Leave(object? sender, EventArgs e)
        {
            isFocused = false;
            SetPlaceholder();
            animationTimer.Start();
        }

        private void TextBox1_Click(object? sender, EventArgs e) => OnClick(e);
        private void TextBox1_KeyDown(object? sender, KeyEventArgs e) => OnKeyDown(e);

        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrEmpty(placeholderText))
            {
                isPlaceholder = true;
                textBox1.Text = placeholderText;
                textBox1.ForeColor = placeholderColor;
                textBox1.UseSystemPasswordChar = false;
            }
            else if (!isPlaceholder)
            {
                textBox1.ForeColor = ForeColor;
            }
        }

        private void UpdateControlHeight()
        {
            if (!textBox1.Multiline)
            {
                int txtHeight = TextRenderer.MeasureText("Text", Font).Height;
                textBox1.Height = txtHeight;
                Height = textBox1.Height + Padding.Top + Padding.Bottom;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateControlHeight();
            if (!textBox1.Multiline)
            {
                textBox1.Location = new Point(Padding.Left, (Height - textBox1.Height) / 2);
            }
            else
            {
                textBox1.Location = new Point(Padding.Left, Padding.Top);
                textBox1.Height = Height - Padding.Top - Padding.Bottom;
            }
            textBox1.Width = Width - Padding.Left - Padding.Right;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear control background with parent backcolor
            if (Parent != null)
            {
                using (SolidBrush parentBrush = new SolidBrush(Parent.BackColor))
                {
                    g.FillRectangle(parentBrush, ClientRectangle);
                }
            }

            RectangleF rect = new RectangleF(0, 0, Width, Height);
            float inset = borderSize / 2f;
            rect.X += inset;
            rect.Y += inset;
            rect.Width -= borderSize;
            rect.Height -= borderSize;

            // Fill text box background
            using (SolidBrush bgBrush = new SolidBrush(BackColor))
            {
                if (underlinedStyle)
                {
                    g.FillRectangle(bgBrush, ClientRectangle);
                }
                else
                {
                    ModernTheme.FillRoundedRectangle(g, bgBrush, rect, borderRadius);
                }
            }

            // Calculate animated border color
            Color currentBorderColor = ModernTheme.BlendColors(borderColor, borderFocusColor, focusProgress);

            // Draw border
            using (Pen borderPen = new Pen(currentBorderColor, borderSize))
            {
                if (underlinedStyle)
                {
                    g.DrawLine(borderPen, 0, Height - borderSize, Width, Height - borderSize);
                }
                else
                {
                    borderPen.Alignment = PenAlignment.Inset;
                    ModernTheme.DrawRoundedRectangle(g, borderPen, rect, borderRadius);
                }
            }
        }
    }
}
