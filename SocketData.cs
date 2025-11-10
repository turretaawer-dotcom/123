using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D2 RID: 1234
	[ProtoContract]
	[Serializable]
	public class SocketData
	{
		// Token: 0x17000AC5 RID: 2757
		[ProtoMember(2)]
		public List<SocketInfo> this[string key]
		{
			get
			{
				if (!this.Dictionary.ContainsKey(key))
				{
					return null;
				}
				return this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		// Token: 0x04001672 RID: 5746
		[ProtoMember(1)]
		public Dictionary<string, List<SocketInfo>> Dictionary = new Dictionary<string, List<SocketInfo>>();
	}
}
