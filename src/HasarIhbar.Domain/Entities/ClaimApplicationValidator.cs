using HasarIhbar.Domain.Entities;

namespace HasarIhbar.Application.Validators
{
    public class ClaimApplicationValidator
    {
        public (bool IsValid, List<string> Errors) Validate(ClaimApplication application)
        {
            var errors = new List<string>();

            // Tab 1 - Policy Info
            if (string.IsNullOrWhiteSpace(application.PolicyNumber))
                errors.Add("Policy number is required.");

            if (string.IsNullOrWhiteSpace(application.NationalId) || application.NationalId.Length != 11)
                errors.Add("National ID must be 11 digits.");

            // Tab 2 - Personal Info
            if (string.IsNullOrWhiteSpace(application.FirstName))
                errors.Add("First name is required.");

            if (string.IsNullOrWhiteSpace(application.LastName))
                errors.Add("Last name is required.");

            if (string.IsNullOrWhiteSpace(application.Phone))
                errors.Add("Phone is required.");

            if (!string.IsNullOrWhiteSpace(application.Email) && !application.Email.Contains("@"))
                errors.Add("Email is not valid.");

            // Tab 3 - Claim Info
            if (application.ClaimDate == default)
                errors.Add("Claim date is required.");

            if (string.IsNullOrWhiteSpace(application.ClaimLocation))
                errors.Add("Claim location is required.");

            if (string.IsNullOrWhiteSpace(application.ClaimDescription))
                errors.Add("Claim description is required.");

            // Tab 6 - Declaration
            if (!application.IsDeclarationAccepted)
                errors.Add("Declaration must be accepted.");

            return (!errors.Any(), errors);
        }
    }
}