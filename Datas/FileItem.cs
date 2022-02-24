namespace MyApplication.Datas
{
    public class FileItem : IItem
    {
        public string Name { get; set; }
        public decimal Size { get; set; }
        public string PathString { get; set; }

        public string[] Paths { get; set; }

        public string OriginLink { get; set; }

        public string Extension { get; set; }

        public string Sha1 { get; set; }

        public string PreidSha1 { get; set; }
        public Guid ID { get; set; }
        public Guid ParentID { get; set; }

        public void CalculateSize()
        {

        }

        public FileItem(string sha1Link,string paths)
        {
            ID = Guid.NewGuid();
            OriginLink = sha1Link;
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

            PathString = paths;
            Paths=paths.Split('|');
        }
    }
}
