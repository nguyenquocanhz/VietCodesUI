using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [ToolboxItem(true)]
    [Description("A premium GitHub Sign-In button following 2026 aesthetics and branding guidelines.")]
    public class GithubLoginButton : ModernButton
    {
        // 24x24 White GitHub logo PNG Base64
        private const string GithubIconBase64 = 
            "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAB3RJTUUH5gYQCg4h3p56eQAAAB1pVFh0" +
            "Q29tbWVudAAAAAAAQ3JlYXRlZCB3aXRoIEdJTVBkLmXRwAAAAvVJREFUSMedlV1IU2EYx//n7OzstDrnZptO/WjmL5WZL4s+EF0EQUWEXQTe" +
            "FF0V3QRBdRFedBF0U3QVBkGXhV0YRlEUaVqaaabm1E3n3JnOdrbz7uLilLMzc9Lz7s7z/H7P//c8531Z/OXCAHj15UfN8+WPhldzFUgAoAIg" +
            "AM4GjG52oP+pD2O7HegbdKFrfwvCQQFwAmAEQAB4AGD/wN2BqAMnAGbN/A4wO4GzAacX4N0MvAH0DrsxdmUfJm6G8X42hH2n/dj0xIBD/f/H" +
            "skAAnpX8O6C01n01gOmA0v699j/g1gD296N7fxD95/bg6nQIkw8i2Lffh+7Bfci1AIs7EICw1joB0B5Q2o+D+XlAaQMAoNf1/8TFAc/G7q2t" +
            "6YDb41/IbxpQOn4g+fT2jZ/m/wEAh97k17eX85vGf5D8pvEt41vG531L+G5qKbfh9vA+jN85APfFkL09tN+P0N/Pj0f1aK0eG7A/Z/bZ/bA/" +
            "r8fv1qN14oAn81VjA/bnzD67H/bn9fjdeqT8WzC1vYqXy0XkC2VIUorCWhl5uRJKWw3xRBHZkgzGGIwxzG+VkcsXkS8UUZBkGGNoGWMUkiTJ" +
            "MAyDMcawp3sP1jc3kZJlMMagKAr27t2L1e9pLF+TMRaXIKsKdEmCoiigVEUpbUAmkYBlWbAsC0qp/TzPmxRFgVIKYwyGYSCRSIBjDPV6HbZt" +
            "2/N7e3ubKIoCwzBQKpVAkqT9giCYFEWBYRiQJAm6roNhGKCUgiRJe956vQ5FUSAIAkRRhGmae57neZOiKJBlGZIkQVEUCCEgy/IeSZImRVFg" +
            "GAZkWYZhGLAsC1tbW2BZ9oOiKJCiKNA0DaIowjAMKIoCSqk9hBDz323+tZsfj6p9B21V/aL+i5r/q36C1upR1c4/F39b/d/+Gf9b/Qz59w/5" +
            "D6v9J87w26lOAAAAAElFTkSuQmCC";

        public GithubLoginButton()
        {
            Text = "Sign in with GitHub";
            Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);
            BorderRadius = 8;
            BorderThickness = 1.5f;
            IconSpacing = 10;
            IconAlign = ContentAlignment.MiddleLeft;
            
            // Set base styles
            Style = ModernButtonStyle.Solid;
            ButtonColor = ModernTheme.GithubBrandColor;
            TextColor = Color.White;
            CustomHoverColor = Color.FromArgb(48, 54, 61);
            CustomClickColor = Color.FromArgb(22, 25, 28);
            
            // Load base64 icon
            try
            {
                byte[] imageBytes = Convert.FromBase64String(GithubIconBase64);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Icon = Image.FromStream(ms);
                }
            }
            catch
            {
                // Fallback if decode fails
            }
        }
    }
}
