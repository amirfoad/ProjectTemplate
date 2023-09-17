namespace Application.Dtos.User.Response
{
    public record ForgotPasswordResponse
    {
        public bool Verified { get; set; }
        public string loginId { get; set; }
    }
}