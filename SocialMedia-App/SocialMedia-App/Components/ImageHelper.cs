namespace SocialMedia_App.Components
{
    public static class ImageHelper
    {
        public static string Url(string? path)
        {
            if (string.IsNullOrEmpty(path)) return "";
            return "/" + path.TrimStart('\\', '/').Replace('\\', '/');
        }
    }
}
