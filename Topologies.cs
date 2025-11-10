using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004EA RID: 1258
	[Flags]
	[Serializable]
	public enum Topologies
	{
		// Token: 0x04001729 RID: 5929
		Field = 1,
		// Token: 0x0400172A RID: 5930
		Cliff = 2,
		// Token: 0x0400172B RID: 5931
		Summit = 4,
		// Token: 0x0400172C RID: 5932
		Beachside = 8,
		// Token: 0x0400172D RID: 5933
		Beach = 16,
		// Token: 0x0400172E RID: 5934
		Forest = 32,
		// Token: 0x0400172F RID: 5935
		Forestside = 64,
		// Token: 0x04001730 RID: 5936
		Ocean = 128,
		// Token: 0x04001731 RID: 5937
		Oceanside = 256,
		// Token: 0x04001732 RID: 5938
		Decor = 512,
		// Token: 0x04001733 RID: 5939
		Monument = 1024,
		// Token: 0x04001734 RID: 5940
		Road = 2048,
		// Token: 0x04001735 RID: 5941
		Roadside = 4096,
		// Token: 0x04001736 RID: 5942
		Swamp = 8192,
		// Token: 0x04001737 RID: 5943
		River = 16384,
		// Token: 0x04001738 RID: 5944
		Riverside = 32768,
		// Token: 0x04001739 RID: 5945
		Lake = 65536,
		// Token: 0x0400173A RID: 5946
		Lakeside = 131072,
		// Token: 0x0400173B RID: 5947
		Offshore = 262144,
		// Token: 0x0400173C RID: 5948
		Powerline = 524288,
		// Token: 0x0400173D RID: 5949
		Runway = 1048576,
		// Token: 0x0400173E RID: 5950
		Building = 2097152,
		// Token: 0x0400173F RID: 5951
		Cliffside = 4194304,
		// Token: 0x04001740 RID: 5952
		Mountain = 8388608,
		// Token: 0x04001741 RID: 5953
		Clutter = 16777216,
		// Token: 0x04001742 RID: 5954
		Alt = 33554432,
		// Token: 0x04001743 RID: 5955
		Tier0 = 67108864,
		// Token: 0x04001744 RID: 5956
		Tier1 = 134217728,
		// Token: 0x04001745 RID: 5957
		Tier2 = 268435456,
		// Token: 0x04001746 RID: 5958
		Mainland = 536870912,
		// Token: 0x04001747 RID: 5959
		Hilltop = 1073741824
	}
}
