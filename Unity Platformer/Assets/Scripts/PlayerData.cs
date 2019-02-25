[System.Serializable]
public class PlayerData
{

	public int level;
	public int health;
	public int armour;
	public float[] position;
	public Element element;

	public PlayerData(Player player)
	{
		level = player.GetCurrentLevel();
		health = player.GetHealth();
		armour = player.GetArmour();

		position = new float[2];
		position[0] = player.transform.position.x;
		position[1] = player.transform.position.y;
		element = player.GetElement();
	}
}
