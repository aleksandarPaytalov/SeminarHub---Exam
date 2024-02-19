using SeminarHub.Models;

namespace SeminarHub.Contracts
{
    public interface ISeminarService
    {
        Task<ICollection<SeminarViewModel>> GetAllSeminarsAsync();

        Task<ICollection<CategoryViewModel>> GetAllCategoriesAsync();

        Task AddNewSeminarAsync(AddNewSeminarViewModel model, string userId);

        Task<ICollection<SeminarViewModel>> GetAllSeminarsFromCollectionAsync(string userId);

        Task<bool> AddToCollectionAsync(int id, string userId, bool isAdded);

        Task RemoveFromCollectionAsync(int id, string userId);
        Task<AddNewSeminarViewModel?> GetSeminarForEditAsync(int id);

        Task EditSeminarAsync(AddNewSeminarViewModel model, int id);
        Task<SeminarDetailsViewModel?> GetSeminarDetailsAsync(int id);
        Task<DeleteSeminarViewModel?> GetSeminarForDeletingAsync(int id);
        Task DeleteSeminarByIdAsync(int id, string userId);
    }
}
