using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ProtoBuf;

namespace ServerTicTac.Cw
{
    [ProtoContract]
    public class Player
    {
        [ProtoMember(1)]
        private int idRoom;
        [ProtoMember(2)]
        private static int recentId = 0;
        [ProtoIgnore]
        public Socket _socket;

        private ASCIIEncoding encoding;

        //public NetworkStream Stream { get; set; }
        [ProtoMember(3)]
        private int bufferLength = 100;
        public Player(Socket socket)
        {

            encoding = new ASCIIEncoding();
            PlayerId = recentId;
            _socket = socket;
            recentId++;
            //Stream = client.GetStream();
        }
        [ProtoMember(4)]
        public int PlayerId { get; set; }
        [ProtoMember(5)]

        public int RoomId { get; set; }
        [ProtoMember(6)]

        public char Symbol { get; set; }
        [ProtoMember(7)]
        public string Address { get; set; }
        [ProtoMember(8)]
        public string Port { get; set; }

        public int getID() => PlayerId;

        public void getIpAdress(Socket socket)
        {
            Address = socket.AddressFamily.ToString();
        }
        public int getIDRoom() => idRoom;
        public void setIDRoom(int id) { idRoom = id; }
        public void getPorts(Socket socket)
        {
            Port = socket.RemoteEndPoint.ToString();

        }
        public void getSockets(Socket socket)
        {
            _socket = socket;

        }
        //public void sendData(string data)
        //{
        //	byte[] byteSend = encoding.GetBytes(data);
        //	socket.Send(byteSend);
        //	Console.WriteLine($"Send {data}");
        //}
        public string getName() => $"User{PlayerId}";
        public string receiveData()
        {
            byte[] byteReceive = new byte[bufferLength];
            _socket.Receive(byteReceive);

            string data = encoding.GetString(byteReceive);
            Console.WriteLine($"Receive: {data}");
            return data;
        }



    }

    public class Data
    {
        public static Player player { get; set; }
    }
}
