using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004DE RID: 1246
	[Serializable]
	public struct FragmentPair
	{
		// Token: 0x0600293A RID: 10554 RVA: 0x0001D8BD File Offset: 0x0001BABD
		public FragmentPair(string fragment, uint id)
		{
			this.fragment = fragment;
			this.id = id;
		}

		// Token: 0x0400169A RID: 5786
		public string fragment;

		// Token: 0x0400169B RID: 5787
		public uint id;
	}
}
