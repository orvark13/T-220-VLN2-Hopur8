
namespace Odie.Models.ViewModels
{
    /// <summary>
    /// Holds notification data for users to be used in views.
    /// </summary>
    public class NoticeViewModel
    {
        public int MessageID { get; set; }
        public string Message { get; set; }
        public int NewID { get; set; }
    }
}