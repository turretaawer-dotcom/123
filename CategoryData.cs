using System;
using System.Collections.Generic;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004DC RID: 1244
	[ProtoContract]
	[Serializable]
	public class CategoryData
	{
		// Token: 0x06002937 RID: 10551 RVA: 0x0001D863 File Offset: 0x0001BA63
		public CategoryData()
		{
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x0001D876 File Offset: 0x0001BA76
		public CategoryData(BreakingData breakingData)
		{
			this.breakingData = breakingData;
		}

		// Token: 0x04001693 RID: 5779
		[ProtoMember(1)]
		public BreakingData breakingData;

		// Token: 0x04001694 RID: 5780
		[ProtoMember(2)]
		public List<ChildrenData> child = new List<ChildrenData>();
	}
}
