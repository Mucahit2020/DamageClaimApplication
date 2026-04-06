using HasarIhbar.Domain.Entities;

namespace HasarIhbar.Domain.Interfaces
{
    public interface IClaimApplicationService
    {
        Task<bool> SubmitAsync(ClaimApplication application, CancellationToken ct = default);
    }
}