using HasarIhbar.Application.DTOs;
using HasarIhbar.Domain.Entities;

namespace HasarIhbar.Application.Mappers
{
    public static class ClaimApplicationMapper
    {
        public static ClaimApplication ToEntity(ClaimApplicationDto dto)
        {
            return new ClaimApplication
            {
                PolicyNumber = dto.PolicyNumber,
                ReporterFullName = dto.ReporterFullName,
                ReporterType = dto.ReporterType,
                ReporterIdType = dto.ReporterIdType,
                Coverage = dto.Coverage,
                ClaimProvince = dto.ClaimProvince,
                ClaimDistrict = dto.ClaimDistrict,
                ClaimDateTime = dto.ClaimDateTime,
                ClaimAmount = dto.ClaimAmount,
                ClaimDescription = dto.ClaimDescription,
                Phone = dto.Phone,
                Email = dto.Email,
                DriverPhone = dto.DriverPhone,
                IsInsuredDriver = dto.IsInsuredDriver,
                DriverNationalId = dto.DriverNationalId,
                DriverFirstName = dto.DriverFirstName,
                DriverLastName = dto.DriverLastName,
                ResidenceProvince = dto.ResidenceProvince,
                ResidenceDistrict = dto.ResidenceDistrict,
                ResidenceStreet = dto.ResidenceStreet,
                ServiceProvince = dto.ServiceProvince,
                ServiceDistrict = dto.ServiceDistrict,
                ServiceNeighborhood = dto.ServiceNeighborhood,
                ServiceStreet = dto.ServiceStreet,
                ServiceAlley = dto.ServiceAlley,
                ServicePhone = dto.ServicePhone,
                ServiceTaxNumber = dto.ServiceTaxNumber,
                ServiceEmail = dto.ServiceEmail
            };
        }

        public static ClaimApplicationDto ToDto(ClaimApplication entity)
        {
            return new ClaimApplicationDto
            {
                PolicyNumber = entity.PolicyNumber,
                ReporterFullName = entity.ReporterFullName,
                ReporterType = entity.ReporterType,
                ReporterIdType = entity.ReporterIdType,
                Coverage = entity.Coverage,
                ClaimProvince = entity.ClaimProvince,
                ClaimDistrict = entity.ClaimDistrict,
                ClaimDateTime = entity.ClaimDateTime,
                ClaimAmount = entity.ClaimAmount,
                ClaimDescription = entity.ClaimDescription,
                Phone = entity.Phone,
                Email = entity.Email,
                DriverPhone = entity.DriverPhone,
                IsInsuredDriver = entity.IsInsuredDriver,
                DriverNationalId = entity.DriverNationalId,
                DriverFirstName = entity.DriverFirstName,
                DriverLastName = entity.DriverLastName,
                ResidenceProvince = entity.ResidenceProvince,
                ResidenceDistrict = entity.ResidenceDistrict,
                ResidenceStreet = entity.ResidenceStreet,
                ServiceProvince = entity.ServiceProvince,
                ServiceDistrict = entity.ServiceDistrict,
                ServiceNeighborhood = entity.ServiceNeighborhood,
                ServiceStreet = entity.ServiceStreet,
                ServiceAlley = entity.ServiceAlley,
                ServicePhone = entity.ServicePhone,
                ServiceTaxNumber = entity.ServiceTaxNumber,
                ServiceEmail = entity.ServiceEmail
            };
        }
    }
}