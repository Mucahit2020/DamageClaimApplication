using HasarIhbar.Domain.Entities;

namespace HasarIhbar.Domain.Interfaces
{
    public interface IFormFiller
    {
        Task FillAsync(ClaimApplication application);
        Task SubmitAsync();
        Task<bool> VerifySuccessAsync();
    }
}