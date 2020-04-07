using System;

namespace Clock.Events
{
    public class ClockEvent
    {
        /// <summary>
        /// Time of the event creation
        /// </summary>
        public readonly SavedTime TimeOfCreation;
        /// <summary>
        /// Time it should activate (all null parameters will be ignored)
        /// </summary>
        public SavedTime ActivationTime { get; protected set; }
        /// <summary>
        /// When is the event called
        /// </summary>
        public EventRepetitionType RepetitionType { get; protected set; }
        /// <summary>
        /// The event is used once by the event system
        /// </summary>
        public bool OnlyOnce { get; set; }
        /// <summary>
        /// Is currently injected on the event system
        /// </summary>
        public bool IsInjected { get; private set; }

        /// <summary>
        /// Create a new clock event
        /// </summary>
        /// <param name="activationTime">Time it should activate</param>
        /// <param name="repetitionType">When will the event try to activate</param>
        /// <param name="onlyOnce">Will it only activate only once</param>
        /// <param name="actions">Callbacks</param>
        public ClockEvent(SavedTime activationTime, 
        EventRepetitionType repetitionType, bool onlyOnce, params Action<SavedTime>[] actions)
        {
            TimeOfCreation = MainClock.NowTime;
            ActivationTime = activationTime;
            RepetitionType = repetitionType;
            OnlyOnce = onlyOnce;
            IsInjected = false;
            
            AddNewListeners(actions);
        }

        /// <summary>
        /// Used by the event handler to assign the containing action for eventual
        /// removal if necessary.
        /// Ignore, you don't need this :)
        /// </summary>
        /// <param name="containedClockAction">Contained action</param>
        public void Setup(ref Action<SavedTime> containedClockAction)
        {
            IsInjected = true;
            containedAction = containedClockAction;
        }

        /// <summary>
        /// Try to call the event
        /// </summary>
        /// <param name="currentTime">time to try to activate</param>
        public void TryActivation(SavedTime currentTime)
        {
            if (currentTime.MatchesWithShallow(ActivationTime))
            {
                if (OnlyOnce) Unsubscribe();

                OnEventTrigger(currentTime);
            }
        }

        /// <summary>
        /// Called when the Clock event can no longer be used
        /// </summary>
        private void Unsubscribe()
        {
            IsInjected = false;
            containedAction -= TryActivation;
        }

        /// <summary>
        /// Inject into event handler
        /// </summary>
        private void InjectToEventHandler()
        {
            EventHandler.AddTimeEvent(this);
        }

        /// <summary>
        /// When the event is triggered
        /// </summary>
        /// <param name="timeOfTrigger">Time of the trigger</param>
        protected void OnEventTrigger(SavedTime timeOfTrigger)
        {
            OnTriggered?.Invoke(timeOfTrigger);
        }

        /// <summary>
        /// Add additional listeners
        /// </summary>
        /// <param name="actions">Listeners to be called when the event
        /// is issued</param>
        public void AddNewListeners(params Action<SavedTime>[] actions)
        {
            for(int i = 0; i < actions.Length; i++)
                OnTriggered += actions[i];
        }

        /// <summary>
        /// Force use the event with the current time
        /// </summary>
        public void ForceUse()
        {
            OnTriggered?.Invoke(MainClock.NowTime);
        }

        /// <summary>
        /// Force use the event with the given time
        /// </summary>
        public void ForceUse(SavedTime time)
        {
            OnTriggered?.Invoke(time);
        }

        /// <summary>
        /// Call this to unsubscribe the clock event
        /// </summary>
        public void UnsubscribeFromClockEvents()
        {
            if (!IsInjected) return;

            Unsubscribe();
        }

        /// <summary>
        /// Call this to make the clock events be heard
        /// </summary>
        public void InjectClockEvent()
        {            
            InjectToEventHandler();
        }

        private Action<SavedTime> containedAction;
        private Action<SavedTime> OnTriggered;
    }
}