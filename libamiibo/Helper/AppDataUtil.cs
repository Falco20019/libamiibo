using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibAmiibo.Attributes;
using LibAmiibo.Data.Settings.AppData;
using LibAmiibo.Data.Settings.AppData.Games;

namespace LibAmiibo.Helper
{
    public static class AppDataUtil
    {
        public static IEnumerable<Title> GetInitializationTitleIDs(Type type)
        {
            return type?.GetCustomAttributes<AppDataInitializationTitleIDAttribute>(true).Select(t => t.TitleID);
        }

        public static IEnumerable<Title> GetInitializationTitleIDs<T>() where T: IGame
        {
            return GetInitializationTitleIDs(typeof(T));
        }

        public static IEnumerable<(PropertyInfo Property, CheatAttribute Cheat)> GetCheats(Type type)
        {
            if (type == null)
                yield break;

            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var cheat = property.GetCustomAttribute<CheatAttribute>(false);
                if (cheat != null)
                    yield return (property, cheat);
            }
        }

        public static Type GetGameTypeForAmiiboAppID(uint appID)
        {
            return (from type in Assembly.GetExecutingAssembly().GetTypes()
                    let typeAppId = GetAppID(type)
                    where typeAppId != null
                    where typeAppId == appID
                    select type
                ).FirstOrDefault<Type>();
        }

        public static uint? GetAppID<T>() where T: IGame
        {
            return AppDataUtil.GetAppID(typeof(T));
        }

        public static IGame GetGameForAmiiboAppData(AmiiboAppData appData)
        {
            if (appData == null)
                return null;

            var gameType = GetGameTypeForAmiiboAppID(appData.AppID);
            if (gameType == null)
                return null;

            return Activator.CreateInstance(gameType, appData.AppData) as IGame;
        }

        public static uint? GetAppID(Type type)
        {
            return type.GetCustomAttribute<AppIDAttribute>(false)?.AppID;
        }

        public static Type GetSupportedGameType(Type type)
        {
            return type?.GetCustomAttribute<SupportedGameAttribute>(false).SupportedGameType;
        }
    }
}
