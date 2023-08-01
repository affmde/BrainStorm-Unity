using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelsData
{
	public static string	task;
	public static string	button1;
	public static string	button2;
	public static string	button3;
	public static string	button4;
	public static List<int>	validOptions;
	public static List<Level> levelsList;
}

public class Level
{
	public string		task;
	public string		button1;
	public string		button2;
	public string		button3;
	public string		button4;
	public List<int>	validOptions;
	public float		time;
	public int			difficulty;
	public int			id;
}
