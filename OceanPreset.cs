using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D1 RID: 1233
	[ConsoleVariable("settings for ocean")]
	[Serializable]
	public struct OceanPreset
	{
		// Token: 0x0400166A RID: 5738
		public string title;

		// Token: 0x0400166B RID: 5739
		public int radius;

		// Token: 0x0400166C RID: 5740
		public int gradient;

		// Token: 0x0400166D RID: 5741
		public int xOffset;

		// Token: 0x0400166E RID: 5742
		public int yOffset;

		// Token: 0x0400166F RID: 5743
		public int s;

		// Token: 0x04001670 RID: 5744
		public int seafloor;

		// Token: 0x04001671 RID: 5745
		public bool perlin;
	}
}
