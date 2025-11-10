using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004BB RID: 1211
	public struct BiomeConditions
	{
		// Token: 0x06002920 RID: 10528 RVA: 0x0001D6AF File Offset: 0x0001B8AF
		public BiomeConditions(TerrainBiome.Enum layer)
		{
			this.Layer = layer;
			this.Weight = new float[5];
			this.CheckLayer = new bool[5];
		}

		// Token: 0x0400160E RID: 5646
		public TerrainBiome.Enum Layer;

		// Token: 0x0400160F RID: 5647
		public float[] Weight;

		// Token: 0x04001610 RID: 5648
		public bool[] CheckLayer;
	}
}
