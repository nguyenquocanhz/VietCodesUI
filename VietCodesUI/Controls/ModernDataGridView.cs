using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [ToolboxItem(true)]
    [Description("A premium modern DataGridView styled for 2026 aesthetics with flat headers, clean rows, and theme integration.")]
    public class ModernDataGridView : DataGridView
    {
        private Color headerBackColor = Color.Empty;
        private Color headerForeColor = Color.Empty;
        private Color selectionBackColor = Color.Empty;
        private Color selectionForeColor = Color.Empty;
        private Color gridLineColor = Color.Empty;

        [Category("Vietcode UI")]
        [Description("Custom background color for column headers. If empty, uses theme colors.")]
        public Color HeaderBackColor
        {
            get => headerBackColor;
            set { headerBackColor = value; SyncThemeColors(); }
        }

        [Category("Vietcode UI")]
        [Description("Custom text color for column headers. If empty, uses theme colors.")]
        public Color HeaderForeColor
        {
            get => headerForeColor;
            set { headerForeColor = value; SyncThemeColors(); }
        }

        [Category("Vietcode UI")]
        [Description("Background color for selected rows. If empty, uses theme colors.")]
        public Color SelectionBackColor
        {
            get => selectionBackColor;
            set { selectionBackColor = value; SyncThemeColors(); }
        }

        [Category("Vietcode UI")]
        [Description("Text color for selected rows. If empty, uses theme colors.")]
        public Color SelectionForeColor
        {
            get => selectionForeColor;
            set { selectionForeColor = value; SyncThemeColors(); }
        }

        [Category("Vietcode UI")]
        [Description("Color of the horizontal grid lines. If empty, uses theme colors.")]
        public Color GridLineColor
        {
            get => gridLineColor;
            set { gridLineColor = value; SyncThemeColors(); }
        }

        public ModernDataGridView()
        {
            // Set double buffering for smooth scrolling and resizing
            DoubleBuffered = true;

            // Remove retro default borders and styling
            BorderStyle = BorderStyle.None;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            EnableHeadersVisualStyles = false;
            RowHeadersVisible = false;

            // Setup default behaviors for modern table
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MultiSelect = false;
            AllowUserToResizeRows = false;
            ColumnHeadersHeight = 38;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            RowTemplate.Height = 34;

            // Standard grid styling
            BackgroundColor = ModernTheme.CardBackground;
            
            SyncThemeColors();
        }

        public void SyncThemeColors()
        {
            Color bg = ModernTheme.CardBackground;
            Color text = ModernTheme.TextPrimary;
            Color secondaryText = ModernTheme.TextSecondary;
            Color border = gridLineColor.IsEmpty ? ModernTheme.Border : gridLineColor;

            Color hBg = headerBackColor.IsEmpty 
                ? (ModernTheme.CurrentTheme == UIThemeMode.Dark ? Color.FromArgb(20, 28, 45) : Color.FromArgb(241, 245, 249)) 
                : headerBackColor;
            Color hFg = headerForeColor.IsEmpty ? ModernTheme.AccentViolet : headerForeColor;

            Color sBg = selectionBackColor.IsEmpty ? Color.FromArgb(45, ModernTheme.AccentViolet) : selectionBackColor;
            Color sFg = selectionForeColor.IsEmpty ? ModernTheme.AccentViolet : selectionForeColor;

            // Apply Background
            BackgroundColor = bg;
            GridColor = border;

            // Header Style
            ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = hBg,
                ForeColor = hFg,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                SelectionBackColor = hBg,
                SelectionForeColor = hFg,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };

            // Rows Style
            DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = bg,
                ForeColor = text,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                SelectionBackColor = sBg,
                SelectionForeColor = sFg,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };

            // Alternating Row Style (Subtle tint change)
            Color altBg = ModernTheme.CurrentTheme == UIThemeMode.Dark 
                ? Color.FromArgb(bg.R + 4, bg.G + 5, bg.B + 7) 
                : Color.FromArgb(250, 250, 250);

            AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = altBg,
                ForeColor = text,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                SelectionBackColor = sBg,
                SelectionForeColor = sFg,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 8, 0)
            };

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Ensure theme changes are synced dynamically
            SyncThemeColors();
            base.OnPaint(e);

            // Draw a subtle border around the entire grid area
            Rectangle borderRect = ClientRectangle;
            borderRect.Width -= 1;
            borderRect.Height -= 1;
            using (Pen p = new Pen(ModernTheme.Border, 1.5f))
            {
                e.Graphics.DrawRectangle(p, borderRect);
            }
        }
    }
}
