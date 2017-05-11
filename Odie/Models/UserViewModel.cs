using System;

namespace Odie.Models
{
    public class UserViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UserSelectItemViewModel
    {
        public string ID { get; set; }
        public string Text { get; set; }        
    }
}