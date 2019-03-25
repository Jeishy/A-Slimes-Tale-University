using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool hasData;
    
    public Vector2 position;
    public int level;
    public int health;
    public int armour;
    public int collectibles;
    public int gemstones;
    public int maxGemstones;
    
    public ElementalStates element;

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadPlayer(true);
        }
        else
        {
            Debug.LogWarning("Two instances of GameManager have been created!");
            Destroy(this);
        }
    }

    public void SavePlayer(Player player)
    {
        SaveSystem.SavePlayer(player);
        Debug.Log("Saving...");

    }
    
    public void OnCollectiblePickup()
    {
        collectibles++;
    }

    public void OnGemstonePickup()
    {
        gemstones++;
        if (gemstones >= maxGemstones)
            LevelChanger.instance.OnLevelComplete();

    }

    public void LoadPlayer(bool LoadLevel = false)
    {
        
        Debug.Log("Loading...");
        PlayerData data = SaveSystem.LoadPlayer();

        position.x = data.position[0];
        position.y = data.position[1];

        level = data.level;
        
        health = data.health;
        armour = data.armour;

        element = data.element;
        
        Debug.Log("Load complete: Level: " + data.level + " | Pos: " + position + " | health: " + health + " | armour: " + armour + " | element: " + element);
        hasData = true;

        if (LoadLevel)
        {
            LevelChanger.instance.FadeToLevel(data.level);
        }

    }
}
