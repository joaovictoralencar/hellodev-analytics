namespace HelloDev.Analytics.Data
{
    public class TutorialStartedEvent : AnalyticsCustomEvent
    {
        public TutorialStartedEvent() : base("tutorialStarted")
        {
        }
    }

    public class TutorialStepCompletedEvent : AnalyticsCustomEvent
    {
        public TutorialStepCompletedEvent() : base("tutorialStepCompleted")
        {
        }

        public int StepIndex
        {
            set
            {
                SetParameter("tutorialStepIndex", value);
                internalData["tutorialStepIndex"] = value;
            }
        }
        
        public string StepName
        {
            set
            {
                SetParameter("tutorialStepName", value);
                internalData["tutorialStepName"] = value;
            }
        }

        public float StageDurationSeconds
        {
            set
            {
                SetParameter("tutorialStepCompleteDuration", value);
                internalData["tutorialStepCompleteDuration"] = value;
            }
        }
    }

    public class TutorialCompletedEvent : AnalyticsCustomEvent
    {
        public TutorialCompletedEvent() : base("tutorialCompleted")
        {
        }

        public int TotalStages
        {
            set
            {
                SetParameter("totalStages", value);
                internalData["totalStages"] = value;
            }
        }

        public bool Skipped
        {
            set
            {
                SetParameter("skipped", value);
                internalData["skipped"] = value;
            }
        }
    }
}