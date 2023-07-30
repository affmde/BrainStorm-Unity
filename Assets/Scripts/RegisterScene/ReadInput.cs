using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
	public void ReadInputString(string str)
	{
		PlayerData.player.username = str;
	}
}
