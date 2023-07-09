using DAtingApp.extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DAtingApp.SginalR
{
	[Authorize]
	public class PresenseHub : Hub
	{
		private readonly PresenceTracker _Tracker;

		public PresenseHub(PresenceTracker tracker)
		{
			_Tracker = tracker;
		}
		public  override async Task OnConnectedAsync()
		{
			var isOnline = await _Tracker.UserConnected(Context.User.GetUserName(),Context.ConnectionId);
			if(isOnline)
			{
				await Clients.Others.SendAsync("UserIsOnLine", Context.User.GetUserName());

			}

			var CurrentUsers = await _Tracker.GetOnlineUsers();

			await Clients.Caller.SendAsync("GetOnLineUsers", CurrentUsers);
		}

		public  override async Task OnDisconnectedAsync(Exception exception)
		{
			var isOffline = await _Tracker.UserDisConnected(Context.User.GetUserName(), Context.ConnectionId);
			if (isOffline)
			{
				await Clients.Others.SendAsync("UserIsOffLine", Context.User.GetUserName());
			}

			await base.OnDisconnectedAsync(exception);
		}
	}
}
