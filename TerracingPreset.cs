using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004CB RID: 1227
	[ConsoleVariable("settings for randomTerracing")]
	[Serializable]
	public struct TerracingPreset
	{
		// Token: 0x0400164C RID: 5708
		public bool flatten;

		// Token: 0x0400164D RID: 5709
		public bool perlinBanks;

		// Token: 0x0400164E RID: 5710
		public bool circular;

		// Token: 0x0400164F RID: 5711
		public float weight;

		// Token: 0x04001650 RID: 5712
		public int zStart;

		// Token: 0x04001651 RID: 5713
		public int gateBottom;

		// Token: 0x04001652 RID: 5714
		public int gateTop;

		// Token: 0x04001653 RID: 5715
		public int gates;

		// Token: 0x04001654 RID: 5716
		public int descaleFactor;

		// Token: 0x04001655 RID: 5717
		public int perlinDensity;
	}
}
