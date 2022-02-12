using MyApplication.Datas;
using System.Text.Json;

namespace MyApplication.Helpers
{
    public static class FileHelper
    {
        public static string CalculateFileSize(decimal size)
        {
            var s = decimal.Parse("1024");
            if (size > (s * s * s * s))
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


        public static async Task<FolderItem> ParseFormFileAsync(Stream stream, string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var name = Path.GetFileNameWithoutExtension(fileName);
            FolderItem root;
            if (extension==".json")
            {

                root=await ParseFormJsonFileAsync(stream);
            }
            else 
            {
                root = await ParseFromTxtFileAsync(stream);
                root.FolderName = name;
            }

            return root;
        }

        private static async Task<FolderItem> ParseFormJsonFileAsync(Stream stream) 
        {
           var tempRoot=  await JsonSerializer.DeserializeAsync<FolderItem>(stream);
           return tempRoot;
        }

        private static async Task<FolderItem> ParseFromTxtFileAsync(Stream stream)
        {
            StreamReader reader = new(stream);
            string text = await reader.ReadToEndAsync();
            reader.Close();
            var lines = text.Split("\r\n");

            FolderItem root = new();
            

            return root;
        }
    }
}
