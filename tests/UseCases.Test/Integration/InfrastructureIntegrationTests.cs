using AutoMapper;
using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Users.Delete;
using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.DataAccess.Repositories;
using BarberBoss.Infrastructure.Services.LoggedUser;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UseCases.Test.Integration;

public class InfrastructureIntegrationTests {
    private readonly IMapper _mapper = CreateMapper();

    private static IMapper CreateMapper() {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>(), NullLoggerFactory.Instance);
        return configuration.CreateMapper();
    }

    [Fact]
    public async Task Deve_persistir_usuario_com_repositorio_real() {
        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var unityOfWork = new UnityOfWork(context);
        var passwordEncripter = new FakePasswordEncripter();
        var tokenGeneration = new FakeAccessTokenGeneration();
        var useCase = new RegisterUserUseCase(_mapper, passwordEncripter, repository, repository, unityOfWork, tokenGeneration);

        var request = new RequestRegisterUserJson {
            Name = "Samuel Torres",
            Email = "samuel@exemplo.com",
            Password = "Senha123!"
        };

        var response = await useCase.Execute(request);

        response.Name.Should().Be(request.Name);
        response.Token.Should().Be($"token-{request.Email}");

        var persistedUser = await context.Users.SingleAsync();
        persistedUser.Email.Should().Be(request.Email);
        persistedUser.Password.Should().Be("hashed-Senha123!");
        persistedUser.UserIdentifier.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Deve_obter_usuario_logado_com_logged_user_real() {
        await using var context = CreateContext();
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var tokenProvider = new FakeTokenProvider(CreateToken(user.UserIdentifier));
        var loggedUser = new LoggedUser(context, tokenProvider);

        var result = await loggedUser.Get();

        result.Should().NotBeNull();
        result!.Email.Should().Be(user.Email);
        result.UserIdentifier.Should().Be(user.UserIdentifier);
    }

    [Fact]
    public async Task Deve_excluir_usuario_com_repositorio_real() {
        await using var context = CreateContext();
        var user = CreateUser();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repository = new UserRepository(context);
        var unityOfWork = new UnityOfWork(context);
        var tokenProvider = new FakeTokenProvider(CreateToken(user.UserIdentifier));
        var loggedUser = new LoggedUser(context, tokenProvider);
        var useCase = new DeleteUserAccountUseCase(repository, loggedUser, unityOfWork);

        await useCase.Execute();

        var users = await context.Users.ToListAsync();
        users.Should().BeEmpty();
    }

    private static BarberBossDbContext CreateContext() {
        var options = new DbContextOptionsBuilder<BarberBossDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BarberBossDbContext(options);
    }

    private static User CreateUser() {
        return new User {
            Name = "Samuel Torres",
            Email = "samuel@exemplo.com",
            Password = "hashed-Senha123!",
            UserIdentifier = Guid.NewGuid()
        };
    }

    private static string CreateToken(Guid userIdentifier) {
        var token = new JwtSecurityToken(claims: [new Claim(ClaimTypes.Sid, userIdentifier.ToString())]);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private sealed class FakePasswordEncripter : IPasswordEncripter {
        public string Encrypt(string password) {
            return $"hashed-{password}";
        }

        public bool Verify(string password, string passwordHash) {
            return passwordHash == Encrypt(password);
        }
    }

    private sealed class FakeAccessTokenGeneration : IAccessTokenGeneration {
        public string Generate(User user) {
            return $"token-{user.Email}";
        }
    }

    private sealed class FakeTokenProvider : ITokenProvider {
        private readonly string _token;

        public FakeTokenProvider(string token) {
            _token = token;
        }

        public string TokenOnRequest() {
            return _token;
        }
    }
}
