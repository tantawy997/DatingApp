namespace DAtingApp.SginalR
{
	public class PresenceTracker
	{
		private static readonly Dictionary<string, List<string>> OnLineUsers = 
			new Dictionary<string, List<string>>();

		public Task<bool> UserConnected(string username,string connectionId)
		{
			bool isOnline = false;

			lock (OnLineUsers)
			{
				if (OnLineUsers.ContainsKey(username))
				{
					OnLineUsers[username].Add(connectionId);
					isOnline = true;

				}
				else
				{
					OnLineUsers.Add(username, new List<string> { connectionId});
				}
			}

			return Task.FromResult(isOnline);
		}

		public Task<bool> UserDisConnected(string username, string connectionId)
		{
			bool isOffline = false;
			lock (OnLineUsers)
			{
				if (!OnLineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

				OnLineUsers[username].Remove(connectionId);

				if (OnLineUsers[username].Count == 0)
				{
					OnLineUsers.Remove(username);
					isOffline = true;
				}

			}
			return Task.FromResult(isOffline);

		}

		public Task<string[]> GetOnlineUsers()
		{
			string[] onLine;

			lock (OnLineUsers)
			{
				onLine = OnLineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
			}

			return Task.FromResult(onLine);

		}

		public static Task<List<string>> GetConnectionsForUser(string UserName)
		{
			List<string> connectionIds;
			lock (OnLineUsers)
			{
				connectionIds = OnLineUsers.GetValueOrDefault(UserName);
			}

			return Task.FromResult(connectionIds);
		}
	}
}
