using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004CE RID: 1230
	[ConsoleVariable("settings for figuredRippling")]
	[Serializable]
	public struct RipplePreset
	{
		// Token: 0x04001660 RID: 5728
		public int size;

		// Token: 0x04001661 RID: 5729
		public int density;

		// Token: 0x04001662 RID: 5730
		public float weight;
	}
}
