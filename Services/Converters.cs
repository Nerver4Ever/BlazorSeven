using BlazorSeven.Data;
using System.Text.Json;

namespace BlazorSeven.Services
{
    public class Converters
    {
        public static async Task<(FolderItem,List<Item>)> ConverFromFileStream(Stream stream,string fileName,Guid rootID) 
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var name = Path.GetFileNameWithoutExtension(fileName);

            var jsonItem= await ParseFromJsonFileAsync(stream);

            return ConverterFromJsonItemInternal(jsonItem, rootID);
        }



        private static (FolderItem Root,List<Item> AllItems) ConverterFromJsonItemInternal(JsonFolderItem json, Guid rootID)
        {
            static void Converter(JsonFolderItem jsonItem, FolderItem item)
            {
                item.Name = jsonItem.FolderName;

                
                foreach (var file in jsonItem.Files)
                {
                    var childFile = new FileItem(file) { ParentID = item.ID };
                    item.Children.Add(childFile);
                }

                foreach (var jsonChildItem in jsonItem.Folders)
                {
                    var childFolder = new FolderItem(jsonChildItem.FolderName) { ParentID = item.ID };
                    item.Children.Add(childFolder);
                    Converter(jsonChildItem, childFolder);
                }
            }


            FolderItem folderItem = new(json.FolderName) { ParentID = rootID };
            Converter(json, folderItem);
            List<Item> items = new();

            CalculateSize(folderItem, items);

            return (folderItem,items);
        }


        private static async Task<JsonFolderItem> ParseFromJsonFileAsync(Stream stream)
        {
            var tempRoot = await JsonSerializer.DeserializeAsync<JsonFolderItem>(stream);
            return tempRoot;
        }


        



        private static void CalculateSizeInternal(FolderItem root, ref decimal size, ref int folderCount, ref int fileCount, List<Item> items)
        {
            var childFiles = root.Children.Where(q => q is FileItem);
            items.AddRange(childFiles);

            fileCount += childFiles.Count();
            size += childFiles.Sum(q => q.Size);

            var childFolders = root.Children.Where(q => q is FolderItem).Cast<FolderItem>();
            items.AddRange(childFolders);

            folderCount += childFolders.Count();
            foreach (var folder in childFolders)
            {
                Converters.CalculateSize(folder,items);
                fileCount += folder.FileCount;
                folderCount += folder.FolderCount;
                size += folder.Size;
            }
        }

        public static void CalculateSize(FolderItem folder,List<Item> items)
        {
            
            decimal size = 0;
            int folderCount = 0;
            int fileCount = 0;

            CalculateSizeInternal(folder, ref size, ref folderCount, ref fileCount,items);

            folder.Size = size;
            folder.FolderCount = folderCount;
            folder.FileCount = fileCount;

        }






    }
}
