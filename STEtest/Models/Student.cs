namespace STEtest.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string CourseId { get; set; }
        public string UserProfileId { get; set; }  // Foreign key to UserProfile
        public UserProfile UserProfile { get; set; }
    }
}
