using HasarIhbar.Application.Services;
using HasarIhbar.Application.Validators;
using HasarIhbar.Domain.Entities;
using HasarIhbar.Domain.Interfaces;
using HasarIhbar.Infrastructure.Playwright;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Register services
services.AddSingleton<PlaywrightService>();
services.AddSingleton<IPlaywrightService>(sp => sp.GetRequiredService<PlaywrightService>());
services.AddSingleton<IFormFiller,FormFiller >();
services.AddSingleton<IClaimApplicationService, ClaimApplicationService>();
services.AddSingleton<ClaimApplicationValidator>();

var provider = services.BuildServiceProvider();

var validator = provider.GetRequiredService<ClaimApplicationValidator>();
var claimService = provider.GetRequiredService<IClaimApplicationService>();

Console.WriteLine("=== Hasar Ihbar Basvuru Sistemi ===");
Console.WriteLine();

var application = new ClaimApplication();

// Tab 1 - Policy Info
Console.WriteLine("--- Tab 1: Police Bilgileri ---");
Console.Write("Police No: ");
application.PolicyNumber = Console.ReadLine() ?? string.Empty;

Console.Write("TC Kimlik No: ");
application.NationalId = Console.ReadLine() ?? string.Empty;

Console.Write("Plaka No (bos birakabilirsiniz): ");
application.PlateNumber = Console.ReadLine() ?? string.Empty;

// Tab 2 - Personal Info
Console.WriteLine();
Console.WriteLine("--- Tab 2: Kisisel Bilgiler ---");
Console.Write("Ad: ");
application.FirstName = Console.ReadLine() ?? string.Empty;

Console.Write("Soyad: ");
application.LastName = Console.ReadLine() ?? string.Empty;

Console.Write("Telefon: ");
application.Phone = Console.ReadLine() ?? string.Empty;

Console.Write("Email (bos birakabilirsiniz): ");
application.Email = Console.ReadLine() ?? string.Empty;

// Tab 3 - Claim Info
Console.WriteLine();
Console.WriteLine("--- Tab 3: Hasar Bilgileri ---");
Console.Write("Hasar Tarihi (gg.aa.yyyy): ");
if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var claimDate))
    application.ClaimDate = claimDate;

Console.Write("Hasar Yeri: ");
application.ClaimLocation = Console.ReadLine() ?? string.Empty;

Console.Write("Hasar Aciklamasi: ");
application.ClaimDescription = Console.ReadLine() ?? string.Empty;

// Tab 4 - Vehicle Info
Console.WriteLine();
Console.WriteLine("--- Tab 4: Arac Bilgileri (bos birakabilirsiniz) ---");
Console.Write("Arac Marka: ");
application.VehicleBrand = Console.ReadLine() ?? string.Empty;

Console.Write("Arac Model: ");
application.VehicleModel = Console.ReadLine() ?? string.Empty;

Console.Write("Arac Yili: ");
if (int.TryParse(Console.ReadLine(), out var vehicleYear))
    application.VehicleYear = vehicleYear;

// Tab 5 - Third Party Info
Console.WriteLine();
Console.WriteLine("--- Tab 5: Karsit Taraf Bilgileri (bos birakabilirsiniz) ---");
Console.Write("Karsit Taraf Plaka: ");
application.ThirdPartyPlate = Console.ReadLine() ?? string.Empty;

Console.Write("Karsit Taraf Ad Soyad: ");
application.ThirdPartyFullName = Console.ReadLine() ?? string.Empty;

// Tab 6 - Declaration
Console.WriteLine();
Console.WriteLine("--- Tab 6: Beyan ---");
Console.Write("Beyani kabul ediyor musunuz? (e/h): ");
application.IsDeclarationAccepted = Console.ReadLine()?.ToLower() == "e";

// Validate
Console.WriteLine();
var (isValid, errors) = validator.Validate(application);
if (!isValid)
{
    Console.WriteLine("=== Dogrulama Hatalari ===");
    foreach (var error in errors)
        Console.WriteLine($"  - {error}");
    Console.WriteLine("Basvuru iptal edildi.");
    return;
}

// Submit
Console.WriteLine("Basvuru gonderiliyor...");
var success = await claimService.SubmitAsync(application);

if (success)
{
    Console.WriteLine("Basvuru basariyla tamamlandi!");
}
else
{
    Console.WriteLine($"Basvuru basarisiz: {application.ErrorMessage}");
}

Console.WriteLine("Cikmak icin bir tusa basin...");
Console.ReadKey();