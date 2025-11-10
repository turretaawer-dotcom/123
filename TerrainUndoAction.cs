using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B7 RID: 1207
	public class TerrainUndoAction : IUndoAction
	{
		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x0600290C RID: 10508 RVA: 0x0001D5BF File Offset: 0x0001B7BF
		public string OperationName
		{
			get
			{
				return this._undoState.OperationName;
			}
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x0001D5CC File Offset: 0x0001B7CC
		public TerrainUndoAction(TerrainUndoState undoState, TerrainUndoState redoState)
		{
			this._undoState = undoState;
			this._redoState = redoState;
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x0001D5E2 File Offset: 0x0001B7E2
		public void Undo()
		{
			TerrainUndoManager.ApplyState(this._undoState);
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x0001D5EF File Offset: 0x0001B7EF
		public void Redo()
		{
			TerrainUndoManager.ApplyState(this._redoState);
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000026C7 File Offset: 0x000008C7
		public void OnRemoved()
		{
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x0001D5FC File Offset: 0x0001B7FC
		public long EstimateMemoryUsage()
		{
			return this._undoState.EstimateMemoryUsage() + this._redoState.EstimateMemoryUsage();
		}

		// Token: 0x040015F9 RID: 5625
		private readonly TerrainUndoState _undoState;

		// Token: 0x040015FA RID: 5626
		private readonly TerrainUndoState _redoState;
	}
}
