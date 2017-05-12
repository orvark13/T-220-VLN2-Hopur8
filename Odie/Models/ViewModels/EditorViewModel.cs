
namespace Odie.Models.ViewModels
{
    /// <summary>
    /// Holds the view model with all the data the Editor needs.
    /// </summary>
    public class EditorViewModel
    {
        public FileViewModel File { get; set; }
        public FileRevisionViewModel FileRevision { get; set; }
    }
}