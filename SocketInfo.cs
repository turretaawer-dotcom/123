using System;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D3 RID: 1235
	[ProtoContract]
	[Serializable]
	public struct SocketInfo
	{
		// Token: 0x0600292A RID: 10538 RVA: 0x000B4D98 File Offset: 0x000B2F98
		public static SocketInfo FromDungeonBaseSocket(DungeonBaseSocket socket)
		{
			return new SocketInfo
			{
				Type = socket.Type,
				Male = socket.Male,
				Female = socket.Female,
				Position = socket.transform.localPosition,
				Rotation = socket.transform.rotation.eulerAngles
			};
		}

		// Token: 0x04001673 RID: 5747
		[ProtoMember(1)]
		public DungeonBaseSocketType Type;

		// Token: 0x04001674 RID: 5748
		[ProtoMember(2)]
		public bool Male;

		// Token: 0x04001675 RID: 5749
		[ProtoMember(3)]
		public bool Female;

		// Token: 0x04001676 RID: 5750
		[ProtoMember(4)]
		public WorldSerialization.VectorData Position;

		// Token: 0x04001677 RID: 5751
		[ProtoMember(5)]
		public WorldSerialization.VectorData Rotation;
	}
}
