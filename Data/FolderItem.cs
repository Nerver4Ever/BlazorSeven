namespace BlazorSeven.Data
{
    public class FolderItem : Item
    {
        public override ItemType ItemType => ItemType.Folder;

        public int FolderCount { get; set; }

        public int FileCount { get; set; }

        public List<Item> Children { get; private set; } = new List<Item>();

        public FolderItem(string name)
        {
            ID = Guid.NewGuid();
            Name = name;
        }


        
    }
}
