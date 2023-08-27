using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilsClass : MonoBehaviour
{
	public static UtilsClass instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	public string SerializeString(List<string> list)
	{
		string str = new string("");

		str = string.Join(",", list);
		Debug.Log("Serialized string for names: " + str);
		return str;
	}

	public List<string> DeserializeString(string str)
	{
		string[] stringArray = str.Split(",");
		List<string> list = new List<string>(stringArray);
		return list;
	}

	public string SerializeInt(List<int> list)
	{
		string str = new string("");

		List<string> stringList = list.ConvertAll(number => number.ToString());
		str = string.Join(",", stringList);
		Debug.Log("Serialized string for ids: " + str);
		//foreach(int num in list)
		//	str += num + ",";
		return str;
	}

	public List<int> DeserializeInt(string str)
	{
		string[] stringArray = str.Split(",");
		List<string> list = new List<string>(stringArray);
		List<int> intList = new List<int>();
		foreach(string s in list)
			intList.Add(int.Parse(s));
		return intList;
	}
}
