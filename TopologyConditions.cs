using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004BD RID: 1213
	public struct TopologyConditions
	{
		// Token: 0x06002922 RID: 10530 RVA: 0x0001D6E0 File Offset: 0x0001B8E0
		public TopologyConditions(TerrainTopology.Enum layer)
		{
			this.Layer = layer;
			this.Texture = new TopologyTextures[31];
			this.CheckLayer = new bool[31];
		}

		// Token: 0x04001613 RID: 5651
		public TerrainTopology.Enum Layer;

		// Token: 0x04001614 RID: 5652
		public TopologyTextures[] Texture;

		// Token: 0x04001615 RID: 5653
		public bool[] CheckLayer;
	}
}
