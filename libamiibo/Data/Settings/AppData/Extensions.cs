using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibAmiibo.Attributes;
using LibAmiibo.Data.Settings.AppData.Games;
using LibAmiibo.Helper;

namespace LibAmiibo.Data.Settings.AppData
{
    public static class Extensions
    {
        public static IEnumerable<Title> GetInitializationTitleIDs(this IAppDataInitializer initializer)
        {
            return AppDataUtil.GetInitializationTitleIDs(initializer?.GetSupportedGameType());
        }
        public static IEnumerable<Title> GetInitializationTitleIDs(this IGame game)
        {
            return AppDataUtil.GetInitializationTitleIDs(game?.GetType());
        }

        public static IEnumerable<(PropertyInfo Property, CheatAttribute Cheat)> GetCheats(this IGame game)
        {
            return AppDataUtil.GetCheats(game?.GetType());
        }

        public static uint? GetAppID(this IAppDataInitializer initializer)
        {
            return AppDataUtil.GetAppID(initializer?.GetSupportedGameType());
        }
        public static uint? GetAppID(this IGame game)
        {
            return AppDataUtil.GetAppID(game?.GetType());
        }

        public static Type GetSupportedGameType(this IAppDataInitializer initializer)
        {
            return AppDataUtil.GetSupportedGameType(initializer?.GetType());
        }

        internal static void ThrowOnInvalidAppId(this IAppDataInitializer game, AmiiboTag tag)
        {
            if (tag == null || !tag.HasAppData || tag.AmiiboSettings.AmiiboAppData.AppID != game.GetAppID())
                throw new InvalidOperationException("The provided tag has not the correct app data. Maybe initialize it to this game before.");
        }
    }
}
