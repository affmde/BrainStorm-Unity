

public static class StaticLevels
{
	public static LevelsConfig	conf;
	public static string		startLevel = "[StartLevel]";
	public static string		levelStart = "[Level]";
	public static string		levelEnd = "[-Level]";
	public static string		totalStart = "[Total]";
	public static string		totalEnd = "[-Total]";
	public static string		xpStart = "[XP]";
	public static string		xpEnd = "[-XP]";
	public static string		timeStart = "[Time]";
	public static string		timeEnd = "[-Time]";
	public static string		end = "[EndLevel]";
}


public class LevelsConfig
{
	public int		level;
	public int		total;
	public int		xp;
	public float	duration;
}
