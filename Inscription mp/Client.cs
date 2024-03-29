﻿using Newtonsoft.Json.Linq;
using Inscryption_Server.Exceptions;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Inscryption_Server.DataTypes;
using Inscryption_Server.Events.INotifyEvent;

namespace Inscryption_mp
{
	internal class Client : Inscryption_Server.Client
	{
		public View CurrentView { get; private set; }

		internal Client(TcpClient socket) : base(socket)
		{
			WaitForMOTD();
		}
		public void WaitForMOTD()
		{
			DateTime endTime = DateTime.Now.AddSeconds(5);
			while (true)
			{
				if (DateTime.Now > endTime)
					throw new ClientDisconnectedException("No Response in 5 sec. Assuming disconnection");
				if (Available)
					break;
				Thread.Sleep(500);
			}

			NetworkPacket[] packets = GetPackets();

			bool notMOTD = false;
			List<JObject> data = packets[0].data.Children<JObject>().ToList();
			JObject token;
			if ((token = data.Find(i => (i.First as JProperty).Name == "Message")) != null)
			{
				string message = token.Value<string>("Message");
				Task.Run(() => MessageBox.Show(message));
			}
			else
				notMOTD = true;
			JToken client = data.Find(i => (i.First as JProperty).Name == "client")["client"];
			if (client != null)
			{
				UserID = client.Value<uint>("ID");
				IsHost = client.Value<bool>("IsHost");
			}
			else
				notMOTD = true;
			if (notMOTD)
				throw new Exception("First package did not contain MOTD data. Disconnecting");
			foreach (NetworkPacket packet in packets)
			{
				HandleActions(packet);
			}
			Username = $"{App.Settings.Username}P{UserID}";
			AddData("Username", Username);
			DataSend();
		}
		public override bool ChangeScene(JObject data)
		{
			base.ChangeScene(data);
			CurrentView = View.GetView(CurrentScene);
			MainWindow.MainWindow_ShowView(CurrentView);
			return true;
		}
		protected override void CurrentScene_ActionRunEvent(Inscryption_Server.Client sender, ActionRunEventData e)
		{
			//Client side
			if(sender == null)
			{ AddAction(e.Runnable.Action, e.Data); }
		}
	}
}
