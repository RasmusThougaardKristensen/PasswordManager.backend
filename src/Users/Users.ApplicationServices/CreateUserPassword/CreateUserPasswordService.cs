﻿using PasswordManager.User.Domain.Operations;
using PasswordManager.Users.ApplicationServices.Components;
using PasswordManager.Users.ApplicationServices.Operations;
using PasswordManager.Users.ApplicationServices.Repositories.User;
using PasswordManager.Users.Domain.Operations;
using PasswordManager.Users.Domain.User;
using Rebus.Bus;
using Users.Messages.CreateUserPassword;

namespace PasswordManager.Users.ApplicationServices.CreateUserPassword;
public class CreateUserPasswordService : ICreateUserPasswordService
{
    private readonly IUserRepository _userRepository;
    private readonly IOperationService _operationService;
    private readonly IBus _bus;
    private readonly IPasswordComponent _passwordComponent;

    public CreateUserPasswordService(IUserRepository userRepository, IOperationService operationService, IBus bus, IPasswordComponent passwordComponent)
    {
        _userRepository = userRepository;
        _operationService = operationService;
        _bus = bus;
        _passwordComponent = passwordComponent;
    }

    public async Task<OperationResult> RequestCreateUserPassword(UserPasswordModel userPasswordModel, OperationDetails operationDetails)
    {
        var user = await _userRepository.Get(userPasswordModel.UserId);

        if (user is null)
        {
            return OperationResult.InvalidState("Cannot create password for user because user was not found");
        }

        if (user.IsDeleted())
        {
            return OperationResult.InvalidState("Cannot create user password because user was marked as deleted");
        }

        var operation = await _operationService.QueueOperation(OperationBuilder.CreateUserPassword(userPasswordModel, operationDetails.CreatedBy));

        await _bus.Send(new CreateUserPasswordCommand(operation.RequestId));

        return OperationResult.Accepted(operation);
    }
    public async Task CreateUserPassword(UserPasswordModel userPasswordModel)
    {
        await _passwordComponent.CreateUserPassword(userPasswordModel);
    }
}
