namespace Game.Events
{
	public static class LobbyEvents
	{
		public delegate void LobbyUpdate();
		public static LobbyUpdate OnLobbyUpdated;

		public delegate void LobbyReady();
		public static LobbyUpdate OnLobbyReady;
	}
}