namespace STEtest.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Position { get; set; }
        public string UserProfileId { get; set; }  // Foreign key to UserProfile
        public UserProfile UserProfile { get; set; }
    }
}
