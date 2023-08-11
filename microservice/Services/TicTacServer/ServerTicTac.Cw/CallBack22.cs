using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace ServerTicTac.Cw
{
	[DataContract]
	public class CallBack22<T>
	{

		public enum State
		{
			room,
			ListRoom,
			Player
		}

		[ProtoMember(1, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public DateTime dateTime { get; set; }
		[ProtoMember(2, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public State CallbackState { get; set; }
		[ProtoMember(3, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public bool DateSet = true;
		[ProtoMember(4, DataFormat = DataFormat.FixedSize, IsRequired = true)]
		public int NumberData { get; set; }
		[ProtoMember(5, DataFormat = DataFormat.Default)]
		public byte[] getObjectData { get; set; }
	}
}
