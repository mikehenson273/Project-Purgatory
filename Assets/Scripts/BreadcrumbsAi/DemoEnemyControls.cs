using UnityEngine;
using System.Collections;
using BreadcrumbAi;

//to enable damage for player
//using UnityStandardAssets.Characters.FirstPerson;

using EasySurvivalScript;
using ManageHealth;

[System.Serializable]
public class DemoEnemySounds
{
    public AudioClip audio_hit_1, audio_hit_2, audio_dead_1, audio_dead_2, audio_melee_attack_1, audio_melee_attack_2;
}

public class DemoEnemyControls : MonoBehaviour
{
    public DemoEnemySounds audioClips;
    public enum EnemyType { Melee, Ranged, Special };
    public GameObject healthPickUpPrefab;
    public bool _canDropPickUp;
    public EnemyType enemyType;
    public Rigidbody rangedProjectilePrefab;

    public GameObject MainPlayer;

    public GameObject bloodPrefab;
    public GameObject specialPrefab;
    private Transform player;
    private Transform waypoint;

    private Ai ai;

    private bool _removeBody, _isHit, _animAttack;
    private AudioSource audioSource;

    private float rangedAttackNext = 0.0f;
    private float rangedAttackRate = 2.0f;
    private float meleeAttackNext = 0.0f;
    private float meleeAttackRate = 1.0f;

    private Animator anim;
    private string animRun = "Run";
    private string animDeath1 = "Death1";
    private string animAttack = "Attack";

    //private DemoScore score;
    private bool _pointScored;

    void Start()
    {
        ai = GetComponent<Ai>();
        anim = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        //score = Camera.main.GetComponent<DemoScore>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
        {
            player = go.transform;
        }
    }

    void Update()
    {
        CheckHealth();
    }

    void FixedUpdate()
    {
        Animation();
        Attack();
    }

    private void Animation()
    {
        if (ai.lifeState == Ai.LIFE_STATE.IsAlive)
        {
            if (ai.moveState != Ai.MOVEMENT_STATE.IsIdle)
            {
                anim.SetBool(animRun, true);
            }
            else
            {
                anim.SetBool(animRun, false);
            }
            if (_animAttack)
            {
                anim.SetBool(animAttack, true);
            }
            else
            {
                anim.SetBool(animAttack, false);
            }
        }
        else if (ai.lifeState == Ai.LIFE_STATE.IsDead)
        {
            anim.SetBool(animDeath1, true);
        }
    }

    private void Attack()
    {
        if (player)
        {
            if (ai.lifeState == Ai.LIFE_STATE.IsAlive)
            {
                if (enemyType != EnemyType.Ranged)
                {
                    if (ai.attackState == Ai.ATTACK_STATE.CanAttackPlayer && Time.time > meleeAttackNext)
                    {
                        meleeAttackNext = Time.time + meleeAttackRate;
                        float rand = Random.value;
                        if (rand <= 0.4f)
                        {
                            audioSource.clip = audioClips.audio_melee_attack_1;
                        }

                        else
                        {
                            audioSource.clip = audioClips.audio_melee_attack_2;
                        }
                        audioSource.PlayOneShot(audioSource.clip);

                        //For player character to take damage
                        //player.GetComponent<FirstPersonController>()._isHit = true;
                        //player.GetComponent<PlayerMovement>()._isHit = true;
                        MainPlayer.GetComponent<HealthManagementSystem>()._isHit = true;

                        /* ORIGINAL CODE
						player.GetComponent<DemoPlayerControls>()._isHit = true;
						player.GetComponent<DemoPlayerControls>().Bleed(transform.rotation);
                        */

                        _animAttack = true;
                    }
                    else
                    {
                        _animAttack = false;
                    }
                }
                else
                {
                    if (ai.attackState == Ai.ATTACK_STATE.CanAttackPlayer && Time.time > rangedAttackNext)
                    {
                        rangedAttackNext = Time.time + rangedAttackRate;
                        Rigidbody spit = Instantiate(rangedProjectilePrefab, transform.position + transform.forward + transform.up, transform.rotation) as Rigidbody;
                        spit.AddForce(transform.forward * 500);
                        _animAttack = true;
                    }
                    else
                    {
                        _animAttack = false;
                    }
                }
            }
        }
    }

    private void CheckHealth()
    {
        if (_isHit && this != null)
        {
            float rand = Random.value;
            if (ai.Health > 0)
            {
                if (rand > 0.5f)
                {
                    if (rand < 0.7f)
                    {
                        audioSource.clip = audioClips.audio_hit_2;
                    }
                    else
                    {
                        audioSource.clip = audioClips.audio_hit_1;
                    }
                    audioSource.PlayOneShot(audioSource.clip);
                }
            }
            if (ai.Health <= 0)
            {
                if (rand > 0.5f)
                {
                    audioSource.clip = audioClips.audio_dead_1;
                }
                else
                {
                    audioSource.clip = audioClips.audio_dead_2;
                }
                audioSource.PlayOneShot(audioSource.clip);
            }
            _isHit = false;
        }

        if (ai.lifeState == Ai.LIFE_STATE.IsDead)
        {
            if (!_pointScored)
            {
                if (enemyType == EnemyType.Special)
                {
                    //score.ScorePoint(50);
                }
                else
                {
                    //score.ScorePoint(15);
                }
                _pointScored = true;
            }
            if (_canDropPickUp)
            {
                float rand = Random.value;
                if (rand <= 0.3f)
                {
                    GameObject healthPickUp = Instantiate(healthPickUpPrefab, transform.position, Quaternion.identity) as GameObject;
                    healthPickUp.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                    Destroy(healthPickUp, 20);
                }
                _canDropPickUp = false;
            }
            else
            {
                transform.position -= new Vector3(0, 0.01f, 0);
            }
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.name.Contains("Bullet"))
        {
            _isHit = true;
            ai.Health -= 25;
            GameObject blood = Instantiate(bloodPrefab, col.collider.transform.position, col.collider.transform.rotation) as GameObject;
            Destroy(blood, 3);
        }
    }
}
