namespace SchemaPal.DataTransferObjects
{
    public class User
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public User()
        {
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
        }
    }
}
