using BlazorSeven.Data;
using System.Text.Json;

namespace BlazorSeven.Services
{
    public class Converters
    {
        public static async Task<List<Item>> ConverFromFileStream(Stream stream,string fileName,Guid rootID) 
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var name = Path.GetFileNameWithoutExtension(fileName);

            var jsonItem= await ParseFromJsonFileAsync(stream);

            return ConverterFromJsonItemInternal(jsonItem, rootID);
        }



        private static List<Item> ConverterFromJsonItemInternal(JsonFolderItem json, Guid rootID)
        {
            static void Converter(JsonFolderItem jsonItem, FolderItem item,List<Item> items)
            {
                item.Name = jsonItem.FolderName;

                decimal directFilesSize = 0;
                decimal directFoldersSize = 0;
                int fileCount = 0;
                int folderCount = 0;


                if (jsonItem.Files != null)
                {
                    foreach (var file in jsonItem.Files)
                    {
                        var childFile = new FileItem(file) { ParentID = item.ID };
                        //item.Children.Add(childFile);
                        directFilesSize += childFile.Size;
                        items.Add(childFile);
                    }

                    fileCount += jsonItem.Files.Count;
                }

                if (jsonItem.Folders != null)
                {
                    foreach (var jsonChildItem in jsonItem.Folders)
                    {
                        var childFolder = new FolderItem(jsonChildItem.FolderName) { ParentID = item.ID };
                        //item.Children.Add(childFolder);
                        items.Add(childFolder);
                        Converter(jsonChildItem, childFolder,items);
                        directFoldersSize += childFolder.Size;
                        fileCount += childFolder.FileCount;
                        folderCount += childFolder.FolderCount;
                        
                    }

                    folderCount += jsonItem.Folders.Count;
                }
                    

                item.Size = directFilesSize + directFoldersSize;
                item.FolderCount = folderCount;
                item.FileCount = fileCount;
            }


            FolderItem folderItem = new(json.FolderName) { ParentID = rootID };
            
            List<Item> items = new();
            Converter(json, folderItem,items);
            //CalculateSize(folderItem, items);
            items.Add(folderItem);

            return items;
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
