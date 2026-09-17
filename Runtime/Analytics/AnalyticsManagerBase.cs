using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using HelloDev.Logging;
using UnityEngine;

namespace HelloDev.Analytics
{
    public abstract class AnalyticsManagerBase : MonoBehaviour, IAnalytics
    {
        protected const string AnalyticsLogSystem = "Analytics";

        #region Properties

        public string PlayerId { get; set; } = string.Empty;
        public Dictionary<string, object> EssentialData { get; set; } = new();

        #endregion

        #region Abstract SDK Implementation

        protected abstract UniTask InitializeCore();
        protected abstract bool IsInitializing();
        protected abstract void HookManagerEvents();
        protected abstract void UnHookManagerEvents();
        protected abstract void SaveConsentStatus(bool consent);
        protected abstract bool LoadConsentStatus();
        protected abstract void StartDataCollection(string playerId);
        protected abstract void StopDataCollection();
        protected abstract void RequestDataDeletion();
        protected abstract void RecordEvent(string eventName, Dictionary<string, object> data);

        #endregion

        #region Essential Data
        public void UpdateEssentialData(string key, object value)
        {
            EssentialData[key] = value;
            Logger.Log(AnalyticsLogSystem, $"Essential Data Updated\n  {key}: {value}");
        }
        
        #endregion
        
        #region Initialization Flow

        public Action OnAnalyticsInitialized { get; set; }
        public bool Initialized { get; set; }

        public async UniTask<bool> Initialize(bool consent)
        {
            try
            {
                await InitializeCore();
                bool initialized = CheckInitialized();
                if (initialized)
                {
                    Initialized = true;
                    SetPlayerConsentStatus(consent, PlayerId);
                    HookManagerEvents();
                    Logger.Log(AnalyticsLogSystem, FormatInitLog(consent));
                    OnAnalyticsInitialized?.Invoke();
                }
                return initialized;
            }
            catch (Exception e)
            {
                Logger.LogError(AnalyticsLogSystem, $"Analytics Initialization Failed: {e.Message}");
                throw;
            }
        }

        public abstract bool CheckInitialized();

        public async UniTask<bool> InitializeIfNot(bool consent)
        {
            if (CheckInitialized()) return true;
            if (IsInitializing())
            {
                await UniTask.WaitUntil(() => !IsInitializing());
                return CheckInitialized();
            }
            return await Initialize(consent);
        }

        public void StopAnalytics()
        {
            if (CheckInitialized()) StopDataCollection();
            UnHookManagerEvents();
            Initialized = false;
            Logger.Log(AnalyticsLogSystem, "Analytics Stopped");
        }

        public void DeletePlayerData()
        {
            if (!CheckInitialized()) return;
            RequestDataDeletion();
            SetPlayerConsentStatus(false, PlayerId);
            Logger.Log(AnalyticsLogSystem, "Player Data Deleted");
        }
        
        #endregion

        #region Player Consent Flow

        public void SetPlayerConsentStatus(bool consent, string playerId)
        {
            PlayerId = playerId;
            if (consent)
                StartDataCollection(playerId);
            else
                StopDataCollection();
            SaveConsentStatus(consent);
            Logger.Log(AnalyticsLogSystem, $"Player Consent Status: {consent}\nPlayer Id: {playerId}");
        }

        public bool GetPlayerConsentStatus() => LoadConsentStatus();

        #endregion

        #region Events Flow

        public void SendEvent(string eventName, Dictionary<string, object> data)
        {
            var mergedData = new Dictionary<string, object>();
            foreach (var kvp in EssentialData) mergedData[kvp.Key] = kvp.Value;
            foreach (var kvp in data) mergedData[kvp.Key] = kvp.Value;

            RecordEvent(eventName, mergedData);
            Logger.Log(AnalyticsLogSystem, FormatEventLog(eventName, data));
        }

        #endregion

        #region Logger Helpers

        private string FormatInitLog(bool consent)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Analytics Initialized.");
            sb.Append($"Player Consent: {consent}");
            return sb.ToString();
        }

        protected string FormatEventLog(string eventName, Dictionary<string, object> data)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Event: {eventName}");
            if (data is { Count: > 0 })
            {
                sb.AppendLine("[Event Data]");
                foreach (var kvp in data)
                    sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }
            return sb.ToString().TrimEnd();
        }

        #endregion
    }
}