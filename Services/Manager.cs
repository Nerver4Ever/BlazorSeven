using BlazorSeven.Data;

namespace BlazorSeven.Services
{
    public class Manager
    {
        public FolderItem RootFolder { get; private set; }

        
        public Manager(string name)
        {
            RootFolder = new FolderItem(name);
        }

        
    }
}
