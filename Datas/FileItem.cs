using MyApplication.Helpers;

namespace MyApplication.Datas
{
    public class FileItem
    {
        public string Name { get; set; }

        public decimal FileSize { get; set; }

        public string Sha1 { get; set; }

        public string Preid { get; set; }

        public string PathsString { get; set; }
        public string[] Paths { get; set; } = Array.Empty<string>();

        public string FormatFileSize { get; private set; }

        public string Extension { get; set; }

        public string OriginSha1Link { get; private set; }

        public static bool TryParseSha1Link(string sha1Link, out FileItem file)
        {
            bool canParse = false;
            file = null;
            var trimedSha1Link = sha1Link.Trim();
            var links = trimedSha1Link.Split('|');

            if (links.Length >= 4)
            {
                var name = links[0].Replace("115://", "");
                var extension = name.Split('.')[^1];
                var fileSize = decimal.Parse(links[1]);
                var sha1 = links[2];
                var preid = links[3];
                file = new FileItem
                {
                    Name = name,
                    FileSize = fileSize,
                    Sha1 = sha1,
                    Preid = preid,
                    FormatFileSize=FileHelper.CalculateFileSize(fileSize),
                    Extension = extension,
                    OriginSha1Link= trimedSha1Link
                };
                if (links.Length > 4)
                {
                    file.PathsString = string.Join( '|', links[4..]);
                }

                canParse = true;
            }


            return canParse;
        }

    }
}
