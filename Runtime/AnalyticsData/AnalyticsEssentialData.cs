using System.Collections.Generic;

namespace HelloDev.Analytics.Data
{
    public class AnalyticsEssentialData
    {
        public static readonly string PlayerNameKey = "essentialPlayerName";
        public static readonly string IsMultiplayerKey = "essentialIsMultiplayer";
        public static readonly string PlayerLevelKey = "essentialPlayerLevel";
        public static readonly string GameSceneKey = "essentialGameScene";
        
        public string PlayerName { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public bool IsMultiplayer { get; set; } = false;
        public int PlayerLevel { get; set; } = 1;
        public string GameScene { get; set; } = string.Empty;

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { PlayerNameKey, PlayerName },
                { IsMultiplayerKey, IsMultiplayer },
                { PlayerLevelKey, PlayerLevel },
                { GameSceneKey, GameScene }
            };
        }
    }
}