using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using EasySurvivalScript;

namespace EasySurvivalScript
{
    public class Hiding2 : MonoBehaviour
    {
        [Header("Interaction Input")]
        public string InteractionInput = "Interaction";

        [Header("References")]
        public Transform PlayerCamera;
        public Text CrateMessageText;

        [Header("Settings")]
        public LayerMask DetectionMask;
        public LayerMask DetectionMask1;
        public float DetectionDistance;
        public bool DebugRay = true;
        public string HideInCrateMsg;
        public string ExitCrateMsg;

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

            CrateHide();
        }

        void CrateHide()
        {
            Crates _currentCrate = GetCrateHide();

            if (!_currentCrate)
            {
                return;
            }

            if (Input.GetButtonDown(InteractionInput))
            {
                if (!Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask1))
                {
                    if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask))
                    {
                        _currentCrate.CrateHide();
                    }
                }
            }
        }

        Crates GetCrateHide()
        {
            Crates _crate = null;

            //red means nothing has been detected
            if (DebugRay)
            {
                Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * DetectionDistance, Color.red);
            }

            if (!Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask1))
            {
                if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask))
                {
                    if (_hitInfo.collider.GetComponent<Crates>())
                    {
                        _crate = _hitInfo.collider.GetComponent<Crates>();

                        //set message
                        CrateMessageText.text = _crate._hasHid ? HideInCrateMsg : ExitCrateMsg;

                        //green means lever has been detected
                        if (DebugRay)
                        {
                            Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * DetectionDistance, Color.green);
                        }
                    }

                }
            }

            else
            {
                //set message
                CrateMessageText.text = "";
            }

            return _crate;
        }
    }
}
