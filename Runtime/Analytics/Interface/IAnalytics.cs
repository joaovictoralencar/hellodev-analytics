using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Sakumon.Managers.Analytics
{
    public interface IAnalytics
    {
        #region Initialization

        public bool Initialized { get; set; }
        UniTask<bool> Initialize(bool consent);
        bool CheckInitialized();
        UniTask<bool> InitializeIfNot(bool consent);
        void StopAnalytics();
        void DeletePlayerData();
        void UpdateEssentialData(string key, object value);
        public System.Action OnAnalyticsInitialized { get; set; }
        
        #endregion

        #region Player

        string PlayerId { get; set; }
        void SetPlayerConsentStatus(bool consent, string playerId);
        bool GetPlayerConsentStatus();
        Dictionary<string, object> EssentialData { get; set; }

        #endregion

        #region Events

        void SendEvent(string eventName, Dictionary<string, object> data);
        #endregion
    }
}