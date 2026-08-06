using AutoMapper;
using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest {
    private readonly IMapper _mapper = CreateMapper();

    private static IMapper CreateMapper() {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>(), NullLoggerFactory.Instance);
        return configuration.CreateMapper();
    }

    [Fact]
    public async Task Deve_cadastrar_usuario_quando_dados_forem_validos() {
        var passwordEncripter = new FakePasswordEncripter();
        var readOnlyRepository = new FakeUserReadOnlyRepository(emailExists: false);
        var writeOnlyRepository = new FakeUserWriteOnlyRepository();
        var unityOfWork = new FakeUnityOfWork();
        var tokenGeneration = new FakeAccessTokenGeneration();
        var useCase = new RegisterUserUseCase(_mapper, passwordEncripter, readOnlyRepository, writeOnlyRepository, unityOfWork, tokenGeneration);

        var request = new RequestRegisterUserJson {
            Name = "Samuel Torres",
            Email = "samuel@exemplo.com",
            Password = "Senha123!"
        };

        var response = await useCase.Execute(request);

        response.Name.Should().Be(request.Name);
        response.Token.Should().Be("token-samuel@exemplo.com");
        writeOnlyRepository.AddedUser.Should().NotBeNull();
        writeOnlyRepository.AddedUser!.Name.Should().Be(request.Name);
        writeOnlyRepository.AddedUser.Email.Should().Be(request.Email);
        writeOnlyRepository.AddedUser.Password.Should().Be("hashed-Senha123!");
        writeOnlyRepository.AddedUser.UserIdentifier.Should().NotBe(Guid.Empty);
        unityOfWork.CommitCount.Should().Be(1);
        passwordEncripter.EncryptedPasswords.Should().ContainSingle().Which.Should().Be(request.Password);
    }

    [Fact]
    public async Task Deve_rejeitar_quando_email_ja_estiver_cadastrado() {
        var useCase = new RegisterUserUseCase(
            _mapper,
            new FakePasswordEncripter(),
            new FakeUserReadOnlyRepository(emailExists: true),
            new FakeUserWriteOnlyRepository(),
            new FakeUnityOfWork(),
            new FakeAccessTokenGeneration());

        var request = new RequestRegisterUserJson {
            Name = "Samuel Torres",
            Email = "samuel@exemplo.com",
            Password = "Senha123!"
        };

        var action = async () => await useCase.Execute(request);

        var exception = await action.Should().ThrowAsync<ErrorOnValidatorException>();
        exception.Which.Errors.Should().Contain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }

    private sealed class FakeUserReadOnlyRepository : IUserReadOnlyRepository {
        private readonly bool _emailExists;

        public FakeUserReadOnlyRepository(bool emailExists) {
            _emailExists = emailExists;
        }

        public Task<bool> ExistActiveUserWithEmail(string email) {
            return Task.FromResult(_emailExists);
        }

        public Task<User?> GetUserByEmail(string email) {
            return Task.FromResult<User?>(null);
        }
    }

    private sealed class FakeUserWriteOnlyRepository : IUserWriteOnlyRepository {
        public User? AddedUser { get; private set; }

        public Task Add(User user) {
            AddedUser = user;
            return Task.CompletedTask;
        }

        public Task Delete(User user) {
            return Task.CompletedTask;
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
        public List<string> EncryptedPasswords { get; } = [];

        public string Encrypt(string password) {
            EncryptedPasswords.Add(password);
            return $"hashed-{password}";
        }

        public bool Verify(string password, string passwordHash) {
            return passwordHash == $"hashed-{password}";
        }
    }

    private sealed class FakeAccessTokenGeneration : IAccessTokenGeneration {
        public string Generate(User user) {
            return $"token-{user.Email}";
        }
    }
}
