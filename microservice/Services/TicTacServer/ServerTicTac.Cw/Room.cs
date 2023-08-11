using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProtoBuf;

namespace ServerTicTac.Cw
{
	[ProtoContract]
	public class Room
	{
		[ProtoMember(1, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public int numberOfPlayer { get; set; }

		//  [ProtoMember(2, DataFormat = DataFormat.FixedSize, IsRequired = true)]

		// [ProtoMember(2, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		[ProtoIgnore]
		public List<Player> listPlayers { get; set; }
		[ProtoMember(3, DataFormat = DataFormat.FixedSize, IsRequired = true)]

		public bool GameInProgress { get; set; }
		[ProtoMember(4, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public int CurrentPlayerId { get; set; }
		[ProtoMember(5, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public int RoomId { get; set; }
		[ProtoMember(6, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn1 { get; set; }
		[ProtoMember(7, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn1 { get; set; }
		[ProtoMember(8, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn2 { get; set; }
		[ProtoMember(9, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn2 { get; set; }
		[ProtoMember(10, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn3 { get; set; }
		[ProtoMember(11, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn3 { get; set; }
		[ProtoMember(12, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn4 { get; set; }
		[ProtoMember(13, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn4 { get; set; }
		[ProtoMember(14, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn5 { get; set; }
		[ProtoMember(15, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn5 { get; set; }
		[ProtoMember(16, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn6 { get; set; }
		[ProtoMember(17, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn6 { get; set; }
		[ProtoMember(18, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn7 { get; set; }
		[ProtoMember(19, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn7 { get; set; }
		[ProtoMember(20, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn8 { get; set; }
		[ProtoMember(21, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn8 { get; set; }
		[ProtoMember(22, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public char btn9 { get; set; }
		[ProtoMember(23, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool Activebtn9 { get; set; }
		// [ProtoMember(24, DataFormat = DataFormat.FixedSize)]
		// public List<Player> players { get; set; } = new List<Player>();

		[ProtoIgnore]
		public List<Player> players { get; set; }
	// Parameterless constructor required by Protobuf

	public Room(int roomId)
	{
		//Clients = new List<Socket>();
		RoomId = roomId;
		// players = new Player[2];

		//players = new Player[2];
	}

	public void addPlayer(Player p)
	{
			//for (int i = 0; i < 2; i++)
			//{
			//	if (players[i] == null)
			//	{
			//		numberOfPlayer++;
			//		players[i] = p;
			//		// players[i].setIDRoom(RoomId);
			//		//players[i].sendData($"room {RoomId} {i}");
			//		//numberOfPlayer++;
			//		//sendRoomate(i);
			//		return;
			//	}

			//}

			players.Add(p);
	}
	//public void RemoveClient(Socket client)
	//{
	//    Clients.Remove(client);
	//}
	public void BroadcastMessage(string message)
	{
		foreach (var client in players)
		{
			byte[] data = Encoding.ASCII.GetBytes(message);
			client._socket.Send(data);
		}
	}

	public bool isReady()
	{
		if (numberOfPlayer == 2)
			return true;
		else
			return false;
	}

	public void startGame()
	{
		GameInProgress = true;
		if (!isReady())
			return;

		Random random = new Random();
		int num = random.Next(40, 60);
		int num2 = random.Next(80, 150);
		if (num < num2)
		{

		}
		else
		{

		}
	}

	public void endGame(string data, Player player)
	{
		GameInProgress = false;
	}


}
}
