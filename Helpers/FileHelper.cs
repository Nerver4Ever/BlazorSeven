using MyApplication.Datas;
using System.Text.Json;
using File = MyApplication.Datas.FileItem;

namespace MyApplication.Helpers
{

    public record class FileData(string Sha1Link, string[] Paths);

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





        public static JsonFolderItem ConverterToJsonItem(FolderItem item)
        {
            JsonFolderItem jsonFolderItem = new JsonFolderItem();

            return jsonFolderItem;
        }


        private static FolderItem ConverterFromJsonItemInternal(JsonFolderItem json,Guid rootID)
        {
            static void Converter(JsonFolderItem jsonItem, FolderItem item, string paths)
            {
                item.Name = jsonItem.FolderName;

                string childPaths = string.IsNullOrWhiteSpace(paths) ? item.Name : $"{paths}|{item.Name}";
                foreach (var file in jsonItem.Files)
                {
                    var childFile = new FileItem(file, childPaths) { ParentID = item.ID };
                    item.Children.Add(childFile);
                }

                foreach (var jsonChildItem in jsonItem.Folders)
                {
                    var childFolder = new FolderItem(jsonChildItem.FolderName, childPaths) { ParentID = item.ID };
                    item.Children.Add(childFolder);
                    Converter(jsonChildItem, childFolder, childPaths);
                }
            }


            FolderItem folderItem = new FolderItem(json.FolderName, "") { ParentID = rootID };
            Converter(json, folderItem, "");

            folderItem.CalculateSize();

            return folderItem;
        }


        /// <summary>
        /// 从文件中获取内容，并转为树形结构
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">文件名</param>
        /// <returns>树形结构</returns>
        public static async Task<FolderItem> ConverterFromFileAsync(Stream stream, string fileName,Guid rootID)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var name = Path.GetFileNameWithoutExtension(fileName);
            JsonFolderItem root;

            if (extension == ".json")
            {
                root = await ParseFromJsonFileAsync(stream);
            }
            else
            {
                root = await ParseFromTextToJsonItemAsync(stream, name);
            }
            return ConverterFromJsonItemInternal(root, rootID);
        }


        public static  List<IItem> ConverterListFromFolderItem(FolderItem root)
        {
            static void AddChildren(FolderItem rootItem, List<IItem> all)
            {
                all.AddRange(rootItem.Children.Where(q => q is FileItem).Cast<FileItem>());

                var folders = rootItem.Children.Where(q => q is FolderItem).Cast<FolderItem>();
                foreach (var folder in folders)
                {
                    all.Add(folder);

                    AddChildren(folder, all);
                }
            }

            var list = new List<IItem>();
            AddChildren(root, list);
            list.Add(root);
            return list;
        }





        private static async Task<JsonFolderItem> ParseFromJsonFileAsync(Stream stream)
        {
            var tempRoot = await JsonSerializer.DeserializeAsync<JsonFolderItem>(stream);
            return tempRoot;
        }





        private static void FilesToJsonTree(List<FileData> children, JsonFolderItem root, int pathIndex)
        {

            var selectedFiles = children.Where(q => (q.Paths.Length == (pathIndex + 1)) && q.Paths[pathIndex] == root.FolderName);
            root.Files = new();
            root.Folders = new();

            var childFiles = selectedFiles.ToList();

            childFiles.ForEach(q => root.Files.Add(q.Sha1Link));


            var others = children.Where(q => (q.Paths.Length > (pathIndex + 1)) && q.Paths[pathIndex] == root.FolderName);

            var childFolders = others.Select(q => q.Paths[pathIndex + 1]).Distinct();


            var index = pathIndex + 1;
            foreach (var item in childFolders)
            {

                var child = new JsonFolderItem
                {
                    FolderName = item
                };
                root.Folders.Add(child);

                FilesToJsonTree(others.ToList(), child, index);
            }
        }


        private static async Task<JsonFolderItem> ParseFromTextToJsonItemAsync(Stream stream, string name)
        {
            JsonFolderItem folder = new();
            folder.FolderName = name;
            List<FileData> files = new List<FileData>();
            StreamReader reader = new(stream);
            string text = await reader.ReadToEndAsync();
            reader.Close();
            var lines = text.Split("\r\n");
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var infos = line.Split("|");
                    string[] paths = new[] { name };
                    if (infos.Length >= 4)
                    {
                        paths = paths.Concat(infos[4..]).ToArray();
                        var file = new FileData(string.Join('|', infos[0..4]), paths);
                        files.Add(file);
                    }


                }
            }
            FilesToJsonTree(files, folder, 0);
            return folder;
        }
    }
}
