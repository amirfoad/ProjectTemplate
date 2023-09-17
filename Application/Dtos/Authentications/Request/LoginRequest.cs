namespace Application.Dtos.Authentications.Request
{
    public record LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}