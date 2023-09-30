namespace TeamService.Models
{
    public class AuthData
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class AuthResponse
    {
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Token { get; set; }
    }
    public class TeamResponse
    {
        public IEnumerable<Team> Data { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime FoudationYear { get; set; }
        public string Division { get; set; }
        public string Conference { get; set; }
        public string ImageUrl { get; set; }
    }
}