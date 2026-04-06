using HasarIhbar.Domain.Entities;
using HasarIhbar.Domain.Interfaces;

namespace HasarIhbar.Application.Services
{
    public class ClaimApplicationService : IClaimApplicationService
    {
        private readonly IFormFiller _formFiller;
        private readonly IPlaywrightService _playwrightService;

        private const string TargetUrl = "https://www.turkiyesigorta.com.tr/hasar/hasar-dosya-ihbar-girisi";

        public ClaimApplicationService(IFormFiller formFiller, IPlaywrightService playwrightService)
        {
            _formFiller = formFiller;
            _playwrightService = playwrightService;
        }

        public async Task<bool> SubmitAsync(ClaimApplication application, CancellationToken ct = default)
        {
            try
            {
                await _playwrightService.InitializeAsync(headless: false);
                await _playwrightService.NavigateAsync(TargetUrl);

                // Sayfanın tam yüklenmesini bekle
                await Task.Delay(3000);

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