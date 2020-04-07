using System;
using System.Collections;
using System.Collections.Generic;
using Clock;
using UnityEngine;

namespace Clock.Events
{
    public class EventHandler : MonoBehaviour
    {
        private static EventHandler Instance { get; set; }

        private const string OBJECT_NAME = "Clock Event Handler";
        private SavedTime lastUpdateTime;

        private static void Init()
        {
            CreateSingletonObject();
        }

        public static void CreateSingletonObject()
        {
            GameObject handlerObj = new GameObject(OBJECT_NAME);

            Instance = handlerObj.AddComponent<EventHandler>();

            DontDestroyOnLoad(handlerObj);
        }

        private void Awake()
        {
            lastUpdateTime = MainClock.NowTime;
            UpdateAction = DoTimeEventsUpdate;
        }

        private void Update()
        {
            UpdateAction?.Invoke();
        }

        private void DoTimeEventsUpdate()
        {
            SavedTime thisUpdateTime = MainClock.NowTime;

            if (thisUpdateTime.Seconds != lastUpdateTime.Seconds)
            {
                OnSecondChange?.Invoke(thisUpdateTime);

                if (thisUpdateTime.Minutes != lastUpdateTime.Minutes)
                {
                    OnMinuteChange?.Invoke(thisUpdateTime);

                    if (thisUpdateTime.Hours24 != lastUpdateTime.Hours24)
                    {
                        if (thisUpdateTime.DayPhase != lastUpdateTime.DayPhase)
                            OnDayPhaseChange?.Invoke(thisUpdateTime);

                        OnHourChange?.Invoke(thisUpdateTime);

                        if (thisUpdateTime.Day != lastUpdateTime.Day)
                        {
                            OnDayChange?.Invoke(thisUpdateTime);

                            if (thisUpdateTime.Month != lastUpdateTime.Month)
                            {
                                OnMonthChange?.Invoke(thisUpdateTime);

                                if (thisUpdateTime.Year != lastUpdateTime.Year)
                                    OnYearChange?.Invoke(thisUpdateTime);
                            }
                        }
                    }
                }
            }

            lastUpdateTime = thisUpdateTime;
        }

        private void AddTimeEvent(EventRepetitionType repetitionType, ClockEvent clockEvent, params Action<SavedTime>[] actions)
        {
            switch (repetitionType)
            {
                case EventRepetitionType.Every_Second:
                    AddTo(ref OnSecondChange);
                    break;
                case EventRepetitionType.Every_Minute:
                    AddTo(ref OnMinuteChange);
                    break;
                case EventRepetitionType.Every_Hour:
                    AddTo(ref OnHourChange);
                    break;
                case EventRepetitionType.Every_Day:
                    AddTo(ref OnDayChange);
                    break;
                case EventRepetitionType.Every_DayPhase:
                    AddTo(ref OnDayPhaseChange);
                    break;
                case EventRepetitionType.Every_Month:
                    AddTo(ref OnMonthChange);
                    break;
                case EventRepetitionType.Every_Year:
                    AddTo(ref OnYearChange);
                    break;
            }

            void AddTo(ref Action<SavedTime> targetEvent)
            {
                clockEvent?.Setup(ref targetEvent);

                for (int i = 0; i < actions.Length; i++)
                    targetEvent += actions[i];
            }
        }

        /// <summary>
        /// Add clock events to be checked 
        /// </summary>
        /// <param name="clockEvent"></param>
        public static void AddTimeEvent(params ClockEvent[] clockEvent)
        {
            if (Instance == null) Init();

            Instance.AddTimeEventListeners(clockEvent);
        }

        /// <summary>
        /// Add a new Event
        /// </summary>
        /// <param name="clockEvent"></param>
        private void AddTimeEventListeners(params ClockEvent[] clockEvent)
        {
            for (int i = 0; i < clockEvent.Length; i++)
                AddTimeEvent(clockEvent[i].RepetitionType, clockEvent[i], clockEvent[i].TryActivation);
        }

        /// <summary>
        /// Force execute all events at a given time
        /// </summary>
        /// <param name="timeToExecuteOn">Time to execute on</param>
        public void ForceExecuteAllEvents(SavedTime timeToExecuteOn)
        {
            OnSecondChange?.Invoke(timeToExecuteOn);
            OnMinuteChange?.Invoke(timeToExecuteOn);
            OnDayPhaseChange?.Invoke(timeToExecuteOn);
            OnHourChange?.Invoke(timeToExecuteOn);
            OnDayChange?.Invoke(timeToExecuteOn);
            OnMonthChange?.Invoke(timeToExecuteOn);
            OnYearChange?.Invoke(timeToExecuteOn);
        }

        private Action<SavedTime> OnSecondChange;
        private Action<SavedTime> OnMinuteChange;
        private Action<SavedTime> OnHourChange;
        private Action<SavedTime> OnDayPhaseChange;
        private Action<SavedTime> OnDayChange;
        private Action<SavedTime> OnMonthChange;
        private Action<SavedTime> OnYearChange;

        private Action UpdateAction;
    }
}