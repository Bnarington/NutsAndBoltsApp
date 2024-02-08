using Infrastructure.Entities;

namespace Presentation.Models.Extensions;

public static class UserModelExtensions
{
    public static UserEntity ToUserEntity(this UserModel userModel)
    {
        if (userModel == null)
            return null!;

        return new UserEntity
        {
            FirstName = userModel.FirstName,
            LastName = userModel.LastName,
            Email = userModel.Email,
            Password = userModel.Password,
            PhoneNumber = userModel.PhoneNumber,
        };
    }
}
