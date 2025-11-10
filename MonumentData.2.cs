using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D7 RID: 1239
	[ProtoContract]
	[Serializable]
	public class MonumentData
	{
		// Token: 0x0400168A RID: 5770
		[ProtoMember(1)]
		public List<CategoryData> category = new List<CategoryData>();

		// Token: 0x0400168B RID: 5771
		[ProtoMember(2)]
		public string monumentName;
	}
}
