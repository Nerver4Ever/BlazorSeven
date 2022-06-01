namespace BlazorSeven.Services
{
    public class SizeHelper
    {
        public static string CalculateFileSize(decimal size)
        {
            var s = decimal.Parse("1024");
            if (size > (s * s * s * s * s))
            {
                return $"{size / s / s / s / s / s:N2} PB";
            }
            else if (size > (s * s * s * s))
            {
                return $"{size / s / s / s / s:N2} TB";
            }
            else if (size > (s * s * s))
            {
                return $"{size / s / s / s:N2} GB";
            }
            else if (size > (s * s))
            {
                return $"{size / s / s:N2} MB";
            }
            else if (size > s)
            {
                return $"{size / s:N2} KB";
            }
            else
            {
                return $"{size} B";
            }
        }
    }
}
