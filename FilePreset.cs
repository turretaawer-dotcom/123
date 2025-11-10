using System;
using System.Collections.Generic;

namespace RustMapEditor.Variables
{
	// Token: 0x020004ED RID: 1261
	[Serializable]
	public struct FilePreset
	{
		// Token: 0x04001759 RID: 5977
		public string rustDirectory;

		// Token: 0x0400175A RID: 5978
		public float prefabRenderDistance;

		// Token: 0x0400175B RID: 5979
		public float pathRenderDistance;

		// Token: 0x0400175C RID: 5980
		public float waterTransparency;

		// Token: 0x0400175D RID: 5981
		public bool loadbundleonlaunch;

		// Token: 0x0400175E RID: 5982
		public bool terrainTextureSet;

		// Token: 0x0400175F RID: 5983
		public int loadBatch;

		// Token: 0x04001760 RID: 5984
		public int newSize;

		// Token: 0x04001761 RID: 5985
		public float newHeight;

		// Token: 0x04001762 RID: 5986
		public TerrainBiome.Enum newBiome;

		// Token: 0x04001763 RID: 5987
		public TerrainSplat.Enum newSplat;

		// Token: 0x04001764 RID: 5988
		public string startupSkin;

		// Token: 0x04001765 RID: 5989
		public List<string> recentFiles;
	}
}
