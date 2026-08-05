using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Serv_ices.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.Update;

public class UpdateUserUseCase : IUpdateUserUseCase {
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;

    public UpdateUserUseCase(IUserUpdateOnlyRepository repository, IUserReadOnlyRepository readOnlyRepository, ILoggedUser loggedUser, IMapper mapper, IUnityOfWork unityOfWork) {
        _repository = repository;
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
    }


    public async Task Execute(RequestUpdateUserJson request) {
        var loggedUser = await _loggedUser.Get();
        await Validate(request, loggedUser!);

       
        _mapper.Map(request, loggedUser);
        _repository.Update(loggedUser!);
        await _unityOfWork.Commit();

    }

    private async Task Validate(RequestUpdateUserJson request, User loggedUser) {
        var result = new UpdateUserValidator().Validate(request);

        var emailExists = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExists && request.Email != loggedUser.Email) {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid is false) {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorMessages);
        }


    }
}
