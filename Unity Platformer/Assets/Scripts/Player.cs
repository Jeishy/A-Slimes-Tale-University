using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField] public int health;
    [SerializeField] public int armour;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _damagedColour;
    [Space]
    [SerializeField] private GameObject _onCoinCollectPE;
    [SerializeField] private GameObject _onHitPE;
    [SerializeField] private ElementalStates element = ElementalStates.None;
    [SerializeField] private AudioManager _audioManager;


    private Image _healthBarFilled;
    private const int MaxHealth = 3;
    private Vector2 damagePoint;                                        // Position where the player was hit by projectile
    private float nextDamageTime;
    private CharacterController2D controller;
    private GameManager gm;
    private AbilityManager _abilityManager;
    [HideInInspector] public bool isDead;

    private void Start()
    {
        _abilityManager = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityManager>();
        controller = GetComponent<CharacterController2D>();
        _healthBarFilled = GameObject.Find("HealthBarFilled").GetComponent<Image>();
        gm = GameManager.instance;
        
        if (gm.hasData)
        {
            health = gm.health;
            armour = gm.armour;
            transform.position = gm.position;
            element = gm.element;
        }

    }

    void Update()
    {
        //Display health and armour in UI
        //_healthBarFilled.fillAmount = health <= 0 ? 0 : (float)health / (float)MaxHealth;
        //_armourText.text = "Armour: " + armour;

        //Call Die() function when player is at or below 0 health
        if (health <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.O))
            gm.SavePlayer(this);

        if (Input.GetKeyDown(KeyCode.P))
            gm.LoadPlayer(true);

        if (Input.GetKeyDown(KeyCode.N))
            Hit(_abilityManager.CurrentPlayerElementalState, ElementalStates.None);

        if (Input.GetKeyDown(KeyCode.M))
            armour++;


    }

    public void Hit(ElementalStates currentElementalState, ElementalStates enemyElementalState, int damage = 1)
    {

        if (nextDamageTime <= Time.time)
        {
            GameObject OnHitParticle = Instantiate(_onHitPE, transform);
            Destroy(OnHitParticle, 2f);

            // Show damage effect on player
            StartCoroutine(ShowDamageMaterial());

            if ((enemyElementalState == ElementalStates.Fire && currentElementalState == ElementalStates.Earth) ||
            (enemyElementalState == ElementalStates.Earth && currentElementalState == ElementalStates.Wind) ||
            (enemyElementalState == ElementalStates.Wind && currentElementalState == ElementalStates.Water) ||
            (enemyElementalState == ElementalStates.Water && currentElementalState == ElementalStates.Fire))
            {
                // Remove all armour slots if hit by element state player is currently weak to
                RemoveArmourSlot();
                RemoveArmourSlot();
            }
            else if (armour <= 0)
            {
                //Oterwise, decrement health by 1
                health -= damage;
            }
            else
            {
                //If so, remove armour slot
                RemoveArmourSlot();
            }

            nextDamageTime = Time.time + damageCooldown;
        }
    }

    private IEnumerator ShowDamageMaterial()
    {
        Color originalColour = _meshRenderer.material.GetColor("_EmissionColor");
        _meshRenderer.material.SetColor("_EmissionColor", _damagedColour);
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material.SetColor("_EmissionColor", originalColour);
    }

    public void SetElement(ElementalStates _element)
    {
        element = _element;
    }

    public void AddArmourSlot()
    {
        armour = 6;
    }

    public void RemoveArmourSlot()
    {
        //1 hit = 3 armor, if player doesnt have full armour, a single hit cannot damage armour below 3 pieces
        if (armour > 3)
        {
            armour = 3;
        }
        else
        {
            armour = 0;
        }

        if (armour <= 0)
            _abilityManager.NoneState();
    }

    void Die()
    {
        //Load current scene
        isDead = true;
        gm.LoadPlayer(true);
    }


    //GET FUNCTIONS
    public int GetHealth()
    {
        return health;
    }

    public int GetArmour()
    {
        return armour;
    }

    public int GetCurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public ElementalStates GetElement()
    {
        return element;
    }




    //Collision checks
    private void OnCollisionEnter(Collision other)
    {
        //Check if the player has collided with an enemy projectile
        if (other.gameObject.CompareTag("EnemyProj"))
        {
            //Apply knockback
            controller.Knockback(other.transform.position.x > transform.position.x);

            ElementalStates enemyElementalState = other.gameObject.GetComponent<EnemyProjectile>().GetElement();

            //Destroy projectile
            Destroy(other.gameObject);

            //Calculate new health/armour
            Hit(_abilityManager.CurrentPlayerElementalState, enemyElementalState);

        }

        if (other.gameObject.CompareTag("DeathTrigger"))
        {
            Die();
        }

        if (other.gameObject.CompareTag("NextLevel"))
        {
            Debug.Log("Level complete!!");
            LevelChanger.instance.FadeToLevel(GetCurrentLevel() + 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            gm.SavePlayer(this);
        }

        if (other.CompareTag("Collectible"))
        {
            gm.OnCollectiblePickup();
            StartCoroutine(WaitToCoinCollect(other.gameObject));
            other.GetComponent<Animator>().SetTrigger("Collect");
            Destroy(other.gameObject, 1f);
        }

        if (other.CompareTag("Gemstone"))
        {
            gm.OnGemstonePickup();
            other.GetComponentInParent<GemstoneCollect>().IsCollected = true;
            StartCoroutine(WaitToGemstoneCollect(other.transform.parent));
            Destroy(other.transform.parent.gameObject, 3f);
}

        if (other.CompareTag("NextLevel"))
        {
            LevelChanger.instance.FadeToLevel(GetCurrentLevel() + 1);
        }
    }

    private IEnumerator WaitToCoinCollect(GameObject other)
    {
        yield return new WaitForSeconds(0.3f);
        Vector3 spawnPos = new Vector3(other.transform.position.x - 0.7f, other.transform.position.y,
            other.transform.position.z);
        GameObject coinCollect = Instantiate(_onCoinCollectPE, spawnPos, Quaternion.identity);
        Destroy(coinCollect, 1f);
    }

    private IEnumerator WaitToGemstoneCollect(Transform other)
    {
        yield return new WaitForSeconds(2.98f);
        Vector3 spawnPos = other.position;
        GameObject gemstoneCollectPE = other.GetComponent<GemstoneCollect>().OnCollectPE;
        GameObject gemstoneCollect = Instantiate(gemstoneCollectPE, spawnPos, Quaternion.identity);
        Destroy(gemstoneCollect, 1f);
    }
    void OnParticleCollision(GameObject other)
    {
        if (_abilityManager.CurrentPlayerElementalState != ElementalStates.Fire)
            Hit(_abilityManager.CurrentPlayerElementalState, ElementalStates.Fire);
    }
}