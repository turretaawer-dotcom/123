using System;
using UnityEngine;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B3 RID: 1203
	[Serializable]
	public struct WindowState
	{
		// Token: 0x060028FF RID: 10495 RVA: 0x0001D54B File Offset: 0x0001B74B
		public WindowState(bool isActive, Vector3 position, Vector3 scale)
		{
			this.isActive = isActive;
			this.position = position;
			this.scale = scale;
		}

		// Token: 0x040015F2 RID: 5618
		public bool isActive;

		// Token: 0x040015F3 RID: 5619
		public Vector3 position;

		// Token: 0x040015F4 RID: 5620
		public Vector3 scale;
	}
}
