using System;

namespace UIRecycleTreeNamespace
{
	// Token: 0x02000209 RID: 521
	public interface IRecycleDataSource
	{
		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000E8C RID: 3724
		int expandedCount { get; }

		// Token: 0x06000E8D RID: 3725
		void MergeDataWithView(RecycleItem recycleItem, int indexInExpandedNodes);
	}
}
