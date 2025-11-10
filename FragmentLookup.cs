using System;
using System.Collections.Generic;

namespace RustMapEditor.Variables
{
	// Token: 0x020004DF RID: 1247
	[Serializable]
	public class FragmentLookup
	{
		// Token: 0x0600293B RID: 10555 RVA: 0x0001D8CD File Offset: 0x0001BACD
		public void LoadPairList(List<FragmentPair> fragmentPairs)
		{
			this.fragmentPairs = fragmentPairs;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0001D8D6 File Offset: 0x0001BAD6
		public void Deserialize()
		{
			this.fragmentNamelist = SettingsManager.ListToDict(this.fragmentPairs);
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x0001D8E9 File Offset: 0x0001BAE9
		public void Serialize()
		{
			this.fragmentPairs = SettingsManager.DictToList(this.fragmentNamelist);
		}

		// Token: 0x0400169C RID: 5788
		public List<FragmentPair> fragmentPairs = new List<FragmentPair>();

		// Token: 0x0400169D RID: 5789
		public Dictionary<string, uint> fragmentNamelist = new Dictionary<string, uint>();
	}
}
