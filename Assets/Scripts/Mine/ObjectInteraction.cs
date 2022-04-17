using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using EasySurvivalScript;

namespace EasySurvivalScript
{
    public class ObjectInteraction : MonoBehaviour
    {
        [Header("Interaction Input")]
        public string InteractionInput = "Interaction";

        [Header("References")]
        public Transform PlayerCamera;
        public Text ObjectMessageText;

        [Header("Settings")]
        public LayerMask DetectionMask;
        public float DetectionDistance;
        public bool DebugRay = true;
        public string ObjectSeenMsg;
        public string ExitMsg;

        RaycastHit _hitInfo;


        private void Start()
        {
            if (!PlayerCamera)
            {
                PlayerCamera = Camera.main.transform;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!PlayerCamera)
            {
                return;
            }

            ObjectInfo();
        }

        void ObjectInfo()
        {
            Objects _currentObject = GetObjectInfo();

            if (!_currentObject)
            {
                return;
            }

            if (Input.GetButtonDown(InteractionInput))
            {
                if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask))
                {
                   _currentObject.ObjectInfo();
                }
            }
        }

        Objects GetObjectInfo()
        {
            Objects _Object = null;

            //red means nothing has been detected
            if (DebugRay)
            {
                Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * DetectionDistance, Color.red);
            }

           if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask))
           {
                if (_hitInfo.collider.GetComponent<Objects>())
                {
                    _Object = _hitInfo.collider.GetComponent<Objects>();

                    //set message
                    ObjectMessageText.text = _Object._ObjectChecked ? ObjectSeenMsg : ExitMsg;

                    //green means object has been detected
                    if (DebugRay)
                    {
                       Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * DetectionDistance, Color.green);
                    }
                }

           }        

           else
           {
               //set message
               ObjectMessageText.text = "";
           }

           return _Object;
        }
    }
}
