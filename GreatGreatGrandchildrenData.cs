using System;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D8 RID: 1240
	[ProtoContract]
	[Serializable]
	public class GreatGreatGrandchildrenData
	{
		// Token: 0x0600292F RID: 10543 RVA: 0x00002822 File Offset: 0x00000A22
		public GreatGreatGrandchildrenData()
		{
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0001D7CD File Offset: 0x0001B9CD
		public GreatGreatGrandchildrenData(BreakingData breakingData)
		{
			this.breakingData = breakingData;
		}

		// Token: 0x0400168C RID: 5772
		[ProtoMember(1)]
		public BreakingData breakingData;
	}
}
