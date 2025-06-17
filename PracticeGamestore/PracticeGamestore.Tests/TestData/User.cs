using PracticeGamestore.DataAccess.Enums;

namespace PracticeGamestore.Tests.TestData;

public class User
{
    public static List<DataAccess.Entities.User> GenerateUserEntities()
    {
        return new()
        {
            new()
            {
                Id = Guid.NewGuid(), UserName = "user1", Email = "user1@example.com", PhoneNumber = "1234567890",
                Role = UserRole.User, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1990, 1, 1)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user2", Email = "user2@example.com", PhoneNumber = "2345678901",
                Role = UserRole.Admin, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1991, 2, 2)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user3", Email = "user3@example.com", PhoneNumber = "3456789012",
                Role = UserRole.User, Status = UserStatus.Banned, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1992, 3, 3)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user4", Email = "user4@example.com", PhoneNumber = "4567890123",
                Role = UserRole.User, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1993, 4, 4)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user5", Email = "user5@example.com", PhoneNumber = "5678901234",
                Role = UserRole.Manager, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1994, 5, 5)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user6", Email = "user6@example.com", PhoneNumber = "6789012345",
                Role = UserRole.User, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1995, 6, 6)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user7", Email = "user7@example.com", PhoneNumber = "7890123456",
                Role = UserRole.User, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1996, 7, 7)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user8", Email = "user8@example.com", PhoneNumber = "8901234567",
                Role = UserRole.Admin, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1997, 8, 8)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user9", Email = "user9@example.com", PhoneNumber = "9012345678",
                Role = UserRole.User, Status = UserStatus.Banned, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1998, 9, 9)
            },
            new()
            {
                Id = Guid.NewGuid(), UserName = "user10", Email = "user10@example.com", PhoneNumber = "0123456789",
                Role = UserRole.User, Status = UserStatus.Active, CountryId = Guid.NewGuid(), BirthDate = new DateTime(1999, 10, 10)
            }
        };
    }
    
    public static List<Business.DataTransferObjects.UserDto> GenerateUserDtos()
    {
        return new()
        {
            new(Guid.NewGuid(), "user1", "user1@example", "1234567890", "password1", Business.Enums.UserRole.User, (Business.Enums.UserStatus)UserStatus.Active, Guid.NewGuid(),
                new DateTime(1990, 1, 1)),
            new(Guid.NewGuid(), "user2", "user2@example.com", "2345678901", "password2", Business.Enums.UserRole.Admin, (Business.Enums.UserStatus)UserStatus.Active,
                Guid.NewGuid(), new DateTime(1991, 2, 2)),
            new(Guid.NewGuid(), "user3", "user3@example.com", "3456789012", "password3", Business.Enums.UserRole.User, (Business.Enums.UserStatus)UserStatus.Banned,
                Guid.NewGuid(), new DateTime(1992, 3, 3)),
            new(Guid.NewGuid(), "user4", "user4@example.com", "4567890123", "password4", Business.Enums.UserRole.User, (Business.Enums.UserStatus)UserStatus.Active,
                Guid.NewGuid(), new DateTime(1993, 4, 4)),
            new(Guid.NewGuid(), "user5", "user5@example.com", "5678901234", "password5", Business.Enums.UserRole.Manager, (Business.Enums.UserStatus)UserStatus.Active,
                Guid.NewGuid(), new DateTime(1994, 5, 5)),
            new(Guid.NewGuid(), "user6", "user6@example.com", "6789012345", "password6", Business.Enums.UserRole.User, (Business.Enums.UserStatus)UserStatus.Active,
                Guid.NewGuid(), new DateTime(1995, 6, 6)),
            new(Guid.NewGuid(), "user7", "user7@example.com", "7890123456", "password7", Business.Enums.UserRole.User, (Business.Enums.UserStatus)UserStatus.Active,
                Guid.NewGuid(), new DateTime(1996, 7, 7)),
            new(Guid.NewGuid(), "user8", "user8@example.com", "8901234567", "password8", Business.Enums.UserRole.Admin, (Business.Enums.UserStatus)UserStatus.Active,
                Guid.NewGuid(), new DateTime(1997, 8, 8)),
            new(Guid.NewGuid(), "user9", "user9@example.com", "9012345678", "password9", Business.Enums.UserRole.User, (Business.Enums.UserStatus)UserStatus.Banned,
                Guid.NewGuid(), new DateTime(1998, 9, 9)),
            new(Guid.NewGuid(), "user10", "user10@example.com", "0123456789", "password10", Business.Enums.UserRole.User, (Business.Enums.UserStatus)UserStatus.Active,
                Guid.NewGuid(), new DateTime(1999, 10, 10))
        };
    }

    public static DataAccess.Entities.User GenerateUserEntity(Guid? id = null)
    {
        return new DataAccess.Entities.User
        {
            Id = id ?? Guid.NewGuid(),
            UserName = "testuser",
            Email = "testuser@example.com",
            PhoneNumber = "123456789",
            PasswordHash = "hashedpassword",
            Role = UserRole.User,
            Status = UserStatus.Active,
            CountryId = Guid.NewGuid(),
            BirthDate = new DateTime(1990, 1, 1)
        };
    }

    public static Business.DataTransferObjects.UserDto GenerateUserDto(Guid? id = null)
    {
        return new Business.DataTransferObjects.UserDto
        (
            id ?? Guid.NewGuid(),
            "testuser",
            "testuser@example.com",
            "123456789",
            "hashedpassword",
            Business.Enums.UserRole.User,
            (Business.Enums.UserStatus)UserStatus.Active,
            Guid.NewGuid(),
            new DateTime(1990, 1, 1));
    }

    public static Models.User.UserRequestModel GenerateUserRequestModel()
    {
        return new Models.User.UserRequestModel
        {
            UserName = "John Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            Password = "StrongPassword123!",
            Role = Business.Enums.UserRole.User,
            CountryId = Guid.NewGuid(),
            BirthDate = DateTime.UtcNow.AddYears(-20)
        };
    }
}