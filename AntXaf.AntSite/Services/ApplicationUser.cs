namespace AntXafSite.Services
{
    public class ApplicationUser
    {
        public Guid Oid { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
