using System;
using System.Drawing;
using System.Windows.Forms;
using VietcodeUI.Theme;
using VietcodeUI.Controls;

namespace VietcodeUI.Demo
{
    public class MainForm : Form
    {
        private Panel sidebar = null!;
        private Label lblLogo = null!;
        private ModernButton btnTabDashboard = null!;
        private ModernButton btnTabForms = null!;
        private ModernButton btnTabAuth = null!;
        private ModernToggleButton toggleTheme = null!;
        private Label lblThemeToggle = null!;

        private Panel mainContentArea = null!;
        private TabControl pageControl = null!;

        // Tab Pages
        private TabPage pageDashboard = null!;
        private TabPage pageForms = null!;
        private TabPage pageAuth = null!;

        // Dashboard Stats
        private System.Windows.Forms.Timer systemTimer = null!;
        private ModernProgressBar cpuProgressBar = null!;
        private Label lblCpuVal = null!;

        public MainForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 650);
            this.Text = "VietCodesUI - Premium WinForms Component Suite (Gu 2026)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(950, 600);

            // --- Layout Panels ---

            // Left Sidebar Panel
            sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = ModernTheme.DarkBackground
            };

            // Main Content Container
            mainContentArea = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernTheme.DarkBackground
            };

            this.Controls.Add(mainContentArea);
            this.Controls.Add(sidebar);

            // --- Sidebar Content ---
            lblLogo = new Label
            {
                Text = "VietCodes UI",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = ModernTheme.AccentViolet,
                Location = new Point(20, 25),
                AutoSize = true
            };
            sidebar.Controls.Add(lblLogo);

            // Tab Navigation Buttons
            btnTabDashboard = new ModernButton
            {
                Text = "   Dashboard",
                Location = new Point(15, 80),
                Width = 190,
                Height = 40,
                Style = ModernButtonStyle.Ghost,
                ButtonColor = ModernTheme.AccentViolet,
                TextColor = ModernTheme.DarkTextPrimary,
                TextAlign = ContentAlignment.MiddleLeft,
                BorderRadius = 8
            };
            btnTabDashboard.Click += (s, e) => SwitchTab(0);
            sidebar.Controls.Add(btnTabDashboard);

            btnTabForms = new ModernButton
            {
                Text = "   Form Controls",
                Location = new Point(15, 130),
                Width = 190,
                Height = 40,
                Style = ModernButtonStyle.Ghost,
                ButtonColor = ModernTheme.AccentViolet,
                TextColor = ModernTheme.DarkTextSecondary,
                TextAlign = ContentAlignment.MiddleLeft,
                BorderRadius = 8
            };
            btnTabForms.Click += (s, e) => SwitchTab(1);
            sidebar.Controls.Add(btnTabForms);

            btnTabAuth = new ModernButton
            {
                Text = "   Auth & Validation",
                Location = new Point(15, 180),
                Width = 190,
                Height = 40,
                Style = ModernButtonStyle.Ghost,
                ButtonColor = ModernTheme.AccentViolet,
                TextColor = ModernTheme.DarkTextSecondary,
                TextAlign = ContentAlignment.MiddleLeft,
                BorderRadius = 8
            };
            btnTabAuth.Click += (s, e) => SwitchTab(2);
            sidebar.Controls.Add(btnTabAuth);

            // Theme Switcher at bottom of Sidebar
            lblThemeToggle = new Label
            {
                Text = "Dark Mode",
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = ModernTheme.DarkTextSecondary,
                Location = new Point(15, 560),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true
            };
            sidebar.Controls.Add(lblThemeToggle);

            toggleTheme = new ModernToggleButton
            {
                Location = new Point(150, 558),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Checked = true,
                OnColor = ModernTheme.AccentViolet,
                OffColor = Color.FromArgb(71, 85, 105)
            };
            toggleTheme.CheckedChanged += ToggleTheme_CheckedChanged;
            sidebar.Controls.Add(toggleTheme);

            // --- Page Switcher (TabControl with hidden headers) ---
            pageControl = new TabControl
            {
                Dock = DockStyle.Fill,
                SizeMode = TabSizeMode.Fixed,
                ItemSize = new Size(0, 1), // Hides headers
                Appearance = TabAppearance.FlatButtons
            };
            mainContentArea.Controls.Add(pageControl);

            // Create Pages
            pageDashboard = new TabPage { Text = "Dashboard" };
            pageForms = new TabPage { Text = "Forms" };
            pageAuth = new TabPage { Text = "Auth" };

            pageControl.TabPages.Add(pageDashboard);
            pageControl.TabPages.Add(pageForms);
            pageControl.TabPages.Add(pageAuth);

            // --- Page 1: Dashboard Content ---
            InitDashboardPage();

            // --- Page 2: Forms Components Content ---
            InitFormsPage();

            // --- Page 3: Auth & Validation Content ---
            InitAuthPage();

            // Background Timer for animating metrics
            systemTimer = new System.Windows.Forms.Timer { Interval = 1500 };
            systemTimer.Tick += SystemTimer_Tick;
            systemTimer.Start();
        }

        private void InitDashboardPage()
        {
            pageDashboard.Padding = new Padding(24);

            // Header Label
            var lblTitle = new Label
            {
                Text = "Analytics Dashboard",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                Location = new Point(24, 20),
                AutoSize = true
            };
            pageDashboard.Controls.Add(lblTitle);

            // Stats Card 1: Users
            var cardUsers = new ModernCard
            {
                Location = new Point(24, 70),
                Size = new Size(220, 130),
                ShowShadow = true,
                ShadowDepth = 5
            };
            var lblCard1Title = new Label
            {
                Text = "ACTIVE USERS",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = ModernTheme.DarkTextSecondary,
                Location = new Point(16, 16),
                AutoSize = true
            };
            var lblCard1Val = new Label
            {
                Text = "12,482",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                Location = new Point(14, 38),
                AutoSize = true
            };
            var lblCard1Trend = new Label
            {
                Text = "+14.3% this week",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = ModernTheme.AccentEmerald,
                Location = new Point(16, 85),
                AutoSize = true
            };
            cardUsers.Controls.Add(lblCard1Title);
            cardUsers.Controls.Add(lblCard1Val);
            cardUsers.Controls.Add(lblCard1Trend);
            pageDashboard.Controls.Add(cardUsers);

            // Stats Card 2: CPU Load
            var cardCpu = new ModernCard
            {
                Location = new Point(268, 70),
                Size = new Size(220, 130),
                ShowShadow = true,
                ShadowDepth = 5
            };
            var lblCard2Title = new Label
            {
                Text = "PROCESSOR LOAD",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = ModernTheme.DarkTextSecondary,
                Location = new Point(16, 16),
                AutoSize = true
            };
            lblCpuVal = new Label
            {
                Text = "34%",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                Location = new Point(14, 38),
                AutoSize = true
            };
            cpuProgressBar = new ModernProgressBar
            {
                Location = new Point(16, 90),
                Size = new Size(188, 10),
                Value = 34,
                ProgressColor1 = ModernTheme.AccentBlue,
                ProgressColor2 = ModernTheme.AccentViolet
            };
            cardCpu.Controls.Add(lblCard2Title);
            cardCpu.Controls.Add(lblCpuVal);
            cardCpu.Controls.Add(cpuProgressBar);
            pageDashboard.Controls.Add(cardCpu);

            // Stats Card 3: Memory Usage
            var cardMemory = new ModernCard
            {
                Location = new Point(512, 70),
                Size = new Size(220, 130),
                ShowShadow = true,
                ShadowDepth = 5
            };
            var lblCard3Title = new Label
            {
                Text = "MEMORY UTILISED",
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = ModernTheme.DarkTextSecondary,
                Location = new Point(16, 16),
                AutoSize = true
            };
            var lblCard3Val = new Label
            {
                Text = "4.2 GB / 8 GB",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                Location = new Point(14, 42),
                AutoSize = true
            };
            var memProgressBar = new ModernProgressBar
            {
                Location = new Point(16, 90),
                Size = new Size(188, 10),
                Value = 52,
                ProgressColor1 = ModernTheme.AccentEmerald,
                ProgressColor2 = ModernTheme.AccentBlue
            };
            cardMemory.Controls.Add(lblCard3Title);
            cardMemory.Controls.Add(lblCard3Val);
            cardMemory.Controls.Add(memProgressBar);
            pageDashboard.Controls.Add(cardMemory);

            // Main Dashboard Graphic Panel
            var cardGraph = new ModernCard
            {
                Location = new Point(24, 220),
                Size = new Size(708, 320),
                ShowShadow = true,
                ShadowDepth = 5
            };
            var lblGraphTitle = new Label
            {
                Text = "Operational System Metrics",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(20, 16),
                AutoSize = true
            };
            var lblGraphDesc = new Label
            {
                Text = "Everything is running smoothly. 2026 Fluent Controls initialized and validated.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = ModernTheme.DarkTextSecondary,
                Location = new Point(20, 48),
                AutoSize = true
            };
            cardGraph.Controls.Add(lblGraphTitle);
            cardGraph.Controls.Add(lblGraphDesc);

            // Modern DataGridView inside graph card
            var dgvMetrics = new ModernDataGridView
            {
                Location = new Point(20, 85),
                Size = new Size(668, 215),
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvMetrics.Columns.Add("serverName", "Server Name");
            dgvMetrics.Columns.Add("location", "Location");
            dgvMetrics.Columns.Add("cpu", "CPU");
            dgvMetrics.Columns.Add("bandwidth", "Bandwidth");
            dgvMetrics.Columns.Add("status", "Status");

            dgvMetrics.Rows.Add("Web Server #01", "Hanoi, VN", "24%", "120 Mbps", "Online");
            dgvMetrics.Rows.Add("Database Main", "HCMC, VN", "42%", "450 Mbps", "Online");
            dgvMetrics.Rows.Add("OAuth Gateway", "Da Nang, VN", "15%", "80 Mbps", "Online");
            dgvMetrics.Rows.Add("Backup Server", "Frankfurt, DE", "0%", "0 Mbps", "Offline");
            
            dgvMetrics.Columns[0].Width = 150;
            dgvMetrics.Columns[1].Width = 150;
            dgvMetrics.Columns[2].Width = 80;
            dgvMetrics.Columns[3].Width = 120;
            dgvMetrics.Columns[4].Width = 100;

            cardGraph.Controls.Add(dgvMetrics);

            pageDashboard.Controls.Add(cardGraph);
        }

        private void InitFormsPage()
        {
            pageForms.Padding = new Padding(24);

            var lblTitle = new Label
            {
                Text = "UI Core Component Library",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                Location = new Point(24, 20),
                AutoSize = true
            };
            pageForms.Controls.Add(lblTitle);

            // Left Section: Buttons
            var cardButtons = new ModernCard
            {
                Location = new Point(24, 70),
                Size = new Size(340, 470),
                ShowShadow = true
            };
            var lblSecButtons = new Label
            {
                Text = "Premium Buttons & Switches",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(20, 16),
                AutoSize = true
            };
            cardButtons.Controls.Add(lblSecButtons);

            var btnSolid = new ModernButton
            {
                Text = "Solid Accent Button",
                Location = new Point(20, 60),
                Width = 300,
                Style = ModernButtonStyle.Solid,
                ButtonColor = ModernTheme.AccentViolet
            };
            cardButtons.Controls.Add(btnSolid);

            var btnOutline = new ModernButton
            {
                Text = "Outline Accent Button",
                Location = new Point(20, 115),
                Width = 300,
                Style = ModernButtonStyle.Outline,
                ButtonColor = ModernTheme.AccentBlue
            };
            cardButtons.Controls.Add(btnOutline);

            var btnGhost = new ModernButton
            {
                Text = "Ghost Accent Button",
                Location = new Point(20, 170),
                Width = 300,
                Style = ModernButtonStyle.Ghost,
                ButtonColor = ModernTheme.AccentViolet
            };
            cardButtons.Controls.Add(btnGhost);

            // Switches
            var toggleSwitch1 = new ModernToggleButton
            {
                Location = new Point(20, 240)
            };
            var lblToggle1 = new Label
            {
                Text = "System Analytics Active",
                Font = new Font("Segoe UI", 9.5f),
                Location = new Point(75, 241),
                AutoSize = true
            };
            cardButtons.Controls.Add(toggleSwitch1);
            cardButtons.Controls.Add(lblToggle1);

            // Checkboxes
            var chkBox1 = new ModernCheckBox
            {
                Text = "Accept software licensing agreements",
                Location = new Point(20, 290),
                Size = new Size(300, 24),
                Checked = true
            };
            var chkBox2 = new ModernCheckBox
            {
                Text = "Receive commercial marketing newsletters",
                Location = new Point(20, 330),
                Size = new Size(300, 24),
                Checked = false
            };
            cardButtons.Controls.Add(chkBox1);
            cardButtons.Controls.Add(chkBox2);

            // Dropdown
            var lblDropdown = new Label
            {
                Text = "Select Region Deployment",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = ModernTheme.DarkTextSecondary,
                Location = new Point(20, 380),
                AutoSize = true
            };
            var cmbList = new ModernComboBox
            {
                Location = new Point(20, 402),
                Width = 300
            };
            cmbList.Items.AddRange(new object[] { "Production Cluster (Hanoi)", "Staging Node (Da Nang)", "Development Sandpit (HCMC)" });
            cmbList.SelectedIndex = 0;
            cardButtons.Controls.Add(lblDropdown);
            cardButtons.Controls.Add(cmbList);

            pageForms.Controls.Add(cardButtons);

            // Right Section: Input Fields & Progress Bars
            var cardInputs = new ModernCard
            {
                Location = new Point(390, 70),
                Size = new Size(340, 470),
                ShowShadow = true
            };
            var lblSecInputs = new Label
            {
                Text = "Forms Inputs & Progress Bars",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Location = new Point(20, 16),
                AutoSize = true
            };
            cardInputs.Controls.Add(lblSecInputs);

            // Form Inputs
            var txtNormal = new ModernTextBox
            {
                Location = new Point(20, 60),
                Width = 300,
                PlaceholderText = "Enter search term..."
            };
            cardInputs.Controls.Add(txtNormal);

            var txtUnderlined = new ModernTextBox
            {
                Location = new Point(20, 120),
                Width = 300,
                UnderlinedStyle = true,
                PlaceholderText = "Underlined style input..."
            };
            cardInputs.Controls.Add(txtUnderlined);

            // Progress Indicators
            var lblProgressGroup = new Label
            {
                Text = "Progress Bars",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(20, 200),
                AutoSize = true
            };
            cardInputs.Controls.Add(lblProgressGroup);

            var bar1 = new ModernProgressBar
            {
                Location = new Point(20, 235),
                Size = new Size(300, 16),
                Value = 75,
                ShowPercentage = true,
                ProgressColor1 = ModernTheme.AccentViolet,
                ProgressColor2 = ModernTheme.AccentBlue
            };
            cardInputs.Controls.Add(bar1);

            var bar2 = new ModernProgressBar
            {
                Location = new Point(20, 270),
                Size = new Size(300, 10),
                Value = 40,
                ProgressColor1 = ModernTheme.AccentEmerald,
                ProgressColor2 = ModernTheme.AccentBlue
            };
            cardInputs.Controls.Add(bar2);

            var bar3 = new ModernProgressBar
            {
                Location = new Point(20, 300),
                Size = new Size(300, 6),
                Value = 90,
                ProgressColor1 = ModernTheme.AccentCrimson,
                ProgressColor2 = ModernTheme.AccentCrimson
            };
            cardInputs.Controls.Add(bar3);

            // Add interactive slider/button to change progress bar values
            var btnRandomProgress = new ModernButton
            {
                Text = "Set Random Progress Values",
                Location = new Point(20, 350),
                Width = 300,
                Style = ModernButtonStyle.Outline,
                ButtonColor = ModernTheme.AccentBlue
            };
            btnRandomProgress.Click += (s, e) =>
            {
                var rand = new Random();
                bar1.Value = rand.Next(10, 101);
                bar2.Value = rand.Next(10, 101);
                bar3.Value = rand.Next(10, 101);
            };
            cardInputs.Controls.Add(btnRandomProgress);

            pageForms.Controls.Add(cardInputs);
        }

        private void InitAuthPage()
        {
            pageAuth.Padding = new Padding(24);

            var lblTitle = new Label
            {
                Text = "Authentication & Form Validation",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                Location = new Point(24, 20),
                AutoSize = true
            };
            pageAuth.Controls.Add(lblTitle);

            // Center card for Login form
            var cardLogin = new ModernCard
            {
                Location = new Point(190, 70),
                Size = new Size(380, 470),
                ShowShadow = true,
                ShadowDepth = 6
            };
            pageAuth.Controls.Add(cardLogin);

            var lblLoginHeader = new Label
            {
                Text = "Sign In",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };
            cardLogin.Controls.Add(lblLoginHeader);

            // Inputs
            var inputEmail = new ModernFormInput
            {
                LabelText = "Email Address",
                PlaceholderText = "name@company.com",
                HelperText = "We will never share your email.",
                Location = new Point(30, 65),
                Width = 320,
                IsRequired = true
            };
            cardLogin.Controls.Add(inputEmail);

            var inputPassword = new ModernFormInput
            {
                LabelText = "Password",
                PlaceholderText = "••••••••••••",
                HelperText = "Must be at least 8 characters.",
                Location = new Point(30, 160),
                Width = 320,
                PasswordChar = true,
                IsRequired = true
            };
            cardLogin.Controls.Add(inputPassword);

            // Sign In Action Button
            var btnSubmit = new ModernButton
            {
                Text = "Log In",
                Location = new Point(30, 260),
                Width = 320,
                Height = 42,
                ButtonColor = ModernTheme.AccentViolet
            };
            btnSubmit.Click += (s, e) =>
            {
                bool hasError = false;

                // Simple validation checks
                if (string.IsNullOrWhiteSpace(inputEmail.Text))
                {
                    inputEmail.ErrorText = "Email address cannot be empty.";
                    inputEmail.IsError = true;
                    hasError = true;
                }
                else if (!inputEmail.Text.Contains("@"))
                {
                    inputEmail.ErrorText = "Please enter a valid email address.";
                    inputEmail.IsError = true;
                    hasError = true;
                }
                else
                {
                    inputEmail.IsError = false;
                }

                if (string.IsNullOrWhiteSpace(inputPassword.Text))
                {
                    inputPassword.ErrorText = "Password cannot be empty.";
                    inputPassword.IsError = true;
                    hasError = true;
                }
                else if (inputPassword.Text.Length < 6)
                {
                    inputPassword.ErrorText = "Password must be at least 6 characters.";
                    inputPassword.IsError = true;
                    hasError = true;
                }
                else
                {
                    inputPassword.IsError = false;
                }

                if (!hasError)
                {
                    MessageBox.Show("Form validation passed!\nSigning in...", "VietCodesUI Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            cardLogin.Controls.Add(btnSubmit);

            // Divider text
            var lblDivider = new Label
            {
                Text = "─────────  or stream with  ─────────",
                Font = new Font("Segoe UI", 7.5f, FontStyle.Bold),
                ForeColor = ModernTheme.DarkTextSecondary,
                Location = new Point(30, 318),
                Size = new Size(320, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            cardLogin.Controls.Add(lblDivider);

            // Social Buttons
            var btnGoogle = new GoogleLoginButton
            {
                Location = new Point(30, 345),
                Width = 320,
                Height = 40
            };
            btnGoogle.Click += (s, e) => MessageBox.Show("Triggering Google OAuth Flow...", "Google Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cardLogin.Controls.Add(btnGoogle);

            var btnGithub = new GithubLoginButton
            {
                Location = new Point(30, 395),
                Width = 320,
                Height = 40
            };
            btnGithub.Click += (s, e) => MessageBox.Show("Triggering GitHub OAuth Flow...", "GitHub Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cardLogin.Controls.Add(btnGithub);
        }

        private void SystemTimer_Tick(object? sender, EventArgs e)
        {
            // Simulate dynamic CPU value changes on Dashboard
            var rand = new Random();
            int delta = rand.Next(-10, 11);
            int newVal = cpuProgressBar.Value + delta;
            if (newVal < 10) newVal = 10;
            if (newVal > 95) newVal = 95;

            cpuProgressBar.Value = newVal;
            lblCpuVal.Text = $"{newVal}%";
        }

        private void SwitchTab(int index)
        {
            pageControl.SelectedIndex = index;

            // Highlight active sidebar navigation button
            btnTabDashboard.TextColor = index == 0 ? ModernTheme.AccentViolet : ModernTheme.DarkTextSecondary;
            btnTabDashboard.Font = new Font(btnTabDashboard.Font.FontFamily, btnTabDashboard.Font.Size, index == 0 ? FontStyle.Bold : FontStyle.Regular);

            btnTabForms.TextColor = index == 1 ? ModernTheme.AccentViolet : ModernTheme.DarkTextSecondary;
            btnTabForms.Font = new Font(btnTabForms.Font.FontFamily, btnTabForms.Font.Size, index == 1 ? FontStyle.Bold : FontStyle.Regular);

            btnTabAuth.TextColor = index == 2 ? ModernTheme.AccentViolet : ModernTheme.DarkTextSecondary;
            btnTabAuth.Font = new Font(btnTabAuth.Font.FontFamily, btnTabAuth.Font.Size, index == 2 ? FontStyle.Bold : FontStyle.Regular);
        }

        private void ToggleTheme_CheckedChanged(object? sender, EventArgs e)
        {
            ModernTheme.CurrentTheme = toggleTheme.Checked ? UIThemeMode.Dark : UIThemeMode.Light;
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            Color formBg, sidebarBg, textPrimary, textSecondary;

            if (ModernTheme.CurrentTheme == UIThemeMode.Dark)
            {
                formBg = ModernTheme.DarkBackground;
                sidebarBg = Color.FromArgb(10, 17, 30); // Deeper dark for sidebar separation
                textPrimary = ModernTheme.DarkTextPrimary;
                textSecondary = ModernTheme.DarkTextSecondary;

                lblLogo.ForeColor = ModernTheme.AccentViolet;
                lblThemeToggle.Text = "Dark Mode";
            }
            else
            {
                formBg = ModernTheme.LightBackground;
                sidebarBg = Color.FromArgb(241, 245, 249); // slate-100 for light sidebar
                textPrimary = ModernTheme.LightTextPrimary;
                textSecondary = ModernTheme.LightTextSecondary;

                lblLogo.ForeColor = ModernTheme.AccentBlue;
                lblThemeToggle.Text = "Light Mode";
            }

            this.BackColor = formBg;
            sidebar.BackColor = sidebarBg;
            mainContentArea.BackColor = formBg;

            lblThemeToggle.ForeColor = textSecondary;

            // Force all child controls to repaint with updated theme values
            RefreshThemeRecursive(this);
        }

        private void RefreshThemeRecursive(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                // Set default Windows Forms labels to sync colors
                if (ctrl is Label lbl && lbl != lblLogo && lbl != lblThemeToggle)
                {
                    if (lbl.ForeColor != ModernTheme.AccentEmerald && lbl.ForeColor != ModernTheme.AccentBlue)
                    {
                        lbl.ForeColor = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                            ? ModernTheme.DarkTextPrimary 
                            : ModernTheme.LightTextPrimary;
                        
                        // Treat sub-labels styled as Slate text secondary
                        if (lbl.Text.ToUpper() == "ACTIVE USERS" || lbl.Text.ToUpper() == "PROCESSOR LOAD" || 
                            lbl.Text.ToUpper() == "MEMORY UTILISED" || lbl.Text.Contains(" week") || lbl.Text.Contains(" licensing") ||
                            lbl.Text.Contains(" marketing") || lbl.Text.Contains(" deployment") || lbl.Text.Contains("STREAM"))
                        {
                            lbl.ForeColor = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                                ? ModernTheme.DarkTextSecondary 
                                : ModernTheme.LightTextSecondary;
                        }
                    }
                }

                if (ctrl is TabPage page)
                {
                    page.BackColor = ModernTheme.Background;
                }

                // Standard controls trigger Redraw
                ctrl.Invalidate();

                if (ctrl.HasChildren)
                {
                    RefreshThemeRecursive(ctrl);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                systemTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
