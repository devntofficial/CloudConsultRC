using CloudConsult.UI.Blazor.Models.Doctor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Text.RegularExpressions;

namespace CloudConsult.UI.Blazor.Pages.Doctor
{
    public class CreateDoctorProfileComponent : ComponentBase
    {
        [Inject] protected ISnackbar Snackbar { get; set; }
        protected ProfileModel profile { get; set; } = new();
        protected string AvatarImageLink { get; set; } = "images/avatar.jpg";
        protected string AvatarIcon { get; set; }
        protected string AvatarButtonText { get; set; } = "Delete Picture";
        protected Color AvatarButtonColor { get; set; } = Color.Error;
        protected bool VisibilitySwitch { get; set; } = true;
        protected bool NotificationEmail_1 { get; set; } = true;
        protected bool NotificationEmail_2 { get; set; }
        protected bool NotificationEmail_3 { get; set; }
        protected bool NotificationEmail_4 { get; set; } = true;
        protected bool NotificationSMS_1 { get; set; }
        protected bool NotificationSMS_2 { get; set; } = true;
        protected bool NotificationSMS_3 { get; set; } = true;
        protected bool NotificationSMS_4 { get; set; }

        protected void DeletePicture()
        {
            if (!String.IsNullOrEmpty(AvatarImageLink))
            {
                AvatarImageLink = null;
                AvatarIcon = Icons.Material.Outlined.SentimentVeryDissatisfied;
                AvatarButtonText = "Upload Picture";
                AvatarButtonColor = Color.Primary;
            }
            else
            {
                return;
            }
        }

        protected void SaveChanges(string message, Severity severity)
        {
            Snackbar.Add(message, severity, config =>
            {
                config.ShowCloseIcon = false;
            });
        }

        protected MudForm form;
        protected MudTextField<string> pwField1;

        protected IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        protected string PasswordMatch(string arg)
        {
            if (pwField1.Value != arg)
                return "Passwords don't match";
            return null;
        }
    }
}
