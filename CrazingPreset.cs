using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004CF RID: 1231
	[ConsoleVariable("settings for splatCrazing")]
	[Serializable]
	public struct CrazingPreset
	{
		// Token: 0x04001663 RID: 5731
		public string title;

		// Token: 0x04001664 RID: 5732
		public int zones;

		// Token: 0x04001665 RID: 5733
		public int minSize;

		// Token: 0x04001666 RID: 5734
		public int maxSize;

		// Token: 0x04001667 RID: 5735
		public int splatLayer;
	}
}
