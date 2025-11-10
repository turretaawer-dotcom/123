using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004E5 RID: 1253
	[Serializable]
	public class WebRoadPreset
	{
		// Token: 0x040016D4 RID: 5844
		public float WaterlineHeight = 0.501f;

		// Token: 0x040016D5 RID: 5845
		public float MinNodeDistance = 15f;

		// Token: 0x040016D6 RID: 5846
		public float MaxNodeDistance = 20f;

		// Token: 0x040016D7 RID: 5847
		public float MaxSlope = 25f;

		// Token: 0x040016D8 RID: 5848
		public float MapEdgeDistance = 30f;

		// Token: 0x040016D9 RID: 5849
		public float CollisionRadius = 10f;

		// Token: 0x040016DA RID: 5850
		public int MaxRetries = 5;

		// Token: 0x040016DB RID: 5851
		public static readonly WebRoadPreset Default = new WebRoadPreset
		{
			WaterlineHeight = 0.501f,
			MinNodeDistance = 15f,
			MaxNodeDistance = 20f,
			MaxSlope = 25f,
			MapEdgeDistance = 30f,
			CollisionRadius = 10f,
			MaxRetries = 5
		};
	}
}
