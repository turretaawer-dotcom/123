using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B5 RID: 1205
	public interface IUndoAction
	{
		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06002901 RID: 10497
		string OperationName { get; }

		// Token: 0x06002902 RID: 10498
		void Undo();

		// Token: 0x06002903 RID: 10499
		void Redo();

		// Token: 0x06002904 RID: 10500
		void OnRemoved();

		// Token: 0x06002905 RID: 10501
		long EstimateMemoryUsage();
	}
}
