using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
}
