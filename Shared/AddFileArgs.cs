using Microsoft.AspNetCore.Components.Forms;

namespace MyApplication.Shared
{
    public record AddFileArgs(SelectedPlace SelectedPlace, IBrowserFile File);
}
