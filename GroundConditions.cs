using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004BA RID: 1210
	public struct GroundConditions
	{
		// Token: 0x0600291F RID: 10527 RVA: 0x0001D68E File Offset: 0x0001B88E
		public GroundConditions(TerrainSplat.Enum layer)
		{
			this.Layer = layer;
			this.Weight = new float[8];
			this.CheckLayer = new bool[8];
		}

		// Token: 0x0400160B RID: 5643
		public TerrainSplat.Enum Layer;

		// Token: 0x0400160C RID: 5644
		public float[] Weight;

		// Token: 0x0400160D RID: 5645
		public bool[] CheckLayer;
	}
}
