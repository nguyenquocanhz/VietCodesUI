using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    public enum ModernMessageIcon
    {
        None,
        Information,
        Success,
        Warning,
        Error
    }

    public class ModernMessageBox : Form
    {
        private readonly string messageText;
        private readonly string titleText;
        private readonly MessageBoxButtons buttons;
        private readonly ModernMessageIcon messageIcon;

        private Panel titleBar = null!;
        private Label lblTitle = null!;
        private Button btnClose = null!;
        private Label lblMessage = null!;
        private Panel footerPanel = null!;
        private Panel iconPanel = null!;

        // Fade-in Timer
        private System.Windows.Forms.Timer? fadeInTimer;

        private ModernMessageBox(string text, string title, MessageBoxButtons buttons, ModernMessageIcon icon)
        {
            this.messageText = text;
            this.titleText = string.IsNullOrEmpty(title) ? "Notification" : title;
            this.buttons = buttons;
            this.messageIcon = icon;

            InitializeComponent();
            ApplyTheme();
        }

        public static DialogResult Show(string text, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK, ModernMessageIcon icon = ModernMessageIcon.None)
        {
            using (var box = new ModernMessageBox(text, title, buttons, icon))
            {
                // Try to find the active form to center on
                Form? activeForm = Form.ActiveForm;
                if (activeForm != null && !activeForm.InvokeRequired)
                {
                    box.Owner = activeForm;
                    box.StartPosition = FormStartPosition.CenterParent;
                }
                else
                {
                    box.StartPosition = FormStartPosition.CenterScreen;
                }

                return box.ShowDialog();
            }
        }

        public static DialogResult Show(IWin32Window owner, string text, string title = "", MessageBoxButtons buttons = MessageBoxButtons.OK, ModernMessageIcon icon = ModernMessageIcon.None)
        {
            using (var box = new ModernMessageBox(text, title, buttons, icon))
            {
                box.StartPosition = FormStartPosition.CenterParent;
                return box.ShowDialog(owner);
            }
        }

        private void InitializeComponent()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new Size(420, 200);
            this.ShowInTaskbar = false;
            this.Opacity = 0; // Start transparent for fade-in

            // --- Custom Title Bar ---
            titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.Transparent
            };

            lblTitle = new Label
            {
                Text = titleText,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Location = new Point(14, 10),
                AutoSize = true
            };

            btnClose = new Button
            {
                Text = "✖",
                Font = new Font("Segoe UI", 8f),
                Size = new Size(24, 24),
                Location = new Point(Width - 34, 6),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            titleBar.Controls.Add(lblTitle);
            titleBar.Controls.Add(btnClose);
            this.Controls.Add(titleBar);

            // --- Bottom Footer for Action Buttons ---
            footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.Transparent
            };
            this.Controls.Add(footerPanel);

            // --- Left Icon Panel ---
            int contentWidth = Width;
            if (messageIcon != ModernMessageIcon.None)
            {
                iconPanel = new Panel
                {
                    Dock = DockStyle.Left,
                    Width = 65,
                    BackColor = Color.Transparent
                };
                iconPanel.Paint += IconPanel_Paint;
                this.Controls.Add(iconPanel);
                contentWidth -= 65;
            }

            // --- Center Message Text Label ---
            lblMessage = new Label
            {
                Text = messageText,
                Font = new Font("Segoe UI", 9.75f),
                Location = new Point(messageIcon != ModernMessageIcon.None ? 5 : 20, 52),
                Size = new Size(contentWidth - 25, 80),
                TextAlign = ContentAlignment.TopLeft
            };
            this.Controls.Add(lblMessage);

            // --- Build Footer Buttons ---
            BuildButtons();

            // Setup entrance animation
            fadeInTimer = new System.Windows.Forms.Timer { Interval = 10 };
            fadeInTimer.Tick += FadeInTimer_Tick;
        }

        private void FadeInTimer_Tick(object? sender, EventArgs e)
        {
            if (this.Opacity < 1.0)
            {
                this.Opacity += 0.12f;
            }
            else
            {
                this.Opacity = 1.0;
                fadeInTimer?.Stop();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            fadeInTimer?.Start();
        }

        private void BuildButtons()
        {
            footerPanel.Controls.Clear();
            int btnWidth = 90;
            int btnHeight = 34;
            int gap = 12;

            if (buttons == MessageBoxButtons.OK)
            {
                var btnOk = CreateModernButton("OK", DialogResult.OK);
                btnOk.Location = new Point((footerPanel.Width - btnWidth) / 2, (footerPanel.Height - btnHeight) / 2);
                footerPanel.Controls.Add(btnOk);
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                var btnOk = CreateModernButton("OK", DialogResult.OK);
                var btnCancel = CreateModernButton("Cancel", DialogResult.Cancel, true);

                int totalW = btnWidth * 2 + gap;
                int startX = (footerPanel.Width - totalW) / 2;

                btnOk.Location = new Point(startX, (footerPanel.Height - btnHeight) / 2);
                btnCancel.Location = new Point(startX + btnWidth + gap, (footerPanel.Height - btnHeight) / 2);

                footerPanel.Controls.Add(btnOk);
                footerPanel.Controls.Add(btnCancel);
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                var btnYes = CreateModernButton("Yes", DialogResult.Yes);
                var btnNo = CreateModernButton("No", DialogResult.No, true);

                int totalW = btnWidth * 2 + gap;
                int startX = (footerPanel.Width - totalW) / 2;

                btnYes.Location = new Point(startX, (footerPanel.Height - btnHeight) / 2);
                btnNo.Location = new Point(startX + btnWidth + gap, (footerPanel.Height - btnHeight) / 2);

                footerPanel.Controls.Add(btnYes);
                footerPanel.Controls.Add(btnNo);
            }
        }

        private ModernButton CreateModernButton(string text, DialogResult result, bool isSecondary = false)
        {
            var btn = new ModernButton
            {
                Text = text,
                Size = new Size(90, 34),
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Style = isSecondary ? ModernButtonStyle.Outline : ModernButtonStyle.Solid,
                ButtonColor = isSecondary ? Color.FromArgb(71, 85, 105) : ModernTheme.AccentViolet,
                TextColor = Color.White
            };
            btn.Click += (s, e) => { this.DialogResult = result; this.Close(); };
            return btn;
        }

        private void IconPanel_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int iconSize = 32;
            int x = (iconPanel.Width - iconSize) / 2;
            int y = 16; // Align near top of text

            RectangleF iconRect = new RectangleF(x, y, iconSize, iconSize);

            switch (messageIcon)
            {
                case ModernMessageIcon.Information:
                    DrawCircleIcon(g, iconRect, ModernTheme.AccentBlue, "ℹ");
                    break;
                case ModernMessageIcon.Success:
                    DrawCircleIcon(g, iconRect, ModernTheme.AccentEmerald, "✔");
                    break;
                case ModernMessageIcon.Warning:
                    DrawCircleIcon(g, iconRect, ModernTheme.AccentBlue, "⚠", Color.FromArgb(245, 158, 11)); // Amber
                    break;
                case ModernMessageIcon.Error:
                    DrawCircleIcon(g, iconRect, ModernTheme.AccentCrimson, "✖");
                    break;
            }
        }

        private void DrawCircleIcon(Graphics g, RectangleF rect, Color accent, string symbol, Color? customBg = null)
        {
            Color circleBg = customBg ?? Color.FromArgb(25, accent);
            using (SolidBrush bgBrush = new SolidBrush(circleBg))
            {
                g.FillEllipse(bgBrush, rect);
            }
            using (Pen borderPen = new Pen(accent, 1.5f))
            {
                g.DrawEllipse(borderPen, rect);
            }

            // Draw Symbol Center
            using (Font f = new Font("Segoe UI", 12f, FontStyle.Bold))
            {
                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText(g, symbol, f, Rectangle.Round(rect), accent, flags);
            }
        }

        private void ApplyTheme()
        {
            Color bg = ModernTheme.CardBackground;
            Color text = ModernTheme.TextPrimary;
            Color textSec = ModernTheme.TextSecondary;
            Color border = ModernTheme.Border;

            this.BackColor = bg;
            lblTitle.ForeColor = textSec;
            lblMessage.ForeColor = text;

            btnClose.ForeColor = textSec;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, text);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(40, text);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw a subtle border around the custom messagebox
            Rectangle borderRect = ClientRectangle;
            borderRect.Width -= 1;
            borderRect.Height -= 1;
            using (Pen p = new Pen(ModernTheme.Border, 1.5f))
            {
                g.DrawRectangle(p, borderRect);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fadeInTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
