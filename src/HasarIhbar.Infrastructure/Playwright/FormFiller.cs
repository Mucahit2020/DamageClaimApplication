using HasarIhbar.Domain.Entities;
using HasarIhbar.Domain.Interfaces;

namespace HasarIhbar.Infrastructure.Playwright
{
    public class FormFiller : IFormFiller
    {
        private readonly PlaywrightService _playwrightService;

        public FormFiller(PlaywrightService playwrightService)
        {
            _playwrightService = playwrightService;
        }

        public async Task FillAsync(ClaimApplication application)
        {
            var page = _playwrightService.Page;

            // Tab 1 - Policy Info
            await _playwrightService.SelectTabAsync(0);
            await page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
            await FillTab1Async(application);

            // Tab 2 - Personal Info
            await _playwrightService.SelectTabAsync(1);
            await page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
            await FillTab2Async(application);

            // Tab 3 - Claim Info
            await _playwrightService.SelectTabAsync(2);
            await page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
            await FillTab3Async(application);

            // Tab 4 - Vehicle Info
            await _playwrightService.SelectTabAsync(3);
            await page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
            await FillTab4Async(application);

            // Tab 5 - Third Party Info
            await _playwrightService.SelectTabAsync(4);
            await page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
            await FillTab5Async(application);

            // Tab 6 - Declaration
            await _playwrightService.SelectTabAsync(5);
            await page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
            await FillTab6Async(application);
        }

        private async Task FillTab1Async(ClaimApplication application)
        {
            var page = _playwrightService.Page;
            await page.FillAsync("input[name='policyNumber'], #policyNumber", application.PolicyNumber);
            await page.FillAsync("input[name='nationalId'], #nationalId", application.NationalId);
            if (!string.IsNullOrEmpty(application.PlateNumber))
                await page.FillAsync("input[name='plateNumber'], #plateNumber", application.PlateNumber);
        }

        private async Task FillTab2Async(ClaimApplication application)
        {
            var page = _playwrightService.Page;
            await page.FillAsync("input[name='firstName'], #firstName", application.FirstName);
            await page.FillAsync("input[name='lastName'], #lastName", application.LastName);
            await page.FillAsync("input[name='phone'], #phone", application.Phone);
            if (!string.IsNullOrEmpty(application.Email))
                await page.FillAsync("input[name='email'], #email", application.Email);
        }

        private async Task FillTab3Async(ClaimApplication application)
        {
            var page = _playwrightService.Page;
            await page.FillAsync("input[name='claimDate'], #claimDate", application.ClaimDate.ToString("dd.MM.yyyy"));
            await page.FillAsync("input[name='claimLocation'], #claimLocation", application.ClaimLocation);
            await page.FillAsync("textarea[name='claimDescription'], #claimDescription", application.ClaimDescription);
        }

        private async Task FillTab4Async(ClaimApplication application)
        {
            var page = _playwrightService.Page;
            if (!string.IsNullOrEmpty(application.VehicleBrand))
                await page.FillAsync("input[name='vehicleBrand'], #vehicleBrand", application.VehicleBrand);
            if (!string.IsNullOrEmpty(application.VehicleModel))
                await page.FillAsync("input[name='vehicleModel'], #vehicleModel", application.VehicleModel);
            if (application.VehicleYear > 0)
                await page.FillAsync("input[name='vehicleYear'], #vehicleYear", application.VehicleYear.ToString());
        }

        private async Task FillTab5Async(ClaimApplication application)
        {
            var page = _playwrightService.Page;
            if (!string.IsNullOrEmpty(application.ThirdPartyPlate))
                await page.FillAsync("input[name='thirdPartyPlate'], #thirdPartyPlate", application.ThirdPartyPlate);
            if (!string.IsNullOrEmpty(application.ThirdPartyFullName))
                await page.FillAsync("input[name='thirdPartyFullName'], #thirdPartyFullName", application.ThirdPartyFullName);
        }

        private async Task FillTab6Async(ClaimApplication application)
        {
            var page = _playwrightService.Page;
            if (application.IsDeclarationAccepted)
                await page.CheckAsync("input[type='checkbox'][name='declaration'], #declaration");
        }

        public async Task SubmitAsync()
        {
            var page = _playwrightService.Page;
            await page.ClickAsync("button[type='submit'], input[type='submit']");
            await page.WaitForLoadStateAsync(Microsoft.Playwright.LoadState.NetworkIdle);
        }

        public async Task<bool> VerifySuccessAsync()
        {
            var page = _playwrightService.Page;
            try
            {
                await page.WaitForSelectorAsync(".success-message, .alert-success", new Microsoft.Playwright.PageWaitForSelectorOptions
                {
                    Timeout = 10000
                });
                return true;
            }
            catch
            {
                await _playwrightService.TakeScreenshotAsync($"error_{DateTime.Now:yyyyMMddHHmmss}.png");
                return false;
            }
        }
    }
}