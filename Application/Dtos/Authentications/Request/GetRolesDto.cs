namespace Application.Dtos.Authentications.Request
{
    public record GetRolesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}