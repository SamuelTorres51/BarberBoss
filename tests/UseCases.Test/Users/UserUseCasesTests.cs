using AutoMapper;
using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Application.UseCases.Users.Delete;
using BarberBoss.Application.UseCases.Users.GetProfile;
using BarberBoss.Application.UseCases.Users.Login;
using BarberBoss.Application.UseCases.Users.Update;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Domain.Serv_ices.LoggedUser;
using BarberBoss.Exception.ExceptionBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace UseCases.Test.Users;

public class UserUseCasesTests {
    private readonly IMapper _mapper = CreateMapper();

    private static IMapper CreateMapper() {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>(), NullLoggerFactory.Instance);
        return configuration.CreateMapper();
    }

    [Fact]
    public async Task Deve_realizar_login_quando_credenciais_forem_validas() {
        var passwordEncripter = new FakePasswordEncripter();
        var user = CreateUser(email: "samuel@exemplo.com", password: passwordEncripter.Encrypt("Senha123!"));
        var repository = new FakeUserReadOnlyRepository(user);
        var tokenGeneration = new FakeAccessTokenGeneration();
        var useCase = new DoLoginUseCase(repository, tokenGeneration, passwordEncripter);

        var response = await useCase.Execute(new RequestLoginJson {
            Email = user.Email,
            Password = "Senha123!"
        });

        response.Name.Should().Be(user.Name);
        response.Token.Should().Be($"token-{user.Email}");
    }

    [Fact]
    public async Task Deve_rejeitar_login_quando_senha_estiver_incorreta() {
        var passwordEncripter = new FakePasswordEncripter();
        var user = CreateUser(email: "samuel@exemplo.com", password: passwordEncripter.Encrypt("Senha123!"));
        var repository = new FakeUserReadOnlyRepository(user);
        var tokenGeneration = new FakeAccessTokenGeneration();
        var useCase = new DoLoginUseCase(repository, tokenGeneration, passwordEncripter);

        var action = async () => await useCase.Execute(new RequestLoginJson {
            Email = user.Email,
            Password = "SenhaErrada1!"
        });

        var exception = await action.Should().ThrowAsync<ErrorOnValidatorException>();
        exception.Which.Errors.Should().ContainSingle().Which.Should().Be("Email or password is incorrect.");
    }

    [Fact]
    public async Task Deve_retornar_perfil_do_usuario_logado() {
        var user = CreateUser();
        var loggedUser = new FakeLoggedUser(user);
        var useCase = new GetUserProfileUseCase(loggedUser, _mapper);

        var response = await useCase.Execute();

        response.Name.Should().Be(user.Name);
        response.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task Deve_atualizar_usuario_logado() {
        var user = CreateUser();
        var loggedUser = new FakeLoggedUser(user);
        var repository = new FakeUserUpdateOnlyRepository(user);
        var readOnlyRepository = new FakeUserReadOnlyRepository(emailExists: true);
        var unityOfWork = new FakeUnityOfWork();
        var useCase = new UpdateUserUseCase(repository, readOnlyRepository, loggedUser, _mapper, unityOfWork);

        var request = new RequestUpdateUserJson {
            Name = "Samuel Torres Atualizado",
            Email = user.Email
        };

        await useCase.Execute(request);

        repository.UpdatedUser.Should().NotBeNull();
        repository.UpdatedUser!.Name.Should().Be(request.Name);
        repository.UpdatedUser.Email.Should().Be(request.Email);
        unityOfWork.CommitCount.Should().Be(1);
    }

    [Fact]
    public async Task Deve_alterar_senha_do_usuario_logado() {
        var passwordEncripter = new FakePasswordEncripter();
        var user = CreateUser(password: passwordEncripter.Encrypt("SenhaAtual1!"));
        var loggedUser = new FakeLoggedUser(user);
        var repository = new FakeUserUpdateOnlyRepository(user);
        var unityOfWork = new FakeUnityOfWork();
        var useCase = new ChangeUserPasswordUseCase(repository, passwordEncripter, loggedUser, unityOfWork);

        await useCase.Execute(new RequestChangePasswordJson {
            CurrentPassword = "SenhaAtual1!",
            NewPassword = "SenhaNova1!"
        });

        repository.UpdatedUser.Should().NotBeNull();
        repository.UpdatedUser!.Password.Should().Be("hashed-SenhaNova1!");
        unityOfWork.CommitCount.Should().Be(1);
    }

    [Fact]
    public async Task Deve_excluir_conta_do_usuario_logado() {
        var user = CreateUser();
        var loggedUser = new FakeLoggedUser(user);
        var repository = new FakeUserWriteOnlyRepository();
        var unityOfWork = new FakeUnityOfWork();
        var useCase = new DeleteUserAccountUseCase(repository, loggedUser, unityOfWork);

        await useCase.Execute();

        repository.DeletedUser.Should().NotBeNull();
        repository.DeletedUser!.UserIdentifier.Should().Be(user.UserIdentifier);
        unityOfWork.CommitCount.Should().Be(1);
    }

    private static User CreateUser(string? email = null, string? password = null) {
        return new User {
            Id = 1,
            Name = "Samuel Torres",
            Email = email ?? "samuel@exemplo.com",
            Password = password ?? "hashed-Senha123!",
            UserIdentifier = Guid.NewGuid()
        };
    }

    private sealed class FakeUserReadOnlyRepository : IUserReadOnlyRepository {
        private readonly User? _user;
        private readonly bool _emailExists;

        public FakeUserReadOnlyRepository(User? user = null, bool emailExists = false) {
            _user = user;
            _emailExists = emailExists;
        }

        public Task<bool> ExistActiveUserWithEmail(string email) {
            return Task.FromResult(_emailExists);
        }

        public Task<User?> GetUserByEmail(string email) {
            return Task.FromResult(_user);
        }
    }

    private sealed class FakeUserWriteOnlyRepository : IUserWriteOnlyRepository {
        public User? DeletedUser { get; private set; }

        public Task Add(User user) {
            return Task.CompletedTask;
        }

        public Task Delete(User user) {
            DeletedUser = user;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeUserUpdateOnlyRepository : IUserUpdateOnlyRepository {
        public User UpdatedUser { get; private set; }
        private readonly User _user;

        public FakeUserUpdateOnlyRepository(User user) {
            _user = user;
            UpdatedUser = user;
        }

        public void Update(User user) {
            UpdatedUser = user;
        }

        public Task<User> GetById(long id) {
            return Task.FromResult(_user);
        }
    }

    private sealed class FakeUnityOfWork : IUnityOfWork {
        public int CommitCount { get; private set; }

        public Task Commit() {
            CommitCount++;
            return Task.CompletedTask;
        }
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

    private sealed class FakeLoggedUser : ILoggedUser {
        private readonly User _user;

        public FakeLoggedUser(User user) {
            _user = user;
        }

        public Task<User?> Get() {
            return Task.FromResult<User?>(_user);
        }
    }
}
