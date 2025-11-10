using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004CD RID: 1229
	[ConsoleVariable("settings for perlinSplat")]
	[Serializable]
	public struct PerlinSplatPreset
	{
		// Token: 0x0400165A RID: 5722
		public int scale;

		// Token: 0x0400165B RID: 5723
		public int splatLayer;

		// Token: 0x0400165C RID: 5724
		public TerrainBiome.Enum biomeLayer;

		// Token: 0x0400165D RID: 5725
		public float strength;

		// Token: 0x0400165E RID: 5726
		public bool invert;

		// Token: 0x0400165F RID: 5727
		public bool paintBiome;
	}
}
