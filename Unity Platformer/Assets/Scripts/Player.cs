using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class Player : MonoBehaviour {

    [SerializeField] public int health;
    [SerializeField] public int armour;
    [SerializeField] private float damageCooldown = 0.5f;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Color _damagedColour;
    [Space]
    [SerializeField] private GameObject _onCoinCollectPE;
    [SerializeField] private Image _healthBar;
    [SerializeField] private ElementalStates element = ElementalStates.None;
    


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
        
        gm = GameManager.instance;
        
        if (gm.hasData)
        {
            health = gm.health;
            armour = gm.armour;
            transform.position = gm.position;
            element = gm.element;
        }
        
    }

	void Update ()
    {
        //Display health and armour in UI
        //_healthBar.fillAmount = health <= 0 ? 0 : (float)health / (float)MaxHealth;
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
            Hit();

        if (Input.GetKeyDown(KeyCode.M))
            armour++;


	}

    public void Hit(int damage = 1)
    {
        // Show damage effect on player
        StartCoroutine(ShowDamageMaterial());

        if (nextDamageTime <= Time.time)
        {
            //Check if player has armour
            if (armour > 0)
            {
                //If so, remove armour slot
                RemoveArmourSlot();
            }
            else
            {
#if UNITY_PS4
                PS4Input.PadSetLightBar(0, 255, 0, 0);
#endif
                //Oterwise, decrement health by 1
                health -= damage;
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
        } else
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
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Check if the player has collided with an enemy projectile
        if (other.gameObject.CompareTag("EnemyProj"))
        {
            //Apply knockback
            controller.Knockback(other.transform.position.x > transform.position.x);
            
            //Destroy projectile
            Destroy(other.gameObject);
            
            //Calculate new health/armour
            Hit();
            
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            gm.SavePlayer(this);
        }

        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Collectible");
            //gm.OnCollectiblePickup();
            StartCoroutine(WaitToCoinCollect(other.gameObject));
            other.GetComponent<Animator>().SetTrigger("Collect");
            Destroy(other.gameObject, 1f);
        }

        if (other.CompareTag("Gemstone"))
        {
            Debug.Log("Gemstone");
            gm.OnGemstonePickup();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("NextLevel"))
        {
            Debug.Log("Level complete!!");
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

}
