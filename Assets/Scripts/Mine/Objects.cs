using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasySurvivalScript
{
    public class Objects : MonoBehaviour
    {
        public bool isLocked;

        [Header("Define what this Object will do.")]
        [SerializeField]
        UnityEngine.Events.UnityEvent OnInspect;
        [SerializeField]
        UnityEngine.Events.UnityEvent OnExit;

        [HideInInspector]
        public bool _ObjectChecked;
        public GameObject Player;


        // Use this for initialization
        void Start()
        {

        }

        public void ObjectInfo()
        {
            if (isLocked)
            {
                return;
            }

            if (!_ObjectChecked)
            {
                OnInspect.Invoke();
                _ObjectChecked = true;
                Player.SetActive(false);
            }

            else
            {
                OnExit.Invoke();
                _ObjectChecked = false;
                Player.SetActive(true);
            }
        }
    }
}