using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{

    public bool hasData;
    
    public Vector2 position;
    public int level;
    public int health;
    public int armour;
    
    public Element element;

    public static GameData instance = null;

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
            SceneManager.LoadScene(data.level);
        }

    }
}



public enum Element
{
    None,
    Earth,
    Water,
    Fire,
    Air
};
