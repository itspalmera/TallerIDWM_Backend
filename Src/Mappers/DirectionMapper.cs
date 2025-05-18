using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIDWM_Backend.Src.Dtos;
using TallerIDWM_Backend.Src.DTOs;
using TallerIDWM_Backend.Src.DTOs.Direction;
using TallerIDWM_Backend.Src.Models;


namespace TallerIDWM_Backend.Src.Mappers
{
    public static class DirectionMapper
    {
        public static Direction FromDto(CreateDirectionDto dto, string userId)
        {
            return new Direction
            {
                Street = dto.street,
                Number = dto.number,
                Commune = dto.commune,
                Region = dto.region,
                PostalCode = dto.postalCode,
                UserId = userId
            };
        }


        public static DirectionDto ToDto(Direction shippingAddress)
        {
            return new DirectionDto
            {
                Street = shippingAddress.Street,
                Number = shippingAddress.Number,
                Commune = shippingAddress.Commune,
                Region = shippingAddress.Region,
                PostalCode = shippingAddress.PostalCode
            };
        }


        public static void UpdateDirectionFromDto(Direction direction, UpdateDirectionDto dto)
        {
            if (dto.street is not null)
                direction.Street = dto.street;


            if (dto.number is not null)
                direction.Number = dto.number;

            if (dto.commune is not null)
                direction.Commune = dto.commune;

            if (dto.region is not null)
                direction.Region = dto.region;

            if (dto.postalCode is not null)
                direction.PostalCode = dto.postalCode;
        }
    }
}