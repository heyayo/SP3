using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Game Configuration",menuName="Options/Configuration",order=1)]
public class Configuration : ScriptableObject
{
	public static Configuration FetchConfig()
	{
		return Resources.Load<Configuration>("configs/cfg");
	}

	public KeyCode Up = KeyCode.W;
	public KeyCode Down = KeyCode.S;
	public KeyCode Left = KeyCode.A;
	public KeyCode Right = KeyCode.D;
	public KeyCode Pause = KeyCode.Escape;
}
