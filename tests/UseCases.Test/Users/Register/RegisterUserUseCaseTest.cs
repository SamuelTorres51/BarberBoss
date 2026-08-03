using BarberBoss.Application.UseCases.Users.Register;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest {
    [Fact]
    public async Task Success() {
        var useCase = new RegisterUserUseCase();
    }
}
