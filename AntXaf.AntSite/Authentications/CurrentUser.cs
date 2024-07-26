namespace AntXafSiteTemplate.Authentications
{
    internal class CurrentUser
    {
        public Guid Oid { get; set; }
        public string? Userid { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public int NotifyCount { get; set; }
        public int UnreadCount { get; set; }
        public string? Token { get; set; }
        public string? Instant { get; set; }
    }
}
