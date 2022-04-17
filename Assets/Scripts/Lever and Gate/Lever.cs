using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasySurvivalScript
{
    public class Lever : MonoBehaviour
    {

        public bool isLocked;

        [Header("Define what this lever will do.")]
        [SerializeField]
        UnityEngine.Events.UnityEvent OnLeverPullDown;
        [SerializeField]
        UnityEngine.Events.UnityEvent OnLeverPullUp;

        [HideInInspector]
        public bool _hasPulled;


        // Use this for initialization
        void Start()
        {

        }

        public void PullLever()
        {
            if (isLocked)
                return;

            if (!_hasPulled)
            {
                OnLeverPullDown.Invoke();
                _hasPulled = true;
                isLocked = true;
            }

            else
            {
                OnLeverPullUp.Invoke();
                _hasPulled = false;
            }
        }

    }
}