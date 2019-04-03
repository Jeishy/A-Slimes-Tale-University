using UnityEngine;
using UnityEngine.SceneManagement;

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
    [HideInInspector] public bool IsLevelComplete;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadPlayer(true);
        }
        else
        {
            Destroy(this);
        }

        IsLevelComplete = false;
    }

    public void SavePlayer(Player player)
    {
        SaveSystem.SavePlayer(player);

    }
    
    public void OnCollectiblePickup()
    {
        collectibles++;
    }

    public void OnGemstonePickup()
    {
        gemstones++;
        Debug.Log(gemstones);
        CheckIfLevelIsComplete();
    }

    public void CheckIfLevelIsComplete()
    {
        if (gemstones >= maxGemstones)
            IsLevelComplete = true;
    }

    public void LoadPlayer(bool LoadLevel = false)
    {
        
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

    public string GetScenePath(string sceneName)
    {
        string path = "Assets/Scenes/" + sceneName + ".unity";
        return path;
    }
}
