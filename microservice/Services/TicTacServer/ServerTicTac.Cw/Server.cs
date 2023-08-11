using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Net.NetworkInformation;
using System;
using System.Threading;
using System.Diagnostics.Metrics;
using ProtoBuf;
using Microsoft.Extensions.Logging;
using System.Numerics;
using ServerTicTac.Cw.Models;

namespace ServerTicTac.Cw
{
	public class Server
	{
		private static readonly ILogger _logger =
		   LoggerFactory.Create(builder => builder.AddConsole())
			   .CreateLogger<Server>();
		public const int Port = 8888;
		public static byte[] Bufferstatic = new byte[1024];
		public static readonly Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		public static readonly List<Socket> Clients = new List<Socket>();
		public static readonly List<Room> Rooms = new List<Room>();
		public static readonly List<Room> getRoomForN = new List<Room>();
		public static readonly List<Player> getPlayer = new List<Player>();
		public static int currentid { get; set; }

		public static TcpClient TcpClient { get; set; }
		public static void SetupServer()
		{
			ServerSocket.Bind(new IPEndPoint(IPAddress.Any, Port));
			ServerSocket.Listen();
			while (true)
			{
				Socket clientSocket = Server.ServerSocket.Accept();
				Player player = new Player(clientSocket);

				player.getIpAdress(clientSocket);
				player.getPorts(clientSocket);
				CallBackName<Player> CallBackName = new CallBackName<Player>
				{
					dateTime = DateTime.Now,
					DateSet = true,
					NumberData = 1,
					CallbackState = CallBackName<Player>.State.Player,
					getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(player)
				};
				if (CallBackName.NumberData == 1 && CallBackName.DateSet == true)
				{
					byte[] serializedata = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackName);
					clientSocket.Send(serializedata);
					//TODO Recive sock as client
					Console.WriteLine("Client connected: " + clientSocket.RemoteEndPoint);
					using (LoggerDbContext connection = new LoggerDbContext())
					{
						//connection.Open();
						var log = "Client Start : " + clientSocket.RemoteEndPoint;
						LogMessageReciver logMessageRecivers = new LogMessageReciver()
						{
							MessageLogs = log,
						};
						connection.LogMessageRecivers.Add(logMessageRecivers);
						connection.SaveChanges();
					}
					//loggerMessages.MessageLogs = "Client connected : " + clientSocket.RemoteEndPoint;

					_logger.LogInformation("This is a test ...");

					// Create a new thread to handle the client's connection
					Thread clientThread = new Thread(() => Server.HandleClient(clientSocket, player));
					clientThread.Start();
				}
			}
		}

		public static async void HandleClient(Socket clientSocket, object obj)
		{
			Room defaultRoom = GetDefaultRoom(obj);
			if (defaultRoom == null)
			{
				Room room = HandleUserNoRoom(clientSocket, obj);
				HandleRoom(room, clientSocket, obj);
			}
			else
			{
				HandleRoom(defaultRoom, clientSocket, obj);
			}
		}
		public static async void HandleRoom(Room room, Socket clientSocket, object player)
		{
			while (true)
			{

				if (room != null && room.players.Count > 1)
				{
					try
					{
						object sys = new object();

						Player players2 = player as Player;
						int currentid = room.players[0].PlayerId;
						room.CurrentPlayerId = currentid;
						room.players[0].Symbol = 'X';
						room.players[1].Symbol = 'O';


						CallBackName<Room> CallBackNameListRoom = new CallBackName<Room>
						{
							dateTime = DateTime.Now,
							DateSet = true,
							NumberData = 1,
							CallbackState = CallBackName<Room>.State.room,
							getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
						};

						byte[] serializedData = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameListRoom);

						players2._socket.Send(serializedData);

					}
					catch (NullReferenceException ex)
					{
						Console.WriteLine("Error: " + ex.Message);
						_logger.LogError(ex.Message, "Return Null Refrence Eception in Code Line 107");
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "return error in code 111.");
						Console.WriteLine(ex);
					}

					while (true)
					{
						try
						{

							Player playerss = player as Player;
							#region Recive btn
							byte[] buffer = new byte[1];
							clientSocket.Receive(buffer);
							if (buffer[0] == 1)
							{
								room.btn1 = playerss.Symbol;

								CallBackName<Room> CallBackNameRoom1 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar1 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom1);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar1);
								}

								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}
								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent1 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom1 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent1);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom1);
								}
							}
							if (buffer[0] == 2)
							{
								room.btn2 = playerss.Symbol;

								CallBackName<Room> CallBackNameRoom2 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar2 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom2);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar2);
								}

								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}
								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent2 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom2 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent2);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom2);
								}
							}
							if (buffer[0] == 3)
							{
								room.btn3 = playerss.Symbol;

								CallBackName<Room> CallBackNameRoom3 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar3 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom3);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar3);
								}
								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}

								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent3 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom3 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent3);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom3);
								}
							}
							if (buffer[0] == 4)
							{
								room.btn4 = playerss.Symbol;

								CallBackName<Room> CallBackNameRoom4 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar4 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom4);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar4);
								}

								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}

								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent4 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom4 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent4);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom4);
								}
							}
							if (buffer[0] == 5)
							{
								room.btn5 = playerss.Symbol;

								CallBackName<Room> CallBackNameRoom5 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar5 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom5);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar5);
								}

								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}
								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent5 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom5 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent5);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom5);
								}
							}
							if (buffer[0] == 6)
							{
								room.btn6 = playerss.Symbol;


								CallBackName<Room> CallBackNameRoom6 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar6 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom6);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar6);
								}

								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}
								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent6 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom6 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent6);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom6);
								}
							}
							if (buffer[0] == 7)
							{
								room.btn7 = playerss.Symbol;


								CallBackName<Room> CallBackNameRoom7 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar7 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom7);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar7);
								}
								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}
								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent7 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom7 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent7);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom7);
								}
							}
							if (buffer[0] == 8)
							{
								room.btn8 = playerss.Symbol;


								CallBackName<Room> CallBackNameRoom8 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar8 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom8);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar8);
								}

								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}
								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent8 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom8 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent8);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom8);
								}
							}
							if (buffer[0] == 9)
							{
								room.btn9 = playerss.Symbol;

								CallBackName<Room> CallBackNameRoom9 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDatar9 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoom9);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDatar9);
								}

								foreach (var item in room.players)
								{
									item._socket.Send(buffer);
								}
								var player23 = room.players.Where(g => g.PlayerId != room.CurrentPlayerId).First();
								currentid = player23.PlayerId;
								room.CurrentPlayerId = player23.PlayerId;
								CallBackName<Room> CallBackNameRoomsforcuurent9 = new CallBackName<Room>
								{
									dateTime = DateTime.Now,
									DateSet = true,
									NumberData = 1,
									CallbackState = CallBackName<Room>.State.room,
									getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(room)
								};
								byte[] serializedDataForRoom9 = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameRoomsforcuurent9);
								foreach (var item in room.players)
								{
									item._socket.Send(serializedDataForRoom9);
								}
							}
							#endregion
						}
						catch (Exception ex)
						{
							_logger.LogError(ex, "Error Message in Line 371");
							Console.WriteLine(ex);
							break;
						}

					}//End


				}
				else if (room.players.Count < 2)
				{
					continue;
				}
			}

		}


		#region Method
		public static Room HandleUserNoRoom(Socket socket, object obj)
		{
			Room room = ReciveDataForConcceptRoom_UserNoRoom(socket, obj);
			return room;
		}
		public static void ReciveDataRoom(Socket socket)
		{
			byte[] receivedData = new byte[4096];
			int bytesReadss = socket.Receive(receivedData);
			string message = Encoding.ASCII.GetString(receivedData, 0, bytesReadss);
			foreach (var item in message)
			{
				Console.WriteLine(item);
			}
		}
		public static Room GetDefaultRoom(object obj)
		{
			Player player = obj as Player;
			string name = player.getName();
			int getRoomPlayer = player.getIDRoom();
			Room defaultRoom = Rooms.Find(r => r.RoomId == getRoomPlayer);
			// TODO MANAGER ERROR FOR NULL defaultRoom
			return defaultRoom;
		}
		public static List<Room> CheckListRoom()
		{
			var List = Rooms.Where(g => g.players.Count < 2).ToList();
			if (List == null)
			{
				return null;
			}
			return List;
		}
		public static Room CreateOrGetRoom(Socket socket, object obj)
		{
			Player playerobjs = obj as Player;
			Random rd = new Random();
			var newRoom = new Room(rd.Next(0, 2000));

			newRoom.players = new List<Player>();
			newRoom.addPlayer(playerobjs);
			Rooms.Add(newRoom);

			return newRoom;
		}

		#region UserNoRoom

		public static Room ReciveDataForConcceptRoom_UserNoRoom(Socket socket, object obj)
		{

			byte[] receivedData = new byte[1080];
			// try catch for keep server running

			while (true)
			{
				int bytesReadss = socket.Receive(receivedData);
				string message = Encoding.ASCII.GetString(receivedData, 0, bytesReadss);
				if (message.ToString() == "GetRoomList")
				{
					Player players = obj as Player;
					List<Room> checkedd = CheckListRoom();

					foreach (var item in checkedd)
					{
						//Thread.Sleep(2000);
						CallBackName<Room> CallBackNameListRoom = new CallBackName<Room>
						{
							dateTime = DateTime.Now,
							DateSet = true,
							NumberData = 1,
							CallbackState = CallBackName<Room>.State.room,
							getObjectData = DeserializeObjectType.SerializeObjectWithInspectingType(item)
						};

						byte[] serializedata = DeserializeObjectType.SerializeObjectWithInspectingType(CallBackNameListRoom);
						socket.Send(serializedata);
						//Thread.Sleep(300);
					}
					if (checkedd == null)
					{
						_logger.LogWarning("Exception Warning for ToDo Send Not Found Room Code Line 398");
						// TODO Send Not Room 
						throw new Exception("Not Found Room !");
					}
					continue;
				}
				else if (receivedData[0] == 1)
				{
					Room rooms = CreateOrGetRoom(socket, obj);
					return rooms;
				}
				else
				{
					Player player = obj as Player;
					int receivedInt = BitConverter.ToInt32(receivedData, 0);
					Room room = Rooms.Find(g => g.RoomId == receivedInt);
					if (room != null)
					{
						room.addPlayer(player);
						// Send Room for Game
						return room;
					}
					else
					{
						_logger.LogError("Error: Room not found Error code Line 505");
						// Handle the case where the room was not found
						Console.WriteLine("Error: Room not found");
					}
				}
			}
		}

		#endregion

		#endregion
		#region Logic game
		private bool CheckState(Room room, Player player)
		{
			//Horizontals
			if (room.btn1.ToString() == room.btn2.ToString() && room.btn2.ToString() == room.btn3.ToString() && room.btn3.ToString() != "")
			{
				if (room.btn1.ToString()[0] == player.Symbol)
				{

					Console.WriteLine("You won");
				}
				else
				{

					Console.WriteLine("You Lost!");
				}
				return true;
			}

			else if (room.btn4.ToString() == room.btn5.ToString() && room.btn5.ToString() == room.btn6.ToString() && room.btn6.ToString() != "")
			{
				if (room.btn4.ToString()[0] == player.Symbol)
				{
					//label1.Text = "You Won!";
					//MessageBox.Show("You Won!");
					Console.WriteLine("you won !");
				}
				else
				{
					Console.WriteLine("You Lost");
				}
				return true;
			}

			else if (room.btn7.ToString() == room.btn8.ToString() && room.btn8.ToString() == room.btn9.ToString() && room.btn9.ToString() != "")
			{
				if (room.btn7.ToString()[0] == player.Symbol)
				{
					//label1.Text = "You Won!";
					//MessageBox.Show("You Won!");
					Console.WriteLine();
				}
				else
				{

					Console.WriteLine("You Lost!");
				}
				return true;
			}

			//Verticals
			else if (room.btn1.ToString() == room.btn4.ToString() && room.btn4.ToString() == room.btn7.ToString() && room.btn7.ToString() != "")
			{
				if (room.btn1.ToString()[0] == player.Symbol)
				{
					//label1.Text = "You Won!";
					//MessageBox.Show("You Won!");
					Console.WriteLine("You Won !");
				}
				else
				{
					//label1.Text = "You Lost!";
					//MessageBox.Show("You Lost!");
					Console.WriteLine("You Lost !");
				}
				return true;
			}

			else if (room.btn2.ToString() == room.btn5.ToString() && room.btn5.ToString() == room.btn8.ToString() && room.btn8.ToString() != "")
			{
				if (room.btn2.ToString()[0] == player.Symbol)
				{
					//label1.Text = "You Won!";
					//MessageBox.Show("You Won!");
					Console.WriteLine("You Won !");
				}
				else
				{
					//label1.Text = "You Lost!";
					//MessageBox.Show("You Lost!");
					Console.WriteLine("You Lost !");
				}
				return true;
			}

			else if (room.btn3.ToString() == room.btn6.ToString() && room.btn6.ToString() == room.btn9.ToString() && room.btn9.ToString() != "")
			{
				if (room.btn3.ToString()[0] == player.Symbol)
				{
					//label1.Text = "You Won!";
					//MessageBox.Show("You Won!");
					Console.WriteLine("You Won!");
				}
				else
				{
					//label1.Text = "You Lost!";
					//MessageBox.Show("You Lost!");
					Console.WriteLine("You Lost!");
				}
				return true;
			}

			else if (room.btn1.ToString() == room.btn5.ToString() && room.btn5.ToString() == room.btn9.ToString() && room.btn9.ToString() != "")
			{
				if (room.btn1.ToString()[0] == player.Symbol)
				{
					//label1.Text = "You Won!";
					//MessageBox.Show("You Won!");



					Console.WriteLine("You Won!");
				}
				else
				{
					//label1.Text = "You Lost!";
					//MessageBox.Show("You Lost!");
				}
				return true;
			}

			else if (room.btn3.ToString() == room.btn5.ToString() && room.btn5.ToString() == room.btn7.ToString() && room.btn7.ToString() != "")
			{
				if (room.btn3.ToString()[0] == player.Symbol)
				{
					//label1.Text = "You Won!";
					//MessageBox.Show("You Won!");
					Console.WriteLine("You Won!");
				}
				else
				{
					//label1.Text = "You Lost!";
					//MessageBox.Show("You Lost!");
					Console.WriteLine("You Lost!");
				}
				return true;
			}

			//Draw
			else if (room.btn1.ToString() != "" && room.btn2.ToString() != "" && room.btn3.ToString() != "" && room.btn4.ToString() != "" && room.btn5.ToString() != "" && room.btn6.ToString() != "" && room.btn7.ToString() != "" && room.btn8.ToString() != "" && room.btn9.ToString() != "")
			{
				//label1.Text = "It's a draw!";
				//MessageBox.Show("It's a draw!");
				Console.WriteLine("It's a draw!");
				return true;
			}
			return false;
		}
		private void FreezeBoard(Room room)
		{

			room.Activebtn1 = false;
			room.Activebtn2 = false;
			room.Activebtn3 = false;
			room.Activebtn4 = false;
			room.Activebtn5 = false;
			room.Activebtn6 = false;
			room.Activebtn7 = false;
			room.Activebtn8 = false;
			room.Activebtn9 = false;
		}

		private void UnfreezeBoard(Room room)
		{
			if (room.btn1 != null)
				room.Activebtn1 = true;
			if (room.btn2 != null)
				room.Activebtn2 = true;
			if (room.btn3 != null)
				room.Activebtn3 = true;
			if (room.btn4 != null)
				room.Activebtn4 = true;
			if (room.btn5 != null)
				room.Activebtn5 = true;
			if (room.btn6 != null)
				room.Activebtn6 = true;
			if (room.btn7 != null)
				room.Activebtn7 = true;
			if (room.btn8 != null)
				room.Activebtn8 = true;
			if (room.btn9 != null)
				room.Activebtn9 = true;
		}

		private void ReceiveMove(Room room, Socket sock, Player player, byte[] bytes)
		{
			///byte[] buffer = new byte[1];
			byte[] buffer = bytes;
			//sock.Receive(buffer);
			if (buffer[0] == 1)
				room.btn1 = player.Symbol;
			if (buffer[0] == 2)
				room.btn2 = player.Symbol;
			if (buffer[0] == 3)
				room.btn3 = player.Symbol;
			if (buffer[0] == 4)
				room.btn4 = player.Symbol;
			if (buffer[0] == 5)
				room.btn5 = player.Symbol;
			if (buffer[0] == 6)
				room.btn6 = player.Symbol;
			if (buffer[0] == 7)
				room.btn7 = player.Symbol;
			if (buffer[0] == 8)
				room.btn8 = player.Symbol;
			if (buffer[0] == 9)
				room.btn9 = player.Symbol;

		}

		#endregion



	}
}
