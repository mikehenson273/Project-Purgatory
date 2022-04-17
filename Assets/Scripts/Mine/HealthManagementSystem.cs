using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace ManageHealth
{
    public class HealthManagementSystem : MonoBehaviour
    {
        public GameObject Player;
        public GameObject PauseMenu;

        #region health
        //taken from demoplayercontrol script
        //public GameObject gameOver;

        [HideInInspector] public bool _isHit, _pickedUpHealth, _hasMaxHealth;

        public float healMaxTimer,
                      healTime,
                      healRate = .025f,
                      startHealth = 100,
                      maxHealth,
                      currentHealth,
                      Health;
        #endregion

        // Use this for initialization
        void Start()
        {
            //taken from demoplayercontrol script
            Health = 100;
            currentHealth = startHealth;
            healTime = healRate;
        }

        // Update is called once per frame
        void Update()
        {
            //taken from demo script
            HealthManager();
        }

        //most of this was taken from demoplayercontrols but the stamina and running bits are mine
        private void HealthManager()
        {
            if (_isHit)
            {
                healTime = 0.0f;
                Health -= 70;
                _isHit = false;
            }
            else
            {
                healTime += Time.deltaTime;
                if (healTime > healRate)
                {
                    if (Health <= currentHealth)
                    {
                        Health++;
                    }
                }
            }

            if (Health <= 0f)
            {
                //Instantiate(gameOver);
                Destroy(Player);
                Destroy(PauseMenu);
                Destroy(gameObject);
                SceneManager.LoadScene("YouDied");
            }
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.collider.name.Contains("Spit"))
            {
                _isHit = true;
            }
        }

        void OnGUI()
        {
            ShowHealth();
        }

        private void ShowHealth()
        {
            Texture2D lifeTexture = new Texture2D(1, 1);
            int y = 0;
            while (y < lifeTexture.height)
            {
                int x = 0;
                while (x < lifeTexture.width)
                {
                    if (Health >= currentHealth / 2)
                    {
                        lifeTexture.SetPixel(x, y, Color.green);
                    }
                    else
                    {
                        lifeTexture.SetPixel(x, y, Color.red);
                    }
                    x++;
                }
                y++;
            }
            lifeTexture.Apply();
            GUI.DrawTexture(new Rect(1, 1, Screen.width / 4 * Health / 200, Screen.height / 100), lifeTexture);
        }
    }
}