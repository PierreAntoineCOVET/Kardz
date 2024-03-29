﻿namespace Domain.Enums
{
    /// <summary>
    /// List of classiq 52 cards.
    /// </summary>
    public enum CardsEnum
    {
        AsSpade = 0,
        TwoSpade = 1,
        ThreeSpade = 2,
        FourSpade = 3,
        FiveSpade = 4,
        SixSpade = 5,
        SevenSpade = 6,
        EightSpade = 7,
        NineSpade = 8,
        TenSpade = 9,
        JackSpade = 10,
        QueenSpade = 11,
        KingSpade = 12,

        AsClub = 13,
        TwoClub = 14,
        ThreeClub = 15,
        FourClub = 16,
        FiveClub = 17,
        SixClub = 18,
        SevenClub = 19,
        EightClub = 20,
        NineClub = 21,
        TenClub = 22,
        JackClub = 23,
        QueenClub = 24,
        KingClub = 25,

        AsDiamond = 26,
        TwoDiamond = 27,
        ThreeDiamond = 28,
        FourDiamond = 29,
        FiveDiamond = 30,
        SixDiamond = 31,
        SevenDiamond = 32,
        EightDiamond = 33,
        NineDiamond = 34,
        TenDiamond = 35,
        JackDiamond = 36,
        QueenDiamond = 37,
        KingDiamond = 38,

        AsHeart = 39,
        TwoHeart = 40,
        ThreeHeart = 41,
        FourHeart = 42,
        FiveHeart = 43,
        SixHeart = 44,
        SevenHeart = 45,
        EightHeart = 46,
        NineHeart = 47,
        TenHeart = 48,
        JackHeart = 49,
        QueenHeart = 50,
        KingHeart = 51,
    }

    /// <summary>
    /// List of cards colors as seen by the contract (include allTrump and noTrump).
    /// </summary>
    // Todo : add typewriterIgnore Attribute ?
    public enum CoincheCardColorsEnum
    {
        Heart,
        Spade,
        Diamond,
        Club,
        AllTrump,
        NoTrump
    }

    /// <summary>
    /// List of cards value.
    /// </summary>
    // Todo : add typewriterIgnore Attribute ?
    public enum CoincheCardValuesEnum
    {
        Seven,
        Eight,
        Nine,
        Jack,
        Queen,
        King,
        Ten,
        As
    }
}
