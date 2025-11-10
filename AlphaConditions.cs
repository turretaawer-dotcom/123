using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004BC RID: 1212
	public struct AlphaConditions
	{
		// Token: 0x06002921 RID: 10529 RVA: 0x0001D6D0 File Offset: 0x0001B8D0
		public AlphaConditions(AlphaTextures texture)
		{
			this.Texture = texture;
			this.CheckAlpha = false;
		}

		// Token: 0x04001611 RID: 5649
		public AlphaTextures Texture;

		// Token: 0x04001612 RID: 5650
		public bool CheckAlpha;
	}
}
