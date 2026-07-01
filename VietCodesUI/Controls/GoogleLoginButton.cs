using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VietcodeUI.Theme;

namespace VietcodeUI.Controls
{
    [ToolboxItem(true)]
    [Description("A premium Google Sign-In button following 2026 aesthetics and branding guidelines.")]
    public class GoogleLoginButton : ModernButton
    {
        // 24x24 Google G logo PNG Base64
        private const string GoogleIconBase64 = 
            "iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAAB3RJTUUH5gYQCg0yN7276QAAAB1pVFh0" +
            "Q29tbWVudAAAAAAAQ3JlYXRlZCB3aXRoIEdJTVBkLmXRwAAAAuBJREFUSMedlj1oU1EYhp8n9yY3tWm1tE2q2ErF1iIdrIIiDkXoIDh1dBV0" +
            "EAdxcRHEwUEc3To6OujgIio4iVJwqVusVioUpE2tNWmS5pfc5P7H4RDTpElj8VnPeznPed/3fd9zDn+5+GgKQLpW4fJokemZIo/fF/AChW1g" +
            "T8D22O1VbOvt0TnbO3e69j9Zz3Ecl4hYQoT44xMiIkTEn+qN684jIj0R8SIizW+F9b1HRLyIyGD0d2yZ3HlEpD0ivYmU/5B1iEgqkW59Gz5v" +
            "O/k09kX68/P5WlG2pYwVf7lZ7F70v0N5XkY0kUgH86xWlG0uY8VfbxZ75GfL8yUi8iQi3/9WlG2pYkV5uVnsUX5f+XyTiGzP75XPd4rId/m9" +
            "8vkeEdlRn+Z6Hw9mZisTExUeTpa5PbpDqg5XhrscH2ywNVth7HqR5G6VsS1lxm7W+bK0y9CFLm0dKq0dKq0dKldHOnj+KrdGW/j0dYe3c3sc" +
            "G2q2lq1Wk6WvVfrPtlP5XeLpWIEPhT3GrlXIH+wgoirfvv3m7kSBy6MFunrblCplPv0s83J2j/jRdo5d8mN57FqR1Z9lXn/dY+Rcl+T+do7k" +
            "tthY2eP1dJnUvg4SSpWl1f/ExspXFpf3GB5K0t7TpqWlhW3bJNYqJJIKSinKmxuUvxVIHkhg2zaJdEpsq8L3pQrjN/qIp2oV4vE4juP8B1hb" +
            "WStel1f/ExspXFpf3GB5K0t7TpqWlhW3bJNYqJJIKSinKmxuUvxVIHkhg2zaJdEpsq8L3pQrjN/qIp2oV4vE4juP8B1hbWxPbqiAiKIWIgAg" +
            "igiM4OEmF11/H8Xg8OI6DlBLHcZBSgqpifX1d4vE4QgiKxSKO44Cq4vF4QKVwHEfW19clFotRKBSoVCqgqng8HkhVxsbGJBaLUalUqFar2LAY" +
            "dqoqrqvX6QVUpl8skEgmKxSKGYWA3GpCqUqlUSCaTFAoFDMNACPEDpKpUKpXZ/T9M4gC1U2lGfwAAAABJRU5ErkJggg==";

        public GoogleLoginButton()
        {
            Text = "Sign in with Google";
            Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);
            BorderRadius = 8;
            BorderThickness = 1.5f;
            IconSpacing = 10;
            IconAlign = ContentAlignment.MiddleLeft;
            
            // Set base styles
            Style = ModernButtonStyle.Outline;
            
            // Load base64 icon
            try
            {
                byte[] imageBytes = Convert.FromBase64String(GoogleIconBase64);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Icon = Image.FromStream(ms);
                }
            }
            catch
            {
                // Fallback if decode fails
            }

            SyncThemeColors();
        }

        public void SyncThemeColors()
        {
            if (ModernTheme.CurrentTheme == UIThemeMode.Dark)
            {
                ButtonColor = Color.FromArgb(47, 55, 71); // Slate gray border glow
                TextColor = ModernTheme.DarkTextPrimary;
                CustomHoverColor = Color.FromArgb(20, Color.White);
                CustomClickColor = Color.FromArgb(40, Color.White);
                CustomBorderColor = Color.FromArgb(71, 85, 105);
            }
            else
            {
                ButtonColor = Color.FromArgb(226, 232, 240); // Slate light border
                TextColor = ModernTheme.LightTextPrimary;
                CustomHoverColor = Color.FromArgb(10, Color.Black);
                CustomClickColor = Color.FromArgb(20, Color.Black);
                CustomBorderColor = Color.FromArgb(203, 213, 225);
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // Sync colors on every paint to match theme shifts
            SyncThemeColors();
            base.OnPaint(pevent);
        }
    }
}
