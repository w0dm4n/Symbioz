using System;
namespace Symbioz.Enums
{
	public enum EffectsEnum : short
	{
		Eff_Teleport = 4,
		Eff_PushBack,
		Eff_PullForward,
		Eff_Divorce,
		Eff_SwitchPosition,
		Eff_Dodge,
		Eff_10,
		Eff_13 = 13,
		Eff_34 = 34,
		Eff_CarryEntity = 50,
		Eff_ThrowEntity,
		Eff_StealMP_77 = 77,
		Eff_AddMP,
		Eff_79,
		Eff_HealHP_81 = 81,
		Eff_StealHPFix,
		Eff_StealAP_84 = 84,
		Eff_DamagePercentWater,
		Eff_DamagePercentEarth,
		Eff_DamagePercentAir,
		Eff_DamagePercentFire,
		Eff_DamagePercentNeutral,
		Eff_GiveHPPercent,
		Eff_StealHPWater,
		Eff_StealHPEarth,
		Eff_StealHPAir,
		Eff_StealHPFire,
		Eff_StealHPNeutral,
		Eff_DamageWater,
		Eff_DamageEarth,
		Eff_DamageAir,
		Eff_DamageFire,
		Eff_DamageNeutral,
		Eff_RemoveAP,
		Eff_AddGlobalDamageReduction_105 = 105,
		Eff_ReflectSpell,
		Eff_AddDamageReflection,
		Eff_HealHP_108,
		Eff_109,
		Eff_AddHealth,
		Eff_AddAP_111,
		Eff_AddDamageBonus,
		Eff_DoubleDamageOrRestoreHP,
		Eff_AddDamageMultiplicator,
		Eff_AddCriticalHit,
		Eff_SubRange,
		Eff_AddRange,
		Eff_AddStrength,
		Eff_AddAgility,
		Eff_RegainAP,
		Eff_AddDamageBonus_121,
		Eff_AddCriticalMiss,
		Eff_AddChance,
		Eff_AddWisdom,
		Eff_AddVitality,
		Eff_AddIntelligence,
		Eff_LostMP,
		Eff_AddMP_128,
		Eff_StealKamas = 130,
		Eff_LoseHPByUsingAP,
		Eff_DispelMagicEffs,
		Eff_LosingAP,
		Eff_LosingMP,
		Eff_SubRange_135,
		Eff_AddRange_136,
		Eff_AddPhysicalDamage_137,
		Eff_IncreaseDamage_138,
		Eff_RestoreEnergyPoints,
		Eff_SkipTurn,
		Eff_Kill,
		Eff_AddPhysicalDamage_142,
		Eff_HealHP_143,
		Eff_DamageFix,
		Eff_SubDamageBonus,
		Eff_ChangesWords,
		Eff_ReviveAlly,
		Eff_Followed,
		Eff_ChangeAppearance,
		Eff_Invisibility,
		Eff_SubChance = 152,
		Eff_SubVitality,
		Eff_SubAgility,
		Eff_SubIntelligence,
		Eff_SubWisdom,
		Eff_SubStrength,
		Eff_IncreaseWeight,
		Eff_DecreaseWeight,
		Eff_IncreaseAPAvoid,
		Eff_IncreaseMPAvoid,
		Eff_SubDodgeAPProbability,
		Eff_SubDodgeMPProbability,
		Eff_AddGlobalDamageReduction,
		Eff_AddDamageBonusPercent,
		Eff_166,
		Eff_SubAP = 168,
		Eff_SubMP,
		Eff_SubCriticalHit = 171,
		Eff_SubMagicDamageReduction,
		Eff_SubPhysicalDamageReduction,
		Eff_AddInitiative,
		Eff_SubInitiative,
		Eff_AddProspecting,
		Eff_SubProspecting,
		Eff_AddHealBonus,
		Eff_SubHealBonus,
		Eff_Double,
		Eff_Summon,
		Eff_AddSummonLimit,
		Eff_AddMagicDamageReduction,
		Eff_AddPhysicalDamageReduction,
		Eff_185,
		Eff_SubDamageBonusPercent,
		Eff_188 = 188,
        Eff_AddRessources = 193,
		Eff_GiveKamas = 194,
		Eff_197 = 197,
		Eff_201 = 201,
		Eff_RevealsInvisible,
		Eff_206 = 206,
		Eff_AddEarthResistPercent = 210,
		Eff_AddWaterResistPercent,
		Eff_AddAirResistPercent,
		Eff_AddFireResistPercent,
		Eff_AddNeutralResistPercent,
		Eff_SubEarthResistPercent,
		Eff_SubWaterResistPercent,
		Eff_SubAirResistPercent,
		Eff_SubFireResistPercent,
		Eff_SubNeutralResistPercent,
		Eff_220,
		Eff_221,
		Eff_222,
		Eff_AddTrapBonus = 225,
		Eff_AddTrapBonusPercent,
		Eff_229 = 229,
		Eff_230,
		Eff_239 = 239,
		Eff_AddEarthElementReduction,
		Eff_AddWaterElementReduction,
		Eff_AddAirElementReduction,
		Eff_AddFireElementReduction,
		Eff_AddNeutralElementReduction,
		Eff_SubEarthElementReduction,
		Eff_SubWaterElementReduction,
		Eff_SubAirElementReduction,
		Eff_SubFireElementReduction,
		Eff_SubNeutralElementReduction,
		Eff_AddPvpEarthResistPercent,
		Eff_AddPvpWaterResistPercent,
		Eff_AddPvpAirResistPercent,
		Eff_AddPvpFireResistPercent,
		Eff_AddPvpNeutralResistPercent,
		Eff_SubPvpEarthResistPercent,
		Eff_SubPvpWaterResistPercent,
		Eff_SubPvpAirResistPercent,
		Eff_SubPvpFireResistPercent,
		Eff_SubPvpNeutralResistPercent,
		Eff_AddPvpEarthElementReduction,
		Eff_AddPvpWaterElementReduction,
		Eff_AddPvpAirElementReduction,
		Eff_AddPvpFireElementReduction,
		Eff_AddPvpNeutralElementReduction,
		Eff_AddArmorDamageReduction,
		Eff_StealChance,
		Eff_StealVitality,
		Eff_StealAgility,
		Eff_StealIntelligence,
		Eff_StealWisdom,
		Eff_StealStrength,
		Eff_275 = 275,
		Eff_276,
		Eff_277,
		Eff_278,
		Eff_279,
		Eff_281 = 281,
		Eff_282,
		Eff_283,
		Eff_284,
		Eff_285,
		Eff_286,
		Eff_287,
		Eff_288,
		Eff_289,
		Eff_290,
		Eff_291,
		Eff_292,
		Eff_SpellBoost,
		Eff_294,
		Eff_310 = 310,
		Eff_StealRange = 320,
		Eff_333 = 333,
		Eff_ChangeAppearance_335 = 335,
		Eff_Trap = 400,
		Eff_Glyph,
		Eff_Glyph_402,
		Eff_405 = 405,
		Eff_406,
		Eff_407,
		Eff_410 = 410,
		Eff_411,
		Eff_412,
		Eff_413,
		Eff_AddPushDamageBonus = 414,
		Eff_SubPushDamageBonus = 415,
		Eff_AddPushDamageReduction = 416,
		Eff_SubPushDamageReduction= 417,
		Eff_AddCriticalDamageBonus= 418,
		Eff_SubCriticalDamageBonus = 419,
		Eff_AddCriticalDamageReduction = 420,
		Eff_SubCriticalDamageReduction = 421,
		Eff_AddEarthDamageBonus = 422,
		Eff_SubEarthDamageBonus = 423,
		Eff_AddFireDamageBonus = 424,
		Eff_SubFireDamageBonus = 425,
		Eff_AddWaterDamageBonus  = 426,
		Eff_SubWaterDamageBonus,
		Eff_AddAirDamageBonus = 428,
		Eff_SubAirDamageBonus,
		Eff_AddNeutralDamageBonus,
		Eff_SubNeutralDamageBonus,
		Eff_StealAP_440 = 440,
		Eff_StealMP_441,
		Eff_513 = 513,
		Eff_Teleport_SavePoint = 600,
		Eff_601,
		Eff_602,
		Eff_603,
		Eff_LearnSpell,
		Eff_605,
		Eff_AddPermanentWisdom,
		Eff_AddPermanentStrength,
		Eff_AddPermanentChance,
		Eff_AddPermanentAgility,
		Eff_AddPermanentVitality,
		Eff_AddPermanentIntelligence,
		Eff_612,
		Eff_AddSpellPoints,
		Eff_614,
		Eff_615,
		Eff_616,
		Eff_620 = 620,
		Eff_621,
		Eff_622,
		Eff_623,
		Eff_624,
		Eff_625,
		Eff_626,
		Eff_627,
		Eff_628,
		Eff_631 = 631,
		Eff_640 = 640,
		Eff_641,
		Eff_642,
		Eff_643,
		Eff_645 = 645,
		Eff_646,
		Eff_647,
		Eff_648,
		Eff_649,
		Eff_654 = 654,
		Eff_666 = 666,
		Eff_669 = 669,
		Eff_670,
		Eff_671,
		Eff_Punishment_Damage,
		Eff_699 = 699,
		Eff_700,
		Eff_701,
		Eff_702,
		Eff_705 = 705,
		Eff_706,
		Eff_707,
		Eff_710 = 710,
		Eff_715 = 715,
		Eff_716,
		Eff_717,
		Eff_720 = 720,
		Eff_AddTitle = 724,
		Eff_725,
		Eff_730 = 730,
		Eff_731,
		Eff_732,
		Eff_740 = 740,
		Eff_741,
		Eff_742,
		Eff_750 = 750,
		Eff_751,
		Eff_AddDodge,
		Eff_AddLock,
		Eff_SubDodge,
		Eff_SubLock,
		Eff_760 = 760,
		Eff_765 = 765,
		Eff_770 = 770,
		Eff_771,
		Eff_772,
		Eff_773,
		Eff_774,
		Eff_775,
		Eff_AddErosion,
		Eff_780 = 780,
		Eff_781,
		Eff_782,
		Eff_RepelsTo,
		Eff_Rollback,
		Eff_785,
		Eff_786,
		Eff_787,
		Eff_Punishment,
		Eff_789,
		Eff_790,
		Eff_791,
		Eff_792,
		Eff_Rewind,
		Eff_795 = 795,
		Eff_800 = 800,
		Eff_805 = 805,
		Eff_806,
		Eff_807,
		Eff_LastMeal,
		Eff_810 = 810,
		Eff_RemainingFights,
		Eff_812,
		Eff_813,
		Eff_814,
		Eff_815,
		Eff_816,
		Eff_825 = 825,
		Eff_905 = 905,
		Eff_930 = 930,
		Eff_931,
		Eff_932,
		Eff_933,
		Eff_934,
		Eff_935,
		Eff_936,
		Eff_937,
		Eff_939 = 939,
		Eff_940,
		Eff_946 = 946,
		Eff_947,
		Eff_948,
		Eff_949,
		Eff_AddState,
        Eff_RemoveState,
		Eff_952,
		Eff_960 = 960,
		Eff_961,
		Eff_962,
		Eff_963,
		Eff_964,
		Eff_LivingObjectId = 970,
		Eff_LivingObjectMood,
		Eff_LivingObjectSkin,
		Eff_LivingObjectCategory,
		Eff_LivingObjectLevel,
		Eff_NonExchangeable_981 = 981,
		Eff_NonExchangeable_982,
		Eff_983,
		Eff_984,
		Eff_985,
		Eff_986,
		Eff_987,
		Eff_988,
		Eff_989,
		Eff_990,
		Eff_994 = 994,
		Eff_995,
		Eff_996,
		Eff_997,
		Eff_998,
		Eff_999,
		Eff_1002 = 1002,
		Eff_1003,
		Eff_1004,
		Eff_1005,
		Eff_1006,
		Eff_1007,
        Eff_SpawnBomb,
		Eff_1009,
		Eff_1010,
		Eff_1011,
		Eff_1012,
		Eff_1013,
		Eff_1014,
		Eff_1015,
		Eff_1016,
		Eff_1017,
		Eff_1018,
		Eff_1019,
		Eff_1021 = 1021,
		Eff_1022,
		Eff_1023,
		Eff_1024,
		Eff_1025,
		Eff_ActivateGlyph,
		Eff_1027,
		Eff_1028,
		Eff_1029,
		Eff_1030,
		Eff_1031,
		Eff_1032,
		Eff_1033,
		Eff_1034,
		Eff_1035,
		Eff_1036,
		Eff_1037,
		Eff_1038,
        Eff_AddShieldPercent,
        Eff_AddShield,
		Eff_1041,
        Eff_BePulled,
		Eff_1043,
		Eff_1044,
		Eff_1045,
		Eff_1046,
		Eff_1047,
		Eff_1048,
		Eff_1049,
		Eff_1050,
		Eff_1051,
		Eff_1052,
		Eff_1053,
		Eff_IncreaseDamage_1054,
		Eff_1055,
		Eff_1057 = 1057,
		Eff_1058,
		Eff_1059,
		Eff_1060,
		Eff_DamageSharing,
		Eff_1062,
		Eff_1063,
		Eff_1064,
		Eff_1065,
		Eff_1066,
		Eff_1067,
		Eff_1068,
		Eff_1069,
		Eff_1070,
		Eff_1071,
		Eff_1072,
		Eff_1073,
		Eff_1074,
		Eff_ReduceEffsDuration,
		Eff_AddResistances,
		Eff_SubResistances,
		Eff_AddVitalityPercent,
		Eff_1079,
		Eff_SubMP_1080,
		Eff_1081,
		Eff_1082,
		Eff_1083,
		Eff_1084,
		Eff_1085,
		Eff_1086,
		Eff_1087,
		Eff_1091 = 1091,
		Eff_1092,
		Eff_1093,
		Eff_1094,
		Eff_1095,
		Eff_1096,
		Eff_1097,
		Eff_1098,
		Eff_1099,
		Eff_1100,
		Eff_1101,
		Eff_1102,
		Eff_PushBack_1103,
		Eff_1104,
		Eff_1105,
		Eff_1106,
		Eff_1107,
		Eff_1108,
		Eff_RestoreHPPercent,
		Eff_1111 = 1111,
		Eff_1118 = 1118,
		Eff_1119,
		Eff_1120,
		Eff_1121,
		Eff_1122,
		Eff_1123,
		Eff_1124,
		Eff_1125,
		Eff_1126,
		Eff_1127,
		Eff_1128,
		Eff_1129,
		Eff_DamageAirPerAP = 1131,
		Eff_DamageWaterPerAP,
		Eff_DamageFirePerAP,
		Eff_DamageNeutralPerAP,
		Eff_DamageEarthPerAP,
		Eff_DamageAirPerMP,
		Eff_DamageWaterPerMP,
		Eff_DamageFirePerMP,
		Eff_DamageNeutralPerMP,
		Eff_DamageEarthPerMP,
		Eff_1141,
		Eff_1142,
        Eff_AddWeaponDamagePercent = 1144,
        Eff_Mimicry = 1151,
        Eff_Summon_TaxCollector = 1153,
        Eff_AddStateTelefrag = 1160,
        Eff_Companion = 1161,
        Eff_MultiplyTakenDamages = 1163,
        Eff_SwapDamages = 1164,
        Eff_Portal = 1181,

		End
	}
}
