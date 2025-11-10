using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004DB RID: 1243
	[ProtoContract]
	[Serializable]
	public class ChildrenData
	{
		// Token: 0x06002935 RID: 10549 RVA: 0x0001D836 File Offset: 0x0001BA36
		public ChildrenData()
		{
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x0001D849 File Offset: 0x0001BA49
		public ChildrenData(BreakingData breakingData)
		{
			this.breakingData = breakingData;
		}

		// Token: 0x04001691 RID: 5777
		[ProtoMember(1)]
		public BreakingData breakingData;

		// Token: 0x04001692 RID: 5778
		[ProtoMember(2)]
		public List<GrandchildrenData> grandchild = new List<GrandchildrenData>();
	}
}
