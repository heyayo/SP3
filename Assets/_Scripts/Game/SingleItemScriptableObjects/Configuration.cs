using UnityEngine;

[CreateAssetMenu(fileName="Game Configuration",menuName="Options/Configuration",order=1)]
public class Configuration : ScriptableObject
{
	public static Configuration FetchConfig()
	{
		return Resources.Load<Configuration>("configs/game_cfg");
	}

	public KeyCode up = KeyCode.W;
	public KeyCode down = KeyCode.S;
	public KeyCode left = KeyCode.A;
	public KeyCode right = KeyCode.D;
	public KeyCode pause = KeyCode.Escape;
	public KeyCode run = KeyCode.LeftShift;
	public KeyCode interact = KeyCode.E;
	public KeyCode attack = KeyCode.Mouse0;
	public KeyCode hotbar1 = KeyCode.Alpha1;
	public KeyCode hotbar2 = KeyCode.Alpha2;
	public KeyCode hotbar3 = KeyCode.Alpha3;
	public KeyCode hotbar4 = KeyCode.Alpha4;
	public KeyCode dropItem = KeyCode.G;
	public KeyCode openInventory = KeyCode.Tab;
}
