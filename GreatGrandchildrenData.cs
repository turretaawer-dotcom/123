using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D9 RID: 1241
	[ProtoContract]
	[Serializable]
	public class GreatGrandchildrenData
	{
		// Token: 0x06002931 RID: 10545 RVA: 0x0001D7DC File Offset: 0x0001B9DC
		public GreatGrandchildrenData()
		{
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x0001D7EF File Offset: 0x0001B9EF
		public GreatGrandchildrenData(BreakingData breakingData)
		{
			this.breakingData = breakingData;
		}

		// Token: 0x0400168D RID: 5773
		[ProtoMember(1)]
		public BreakingData breakingData;

		// Token: 0x0400168E RID: 5774
		[ProtoMember(2)]
		public List<GreatGreatGrandchildrenData> greatgreatgrandchild = new List<GreatGreatGrandchildrenData>();
	}
}
