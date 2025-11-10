using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004DA RID: 1242
	[ProtoContract]
	[Serializable]
	public class GrandchildrenData
	{
		// Token: 0x06002933 RID: 10547 RVA: 0x0001D809 File Offset: 0x0001BA09
		public GrandchildrenData()
		{
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0001D81C File Offset: 0x0001BA1C
		public GrandchildrenData(BreakingData breakingData)
		{
			this.breakingData = breakingData;
		}

		// Token: 0x0400168F RID: 5775
		[ProtoMember(1)]
		public BreakingData breakingData;

		// Token: 0x04001690 RID: 5776
		[ProtoMember(2)]
		public List<GreatGrandchildrenData> greatgrandchild = new List<GreatGrandchildrenData>();
	}
}
