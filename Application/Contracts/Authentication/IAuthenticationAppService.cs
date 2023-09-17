using Application.Dtos.Authentications.Request;
using Application.Dtos.Authentications.Response;
using Application.Dtos.Common;
using Application.Dtos.Jwt;
using Application.Dtos.User.Response;

namespace Application.Contracts.Authentication
{
    public interface IAuthenticationAppService
    {
        Task<OperationResult<bool>> Register(RegisterRequest request);

        Task<OperationResult<AccessToken>> Login(LoginRequest request);

        Task<OperationResult<AccessToken>> GetRefreshToken(string refreshToken);

        Task<OperationResult<bool>> Logout(int userId);

        Task<OperationResult<bool>> AddRoleToUser(AddRoleToUserRequest request);

        Task<OperationResult<List<GetUsersResponse>>> GetAllUsers();

        Task<OperationResult<GetUsersResponse>> GetUserById(int userId);

        Task<OperationResult<List<GetAllRolesResponse>>> GetAllRoles();

        Task<OperationResult<int>> AddRole(AddRoleRequest request);
    }
}