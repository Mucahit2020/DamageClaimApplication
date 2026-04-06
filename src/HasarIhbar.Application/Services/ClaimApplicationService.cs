using HasarIhbar.Domain.Entities;
using HasarIhbar.Domain.Interfaces;

namespace HasarIhbar.Application.Services
{
    public class ClaimApplicationService : IClaimApplicationService
    {
        private readonly IFormFiller _formFiller;

        public ClaimApplicationService(IFormFiller formFiller)
        {
            _formFiller = formFiller;
        }

        public async Task<bool> SubmitAsync(ClaimApplication application, CancellationToken ct = default)
        {
            try
            {
                await _formFiller.FillAsync(application);
                await _formFiller.SubmitAsync();

                var success = await _formFiller.VerifySuccessAsync();
                return success;
            }
            catch (Exception ex)
            {
                application.ErrorMessage = ex.Message;
                return false;
            }
        }
    }
}