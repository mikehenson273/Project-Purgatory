using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace EasySurvivalScript
{
    public enum PlayerStates
    {
        Idle,
        Walking,
        Running,
        Jumping
    }

    public class MainPlayer : MonoBehaviour
    {
        //personally created variables to monitor how long a player has been sprinting
        private float staminaRate = .25f,
                      staminaRateDecrease = 20f,
                      staminaRateIncrease = 15f,
                      startStamina = 100,
                      maxStamina = 100,
                      currentStamina,
                      Stamina;

        private const float StaminaRegenTime = 3.0f;

        private bool currentlyRunning = false;


        //Original

        public PlayerStates playerStates;

        [Header("Inputs")]
        public string HorizontalInput = "Horizontal";
        public string VerticalInput = "Vertical";
        public string RunInput = "Run";
        //public string JumpInput = "Jump";

        [Header("Player Motor")]
        [Range(1f, 15f)]
        public float walkSpeed;
        [Range(1f, 15f)]
        public float runSpeed;
        [Range(0f, 15f)]
        public float JumpForce;
        public Transform FootLocation;

        [Header("Animator and Parameters")]
        public Animator CharacterAnimator;
        public float HorzAnimation;
        public float VertAnimation;
        public bool JumpAnimation;
        public bool LandAnimation;

        [Header("Sounds")]
        public List<AudioClip> FootstepSounds;
        public List<AudioClip> JumpSounds;
        public List<AudioClip> LandSounds;

        CharacterController characterController;

        float _footstepDelay;
        AudioSource _audioSource;
        float footstep_et = 0;

        // Use this for initialization
        void Start()
        {
            Cursor.visible = false;
            Time.timeScale = 1;
            characterController = GetComponent<CharacterController>();
            _audioSource = gameObject.AddComponent<AudioSource>();

            //personal variables to keep player from sprinting infinitely
            Stamina = 100;
            currentStamina = startStamina;

            //Invoke("DisableText", 15f);//invoke after 15 seconds
        }

        // Update is called once per frame
        private void Update()
        {
            //handle controller
            HandlePlayerControls();

            //sync animations with controller
            SetCharacterAnimations();

            //sync footsteps with controller
            PlayFootstepSounds();

            StaminaManager();

        }

        private void FixedUpdate()
        {
        }

        void HandlePlayerControls()
        {
            currentlyRunning = false;

            float hInput = Input.GetAxisRaw(HorizontalInput);
            float vInput = Input.GetAxisRaw(VerticalInput);

            Vector3 fwdMovement = characterController.isGrounded == true ? transform.forward * vInput : Vector3.zero;
            Vector3 rightMovement = characterController.isGrounded == true ? transform.right * hInput : Vector3.zero;

            float _speed = Input.GetButton(RunInput) ? runSpeed : walkSpeed;
            characterController.SimpleMove(Vector3.ClampMagnitude(fwdMovement + rightMovement, 1f) * _speed);

            /*
            if (characterController.isGrounded)
                Jump();*/


            //Managing Player States
            if (characterController.isGrounded)
            {
                if (hInput == 0 && vInput == 0)
                    playerStates = PlayerStates.Idle;
                else
                {
                    if (_speed == walkSpeed)
                    {
                        playerStates = PlayerStates.Walking;
                    }

                    else
                    {
                        playerStates = PlayerStates.Running;
                        currentlyRunning = true;
                    }
                    _footstepDelay = (2 / _speed);
                }
            }
            else
                playerStates = PlayerStates.Jumping;
        }

        /* Jump
        void Jump()
        {
            if (Input.GetButtonDown(JumpInput))
            {
                StartCoroutine(PerformJumpRoutine());
                JumpAnimation = true;
            }
        }

        
        IEnumerator PerformJumpRoutine()
        {
            //play jump sound
            if (_audioSource)
                _audioSource.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Count)]);

            float _jump = JumpForce;

            do
            {
                characterController.Move(Vector3.up * _jump * Time.deltaTime);
                _jump -= Time.deltaTime;
                yield return null;
            }
            while (!characterController.isGrounded);

            //play land sound
            if (_audioSource)
                _audioSource.PlayOneShot(LandSounds[Random.Range(0, LandSounds.Count)]);

        }*/


        void SetCharacterAnimations()
        {
            if (!CharacterAnimator)
                return;

            switch (playerStates)
            {
                case PlayerStates.Idle:
                    HorzAnimation = Mathf.Lerp(HorzAnimation, 0, 5 * Time.deltaTime);
                    VertAnimation = Mathf.Lerp(VertAnimation, 0, 5 * Time.deltaTime);
                    break;

                case PlayerStates.Walking:
                    HorzAnimation = Mathf.Lerp(HorzAnimation, 1 * Input.GetAxis("Horizontal"), 5 * Time.deltaTime);
                    VertAnimation = Mathf.Lerp(VertAnimation, 1 * Input.GetAxis("Vertical"), 5 * Time.deltaTime);
                    break;

                case PlayerStates.Running:
                    HorzAnimation = Mathf.Lerp(HorzAnimation, 2 * Input.GetAxis("Horizontal"), 5 * Time.deltaTime);
                    VertAnimation = Mathf.Lerp(VertAnimation, 2 * Input.GetAxis("Vertical"), 5 * Time.deltaTime);
                    break;


                case PlayerStates.Jumping:
                    if (JumpAnimation)
                    {
                        CharacterAnimator.SetTrigger("Jump");
                        JumpAnimation = false;
                    }
                    break;

            }

            LandAnimation = characterController.isGrounded;
            CharacterAnimator.SetFloat("Horizontal", HorzAnimation);
            CharacterAnimator.SetFloat("Vertical", VertAnimation);
            CharacterAnimator.SetBool("isGrounded", LandAnimation);
        }

        bool onGround()
        {
            bool retVal = false;

            if (Physics.Raycast(FootLocation.position, Vector3.down, 0.1f))
                retVal = true;
            else
                retVal = false;

            return retVal;
        }

        void PlayFootstepSounds()
        {
            if (playerStates == PlayerStates.Idle || playerStates == PlayerStates.Jumping)
                return;

            if (footstep_et < _footstepDelay)
                footstep_et += Time.deltaTime;
            else
            {
                footstep_et = 0;
                _audioSource.PlayOneShot(FootstepSounds[Random.Range(0, FootstepSounds.Count)]);
            }
        }


        //most of this was taken from demoplayercontrols but the stamina and running bits are mine
        private void StaminaManager()
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            if (isRunning)
            {
                if (currentlyRunning)
                {
                    Stamina = Mathf.Clamp(Stamina - (staminaRateDecrease * Time.deltaTime), 0.0f, maxStamina);

                    if (Stamina == 0)
                    {
                        isRunning = false;
                        runSpeed = walkSpeed;
                        currentlyRunning = false;
                    }
                    staminaRate = 0.0f;
                }

                else if (Stamina >= 10)
                {
                    isRunning = Input.GetKey(KeyCode.LeftShift);
                    runSpeed = walkSpeed * 2;
                    currentlyRunning = true;
                }

                else
                {
                }
            }

            else if (Stamina < maxStamina)
            {
                if (staminaRate >= StaminaRegenTime)
                    Stamina = Mathf.Clamp(Stamina + (staminaRateIncrease * Time.deltaTime), 0.0f, maxStamina);
                else
                    staminaRate += Time.deltaTime;
            }
        }

        void OnGUI()
        {
            ShowStamina();
        }

        //created as a variant of ShowHealth
        private void ShowStamina()
        {
            Texture2D lifeTexture1 = new Texture2D(1, 10);
            int y = 0;
            while (y < lifeTexture1.height)
            {
                int x = 0;
                while (x < lifeTexture1.width)
                {
                    if (Stamina >= currentStamina / 2)
                    {
                        lifeTexture1.SetPixel(x, y, Color.blue);
                    }
                    else
                    {
                        lifeTexture1.SetPixel(x, y, Color.red);
                    }
                    x++;
                }
                y++;
            }
            lifeTexture1.Apply();
            GUI.DrawTexture(new Rect(1, 10, Screen.width / 4 * Stamina / 200, Screen.height / 100), lifeTexture1);
        }
    }
}