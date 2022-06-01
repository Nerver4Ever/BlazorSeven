namespace BlazorSeven.Data
{
    public class FileItem : Item
    {
        public override ItemType ItemType => ItemType.File;

        public string Sha1 { get; set; }

        public string PreidSha1 { get; set; }

        public string Extension { get; set; }

        public FileItem(string sha1Link)
        {
            ID = Guid.NewGuid();
            var trimedSha1Link = sha1Link.Trim();
            var links = trimedSha1Link.Split('|');
            if (links.Length >= 4)
            {
                Name = links[0].Replace("115://", "");
                Extension = Name.Split('.')[^1];
                Size = decimal.Parse(links[1]);
                Sha1 = links[2];
                PreidSha1 = links[3];
            }
        }



    }
}
