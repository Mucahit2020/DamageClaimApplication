namespace HasarIhbar.Application.DTOs
{
    public class ClaimApplicationDto
    {
        // Tab 1 - Temel Bilgiler
        public string PolicyNumber { get; set; } = string.Empty;
        public string ReporterFullName { get; set; } = string.Empty;
        public string ReporterType { get; set; } = string.Empty;
        public string ReporterIdType { get; set; } = string.Empty;
        public string Coverage { get; set; } = string.Empty;

        // Tab 2 - Hasar Açıklama
        public string ClaimProvince { get; set; } = string.Empty;
        public string ClaimDistrict { get; set; } = string.Empty;
        public string ClaimDateTime { get; set; } = string.Empty;
        public string ClaimAmount { get; set; } = string.Empty;
        public string ClaimDescription { get; set; } = string.Empty;

        // Tab 3 - İletişim
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DriverPhone { get; set; } = string.Empty;
        public string IsInsuredDriver { get; set; } = string.Empty;
        public string DriverNationalId { get; set; } = string.Empty;
        public string DriverFirstName { get; set; } = string.Empty;
        public string DriverLastName { get; set; } = string.Empty;

        // Tab 4 - Adres
        public string ResidenceProvince { get; set; } = string.Empty;
        public string ResidenceDistrict { get; set; } = string.Empty;
        public string ResidenceStreet { get; set; } = string.Empty;

        // Tab 5 - Servis Bilgileri
        public string ServiceProvince { get; set; } = string.Empty;
        public string ServiceDistrict { get; set; } = string.Empty;
        public string ServiceNeighborhood { get; set; } = string.Empty;
        public string ServiceStreet { get; set; } = string.Empty;
        public string ServiceAlley { get; set; } = string.Empty;
        public string ServicePhone { get; set; } = string.Empty;
        public string ServiceTaxNumber { get; set; } = string.Empty;
        public string ServiceEmail { get; set; } = string.Empty;
    }
}