using HasarIhbar.Application.Services;
using HasarIhbar.Application.Validators;
using HasarIhbar.Domain.Entities;
using HasarIhbar.Domain.Interfaces;
using HasarIhbar.Infrastructure.Playwright;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<PlaywrightService>();
services.AddSingleton<IPlaywrightService>(sp => sp.GetRequiredService<PlaywrightService>());
services.AddSingleton<IFormFiller, FormFiller>();
services.AddSingleton<IClaimApplicationService, ClaimApplicationService>();
services.AddSingleton<ClaimApplicationValidator>();

var provider = services.BuildServiceProvider();
var validator = provider.GetRequiredService<ClaimApplicationValidator>();
var claimService = provider.GetRequiredService<IClaimApplicationService>();

// --- ÖRNEK VERİ ---
var app = new ClaimApplication
{
    // Tab 1 - Temel Bilgiler
    PolicyNumber = "123456789",
    ReporterFullName = "Mucahit Kizilgunes",
    ReporterType = "SIGORTALI",
    ReporterIdType = "TC_KIMLIK_NO",
    Coverage = "12", // Cam Kırılması

    // Tab 2 - Hasar Açıklama
    ClaimProvince = "40",     // İstanbul
    ClaimDistrict = "97506",  // Kadıköy
    ClaimDateTime = "2026-04-05T10:30",
    ClaimAmount = "5000",
    ClaimDescription = "Aracın ön camı park halindeyken taş isabet sonucu kırılmıştır.",

    // Tab 3 - İletişim
    Phone = "05321234567",
    Email = "mucahit@example.com",
    DriverPhone = "05321234567",
    IsInsuredDriver = "true",
    DriverNationalId = "12345678901",
    DriverFirstName = "Mucahit",
    DriverLastName = "Kizilgunes",

    // Tab 4 - Adres
    ResidenceProvince = "40",     // İstanbul
    ResidenceDistrict = "97506",  // Kadıköy
    ResidenceStreet = "Bagdat Caddesi",

    // Tab 5 - Servis Bilgileri
    ServiceProvince = "40",     // İstanbul
    ServiceDistrict = "97506",  // Kadıköy
    ServiceNeighborhood = "Moda Mahallesi",
    ServiceStreet = "Moda Caddesi",
    ServiceAlley = "Caferaga Sokak",
    ServicePhone = "02161234567",
    ServiceTaxNumber = "12345678901",
    ServiceEmail = "servis@example.com"
};

Console.WriteLine("=== Hasar Ihbar Basvuru Sistemi ===");
Console.WriteLine("Ornek veri yuklendi, basvuru baslatiliyor...");
Console.WriteLine();

var (isValid, errors) = validator.Validate(app);
if (!isValid)
{
    Console.WriteLine("=== Dogrulama Hatalari ===");
    foreach (var error in errors)
        Console.WriteLine($"  - {error}");
    return;
}

Console.WriteLine("Tarayici aciliyor...");
var success = await claimService.SubmitAsync(app);

Console.WriteLine(success
    ? "Basvuru basariyla tamamlandi!"
    : $"Basvuru basarisiz: {app.ErrorMessage}");

Console.WriteLine("Cikmak icin bir tusa basin...");
Console.ReadKey();