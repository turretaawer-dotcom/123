using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004EB RID: 1259
	[Serializable]
	public struct HeightSelector
	{
		// Token: 0x04001748 RID: 5960
		public float slopeLow;

		// Token: 0x04001749 RID: 5961
		public float slopeHigh;

		// Token: 0x0400174A RID: 5962
		public float heightMin;

		// Token: 0x0400174B RID: 5963
		public float heightMax;

		// Token: 0x0400174C RID: 5964
		public float curveMin;

		// Token: 0x0400174D RID: 5965
		public float curveMax;

		// Token: 0x0400174E RID: 5966
		public float slopeWeight;

		// Token: 0x0400174F RID: 5967
		public float curveWeight;
	}
}
