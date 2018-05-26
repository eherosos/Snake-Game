//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elven_Path
{
    public class SoundFxManager : MonoBehaviour
    {
        public static SoundFxManager instance;

        public UnityEvent Sound_OnDieEvent = new UnityEvent();
        public UnityEvent Sound_OnAttackEvent = new UnityEvent();
        public UnityEvent Sound_OnStartBattleEvent = new UnityEvent();
        public UnityEvent Sound_OnGenerateEvent = new UnityEvent();
        public UnityEvent Sound_OnHealingEvent = new UnityEvent();
        public UnityEvent Sound_OnLoseEvent = new UnityEvent();
        public UnityEvent Sound_OnVictoryEvent = new UnityEvent();

        private void Awake()
        {
            instance = this;
        }

    }
}