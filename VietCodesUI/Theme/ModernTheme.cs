using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VietcodeUI.Theme
{
    public enum UIThemeMode
    {
        Dark,
        Light
    }

    public static class ModernTheme
    {
        public static UIThemeMode CurrentTheme { get; set; } = UIThemeMode.Dark;

        // --- Dark Theme Colors (Obsidian Dark) ---
        public static Color DarkBackground = Color.FromArgb(15, 23, 42);       // #0f172a
        public static Color DarkCardBackground = Color.FromArgb(30, 41, 59);   // #1e293b
        public static Color DarkBorder = Color.FromArgb(51, 65, 85);           // #334155
        public static Color DarkTextPrimary = Color.FromArgb(248, 250, 252);   // #f8fafc
        public static Color DarkTextSecondary = Color.FromArgb(148, 163, 184); // #94a3b8

        // --- Light Theme Colors (Milk Light) ---
        public static Color LightBackground = Color.FromArgb(248, 250, 252);   // #f8fafc
        public static Color LightCardBackground = Color.White;
        public static Color LightBorder = Color.FromArgb(226, 232, 240);       // #e2e8f0
        public static Color LightTextPrimary = Color.FromArgb(15, 23, 42);      // #0f172a
        public static Color LightTextSecondary = Color.FromArgb(100, 116, 139); // #64748b

        // --- Accent Colors (Gu 2026 Vibrant) ---
        public static Color AccentViolet = Color.FromArgb(139, 92, 246);       // #8b5cf6
        public static Color AccentEmerald = Color.FromArgb(16, 185, 129);      // #10b981
        public static Color AccentBlue = Color.FromArgb(59, 130, 246);         // #3b82f6
        public static Color AccentCrimson = Color.FromArgb(239, 68, 68);       // #ef4444

        // --- Brand Colors ---
        public static Color GoogleBrandColor = Color.FromArgb(219, 68, 85);    // Soft Google red/pink
        public static Color GithubBrandColor = Color.FromArgb(36, 41, 46);      // #24292e

        // --- Theme Getters ---
        public static Color Background => CurrentTheme == UIThemeMode.Dark ? DarkBackground : LightBackground;
        public static Color CardBackground => CurrentTheme == UIThemeMode.Dark ? DarkCardBackground : LightCardBackground;
        public static Color Border => CurrentTheme == UIThemeMode.Dark ? DarkBorder : LightBorder;
        public static Color TextPrimary => CurrentTheme == UIThemeMode.Dark ? DarkTextPrimary : LightTextPrimary;
        public static Color TextSecondary => CurrentTheme == UIThemeMode.Dark ? DarkTextSecondary : LightTextSecondary;

        // --- Core Helpers ---
        public static Color BlendColors(Color color1, Color color2, double ratio)
        {
            ratio = Math.Max(0.0, Math.Min(1.0, ratio));
            int r = (int)(color1.R * (1.0 - ratio) + color2.R * ratio);
            int g = (int)(color1.G * (1.0 - ratio) + color2.G * ratio);
            int b = (int)(color1.B * (1.0 - ratio) + color2.B * ratio);
            int a = (int)(color1.A * (1.0 - ratio) + color2.A * ratio);
            return Color.FromArgb(a, r, g, b);
        }

        public static GraphicsPath GetRoundedPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2f;

            if (radius <= 0)
            {
                path.AddRectangle(rect);
                return path;
            }

            // Cap the diameter to the smaller of width or height to prevent self-intersection
            if (diameter > rect.Width) diameter = rect.Width;
            if (diameter > rect.Height) diameter = rect.Height;

            RectangleF arc = new RectangleF(rect.X, rect.Y, diameter, diameter);

            // Top-left
            path.AddArc(arc, 180, 90);

            // Top-right
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom-right
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom-left
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void DrawRoundedRectangle(Graphics g, Pen pen, RectangleF rect, float radius)
        {
            using (GraphicsPath path = GetRoundedPath(rect, radius))
            {
                g.DrawPath(pen, path);
            }
        }

        public static void FillRoundedRectangle(Graphics g, Brush brush, RectangleF rect, float radius)
        {
            using (GraphicsPath path = GetRoundedPath(rect, radius))
            {
                g.FillPath(brush, path);
            }
        }
    }
}
