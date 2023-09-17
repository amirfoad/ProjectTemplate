namespace Application.Dtos.Authentications.Request
{
    public record EditRolePermissionsDto
    {
        public int RoleId { get; set; }
        public List<string> Permissions { get; set; }
    }
}