using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasySurvivalScript
{
    public class Crates : MonoBehaviour
    {
        public bool isLocked;

        [Header("Define what this crate will do.")]
        [SerializeField]
        UnityEngine.Events.UnityEvent OnHide;
        [SerializeField]
        UnityEngine.Events.UnityEvent OnExit;

        [HideInInspector]
        public bool _hasHid;


        // Use this for initialization
        void Start()
        {

        }

        public void CrateHide()
        {
            if (isLocked)
            {
                return;
            }

            if (!_hasHid)
            {
                OnHide.Invoke();
                _hasHid = true;
            }
            else
            {
                OnExit.Invoke();
                _hasHid = false;
            }
        }
    }
}