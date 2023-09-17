using Application.Profiles;
using Domain.Entities.Authentication;

namespace Application.Dtos.Authentications.Response
{
    public record GetAllRolesResponse : ICreateMapper<Role>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}