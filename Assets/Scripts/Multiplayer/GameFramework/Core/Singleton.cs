using UnityEngine;


namespace GameFramework_Core
{
	public class Singleton<T> : MonoBehaviour where T : Component
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					T[] objs = FindObjectsOfType<T>();
					if (objs.Length > 0)
					{
						T this_instance = objs[0];
						instance = this_instance;
					}
					else
					{
						GameObject go = new GameObject();
						go.name = typeof(T).Name;
						instance = go.AddComponent<T>();
						DontDestroyOnLoad(go);
					}
				}
				return instance;
			}
		}
	}
	
}
