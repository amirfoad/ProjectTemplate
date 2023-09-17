using Application.Profiles;

namespace Application.Dtos.User.Response
{
    public record GetUsersResponse : ICreateMapper<Domain.Entities.Authentication.User>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int ChannelId { get; set; }
        public bool IsActive { get; set; }
    }
}