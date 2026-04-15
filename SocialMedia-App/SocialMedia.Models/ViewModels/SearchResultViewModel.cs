namespace SocialMedia.Models.ViewModels
{
    public class SearchResultViewModel
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? ImageUrl { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
