using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [DefaultEvent("TextChanged")]
    [ToolboxItem(true)]
    [Description("A composite form input control containing a label, ModernTextBox, and helper/error label.")]
    public class ModernFormInput : UserControl
    {
        private readonly Label lblTitle;
        private readonly ModernTextBox txtInput;
        private readonly Label lblHelper;

        private bool isRequired = false;
        private bool isError = false;
        private string helperText = "";
        private string errorText = "";

        [Category("Vietcode UI")]
        [Description("The label text displayed above the input field.")]
        public string LabelText
        {
            get => lblTitle.Text.TrimEnd(' ', '*');
            set
            {
                lblTitle.Text = value + (isRequired ? " *" : "");
                Invalidate();
            }
        }

        [Category("Vietcode UI")]
        [Description("The value entered in the text box.")]
        public override string Text
        {
            get => txtInput.Text;
            set => txtInput.Text = value;
        }

        [Category("Vietcode UI")]
        [Description("Placeholder text shown when the input is empty.")]
        public string PlaceholderText
        {
            get => txtInput.PlaceholderText;
            set => txtInput.PlaceholderText = value;
        }

        [Category("Vietcode UI")]
        [Description("Indicates if this input is required (displays a red asterisk).")]
        [DefaultValue(false)]
        public bool IsRequired
        {
            get => isRequired;
            set
            {
                isRequired = value;
                string cleanLabel = LabelText;
                lblTitle.Text = cleanLabel + (isRequired ? " *" : "");
            }
        }

        [Category("Vietcode UI")]
        [Description("Defines if the input is in an error state.")]
        [DefaultValue(false)]
        public bool IsError
        {
            get => isError;
            set
            {
                isError = value;
                UpdateState();
            }
        }

        [Category("Vietcode UI")]
        [Description("Helper/Hint text displayed below the input field.")]
        public string HelperText
        {
            get => helperText;
            set
            {
                helperText = value;
                UpdateState();
            }
        }

        [Category("Vietcode UI")]
        [Description("Error message displayed below the input field when IsError is true.")]
        public string ErrorText
        {
            get => errorText;
            set
            {
                errorText = value;
                UpdateState();
            }
        }

        [Category("Vietcode UI")]
        [Description("Mask input characters for password fields.")]
        [DefaultValue(false)]
        public bool PasswordChar
        {
            get => txtInput.PasswordChar;
            set => txtInput.PasswordChar = value;
        }

        [Category("Vietcode UI")]
        [Description("The corner radius of the input field.")]
        [DefaultValue(8)]
        public int BorderRadius
        {
            get => txtInput.BorderRadius;
            set => txtInput.BorderRadius = value;
        }

        public new event EventHandler? TextChanged;

        public ModernFormInput()
        {
            // Set styles
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            // Initialize Title Label
            lblTitle = new Label
            {
                Text = "Input Label",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ModernTheme.DarkTextSecondary,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            // Initialize Modern TextBox
            txtInput = new ModernTextBox
            {
                Location = new Point(0, 22),
                Size = new Size(Width, 40),
                BorderRadius = 8,
                BorderSize = 1,
                UnderlinedStyle = false,
                BorderColor = ModernTheme.DarkBorder,
                BorderFocusColor = ModernTheme.AccentViolet
            };
            txtInput.TextChanged += (s, e) => TextChanged?.Invoke(this, e);

            // Initialize Helper Label
            lblHelper = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 8f, FontStyle.Regular),
                ForeColor = ModernTheme.DarkTextSecondary,
                AutoSize = true,
                Location = new Point(2, 64)
            };

            // Add controls
            Controls.Add(lblTitle);
            Controls.Add(txtInput);
            Controls.Add(lblHelper);

            // Layout setup
            UpdateState();
            Size = new Size(280, 82);
        }

        private void UpdateState()
        {
            if (txtInput == null || lblHelper == null) return;

            if (isError)
            {
                txtInput.BorderColor = ModernTheme.AccentCrimson;
                txtInput.BorderFocusColor = ModernTheme.AccentCrimson;
                lblHelper.Text = string.IsNullOrEmpty(errorText) ? helperText : errorText;
                lblHelper.ForeColor = ModernTheme.AccentCrimson;
            }
            else
            {
                // Normal
                txtInput.BorderColor = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                    ? ModernTheme.DarkBorder 
                    : ModernTheme.LightBorder;
                txtInput.BorderFocusColor = ModernTheme.AccentViolet;
                lblHelper.Text = helperText;
                lblHelper.ForeColor = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                    ? ModernTheme.DarkTextSecondary 
                    : ModernTheme.LightTextSecondary;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (txtInput != null) txtInput.Width = Width;
            if (lblHelper != null) lblHelper.Width = Width - 4;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Sync theme values dynamically if theme switches
            lblTitle.ForeColor = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? ModernTheme.DarkTextSecondary 
                : ModernTheme.LightTextSecondary;
            
            UpdateState();
            base.OnPaint(e);
        }
    }
}
