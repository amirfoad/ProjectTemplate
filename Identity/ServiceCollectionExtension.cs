using Application.Contracts.Authentication;
using Application.Dtos.ApiResult;
using Application.Dtos.Jwt;
using Domain.Entities.Authentication;
using Hosumand.KarizWrapper.Identity.Identity.Manager;
using Identity.Identity;
using Identity.Identity.Dtos;
using Identity.Identity.Extensions;
using Identity.Identity.Manager;
using Identity.Identity.PermissionManager;
using Identity.Identity.Seed;
using Identity.Identity.Store;
using Identity.Identity.validator;
using Identity.Jwt;
using Identity.UserManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Utils;

namespace Identity
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterIdentityServices(this IServiceCollection services, IdentitySettings identitySettings)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAppUserManager, AppUserManagerImplementation>();
            services.AddScoped<ISeedDataBase, SeedDataBase>();

            services.AddScoped<IUserValidator<User>, AppUserValidator>();
            services.AddScoped<UserValidator<User>, AppUserValidator>();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, AppUserClaimsPrincipleFactory>();

            services.AddScoped<IRoleValidator<Role>, AppRoleValidator>();
            services.AddScoped<RoleValidator<Role>, AppRoleValidator>();

            services.AddScoped<IAuthorizationHandler, DynamicPermissionHandler>();
            services.AddScoped<IDynamicPermissionService, DynamicPermissionService>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();
            services.AddScoped<IUserStore<User>, AppUserStore>();
            services.AddScoped<IAppUserPrincipalFactory, AppUserClaimsPrincipleFactory>();

            services.AddIdentity<User, Role>(options =>
                {
                    options.Stores.ProtectPersonalData = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireUppercase = false;

                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = false;
                    options.User.RequireUniqueEmail = false;

                    //options.Stores.ProtectPersonalData = true;
                }).AddUserStore<AppUserStore>()
                .AddRoleStore<RoleStore>().
                //.AddUserValidator<AppUserValidator>().
                //AddRoleValidator<AppRoleValidator>().
                AddUserManager<AppUserManager>().
                AddRoleManager<AppRoleManager>().
                AddErrorDescriber<AppErrorDescriber>()
                //.AddClaimsPrincipalFactory<AppUserClaimsPrincipleFactory>()
                .AddDefaultTokenProviders().
                AddSignInManager<AppSignInManager>()
                .AddDefaultTokenProviders()
                .AddPasswordlessLoginTotpTokenProvider();

            //For [ProtectPersonalData] Attribute In Identity

            //services.AddScoped<ILookupProtectorKeyRing, KeyRing>();

            //services.AddScoped<ILookupProtector, LookupProtector>();

            //services.AddScoped<IPersonalDataProtector, PersonalDataProtector>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ConstantPolicies.DynamicPermission, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new DynamicPermissionRequirement());
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes(identitySettings.SecretKey);
                var encryptionkey = Encoding.UTF8.GetBytes(identitySettings.Encryptkey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = identitySettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = identitySettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey),
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<AppSignInManager>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (!securityStamp.HasValue())
                            context.Fail("This token has no security stamp");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId<int>();
                        // var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                        //if (user.SecurityStamp != Guid.Parse(securityStamp))
                        //    context.Fail("Token secuirty stamp is not valid.");

                        var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                            context.Fail("Token security stamp is not valid.");
                    },
                    OnChallenge = async context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                        if (context.AuthenticateFailure is SecurityTokenExpiredException)
                        {
                            context.HandleResponse();

                            var jwtService = context.HttpContext.RequestServices.GetRequiredService<IJwtService>();
                            StringValues beareetoken;

                            StringValues refreshToken;
                            StringValues actor;
                            context.HttpContext.Request.Headers.TryGetValue("refresh_Token", out refreshToken);
                            context.HttpContext.Request.Headers.TryGetValue("actorId", out actor);
                            context.HttpContext.Request.Headers.TryGetValue("Authorization", out beareetoken);
                            var stringArray = beareetoken.First().Split(" ");
                            var token = stringArray[1];
                            var claimsprincipal = await jwtService.GetPrincipalFromExpiredToken(token);
                            var actorClaim = claimsprincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor);
                            int actorId = 0;
                            if (actorClaim != null)
                                actorId = int.Parse(actorClaim.Value);

                            if (!refreshToken.Any())
                            {
                                var response = new ApiResult(false,
                                    ApiResultStatusCode.UnAuthorized, "Refresh Token Not Found");
                                context.Response.StatusCode = StatusCodes.Status424FailedDependency;
                                await context.Response.WriteAsJsonAsync(response);
                            }
                            else
                            {
                                var newToken = await jwtService.RefreshToken(refreshToken.ToString(), actorId);

                                if (newToken is null)
                                {
                                    var failedResponse = new ApiResult(false,
                                        ApiResultStatusCode.UnAuthorized, "Refresh Token Not Valid");
                                    context.Response.StatusCode = StatusCodes.Status424FailedDependency;
                                    await context.Response.WriteAsJsonAsync(failedResponse);
                                }

                                var response = new ApiResult<AccessToken>(true, ApiResultStatusCode.NotAcceptable, newToken);

                                context.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                                await context.Response.WriteAsJsonAsync(response);
                            }
                        }
                        else if (context.AuthenticateFailure != null)
                        {
                            context.HandleResponse();

                            var response = new ApiResult(false,
                                ApiResultStatusCode.UnAuthorized, "Token is Not Valid");
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(response);
                        }
                    }
                };
            });

            IdentityModelEventSource.ShowPII = true;

            return services;
        }
    }
}