using System;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D0 RID: 1232
	[ProtoContract]
	[Serializable]
	public struct BreakerPreset
	{
		// Token: 0x04001668 RID: 5736
		[ProtoMember(1)]
		public string title;

		// Token: 0x04001669 RID: 5737
		[ProtoMember(2)]
		public MonumentData monument;
	}
}
