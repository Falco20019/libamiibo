/*
 * Copyright (C) 2018 Benjamin Krämer
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using LibAmiibo.Attributes;
using LibAmiibo.Helper;
using Newtonsoft.Json;

namespace LibAmiibo.Data.Settings.AppData.Games
{
    [AppID(0x10110E00)]
    [AppDataInitializationTitleID("00040000000EE000")]
    [AppDataInitializationTitleID("00040000000B8B00")]
    [AppDataInitializationTitleID("00040000000EDF00")]
    [AppDataInitializationTitleID("0005000010145000")]
    [AppDataInitializationTitleID("0005000010110E00")]
    [AppDataInitializationTitleID("0005000010144F00")]
    public class SmashBros : IGame
    {
        private ArraySegment<byte> AppData { get; set; }

        private static readonly ushort[] XpToLevel = new ushort[] {
            0x006,
            0x010,
            0x01D,
            0x02D,
            0x040,
            0x056,
            0x06F,
            0x08B,
            0x0AA,
            0x0CB,
            0x0EE,
            0x113,
            0x13A,
            0x162,
            0x18B,
            0x1B5,
            0x1E0,
            0x20C,
            0x239,
            0x267,
            0x296,
            0x2C6,
            0x2F7,
            0x329,
            0x35C,
            0x390,
            0x3C5,
            0x3FB,
            0x432,
            0x46A,
            0x4A3,
            0x4DD,
            0x518,
            0x554,
            0x591,
            0x5CF,
            0x60E,
            0x64E,
            0x68F,
            0x6D1,
            0x714,
            0x757,
            0x79B,
            0x7DF,
            0x824,
            0x869,
            0x8AF,
            0x8F6,
            0x93E
        };

        public enum BonusEffectValue : byte
        {
            Sprinter,
            Stroller,
            Glider,
            Antiglide,
            Leaper,
            Antileap,
            SpeedSkater,
            HiJump,
            LoJump,
            DoubleJumpBoost,
            DoubleJumpDrag,
            ThistleJump,
            AnchorJump,
            SpeedWalker,
            Meanderer,
            LingeringEdge,
            HastyEdge,
            GlueyEdge,
            ToughEdge,
            HardBraker,
            PerfectShieldHelper,
            ImperfectShield,
            ShieldRegenerator,
            ShieldDegenerator,
            AirDefender,
            AirPinata,
            NimbleDodger,
            DodgyDodger,
            SmoothLander,
            CrashLander,
            QuickSmasher,
            HyperSmasher,
            AirAttacker,
            AirPushover,
            MeteorMaster,
            DesperateAttacker,
            DesperateDefender,
            DesperateSpeedster,
            DesparateSpecialist,
            DesperateImmortal,
            UnharmedAttacker,
            UnharmedSpeedster,
            UnharmedSpeedDemon,
            TradeOffAttacker,
            TradeOffDefender,
            TradeOffSpeedster,
            AllAroundTradeOff,
            MoonLauncher,
            VampireLifesteal,
            NoFlinchSmasher,
            CriticalHitter,
            InsultToInjury,
            FirstStriker,
            Countdown,
            SpeedCrasher,
            ShieldExploder,
            ShieldHealer,
            ShieldReflector,
            EscapeArtist,
            ItemHurler,
            ItemLobber,
            ItemHitter,
            ItemPitcher,
            ItemShooter,
            QuickBatter,
            StarRod,
            LipsStick,
            SuperScope,
            RayGun,
            FireFlower,
            BeamSword,
            HomeRunBat,
            BobOmb,
            MrSaturn,
            FoodLover,
            PickyEater,
            CrouchHealer,
            CaloricAttacker,
            CaloricSpeedster,
            CaloricDefender,
            CaloricPowerhouse,
            KoHealer,
            CaloricImmortal,
            AutoHealer,
            SmashBallAttractor,
            Pity,
            SmashBallClinger,
            SuperFinalSmasher,
            FinalSmashHealer,
            DoubleFinalSmasher,
            SuddenDeathGambler,
            SafeRespawner,
            RiskyRespawner,
            NoEffect = 0xFF
        }

        [Flags]
        public enum FoodEffectValue : byte
        {
            [JsonIgnore]
            Undefined = 0b000,
            Bit1 = 0b001,
            Bit2 = 0b010
        }

        [Flags]
        public enum TrophyPositionValue : byte
        {
            [JsonIgnore]
            Undefined = 0b00000000,
            TrophySlot1 = 0b10000000,
            TrophySlot2 = 0b01000000,
            TrophySlot3 = 0b00100000,
            TrophySlot4 = 0b00010000,
            TrophySlot5 = 0b00001000,
            TrophySlot6 = 0b00000100,
            TrophySlot7 = 0b00000010,
            TrophySlot8 = 0b00000001,
        }

        public enum MiiHeadgearValue : byte
        {
            None,
            MarioHat,
            LuigiHat,
            PeachCrown,
            DaisyCrown,
            WarioCap,
            WaluigiCap,
            SuperMushroomHat,
            OneUpMushroomHat,
            RedShellHat,
            SpinyHat,
            ChompHat,
            ShyGuyMask,
            PrincessZeldaWig,
            SheikMask,
            SamusHelmet,
            MetaKnightMask,
            CaptainFalconHelmet,
            BarbaraTheBatWig,
            RegalCrown,
            PrinceCrown,
            PrincessCrown,
            MasterGardenerCrown,
            MiiForceHelmet,
            CatEars,
            DogEars,
            BearHat,
            PandaHat,
            ChickenHat,
            FrogHat,
            LionHat,
            CowSkullHat,
            RedRibbon,
            StrawHat,
            TopHat,
            Hibiscus,
            ChefHat,
            MiniTopHat,
            SwimmingCap,
            FootballHelmet,
            HockeyMask,
            WeddingVeil,
            BeehiveWig,
            LacyHeadband,
            NinjaHeadband,
            DevilHorns,
            MagicHat,
            FloralHat,
            CowboyHat,
            WesternHat,
            PirateHat,
            FancyPirateHat,
            SpartanHelmet,
            SantaHat,
            SamuraiHelmet,
            DragonHelmet,
            IsabelleHat,
            KKSliderHat,
            ProtoManHelmet,
            XHelmet,
            CatHat1,
            CatHat2,
            ZeroHelmet,
            MegaManExeHelmet,
            InklingWig1,
            InklingWig2,
            InklingSquidHat,
            HeihachiWig,
            LinkCap,
            DunbanWig,
            BearHat1,
            BearHat2,
            MajoraMask,
            AkiraWig,
            JackyWig,
            LloydWig,
            MonkeyHat1,
            MonkeyHat2,
            FlyingManHat,
            AshleyHat,
            HunterHelm1,
            HunterHelm2,
            RathalosHelm1,
            RathalosHelm2,
            KingKRoolHat,
            ChromWig,
            BlackKnightHelm,
            FoxHat,
            BionicHelmet1,
            KnucklesHat,
            ViridiWig,
            GilHelmet,
            TailHat,
            TakamaruWig,
            GenoHat,
            ChocoboHat,
            ToadHat,
            BionicHelmet2
        }

        public enum MiiOutfitValue : byte
        {
            StandardOutfitBrawlerMale,
            StandardOutfitBrawlerFemale,
            StandardOutfitSwordfighterMale,
            StandardOutfitSwordfighterFemale,
            StandardOutfitGunnerMale,
            StandardOutfitGunnerFemale,
            ProtectiveGearBrawlerMale,
            ProtectiveGearBrawlerFemale,
            FighterUniformBrawlerMale,
            FighterUniformBrawlerFemale,
            TracksuitBrawlerMale,
            TracksuitBrawlerFemale,
            BikerGearBrawlerMale,
            BikerGearBrawlerFemale,
            FancySuitGunnerMale,
            FancySuitGunnerFemale,
            MechaSuitBrawlerMale,
            MechaSuitBrawlerFemale,
            NinjaSuitSwordfighterMale,
            NinjaSuitSwordfighterFemale,
            SamuraiArmorSwordfighterMale,
            SamuraiArmorSwordfighterFemale,
            PlateArmorSwordfighterMale,
            PlateArmorSwordfighterFemale,
            CyberneticSuitSwordfighterMale,
            CyberneticSuitSwordfighterFemale,
            NeonSuitSwordfighterMale,
            NeonSuitSwordfighterFemale,
            VampireGarBrawlerBrawlerMale,
            VampireGarBrawlerBrawlerFemale,
            MageRobeGunnerMale,
            MageRobeGunnerFemale,
            SteampunkGetupGunnerMale,
            SteampunkGetupGunnerFemale,
            WildWestWearGunnerMale,
            WildWestWearGunnerFemale,
            PirateOutfitSwordfighterMale,
            PirateOutfitSwordfighterFemale,
            HighTechArmorGunnerMale,
            HighTechArmorGunnerFemale,
            DragonArmorGunnerMale,
            DragonArmorGunnerFemale,
            SSBTShirtBrawlerMale,
            SSBTShirtBrawlerFemale,
            SSBTShirtSwordfighterMale,
            SSBTShirtSwordfighterFemale,
            SSBTShirtGunnerMale,
            SSBTShirtGunnerFemale,
            IsabelleOutfitGunner,
            KKSliderOutfitGunnerMale,
            KKSliderOutfitGunnerFemale,
            ProtoManArmorGunnerMale,
            ProtoManArmorGunnerFemale,
            XArmorGunnerMale,
            XArmorGunnerFemale,
            CatSuitBrawlerMale,
            CatSuitBrawlerFemale,
            ZeroArmorSwordfighterMale,
            ZeroArmorSwordfighterFemale,
            MegaManExeArmorGunnerMale,
            MegaManExeArmorGunnerFemale,
            InklingOutfitGunnerMale,
            InklingOutfitGunnerFemale,
            HeihaciOutfitBrawlerMale,
            HeihaciOutfitBrawlerFemale,
            LinkOutfitSwordfighterMale,
            LinkOutfitSwordfighterFemale,
            DunbanOutfitSwordfighterMale,
            DunbanOutfitSwordfighterFemale,
            BearSuitGunnerMale,
            BearSuitGunnerFemale,
            AkiraOutfitBrawlerMale,
            AkiraOutfitBrawlerFemale,
            JackyOutfitBrawlerMale,
            JackyOutfitBrawlerFemale,
            LloydOutfitSwordfighterMale,
            LloydOutfitSwordfighterFemale,
            MonkeySuitSwordfighterMale,
            MonkeySuitSwordfighterFemale,
            SamusArmorGunnerMale,
            SamusArmorGunnerFemale,
            HoodieBrawlerMale,
            HoodieBrawlerFemale,
            HoodieSwordfighterMale,
            HoodieSwordfighterFemale,
            HoodieGunnerMale,
            HoodieGunnerFemale,
            FlyingManOutfitBrawlerMale,
            FlyingManOutfitBrawlerFemale,
            AshleyOutfitSwordfighter,
            HunterMailSwordfighterMale,
            HunterMailSwordfighterFemale,
            RathalosMailSwordfighterMale,
            RathalosMailSwordfighterFemale,
            KingKRoolOutfitBrawlerMale,
            KingKRoolOutfitBrawlerFemale,
            ChromOutfitSwordfighterMale,
            ChromOutfitSwordfighterFemale,
            BlackKnightArmorSwordfighterMale,
            BlackKnightArmorSwordfighterFemale,
            FoxOutfitGunnerMale,
            FoxOutfitGunnerFemale,
            BionicArmorBrawlerMale,
            BionicArmorBrawlerFemale,
            BuisnessSuitBrawlerMale,
            BuisnessSuitBrawlerFemale,
            BuisnessSuitSwordfighterMale,
            BuisnessSuitSwordfighterFemale,
            BuisnessSuitGunnerMale,
            BuisnessSuitGunnerFemale,
            KnucklesOutfitBrawlerMale,
            KnucklesOutfitBrawlerFemale,
            ViridiOutfitSwordfighter,
            GilArmorSwordfighterMale,
            GilArmorSwordfighterFemale,
            TailsOutfitGunnerMale,
            TailsOutfitGunnerFemale,
            CaptainFalconOutfitBrawlerMale,
            CaptainFalconOutfitBrawlerFemale,
            ToadOutfitBrawlerMale,
            ToadOutfitBrawlerFemale,
            TakamaruOutfitSwordfighterMale,
            TakamaruOutfitSwordfighterFemale,
            GenoOutfitGunnerMale,
            GenoOutfitGunnerFemale
        }

        [Flags]
        public enum OpponentsValue : ulong
        {
            MiiBrawler = 0x8000000000000000,
            MiiFighter = 0x4000000000000000,
            MiiGunner = 0x2000000000000000,
            Mario = 0x1000000000000000,
            DonkeyKong = 0x0800000000000000,
            Link = 0x0400000000000000,
            Samus = 0x0200000000000000,
            Yoshi = 0x0100000000000000,
            Kirby = 0x0080000000000000,
            Fox = 0x0040000000000000,
            Pikachu = 0x0020000000000000,
            Luigi = 0x0010000000000000,
            CaptainFalcon = 0x0008000000000000,
            Ness = 0x0004000000000000,
            Peach = 0x0002000000000000,
            Bowser = 0x0001000000000000,
            Zelda = 0x0000800000000000,
            Sheik = 0x0000400000000000,
            Marth = 0x0000200000000000,
            MrGameAndWatch = 0x0000100000000000,
            Ganondorf = 0x0000080000000000,
            Falco = 0x0000040000000000,
            Wario = 0x0000020000000000,
            MetaKnight = 0x0000010000000000,
            Pit = 0x0000008000000000,
            ZeroSuitSamus = 0x0000004000000000,
            Olimar = 0x0000002000000000,
            DiddyKong = 0x0000001000000000,
            KingDedede = 0x0000000800000000,
            Ike = 0x0000000400000000,
            Lucario = 0x0000000200000000,
            ROB = 0x0000000100000000,
            ToonLink = 0x0000000080000000,
            Charizard = 0x0000000040000000,
            Sonic = 0x0000000020000000,
            DrMario = 0x0000000010000000,
            Rosalina = 0x0000000008000000,
            WiiFitTrainer = 0x0000000004000000,
            LittleMac = 0x0000000002000000,
            Villager = 0x0000000001000000,
            Palutena = 0x0000000000800000,
            Robin = 0x0000000000400000,
            DuckHunt = 0x0000000000200000,
            BowserJr = 0x0000000000100000,
            Shulk = 0x0000000000080000,
            Jigglypuff = 0x0000000000040000,
            Lucina = 0x0000000000020000,
            DarkPit = 0x0000000000010000,
            Greninja = 0x0000000000008000,
            PacMan = 0x0000000000004000,
            MegaMan = 0x0000000000002000,
            Mewtwo = 0x0000000000001000,
            Ryu = 0x0000000000000800,
            Lucas = 0x0000000000000400,
            Roy = 0x0000000000000200,
            Cloud = 0x0000000000000100,
            Bayonetta = 0x0000000000000080,
            Corrin = 0x0000000000000040,
            [JsonIgnore] Reserved1 = 0x0000000000000020,
            [JsonIgnore] Reserved2 = 0x0000000000000010,
            [JsonIgnore] Reserved3 = 0x0000000000000008,
            [JsonIgnore] Reserved4 = 0x0000000000000004,
            [JsonIgnore] Reserved5 = 0x0000000000000002,
            [JsonIgnore] Reserved6 = 0x0000000000000001,
            [JsonIgnore] Undefined = 0x0000000000000000
        }

        public enum TrophyValue
        {
            Undefined = 0x000,
            Mario = 0x800,
            MarioAlternative,
            DonkeyKong,
            DonkeyKongAlternative,
            Link,
            LinkAlternative,
            Samus,
            SamusAlternative,
            Yoshi,
            YoshiAlternative,
            Kirby,
            KirbyAlternative,
            Fox,
            FoxAlternative,
            Pikachu,
            PikachuAlternative,
            Luigi,
            LuigiAlternative,
            CaptainFalcon,
            CaptainFalconAlternative,
            Ness,
            NessAlternative,
            Peach,
            PeachAlternative,
            Bowser,
            BowserAlternative,
            Zelda,
            ZeldaAlternative,
            Sheik,
            SheikAlternative,
            Marth,
            MarthAlternative,
            MrGameAndWatch,
            MrGameAndWatchAlternative,
            Ganondorf,
            GanondorfAlternative,
            Daphne,
            Falco,
            Nutski,
            FalcoAlternative,
            Skulltula,
            Wario,
            LikeLike,
            WarioAlternative,
            MetaKnight,
            MetaKnightAlternative,
            Pit,
            PitAlternative,
            ZeroSuitSamus,
            ZeroSuitSamusAlternative,
            Olimar,
            OlimarAlternative,
            DiddyKong,
            DiddyKongAlternative,
            KingDedede,
            KingDededeAlternative,
            Ike,
            IkeAlternative,
            Lucario,
            LucarioAlternative,
            ROB,
            ROBAlternative,
            ToonLink,
            ToonLinkAlternative,
            Charizard,
            CharizardAlternative,
            Sonic,
            SonicAlternative,
            Jigglypuff,
            JigglypuffAlternative,
            DrMario,
            DrMarioAlternative,
            Lucina,
            LucinaAlternative,
            RosalinaAndLuma,
            RosalinaAndLumaAlternative,
            WiiFitTrainer,
            WiiFitTrainerAlternative,
            LittleMac,
            LittleMacAlternative,
            Villager,
            VillagerAlternative,
            Palutena,
            PalutenaAlternative,
            Robin,
            RobinAlternative,
            DuckHunt,
            DuckHuntAlternative,
            BowserJr,
            BowserJrAlternative,
            Shulk,
            ShulkAlternative,
            Greninja,
            GreninjaAlternative,
            PacMan,
            PacManAlternative,
            MegaMan,
            MageManAlternative,
            DarkPit,
            DarkPitAlternative,
            RedPikmin,
            BluePikmin,
            YellowPikmin,
            PurplePikmin,
            WhitePikmin,
            RockPikmin,
            WingedPikmin,
            Larry,
            Morton,
            Wendy,
            Iggy,
            Roy1,
            Lemmy,
            Ludwig,
            Alph,
            MiiBrawler,
            MiiBrawlerAlternative,
            MiiSwordfighter,
            MiiSwordfighterAlternative,
            MiiGunner,
            MiiGunnerAlternative,
            Toad,
            PiranhaPlant,
            BuzzyBeetle,
            Paragoomba,
            CheepCheep,
            Blooper,
            DryBones,
            Wiggler,
            Thwomp,
            KingBoo,
            KingBobOmb,
            Pokey,
            BabyMario,
            BabyPeach,
            QuestionBlock,
            Pipe,
            OneUpMushroom,
            Ghosts,
            Luigi_Poltergust,
            ProfessorEGadd,
            Polterpup,
            PaperMario,
            PaperLuigi,
            PaperPeach,
            PaperBowser,
            PaperKersti,
            PaperWiggler,
            PaperGooperBlooper,
            PaperBowser_SecondForm,
            TanookiMarioAndKitsuneLuigi,
            BoomerangMario,
            Mario_WithPropellerBox,
            StatueMario,
            MarioAndStandardKart,
            PeachAndBirthdayGirl,
            YoshiAndEgg,
            BowserAndStandardKart,
            DonkeyKongAndBarrelTrain,
            LakituAndCloud,
            WarioAndBruiser,
            Peach_TennisOufit,
            Daisy_TennisOutfit,
            Mario_GoldBlock,
            Link_SpiritTracks,
            Zelda_SpiritTracks,
            ZeldasSpirit_SpiritTracks,
            Phantom,
            DemonTrain,
            Alfonzo,
            Byrne,
            Anjean,
            DemonKingMalladus,
            Stagnox,
            AdultLink_OcarinaOfTime,
            YoungZelda_OcarinaOfTime,
            AdultZelda_OcarinaOfTime,
            Ganondorf_OcarinaOfTime,
            Twinrova,
            Epona,
            Saria,
            Malon,
            Impa_OcarinaOfTime,
            Gorons,
            Zoras,
            Linebeck,
            Ciela,
            Medusa_QueenOfTheUnderworld,
            TwinbellowstheFerocious,
            Three_HeadedHewdraw,
            GreatReaper,
            Thanatos_GodOfDeath,
            GalacticFiendKraken,
            Hades_LordOfTheUnderworld,
            CragalanchetheMighty,
            ArlontheSerene,
            Pyrrhon,
            ChariotMaster,
            ChaosKin,
            PsuedoPaluteno,
            Mik,
            Specknose,
            Centurions,
            DarkPitStaff,
            PalutenaBow,
            GuardianOrbitars,
            UpperdashArm,
            ThreeSacredTreasures,
            FiendCauldron,
            ResetBomb,
            Pit_Eggplant,
            SpacePirateShip,
            Rover,
            Isabelle,
            Pete,
            Porter,
            TomNook,
            ReeseAndCyrus,
            DrShrunk,
            Redd,
            Leif,
            Snowpeople,
            Gulliver,
            DJKK,
            TimmyAndTommy,
            Resetti,
            AbleSisters,
            Kicks,
            Tails,
            DrEggman,
            Sliver,
            Knuckles,
            Jet,
            Amy,
            CreamAndCheese,
            Chao,
            Blaze,
            MetalSonic,
            Big,
            Rouge,
            Dunban,
            DixieKong,
            FunkyKong,
            CandyKong,
            Rambi,
            CrankyKong,
            Squitter,
            Expresso,
            KingKRool,
            Squawks,
            PeanutPopgun,
            RocketbarrelPack,
            DKBarrel,
            Pauline,
            TutorialPig,
            Kalimba,
            Mugly,
            CapnGreenbeard,
            Samus_VariaSuit,
            Samus_GravitySuit,
            PlasmaKirby,
            Wheelie,
            Bomber,
            Bugzzy,
            ComboCannon,
            Halberd,
            PeppyHare,
            SlippyToad,
            Krystal,
            ROB64,
            GeneralPepper,
            WolfODonnel,
            LeonPowalski,
            PantherCaroso,
            PigmaDengar,
            Arwing,
            GreatFox,
            Wolfen,
            AndrewOikonny,
            JamesMcCloud,
            Andross_TrueForm,
            Louie,
            Pellets,
            DrWily,
            Beat,
            X,
            MegaManVolnutt,
            MegaManExe,
            StarForceMegaMan,
            EnergyTank,
            Tree,
            Dancer,
            Warrior,
            Bridge,
            Gate,
            Jackknife,
            ArmAndLegLift,
            SuperHoop,
            Blinky,
            Inky,
            Pinky,
            Clyde,
            PowerPellet,
            BonusFruit,
            FireHydrant,
            GlassJoe,
            BaldBull,
            MrSandman,
            DrStewart,
            Pico,
            FalconFlyer,
            Lucas1,
            PorkyStatue,
            WarioBike,
            WildGunmen,
            Chrom,
            Tiki,
            Owain,
            Gaius,
            Cordelia,
            Lonqu,
            Lissa,
            Anna,
            Inigo,
            Validar,
            Lloid,
            Isabelle_WinterOutfit,
            Digby,
            CopperAndBooker,
            Luna,
            Blanca,
            Chip,
            Jack,
            Franklin,
            Nat,
            Zipper,
            Pave,
            Omega,
            Vector,
            Espio,
            Charmy,
            PokemonTrainer,
            Squirtle,
            Ivysaur,
            Mewtwo1,
            Pichu,
            Sylveon,
            Articuno,
            PorygonZ,
            MegaBlastoise,
            MegaVenusuar,
            Audino,
            Celebi,
            PokemonTrainers,
            ProfessorSycamore,
            RollingCrates,
            Crates,
            Barrels,
            Capsule,
            PartyBall,
            Sandbag,
            GoldenHammer,
            StarRod,
            Hammer,
            BeamSword,
            HomeRunBat,
            LipStick,
            SuperScope,
            FireFlower,
            RayGun,
            GooeyBomb,
            BananaPeel,
            Pitfall,
            SmartBomb,
            Hothead,
            Unira,
            Spring,
            TeamHealer,
            GreenShell,
            Freezie,
            MrSaturn,
            BobOmb,
            MotionSensorBomb,
            Bumper,
            Hammerhead,
            SuperspicyCurry,
            Lightning,
            Timer,
            SuperStar,
            SuperMushroom,
            PoisonMushroom,
            MetalBox,
            BunnyHood,
            FranklinBadge,
            ScrewAttack,
            Food,
            MaximTomato,
            HeartContainer,
            Dragoon,
            SmashBall,
            BlastBox,
            SoccerBall,
            WarpStar,
            SuperLeaf,
            POWBlock,
            Grass,
            SpinyShell,
            BulletBill,
            Boomerang,
            GustBellows,
            Beetle,
            Bombchu,
            FairyBottle,
            Cucco1,
            Beehive,
            Daybreak,
            OreClub,
            BackShield,
            XBomb,
            KillerEye,
            HocotateBomb,
            MasterBall,
            RocketBelt,
            SteelDiver,
            Drill,
            BossGalaga,
            DekuNuts,
            SpecialFlag,
            SmokeBall,
            FireBar,
            StatBoosts,
            Power,
            CustomPart,
            Gold_SmashRun,
            Gold_Classic,
            AssistTrophy,
            SamuraiGoroh,
            DrWright,
            Devil,
            Andross,
            Metroid,
            HammerBro,
            KnuckleJoe,
            SakiAmamiya,
            Starfy,
            Waluigi,
            Jeff,
            KatAndAna,
            Tingle,
            LakituAndSpinies,
            Nintendog,
            InfantryandTanks,
            Shadow,
            Lyn,
            IceClimbers,
            Excitebike,
            Barbara,
            Ghirahim,
            ElecMan,
            SkullKid,
            Midna,
            MotherBrain,
            Phosphora,
            Starman,
            Sheriff,
            DarkSamus,
            Ashley,
            DocLouis,
            Nightmare,
            Ghosts_PACMAN,
            ReDead,
            DrKawashima,
            Takamau,
            PrinceofSable,
            ColorTvGame,
            Riki,
            Magnus,
            Dillon,
            PokeBall,
            Meowth,
            Electrode,
            Goldeen,
            Staryu,
            Eevee,
            Snorlax,
            Togepi,
            Bellossom,
            Gardevoir,
            Metagross,
            Abomasnow,
            Snivy,
            Oshawott,
            Chespin,
            Fennekin,
            Fletchling,
            Spewpa,
            Gogoat,
            Swirlix,
            Inkay,
            Dedenne,
            Moltres,
            Mew,
            Entei,
            Suicune,
            Lugia,
            LatiasAndLatios,
            Kyogre,
            Deoxys,
            Palkia,
            Girantina,
            Darkai,
            Arceus,
            Victini,
            Zorark,
            Kyurem,
            Keldeo,
            Meloetta,
            Genesect,
            Xerneas,
            Goomba,
            Tac,
            KoopaTroopa_Green,
            KoopaTroopa_Red,
            KoopaParatroopa_Green,
            KoopaParatroopa_Red,
            Glunder,
            Glice,
            Glire,
            Peahat,
            Gordo,
            Kihunter,
            Monoeye,
            ShyGuy,
            BlueShyGuy,
            YellowShyGuy,
            GreenShyGuy,
            Mettaur,
            Shotzo,
            Geemer,
            TikiBuzz,
            Ghosts_FindMii,
            Gastly,
            PlasmaWisp,
            ParasolWaddleDee,
            Kritter,
            Octorok,
            SpikeTop,
            BumpetyBomb,
            Mites,
            WaddleDoo,
            Cryogonal,
            SkuttlerCannoneer,
            BrontoBurt,
            Koffing,
            Petilil,
            Pooka,
            Bytan,
            Lethinium,
            BanzaiBill,
            BigGoomba,
            FlameChomp,
            Reo,
            Mahva,
            Chandelure,
            Kamek,
            Bubble,
            Eggrobo,
            Zuree,
            Stalfos,
            Skuttler,
            Bacura,
            Roturret,
            Flage,
            Cucco2,
            Hitmonlee,
            BoomStomper,
            SkuttlerMage,
            Reaper,
            ChainChomp,
            Clubberskull,
            Bulborb,
            Lurchthorn,
            PolarBear,
            Darknut,
            Megonta,
            Orne,
            Bonkers,
            DevilCar,
            Gamyga,
            Poppant,
            FlyGuy,
            Souflee,
            Mimicutie,
            IridescentGlintBeetle,
            SneakySpirits,
            Generator,
            Spiny,
            BillBlaster,
            MasterHand,
            MasterCore,
            CrazyHand,
            DarkEmperor,
            FlyingMan,
            Boo,
            YellowDevil,
            Lakitu,
            FightingMiiTeam,
            EngineerLink,
            Virdi_GoddessOfNature,
            Tortimer,
            Kappn,
            Leilani,
            Leila,
            Grams,
            MiiApartments,
            PrismTower,
            Whimsicott,
            Reshiram,
            Zekrom,
            Magnemite,
            Zapdos,
            Emolga,
            Helioptile,
            Yveltal,
            TurnToBlue,
            PacMaze,
            GoldenFox,
            Wildgoose,
            FireStingray,
            MuteCity,
            BlueFalcon,
            Magicant,
            SkyRunner,
            DungeonMan,
            Fire,
            Chef,
            Lion,
            OilPanic,
            RainbowRoad,
            ShyGuyAndStandardKart,
            PSwitch,
            SpiritTrain,
            DarkTrain,
            ArmoredTrain,
            TortimerIsland,
            ShibaInu,
            Calico,
            ToyPoodle,
            GoldenRetriever,
            Beagle,
            ReaperGeneral,
            DiamondGolem,
            JackRussell,
            Golden,
            RushCoil,
            IrisArchwell,
            DemonKingArzodius,
            Wentworth,
            TheEmporer,
            MiiForceCaptain,
            GoldBone,
            MrMendelAndMsBlossom,
            Michaela,
            Nintendoji,
            Maya,
            KarateJoe,
            TheChorusKids,
            GalloBooneAndNomad,
            SakuraSamurai,
            Mallo,
            Goligan,
            Tempo,
            FossilFighters_Heroes,
            InjuiDarumeshi,
            ChibiRoboAndChibiTot,
            Athletes,
            Eddy,
            Guardian,
            KingRoy,
            SteelDivers,
            RustySlugger,
            Shaymin,
            Milotic,
            MarioGolfWorldTour,
            YoshisNewIsland,
            Mewtwo2,
            Mewtwo2Alternative,
            Inkling,
            Lucas2,
            Lucas2Alternative,
            Ryu,
            RyuAlternative,
            Roy2,
            Roy2Alternative,
            Ken,
            Cloud,
            CloudAlternative,
            Corrin,
            CorrinAlternative,
            Ryoma,
            Xander,
            Bayonetta,
            BayonettaAlternative,
            Jeanne,
            Rodin,
            Bayonetta_Original,
            Cereza
        }

        public uint RequiredVersion
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x00); }
            set { NtagHelpers.UInt32ToTag(AppData, 0x00, value); }
        }

        public ArraySegment<byte> OwnerTag
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0x04, 0x04); }
            set { OwnerTag.CopyFrom(value); }
        }

        public ushort Experience
        {
            get { return NtagHelpers.UInt16FromTag(AppData, 0x7C); }
            set { NtagHelpers.UInt16ToTag(AppData, 0x7C, value); }
        }

        public ushort RewardsCounter
        {
            get { return NtagHelpers.UInt16FromTag(AppData, 0x98); }
            set { NtagHelpers.UInt16ToTag(AppData, 0x98, value); }
        }

        public byte CustomPartsCount
        {
            get { return AppData.Array[AppData.Offset + 0x9A]; }
            set { AppData.Array[AppData.Offset + 0x9A] = value; }
        }

        public TrophyPositionValue TrophyPositions
        {
            get { return (TrophyPositionValue)AppData.Array[AppData.Offset + 0x9B]; }
            set { AppData.Array[AppData.Offset + 0x9B] = (byte)value; }
        }

        public uint RewardsTag
        {
            get { return NtagHelpers.UInt32FromTag(AppData, 0x9C); }
            set { NtagHelpers.UInt32ToTag(AppData, 0x9C, value); }
        }

        public ArraySegment<byte> AiSeeds
        {
            get { return new ArraySegment<byte>(AppData.Array, AppData.Offset + 0xA0, 0x2A); }
            set { AiSeeds.CopyFrom(value); }
        }

        private FoodEffectValue GetFoodEffectValue(int effectId)
        {
            var offset = AppData.Offset + 0x17;
            var shift = (3 - (effectId % 4)) * 2;
            var mask = (byte)(0b11 << shift);
            var value = AppData.Array[offset] & mask;
            return (FoodEffectValue)(value >> shift);
        }

        private void SetFoodEffectValue(int effectId, FoodEffectValue value)
        {
            var offset = AppData.Offset + 0x17;
            var shift = (3 - (effectId % 4)) * 2;
            var mask = (byte)(0b11 << shift);
            var shiftValue = (byte)((byte)value << shift);
            AppData.Array[offset] &= (byte)~mask;
            AppData.Array[offset] |= shiftValue;
        }

        private TrophyValue GetTrophy(int trophyId)
        {
            var isOdd = (trophyId % 2);
            var offset = 0x8C + (trophyId / 2) * 3 + isOdd;
            var shift = isOdd == 0 ? 4 : 0;
            var mask = 0xFFF << shift;
            var value = NtagHelpers.UInt16FromTag(AppData, offset) & mask;
            return (TrophyValue)(value >> shift);
        }

        private void SetTrophy(int trophyId, TrophyValue value)
        {
            var isOdd = (trophyId % 2);
            var offset = 0x8C + (trophyId / 2) * 3 + isOdd;
            var shift = isOdd == 0 ? 4 : 0;
            var mask = 0xFFF << shift;
            var shiftValue = (ushort)value << shift;
            var oldValue = NtagHelpers.UInt16FromTag(AppData, offset) & ~mask;
            NtagHelpers.UInt16ToTag(AppData, offset, (ushort)(oldValue | shiftValue));
        }

        private void SetTrophyPosition(TrophyPositionValue trophySlot, TrophyValue value)
        {
            if (value == TrophyValue.Undefined)
                TrophyPositions &= ~trophySlot;
            else
                TrophyPositions |= trophySlot;
        }
        
        public SmashBros(ArraySegment<byte> appData)
        {
            this.AppData = appData;
        }

        [SupportedGame(typeof(SmashBros))]
        public class Initializer : IAppDataInitializer
        {
            public void InitializeAppData(AmiiboTag tag)
            {
                this.ThrowOnInvalidAppId(tag);
                var game = new SmashBros(tag.AppData);
                game.AppData.CopyFrom(new byte[0xD8]); // TODO: Use for-loop and create extension method
                game.RequiredVersion = 0;
                //game.OwnerTag.CopyFrom(new byte[] { }); // TODO: Unknown value!
            }
        }

        #region Bonus Effects
        
        [Cheat(CheatAttribute.Type.DropDown, "Bonus Effects", "Slot 1")]
        public BonusEffectValue Effect1
        {
            get { return (BonusEffectValue)AppData.Array[AppData.Offset + 0x0D]; }
            set { AppData.Array[AppData.Offset + 0x0D] = (byte)value; }
        }
        
        
        [Cheat(CheatAttribute.Type.DropDown, "Bonus Effects", "Slot 2")]
        public BonusEffectValue Effect2
        {
            get { return (BonusEffectValue)AppData.Array[AppData.Offset + 0x0E]; }
            set { AppData.Array[AppData.Offset + 0x0E] = (byte)value; }
        }
        
        
        [Cheat(CheatAttribute.Type.DropDown, "Bonus Effects", "Slot 3")]
        public BonusEffectValue Effect3
        {
            get { return (BonusEffectValue)AppData.Array[AppData.Offset + 0x0F]; }
            set { AppData.Array[AppData.Offset + 0x0F] = (byte)value; }
        }

        #endregion

        #region Stats
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Stats", "Attack", Min = 0, Max = 200)]
        public ushort StatsAttack
        {
            get { return NtagHelpers.UInt16FromTag(AppData, 0x10); }
            set { NtagHelpers.UInt16ToTag(AppData, 0x10, value); }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Stats", "Defense", Min = 0, Max = 200)]
        public ushort StatsDefense
        {
            get { return NtagHelpers.UInt16FromTag(AppData, 0x12); }
            set { NtagHelpers.UInt16ToTag(AppData, 0x12, value); }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Stats", "Speed", Min = 0, Max = 200)]
        public ushort StatsSpeed
        {
            get { return NtagHelpers.UInt16FromTag(AppData, 0x14); }
            set { NtagHelpers.UInt16ToTag(AppData, 0x14, value); }
        }
        #endregion

        #region Special Moves
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Special Moves", "Neutral", Min = 0, Max = 3)]
        public byte SpecialNeutral
        {
            get { return AppData.Array[AppData.Offset + 0x09]; }
            set { AppData.Array[AppData.Offset + 0x09] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Special Moves", "Side", Min = 0, Max = 3)]
        public byte SpecialSide
        {
            get { return AppData.Array[AppData.Offset + 0x0A]; }
            set { AppData.Array[AppData.Offset + 0x0A] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Special Moves", "Up", Min = 0, Max = 3)]
        public byte SpecialUp
        {
            get { return AppData.Array[AppData.Offset + 0x0B]; }
            set { AppData.Array[AppData.Offset + 0x0B] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Special Moves", "Down", Min = 0, Max = 3)]
        public byte SpecialDown
        {
            get { return AppData.Array[AppData.Offset + 0x0C]; }
            set { AppData.Array[AppData.Offset + 0x0C] = value; }
        }

        #endregion

        #region Misc
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Misc", "Level", Min = 1, Max = 50)]
        public byte Level
        {
            get
            {
                var experience = Experience;
                byte level = 1;
                foreach (var xpThreshold in XpToLevel)
                {
                    if (experience < xpThreshold)
                        break;
                    level++;
                }
                return level;
            }
            set
            {
                if (value <= 1)
                {
                    Experience = 0;
                    return;
                }

                if (value > 50)
                    value = 50;

                Experience = XpToLevel[value - 2];
            }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Misc", "Appearance", Min = 0, Max = 7)]
        public byte Appearance
        {
            get { return AppData.Array[AppData.Offset + 0x08]; }
            set { AppData.Array[AppData.Offset + 0x08] = value; }
        }
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Misc", "AI IQ")]
        public ushort IQ
        {
            get { return NtagHelpers.UInt16FromTag(AppData, 0x7E); }
            set { NtagHelpers.UInt16ToTag(AppData, 0x7E, value); }
        }

        #endregion

        #region Trophies
        
        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 1")]
        public TrophyValue Trophy1
        {
            get { return GetTrophy(0); }
            set
            {
                SetTrophy(0, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot1, value);
            }
        }

        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 2")]
        public TrophyValue Trophy2
        {
            get { return GetTrophy(1); }
            set
            {
                SetTrophy(1, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot2, value);
            }
        }
        
        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 3")]
        public TrophyValue Trophy3
        {
            get { return GetTrophy(2); }
            set
            {
                SetTrophy(2, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot3, value);
            }
        }
        
        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 4")]
        public TrophyValue Trophy4
        {
            get { return GetTrophy(3); }
            set
            {
                SetTrophy(3, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot4, value);
            }
        }
        
        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 5")]
        public TrophyValue Trophy5
        {
            get { return GetTrophy(4); }
            set
            {
                SetTrophy(4, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot5, value);
            }
        }
        
        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 6")]
        public TrophyValue Trophy6
        {
            get { return GetTrophy(5); }
            set
            {
                SetTrophy(5, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot6, value);
            }
        }
        
        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 7")]
        public TrophyValue Trophy7
        {
            get { return GetTrophy(6); }
            set
            {
                SetTrophy(6, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot7, value);
            }
        }
        
        [Cheat(CheatAttribute.Type.DropDown, "Trophies", "Slot 8")]
        public TrophyValue Trophy8
        {
            get { return GetTrophy(7); }
            set
            {
                SetTrophy(7, value);
                SetTrophyPosition(TrophyPositionValue.TrophySlot8, value);
            }
        }

        #endregion

        #region Mii
        
        [Cheat(CheatAttribute.Type.DropDown, "Mii", "Headgear")]
        public MiiHeadgearValue MiiHeadgear
        {
            get { return (MiiHeadgearValue)AppData.Array[AppData.Offset + 0x19]; }
            set { AppData.Array[AppData.Offset + 0x19] = (byte)value; }
        }
        
        [Cheat(CheatAttribute.Type.DropDown, "Mii", "Outfit")]
        public MiiOutfitValue MiiOutfit
        {
            get { return (MiiOutfitValue)AppData.Array[AppData.Offset + 0x1B]; }
            set { AppData.Array[AppData.Offset + 0x1B] = (byte)value; }
        }

        #endregion

        #region Food
        
        [Cheat(CheatAttribute.Type.NumberSpinner, "Food", "Food eaten")]
        public byte FoodEaten
        {
            get { return AppData.Array[AppData.Offset + 0x16]; }
            set { AppData.Array[AppData.Offset + 0x16] = value; }
        }
        
        // TODO: Not fully understood what the 2 bits of FoodEffectValue mean...
        [Cheat(CheatAttribute.Type.MultiDropDown, "Food", "Effect 1")]
        public FoodEffectValue FoodEffect1
        {
            get { return GetFoodEffectValue(0); }
            set { SetFoodEffectValue(0, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Food", "Effect 2")]
        public FoodEffectValue FoodEffect2
        {
            get { return GetFoodEffectValue(1); }
            set { SetFoodEffectValue(1, value); }
        }
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Food", "Effect 3")]
        public FoodEffectValue FoodEffect3
        {
            get { return GetFoodEffectValue(2); }
            set { SetFoodEffectValue(2, value); }
        }

        #endregion

        #region Oponents Faced
        
        [Cheat(CheatAttribute.Type.MultiDropDown, "Oponents Faced", "Oponent faced")]
        public OpponentsValue OpponentsFaced
        {
            get { return (OpponentsValue)NtagHelpers.UInt64FromTag(AppData, 0x80); }
            set { NtagHelpers.UInt64ToTag(AppData, 0x80, (ulong)value); }
        }

        #endregion
    }
}
