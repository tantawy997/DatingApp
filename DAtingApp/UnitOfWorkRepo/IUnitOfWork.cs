using DAtingApp.interfaces.repositoryInterfaces;

namespace DAtingApp.UnitOfWorkRepo
{
    public interface IUnitOfWork
    {
        IUserLikeRepo _UserLikeRepo { get; }

        IUserRepo _UserRepo { get; }

        IMessagesRepo _MessagesRepo { get; }

        Task<bool> Completes();

        bool HasChanges();

    }
}
