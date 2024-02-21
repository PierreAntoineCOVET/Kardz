using DtoCoincheCardColorsEnum = DTOs.Enums.CoincheCardColorsEnum;
using DomainCoincheCardColorsEnum = Domain.Enums.CoincheCardColorsEnum;
using DtoContractStatesEnum = DTOs.Enums.ContractStatesEnum;
using DomainContractStatesEnum = Domain.Enums.ContractStatesEnum;

namespace EventHandlers.Mappers
{
    public static class EnumsMapper
    {
        public static DtoCoincheCardColorsEnum? ToDtoEnum(this DomainCoincheCardColorsEnum? @enum)
        {
            if (@enum == null) return null;

            return @enum switch
            {
                DomainCoincheCardColorsEnum.AllTrump => DtoCoincheCardColorsEnum.AllTrump,
                DomainCoincheCardColorsEnum.NoTrump => DtoCoincheCardColorsEnum.NoTrump,
                DomainCoincheCardColorsEnum.Club => DtoCoincheCardColorsEnum.Club,
                DomainCoincheCardColorsEnum.Diamond => DtoCoincheCardColorsEnum.Diamond,
                DomainCoincheCardColorsEnum.Spade => DtoCoincheCardColorsEnum.Spade,
                DomainCoincheCardColorsEnum.Heart => DtoCoincheCardColorsEnum.Heart,
            };
        }

        public static DomainCoincheCardColorsEnum? ToDomainEnum(this DtoCoincheCardColorsEnum? @enum)
        {
            if (@enum == null) return null;

            return @enum switch
            {
                DtoCoincheCardColorsEnum.AllTrump => DomainCoincheCardColorsEnum.AllTrump,
                DtoCoincheCardColorsEnum.NoTrump => DomainCoincheCardColorsEnum.NoTrump,
                DtoCoincheCardColorsEnum.Club => DomainCoincheCardColorsEnum.Club,
                DtoCoincheCardColorsEnum.Diamond => DomainCoincheCardColorsEnum.Diamond,
                DtoCoincheCardColorsEnum.Spade => DomainCoincheCardColorsEnum.Spade,
                DtoCoincheCardColorsEnum.Heart => DomainCoincheCardColorsEnum.Heart,
            };
        }

        public static DtoContractStatesEnum ToDtoEnum(this DomainContractStatesEnum @enum)
        => @enum switch
        {
            DomainContractStatesEnum.Closed => DtoContractStatesEnum.Closed,
            DomainContractStatesEnum.Failed => DtoContractStatesEnum.Failed,
            DomainContractStatesEnum.Valid => DtoContractStatesEnum.Valid
        };
    }
}
