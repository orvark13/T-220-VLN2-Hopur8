
namespace Odie.Models.ViewModels
{
    /// <summary>
    /// Holds minimal user data for use in views.
    /// </summary>
    public class UserViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// Holds ID and name of user for use in select options in views.
    /// </summary>
    public class UserSelectItemViewModel
    {
        public string ID { get; set; }
        public string Text { get; set; }        
    }
}