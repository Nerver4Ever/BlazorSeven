

using System.Text.Json.Serialization;

namespace MyApplication.Datas
{
    public class FolderItem
    {
        [JsonPropertyName("dir_name")]
        public string FolderName { get; set; }

        [JsonPropertyName("files")]
        public string[] Files { get; set; }

        [JsonPropertyName("dirs")]
        public FolderItem[] Folders { get; set; }

        
    }
}
