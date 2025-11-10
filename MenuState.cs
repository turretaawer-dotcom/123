using System;
using UnityEngine;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B4 RID: 1204
	[Serializable]
	public struct MenuState
	{
		// Token: 0x06002900 RID: 10496 RVA: 0x0001D562 File Offset: 0x0001B762
		public MenuState(Vector3 scale, Vector3 position)
		{
			this.scale = scale;
			this.position = position;
		}

		// Token: 0x040015F5 RID: 5621
		public Vector3 scale;

		// Token: 0x040015F6 RID: 5622
		public Vector3 position;
	}
}
