using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004E2 RID: 1250
	[Serializable]
	public class GeologyCollisions
	{
		// Token: 0x06002946 RID: 10566 RVA: 0x0001D985 File Offset: 0x0001BB85
		public GeologyCollisions(GeologyCollisions geoCollisions)
		{
			this.minMax = geoCollisions.minMax;
			this.radius = geoCollisions.radius;
			this.layer = geoCollisions.layer;
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x00002822 File Offset: 0x00000A22
		public GeologyCollisions()
		{
		}

		// Token: 0x040016CF RID: 5839
		public bool minMax;

		// Token: 0x040016D0 RID: 5840
		public float radius;

		// Token: 0x040016D1 RID: 5841
		public ColliderLayer layer;
	}
}
