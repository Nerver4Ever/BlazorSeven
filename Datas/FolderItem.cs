namespace MyApplication.Datas
{
	public class FolderItem : IItem
	{
        public Guid ID { get; set; }

        public Guid ParentID { get; set; }

        public string Name { get; set; }
		public decimal Size { get; set; }
		public string PathString { get; set; }

		public List<IItem> Children { get; private set; } = new List<IItem>();

		public int FolderCount { get; set; }

		public int FileCount { get; set; }

		public FolderItem(string name, string paths)
		{
			ID=Guid.NewGuid();
			Name = name;
			PathString = paths;
		}

		public void CalculateSize()
		{
			decimal size = 0;
			int folderCount = 0;
			int fileCount = 0;
			CalculateSizeInternal(this, ref size, ref folderCount, ref fileCount);

			Size = size;
			FolderCount = folderCount;
			FileCount = fileCount;
		}

		private void CalculateSizeInternal(FolderItem root, ref decimal size, ref int folderCount, ref int fileCount)
		{
			var childFiles = root.Children.Where(q => q is FileItem);

			fileCount += childFiles.Count();
			size += childFiles.Sum(q => q.Size);

			var childFolders = root.Children.Where(q => q is FolderItem).Cast<FolderItem>();

			folderCount += childFolders.Count();
			foreach (var folder in childFolders)
			{
				folder.CalculateSize();
				fileCount += folder.FileCount;
				folderCount += folder.FolderCount;
				size += folder.Size;
			}
		}

		private void GetAllFilesInternal(FolderItem root, List<FileItem> files)
		{
			var children = root.Children.Where(q => q is FileItem).Cast<FileItem>();
			files.AddRange(children);

			var childFolders = root.Children.Where(q => q is FolderItem).Cast<FolderItem>();
			foreach (var item in childFolders)
			{
				GetAllFilesInternal(item, files);
			}

		}

		public List<FileItem> GetAllFiles()
		{
			List<FileItem> files = new();
			GetAllFilesInternal(this, files);
			return files;
		}


	}

}
