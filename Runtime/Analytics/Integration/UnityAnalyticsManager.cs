using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using HelloDev.Logging;
using Sakumon.Analytics.Data;
using Sakumon.Locators;
using Scripts.Connection;
using Scripts.SaveManagement;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using Logger = HelloDev.Logging.Logger;

namespace Sakumon.Managers.Analytics
{
    public class UnityAnalyticsManager : AnalyticsManagerBase
    {
        [SerializeField] private AnalyticsLocatorSO _locator;

        #region Events

        public static Action ConsentShareData;
        public static Action ConsentRejectData;

        public static Action ShowConsentUI;
        public static Action OnConsentSharingData;

        #endregion

        private static UnityAnalyticsManager _instance;

        #region Life Cycle

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                _locator.Register(this, gameObject);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (_instance != this) return;
            _instance = null;
            _locator.Unregister(this);
            OnAnalyticsInitialized -= SaveEssentialData;
        }

        public void Start()
        {
            if (Initialized) return;
            AuthenticationHelper.OnSignInSuccess += InitializeAnalytics;
            OnAnalyticsInitialized += SaveEssentialData;
        }

        #endregion

        #region Private Methods

        private void SaveEssentialData()
        {
            EssentialData = new AnalyticsEssentialData
            {
                PlayerName = ConnectionManager.Instance.LocalPlayerId,
                Language = Application.systemLanguage.ToString(),
                IsMultiplayer = false,
                PlayerLevel = 1,
                GameScene = Scenes.MainMenu
            }.ToDictionary();

            var sb = new StringBuilder();
            if (EssentialData is { Count: > 0 })
            {
                sb.AppendLine("[Event Data]");
                foreach (var kvp in EssentialData)
                    sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
            }

            Logger.Log(LogIds.Analytics, $"Essential Data Saved: {sb.ToString().TrimEnd()}");
        }

        private async void InitializeAnalytics()
        {
            try
            {
                var ct = this.GetCancellationTokenOnDestroy();
                while (!SaveManager.Instance.LastLoadSucceeded)
                {
                    await UniTask.Yield(ct);
                }

                // Get the live save data
                SaveData save = await SaveManager.Instance.GetSaveCachedData();
                var flags = save.Flags; // local reference for reading only

                if (flags.AnalyticsConsentShowed)
                {
                    if (flags.AnalyticsConsent)
                    {
                        await Initialize(flags.AnalyticsConsent);
                        return;
                    }
                }

                Logger.Log(LogIds.Analytics, "Analytics Consent Not Showed. Showing Consent Screen");
                ConsentShareData += OnConsentShareData;
                ConsentRejectData += OnConsentRejectData;
                ShowConsentUI?.Invoke();
            }
            catch (Exception e)
            {
                Logger.LogError(LogIds.Analytics, $"Analytics Initialization Failed: {e.Message}");
            }
        }

        private async void OnConsentShareData()
        {
            try
            {
                SaveData save = await SaveManager.Instance.GetSaveCachedData();
                var flags = save.Flags;

                SaveConsentStatus(true);
                flags.SetAnalyticsConsentShowed(true);

                // Save immediately – this will persist the changes
                await SaveManager.Instance.SaveDataImmediately();

                Logger.Log(LogIds.Analytics, "Accepted Analytics Consent Data Sharing");

                // Initialize SDK
                await InitializeIfNot(true);

                // Unregister events
                ConsentShareData -= OnConsentShareData;
                ConsentRejectData -= OnConsentRejectData;
                OnConsentSharingData?.Invoke();
            }
            catch (Exception e)
            {
                Logger.LogError(LogIds.Analytics, $"Failed to accept analytics consent: {e.Message}");
            }
        }

        private async void OnConsentRejectData()
        {
            try
            {
                SaveData save = await SaveManager.Instance.GetSaveCachedData();
                var flags = save.Flags;

                SaveConsentStatus(false);
                flags.SetAnalyticsConsentShowed(true);
                await SaveManager.Instance.SaveDataImmediately();

                Logger.Log(LogIds.Analytics, "Rejected Analytics Consent Data Sharing");

                ConsentShareData -= OnConsentShareData;
                ConsentRejectData -= OnConsentRejectData;
            }
            catch (Exception e)
            {
                Logger.LogError(LogIds.Analytics, $"Failed to reject analytics consent: {e.Message}");
            }
        }

        #endregion

        #region SDK Initialization (unchanged)

        protected override async UniTask InitializeCore()
        {
            await UnityServices.InitializeAsync().AsUniTask();
        }

        public override bool CheckInitialized()
        {
            return UnityServices.State == ServicesInitializationState.Initialized;
        }

        protected override bool IsInitializing()
        {
            return UnityServices.State == ServicesInitializationState.Initializing;
        }

        #endregion

        #region SDK Events Hooks (unchanged)

        protected override void HookManagerEvents()
        {
            ConnectionManager.OnConnectionSignedOut += StopAnalytics;
        }

        protected override void UnHookManagerEvents()
        {
            ConnectionManager.OnConnectionSignedOut -= StopAnalytics;
        }

        #endregion

        #region SDK Consent Storage

        protected override void SaveConsentStatus(bool consent)
        {
            SaveManager.Instance.GetSaveData.Flags.SetAnalyticsConsent(consent);
        }

        protected override bool LoadConsentStatus()
        {
            return SaveManager.Instance.GetSaveData.Flags.AnalyticsConsent;
        }

        #endregion

        #region SDK Data Collection (unchanged)

        protected override void StartDataCollection(string playerId)
        {
            UnityServices.ExternalUserId = playerId;
            AnalyticsService.Instance.StartDataCollection();
        }

        protected override void StopDataCollection()
        {
            AnalyticsService.Instance.StopDataCollection();
        }

        protected override void RequestDataDeletion()
        {
            AnalyticsService.Instance.RequestDataDeletion();
        }

        #endregion

        #region SDK Record Event (unchanged)

        protected override void RecordEvent(string eventName, Dictionary<string, object> data)
        {
            var gameEvent = new CustomEvent(eventName);
            foreach (var kvp in data)
                gameEvent.Add(kvp.Key, kvp.Value);
            AnalyticsService.Instance.RecordEvent(gameEvent);
            AnalyticsService.Instance.Flush();
        }

        public void RecordEvent(AnalyticsCustomEvent customEvent)
        {
            foreach (var kvp in EssentialData) customEvent.Add(kvp.Key, kvp.Value);
            AnalyticsService.Instance.RecordEvent(customEvent);
            Logger.Log(LogIds.Analytics, FormatEventLog(customEvent.EventName, customEvent.InternalData));
            AnalyticsService.Instance.Flush();
        }

        #endregion
    }
}