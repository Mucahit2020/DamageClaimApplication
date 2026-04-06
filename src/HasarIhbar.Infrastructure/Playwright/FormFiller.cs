using HasarIhbar.Domain.Entities;
using HasarIhbar.Domain.Interfaces;
using Microsoft.Playwright;

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
            await FillTab1Async(application);
            await ClickNextAsync(1);

            await FillTab2Async(application);
            await ClickNextAsync(2);

            await FillTab3Async(application);
            await ClickNextAsync(3);

            await FillTab4Async(application);
            await ClickNextAsync(4);

            await FillTab5Async(application);
            await ClickNextAsync(5);
        }

        private async Task FillTab1Async(ClaimApplication application)
        {
            var frame = _playwrightService.Frame;
            Console.WriteLine("══ ADIM 1: Temel Bilgiler ══");

            await Doldur(frame, "#policeNo", application.PolicyNumber);
            await Doldur(frame, "#ihbarEdenAdSoyad", application.ReporterFullName);
            await SecDropdown(frame, "ihbarEdentipi", application.ReporterType);
            await SecDropdown(frame, "ihbarEdenKimlikTipi", application.ReporterIdType);
            await SecDropdown(frame, "teminat", application.Coverage);
        }

        private async Task FillTab2Async(ClaimApplication application)
        {
            var frame = _playwrightService.Frame;
            var page = _playwrightService.Page;
            Console.WriteLine("══ ADIM 2: Hasar Detaylari ══");

            await frame.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await SecDropdown(frame, "hasarIli", application.ClaimProvince);
            await page.WaitForTimeoutAsync(1500);
            await SecDropdown(frame, "hasarIlce", application.ClaimDistrict);

            await frame.EvaluateAsync($@"
                const el = document.getElementById('hasarTarihi');
                if (el) {{
                    el.value = '{application.ClaimDateTime}';
                    el.dispatchEvent(new Event('change', {{ bubbles: true }}));
                    el.dispatchEvent(new Event('input',  {{ bubbles: true }}));
                }}
            ");
            Console.WriteLine($"  ✓ #hasarTarihi = \"{application.ClaimDateTime}\"");

            await Doldur(frame, "#hasarMeblag", application.ClaimAmount);
            await Doldur(frame, "#izahat", application.ClaimDescription);
        }

        private async Task FillTab3Async(ClaimApplication application)
        {
            var frame = _playwrightService.Frame;
            Console.WriteLine("══ ADIM 3: Iletisim ══");

            await frame.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await Doldur(frame, "#cepTelefonu", application.Phone);
            await Doldur(frame, "#mailAdresi1", application.Email);
            await Doldur(frame, "#surucuTelNo", application.DriverPhone);
            await SecDropdown(frame, "sigortaliMiId", application.IsInsuredDriver);
            await Doldur(frame, "#surucuTcNo", application.DriverNationalId);
            await Doldur(frame, "#surucuAd", application.DriverFirstName);
            await Doldur(frame, "#surucuSoyad", application.DriverLastName);
        }

        private async Task FillTab4Async(ClaimApplication application)
        {
            var frame = _playwrightService.Frame;
            var page = _playwrightService.Page;
            Console.WriteLine("══ ADIM 4: Adres ══");

            await frame.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await SecDropdown(frame, "ikametgahIli", application.ResidenceProvince);
            await page.WaitForTimeoutAsync(1500);
            await SecDropdown(frame, "ikametgahIlce", application.ResidenceDistrict);
            await Doldur(frame, "#ikametgahCadde", application.ResidenceStreet);
        }

        private async Task FillTab5Async(ClaimApplication application)
        {
            var frame = _playwrightService.Frame;
            var page = _playwrightService.Page;
            Console.WriteLine("══ ADIM 5: Servis ══");

            await frame.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await SecDropdown(frame, "servisIli", application.ServiceProvince);
            await page.WaitForTimeoutAsync(1500);
            await SecDropdown(frame, "servisIlce", application.ServiceDistrict);
            await Doldur(frame, "#servisMahalleKoy", application.ServiceNeighborhood);
            await Doldur(frame, "#servisCadde", application.ServiceStreet);
            await Doldur(frame, "#servisSokak", application.ServiceAlley);
            await Doldur(frame, "#servisTelNo", application.ServicePhone);
            await Doldur(frame, "#vergiNo", application.ServiceTaxNumber);
            await Doldur(frame, "#servisMailAdresi", application.ServiceEmail);
        }

        private async Task ClickNextAsync(int adim)
        {
            var frame = _playwrightService.Frame;
            var page = _playwrightService.Page;
            Console.WriteLine($"  → Adim {adim} Ileri...");

            for (int i = 0; i < 20; i++)
            {
                try
                {
                    var btn = await frame.QuerySelectorAsync($"[data-step='{adim}'] button.next");
                    if (btn != null)
                    {
                        var disabled = await btn.GetAttributeAsync("disabled");
                        if (disabled == null) break;
                    }
                }
                catch { }
                await page.WaitForTimeoutAsync(500);
            }

            await frame.EvaluateAsync($@"
                const btn = document.querySelector('[data-step=""{adim}""] button.next');
                if (btn) {{ btn.removeAttribute('disabled'); btn.click(); }}
            ");

            Console.WriteLine($"  ✓ Adim {adim} Ileri tiklandi");
            await frame.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await page.WaitForTimeoutAsync(800);
        }

        public async Task SubmitAsync()
        {
            var frame = _playwrightService.Frame;
            var page = _playwrightService.Page;
            Console.WriteLine("══ ADIM 6: Gonder ══");

            await frame.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await page.WaitForTimeoutAsync(2000);

            var step6Display = await frame.EvaluateAsync<string>(@"
        (() => {
            const el = document.querySelector('[data-step=""6""]');
            return el ? el.style.display : 'bulunamadi';
        })()
    ");
            Console.WriteLine($"  Adim 6 display: {step6Display}");

            await frame.EvaluateAsync(@"
        (() => {
            const btn = document.getElementById('submitPhotosButton');
            if (btn) { btn.removeAttribute('disabled'); btn.click(); }
        })()
    ");

            Console.WriteLine("  ✓ Gonder tiklandi");
            await page.WaitForTimeoutAsync(3000);

            var alertText = await frame.EvaluateAsync<string>(@"
        (() => {
            const el = document.querySelector('.alertify-log, .alertify-message');
            return el ? el.innerText : '';
        })()
    ");

            if (!string.IsNullOrEmpty(alertText))
                Console.WriteLine($"  Alertify mesaji: {alertText}");
        }
        public async Task<bool> VerifySuccessAsync()
        {
            var page = _playwrightService.Page;
            var frame = _playwrightService.Frame;
            try
            {
                await page.WaitForTimeoutAsync(3000);

                var ss = $"hasar_sonuc_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                await _playwrightService.TakeScreenshotAsync(ss);
                Console.WriteLine($"  Screenshot: {ss}");

                // Hangi adımda olduğumuzu öğren
                var activeStep = await frame.EvaluateAsync<string>(@"
            (() => {
                const active = document.querySelector('.step.active');
                return active ? active.getAttribute('data-step') : 'bilinmiyor';
            })()
        ");
                Console.WriteLine($"  Aktif adim: {activeStep}");

                // 6. adımda mıyız?
                var step6Visible = await frame.EvaluateAsync<bool>(@"
            (() => {
                const el = document.querySelector('[data-step=""6""]');
                return el ? el.style.display === 'block' : false;
            })()
        ");
                Console.WriteLine($"  6. adim gorunuyor mu: {step6Visible}");

                // Alertify veya başarı mesajı
                var mesaj = await frame.EvaluateAsync<string>(@"
            (() => {
                const el = document.querySelector('.alertify-log, .alertify-message, .swal2-title, .swal2-content');
                return el ? el.innerText : 'mesaj yok';
            })()
        ");
                Console.WriteLine($"  Mesaj: {mesaj}");

                // Gerçek başarı kontrolü - 6. adımda olmalı
                if (!step6Visible)
                {
                    Console.WriteLine("  ✗ Form 6. adima geçemedi, basari sayilmaz!");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VerifySuccess hatasi: {ex.Message}");
                return false;
            }
        }
        private static async Task Doldur(IFrame frame, string selector, string deger)
        {
            if (string.IsNullOrEmpty(deger)) return;
            try
            {
                var el = await frame.WaitForSelectorAsync(selector,
                    new FrameWaitForSelectorOptions { State = WaitForSelectorState.Visible, Timeout = 5000 });

                if (el == null) { Console.WriteLine($"  ⚠  Bulunamadi: {selector}"); return; }

                await el.ScrollIntoViewIfNeededAsync();
                await el.ClickAsync();
                await el.FillAsync(deger);
                Console.WriteLine($"  ✓ {selector} = \"{deger}\"");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠  Hata ({selector}): {ex.Message.Split('\n')[0]}");
            }
        }

        private static async Task SecDropdown(IFrame frame, string id, string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            try
            {
                await frame.EvaluateAsync($@"
                    const sel = document.getElementById('{id}');
                    if (sel) {{
                        sel.value = '{value}';
                        const label = sel.closest('.field')?.querySelector('.ui.dropdown .text');
                        if (label) {{
                            const opt = sel.options[sel.selectedIndex];
                            if (opt) label.textContent = opt.text;
                        }}
                        sel.dispatchEvent(new Event('change', {{ bubbles: true }}));
                        sel.dispatchEvent(new Event('input',  {{ bubbles: true }}));
                    }}
                ");
                Console.WriteLine($"  ✓ #{id} = \"{value}\"");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠  Dropdown hatasi (#{id}): {ex.Message.Split('\n')[0]}");
            }
        }
    }
}