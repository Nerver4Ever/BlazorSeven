using BlazorSeven.Data;

namespace BlazorSeven.Services
{
    public class Manager
    {
        public FolderItem RootFolder { get; private set; }

        
        public Manager()
        {
            RootFolder = new FolderItem("根目录");
        }

        
    }
}
