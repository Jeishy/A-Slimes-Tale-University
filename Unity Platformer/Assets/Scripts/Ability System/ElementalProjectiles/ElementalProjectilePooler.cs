using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalProjectilePooler : MonoBehaviour {

	// Class for the pools in the game
	// Contains a tag, a specified prefab, and a max size
	// for the pools
	[System.Serializable]
	public class Pool
	{
		[Tooltip("Set tag to be name of the element")]
		public string tag;
		[Tooltip("Set prefab to be used for the pool")]
		public GameObject prefab;
		[Tooltip("Set amount of gameobjects instantiated. Ensure size is set to more than 0")]
		public int size;
	}

	// Instance for pooler
	public static ElementalProjectilePooler Instance;

	private void Awake()
	{
		Instance = this;
	}

	// Defines a list of pools
	public List<Pool> pools;
	// poolDictionary holds a queue of gameobjects
    public Dictionary<string, Queue<GameObject>> poolDictionary;

	// Use this for initialization
	void Start () {
		// setting poolDictionary to a new Dictionary of type string and Queue<GameObject>
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

		// Loops through the pools list and populates Queue with specified instatiated gameobjects
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
				// Sets gameobjects to false after instantiation
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
			// Adds queue to the dictionary
            poolDictionary.Add(pool.tag, objectPool);
        }
	}

	// Function for spawning projectile from specified pool
    public GameObject SpawnProjectileFromPool (string tag, Vector3 position, Quaternion rotation)
    {
		// Check if tag exists, if not log error to console
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag" + tag + "doesn't exist!");
            return null;
        }

		// Store projectile from pool of specified tag to projectile variable
        GameObject elementalProj = poolDictionary[tag].Dequeue();

		// Set projectile gameobject to true
        elementalProj.SetActive(true);
		// Set projectile's transform
        elementalProj.transform.position = position;
		// Set projectile's rotation
        elementalProj.transform.rotation = rotation;

		
        IPooledProjectile pooledProjectile = elementalProj.GetComponent<IPooledProjectile>();

		// Shoot on elemental projectile prefab is run
		// if there is an implementation of the function
        if (pooledProjectile != null)
        {
            pooledProjectile.Shoot();
        }

        poolDictionary[tag].Enqueue(elementalProj);

        return elementalProj;
    }
}
