namespace Account.API.Models
{
    public class AudienceSettings
    {
        public string? Secret { get; set; }
        public string? Iss { get; set; }
        public string? Aud { get; set; }
        public string? ClientId { get; set; }
    }
}
