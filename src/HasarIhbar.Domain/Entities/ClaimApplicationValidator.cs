using HasarIhbar.Domain.Entities;

namespace HasarIhbar.Application.Validators
{
    public class ClaimApplicationValidator
    {
        public (bool IsValid, List<string> Errors) Validate(ClaimApplication application)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(application.PolicyNumber))
                errors.Add("Policy number is required.");

            if (string.IsNullOrWhiteSpace(application.ReporterFullName))
                errors.Add("Reporter full name is required.");

            if (string.IsNullOrWhiteSpace(application.ReporterType))
                errors.Add("Reporter type is required.");

            if (string.IsNullOrWhiteSpace(application.ClaimProvince))
                errors.Add("Claim province is required.");

            if (string.IsNullOrWhiteSpace(application.ClaimDistrict))
                errors.Add("Claim district is required.");

            if (string.IsNullOrWhiteSpace(application.ClaimDateTime))
                errors.Add("Claim date/time is required.");

            if (string.IsNullOrWhiteSpace(application.ClaimAmount))
                errors.Add("Claim amount is required.");

            if (string.IsNullOrWhiteSpace(application.ClaimDescription))
                errors.Add("Claim description is required.");

            if (string.IsNullOrWhiteSpace(application.Phone))
                errors.Add("Phone is required.");

            if (string.IsNullOrWhiteSpace(application.DriverNationalId))
                errors.Add("Driver national ID is required.");

            if (string.IsNullOrWhiteSpace(application.DriverFirstName))
                errors.Add("Driver first name is required.");

            if (string.IsNullOrWhiteSpace(application.DriverLastName))
                errors.Add("Driver last name is required.");

            if (string.IsNullOrWhiteSpace(application.ServicePhone))
                errors.Add("Service phone is required.");

            if (string.IsNullOrWhiteSpace(application.ServiceTaxNumber))
                errors.Add("Service tax number is required.");

            return (!errors.Any(), errors);
        }
    }
}