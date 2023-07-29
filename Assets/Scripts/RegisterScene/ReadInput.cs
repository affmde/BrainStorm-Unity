using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
	public void ReadInputString(string str)
	{
		PlayerData.username = str;
		Debug.Log("username: " + PlayerData.username);
	}
}
