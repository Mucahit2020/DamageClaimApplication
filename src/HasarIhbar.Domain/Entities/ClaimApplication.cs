using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasarIhbar.Domain.Entities
{
    public class ClaimApplication
    {
        // Tab 1 - Policy Info
        public string PolicyNumber { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;

        // Tab 2 - Personal Info
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Tab 3 - Claim Info
        public DateTime ClaimDate { get; set; }
        public string ClaimLocation { get; set; } = string.Empty;
        public string ClaimDescription { get; set; } = string.Empty;

        // Tab 4 - Vehicle Info
        public string VehicleBrand { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public int VehicleYear { get; set; }

        // Tab 5 - Third Party Info
        public string ThirdPartyPlate { get; set; } = string.Empty;
        public string ThirdPartyFullName { get; set; } = string.Empty;

        // Tab 6 - Declaration
        public bool IsDeclarationAccepted { get; set; }

        public string? ErrorMessage { get; set; }

    }
}
