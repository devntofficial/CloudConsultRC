namespace CloudConsult.UI.Blazor.Models.Identity
{
    public class RegistrationModel
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
