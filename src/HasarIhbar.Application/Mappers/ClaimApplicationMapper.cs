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
                NationalId = dto.NationalId,
                PlateNumber = dto.PlateNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email,
                ClaimDate = dto.ClaimDate,
                ClaimLocation = dto.ClaimLocation,
                ClaimDescription = dto.ClaimDescription,
                VehicleBrand = dto.VehicleBrand,
                VehicleModel = dto.VehicleModel,
                VehicleYear = dto.VehicleYear,
                ThirdPartyPlate = dto.ThirdPartyPlate,
                ThirdPartyFullName = dto.ThirdPartyFullName,
                IsDeclarationAccepted = dto.IsDeclarationAccepted
            };
        }

        public static ClaimApplicationDto ToDto(ClaimApplication entity)
        {
            return new ClaimApplicationDto
            {
                PolicyNumber = entity.PolicyNumber,
                NationalId = entity.NationalId,
                PlateNumber = entity.PlateNumber,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Phone = entity.Phone,
                Email = entity.Email,
                ClaimDate = entity.ClaimDate,
                ClaimLocation = entity.ClaimLocation,
                ClaimDescription = entity.ClaimDescription,
                VehicleBrand = entity.VehicleBrand,
                VehicleModel = entity.VehicleModel,
                VehicleYear = entity.VehicleYear,
                ThirdPartyPlate = entity.ThirdPartyPlate,
                ThirdPartyFullName = entity.ThirdPartyFullName,
                IsDeclarationAccepted = entity.IsDeclarationAccepted
            };
        }
    }
}