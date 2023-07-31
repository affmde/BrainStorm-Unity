using System.Collections.Generic;

public static class PlayerData
{
	public static Player player;
	public static bool won;

	public static void Reset()
	{
		won = false;
	}
}

public class Player
{
	public string		username;
	public int			level;
	public float		xp;
	public int			currentGameLevel = 1;
	public List<int>	levelsCompleted;
}
