using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004CC RID: 1228
	[ConsoleVariable("settings for perlinSimple and perlinRidiculous")]
	[Serializable]
	public struct PerlinPreset
	{
		// Token: 0x04001656 RID: 5718
		public int layers;

		// Token: 0x04001657 RID: 5719
		public int period;

		// Token: 0x04001658 RID: 5720
		public int scale;

		// Token: 0x04001659 RID: 5721
		public bool simple;
	}
}
