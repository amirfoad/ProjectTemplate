namespace Application.Dtos.Authentications.Request
{
    public record AddRoleToUserRequest
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}