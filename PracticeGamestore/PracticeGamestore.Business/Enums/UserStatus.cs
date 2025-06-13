namespace PracticeGamestore.Business.Enums;

public enum UserStatus
{
    Active = DataAccess.Enums.UserStatus.Active,
    Banned = DataAccess.Enums.UserStatus.Banned,
    Deleted = DataAccess.Enums.UserStatus.Deleted
}