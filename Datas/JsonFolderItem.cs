﻿

using System.Text.Json.Serialization;

namespace MyApplication.Datas
{
    public class JsonFolderItem
    {
        [JsonPropertyName("dir_name")]
        public string FolderName { get; set; }

        [JsonPropertyName("dirs")]
        public List<JsonFolderItem> Folders { get; set; }

        [JsonPropertyName("files")]
        public List<string> Files { get; set; }

        
    }
}
