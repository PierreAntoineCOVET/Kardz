using DtoCoincheCardColorsEnum = DTOs.Enums.CoincheCardColorsEnum;
using DomainCoincheCardColorsEnum = Domain.Enums.CoincheCardColorsEnum;
using DtoContractStatesEnum = DTOs.Enums.ContractStatesEnum;
using DomainContractStatesEnum = Domain.Enums.ContractStatesEnum;
using DtoCoincheCardsEnum = DTOs.Enums.CardsEnum;
using DomainCoincheCardsEnum = Domain.Enums.CardsEnum;

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
                _ => throw new System.InvalidCastException()
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
                _ => throw new System.InvalidCastException()
            };
        }

        public static DtoContractStatesEnum ToDtoEnum(this DomainContractStatesEnum @enum)
        => @enum switch
        {
            DomainContractStatesEnum.Closed => DtoContractStatesEnum.Closed,
            DomainContractStatesEnum.Failed => DtoContractStatesEnum.Failed,
            DomainContractStatesEnum.Valid => DtoContractStatesEnum.Valid,
                _ => throw new System.InvalidCastException()
        };

        public static DomainCoincheCardsEnum ToDtoEnum(this DtoCoincheCardsEnum @enum)
        => @enum switch
        {
            DtoCoincheCardsEnum.AsClub => DomainCoincheCardsEnum.AsClub,
            DtoCoincheCardsEnum.TwoClub => DomainCoincheCardsEnum.TwoClub,
            DtoCoincheCardsEnum.ThreeClub => DomainCoincheCardsEnum.ThreeClub,
            DtoCoincheCardsEnum.FourClub => DomainCoincheCardsEnum.FourClub,
            DtoCoincheCardsEnum.FiveClub => DomainCoincheCardsEnum.FiveClub,
            DtoCoincheCardsEnum.SixClub => DomainCoincheCardsEnum.SixClub,
            DtoCoincheCardsEnum.SevenClub => DomainCoincheCardsEnum.SevenClub,
            DtoCoincheCardsEnum.EightClub => DomainCoincheCardsEnum.EightClub,
            DtoCoincheCardsEnum.NineClub => DomainCoincheCardsEnum.NineClub,
            DtoCoincheCardsEnum.TenClub => DomainCoincheCardsEnum.TenClub,
            DtoCoincheCardsEnum.JackClub => DomainCoincheCardsEnum.JackClub,
            DtoCoincheCardsEnum.QueenClub => DomainCoincheCardsEnum.QueenClub,
            DtoCoincheCardsEnum.KingClub => DomainCoincheCardsEnum.KingClub,

            DtoCoincheCardsEnum.AsDiamond => DomainCoincheCardsEnum.AsDiamond,
            DtoCoincheCardsEnum.TwoDiamond => DomainCoincheCardsEnum.TwoDiamond,
            DtoCoincheCardsEnum.ThreeDiamond => DomainCoincheCardsEnum.ThreeDiamond,
            DtoCoincheCardsEnum.FourDiamond => DomainCoincheCardsEnum.FourDiamond,
            DtoCoincheCardsEnum.FiveDiamond => DomainCoincheCardsEnum.FiveDiamond,
            DtoCoincheCardsEnum.SixDiamond => DomainCoincheCardsEnum.SixDiamond,
            DtoCoincheCardsEnum.SevenDiamond => DomainCoincheCardsEnum.SevenDiamond,
            DtoCoincheCardsEnum.EightDiamond => DomainCoincheCardsEnum.EightDiamond,
            DtoCoincheCardsEnum.NineDiamond => DomainCoincheCardsEnum.NineDiamond,
            DtoCoincheCardsEnum.TenDiamond => DomainCoincheCardsEnum.TenDiamond,
            DtoCoincheCardsEnum.JackDiamond => DomainCoincheCardsEnum.JackDiamond,
            DtoCoincheCardsEnum.QueenDiamond => DomainCoincheCardsEnum.QueenDiamond,
            DtoCoincheCardsEnum.KingDiamond => DomainCoincheCardsEnum.KingDiamond,

            DtoCoincheCardsEnum.AsHeart => DomainCoincheCardsEnum.AsHeart,
            DtoCoincheCardsEnum.TwoHeart => DomainCoincheCardsEnum.TwoHeart,
            DtoCoincheCardsEnum.ThreeHeart => DomainCoincheCardsEnum.ThreeHeart,
            DtoCoincheCardsEnum.FourHeart => DomainCoincheCardsEnum.FourHeart,
            DtoCoincheCardsEnum.FiveHeart => DomainCoincheCardsEnum.FiveHeart,
            DtoCoincheCardsEnum.SixHeart => DomainCoincheCardsEnum.SixHeart,
            DtoCoincheCardsEnum.SevenHeart => DomainCoincheCardsEnum.SevenHeart,
            DtoCoincheCardsEnum.EightHeart => DomainCoincheCardsEnum.EightHeart,
            DtoCoincheCardsEnum.NineHeart => DomainCoincheCardsEnum.NineHeart,
            DtoCoincheCardsEnum.TenHeart => DomainCoincheCardsEnum.TenHeart,
            DtoCoincheCardsEnum.JackHeart => DomainCoincheCardsEnum.JackHeart,
            DtoCoincheCardsEnum.QueenHeart => DomainCoincheCardsEnum.QueenHeart,
            DtoCoincheCardsEnum.KingHeart => DomainCoincheCardsEnum.KingHeart,

            DtoCoincheCardsEnum.AsSpade => DomainCoincheCardsEnum.AsSpade,
            DtoCoincheCardsEnum.TwoSpade => DomainCoincheCardsEnum.TwoSpade,
            DtoCoincheCardsEnum.ThreeSpade => DomainCoincheCardsEnum.ThreeSpade,
            DtoCoincheCardsEnum.FourSpade => DomainCoincheCardsEnum.FourSpade,
            DtoCoincheCardsEnum.FiveSpade => DomainCoincheCardsEnum.FiveSpade,
            DtoCoincheCardsEnum.SixSpade => DomainCoincheCardsEnum.SixSpade,
            DtoCoincheCardsEnum.SevenSpade => DomainCoincheCardsEnum.SevenSpade,
            DtoCoincheCardsEnum.EightSpade => DomainCoincheCardsEnum.EightSpade,
            DtoCoincheCardsEnum.NineSpade => DomainCoincheCardsEnum.NineSpade,
            DtoCoincheCardsEnum.TenSpade => DomainCoincheCardsEnum.TenSpade,
            DtoCoincheCardsEnum.JackSpade => DomainCoincheCardsEnum.JackSpade,
            DtoCoincheCardsEnum.QueenSpade => DomainCoincheCardsEnum.QueenSpade,
            DtoCoincheCardsEnum.KingSpade => DomainCoincheCardsEnum.KingSpade,

            _ => throw new System.InvalidCastException()
        };
    }
}
