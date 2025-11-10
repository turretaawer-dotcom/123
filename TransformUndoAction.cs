using System;
using RTG;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B6 RID: 1206
	public class TransformUndoAction : IUndoAction
	{
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06002906 RID: 10502 RVA: 0x0001D572 File Offset: 0x0001B772
		public string OperationName { get; }

		// Token: 0x06002907 RID: 10503 RVA: 0x0001D57A File Offset: 0x0001B77A
		public TransformUndoAction(string name, IUndoRedoAction action)
		{
			this.OperationName = name;
			this._action = action;
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x0001D590 File Offset: 0x0001B790
		public void Undo()
		{
			this._action.Undo();
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x0001D59D File Offset: 0x0001B79D
		public void Redo()
		{
			this._action.Redo();
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x0001D5AA File Offset: 0x0001B7AA
		public void OnRemoved()
		{
			this._action.OnRemovedFromUndoRedoStack();
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x0001D5B7 File Offset: 0x0001B7B7
		public long EstimateMemoryUsage()
		{
			return 1024L;
		}

		// Token: 0x040015F7 RID: 5623
		private readonly IUndoRedoAction _action;
	}
}
