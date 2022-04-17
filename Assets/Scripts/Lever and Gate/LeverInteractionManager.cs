using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasySurvivalScript
{
    public class LeverInteractionManager : MonoBehaviour
    {
        [Header("Interaction Input")]
        public string InteractionInput = "Interaction";

        [Header("References")]
        public Transform PlayerCamera;
        public Text LeverMessageText;

        [Header("Settings")]
        public LayerMask DetectionMask;
        public LayerMask DetectionMask1;
        public float DetectionDistance;
        public bool DebugRay = true;
        public string LeverPullUpMsg = "Pull Lever Up";
        public string LeverPullDownMsg = "Pull Lever Down";
        public string LeverLockedMsg = "Lever Locked. Find the Key.";

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

            PullLever();
        }

        void PullLever()
        {
            Lever _currentLever = GetPullLever();

            if (!_currentLever)
            {
                return;
            }

            if (Input.GetButtonDown(InteractionInput))
            {
                if (!Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask1))
                {
                    if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask))
                    {
                        _currentLever.PullLever();
                    }
                }
            }
        }

        Lever GetPullLever()
        {
            Lever _lever = null;

            //red means nothing has been detected
            if (DebugRay)
            {
                Debug.DrawRay(PlayerCamera.position, PlayerCamera.forward * DetectionDistance, Color.red);
            }

            if (!Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask1))
            {
                if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out _hitInfo, DetectionDistance, DetectionMask))
                {
                    if (_hitInfo.collider.GetComponent<Lever>())
                    {
                        _lever = _hitInfo.collider.GetComponent<Lever>();

                        //set message
                        LeverMessageText.text = _lever._hasPulled ? LeverPullUpMsg : LeverPullDownMsg;
                        LeverMessageText.text = _lever.isLocked ? LeverLockedMsg : LeverMessageText.text;

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
                LeverMessageText.text = "";
            }

            return _lever;

        }

    }
}