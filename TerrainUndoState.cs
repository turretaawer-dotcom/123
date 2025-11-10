using System;
using UnityEngine;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B8 RID: 1208
	public class TerrainUndoState
	{
		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06002912 RID: 10514 RVA: 0x0001D615 File Offset: 0x0001B815
		public string OperationName { get; }

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06002913 RID: 10515 RVA: 0x0001D61D File Offset: 0x0001B81D
		public TerrainUndoManager.TerrainOperationType OperationType { get; }

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06002914 RID: 10516 RVA: 0x0001D625 File Offset: 0x0001B825
		public object Data { get; }

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06002915 RID: 10517 RVA: 0x0001D62D File Offset: 0x0001B82D
		public int StartX { get; }

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06002916 RID: 10518 RVA: 0x0001D635 File Offset: 0x0001B835
		public int StartY { get; }

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06002917 RID: 10519 RVA: 0x0001D63D File Offset: 0x0001B83D
		public int Width { get; }

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06002918 RID: 10520 RVA: 0x0001D645 File Offset: 0x0001B845
		public int Height { get; }

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06002919 RID: 10521 RVA: 0x0001D64D File Offset: 0x0001B84D
		public TerrainManager.TerrainType TerrainType { get; }

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x0600291A RID: 10522 RVA: 0x0001D655 File Offset: 0x0001B855
		public TerrainManager.LayerType LayerType { get; }

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x0600291B RID: 10523 RVA: 0x0001D65D File Offset: 0x0001B85D
		public int TopologyLayer { get; }

		// Token: 0x0600291C RID: 10524 RVA: 0x000B4C9C File Offset: 0x000B2E9C
		public TerrainUndoState(string name, TerrainUndoManager.TerrainOperationType operationType, object data, int startX, int startY, int width, int height, TerrainManager.TerrainType terrainType = TerrainManager.TerrainType.Land, TerrainManager.LayerType layerType = TerrainManager.LayerType.Ground, int topologyLayer = -1)
		{
			this.OperationName = name;
			this.OperationType = operationType;
			this.Data = data;
			this.StartX = startX;
			this.StartY = startY;
			this.Width = width;
			this.Height = height;
			this.TerrainType = terrainType;
			this.LayerType = layerType;
			this.TopologyLayer = topologyLayer;
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x000B4CFC File Offset: 0x000B2EFC
		public long EstimateMemoryUsage()
		{
			long result = 0L;
			float[,] array = this.Data as float[,];
			if (array != null)
			{
				result = (long)(array.Length * 4);
			}
			else
			{
				float[,,] array2 = this.Data as float[,,];
				if (array2 != null)
				{
					result = (long)(array2.Length * 4);
				}
				else
				{
					bool[,] array3 = this.Data as bool[,];
					if (array3 != null)
					{
						result = (long)array3.Length;
					}
					else
					{
						int[,] array4 = this.Data as int[,];
						if (array4 != null)
						{
							result = (long)(array4.Length * 4);
						}
						else
						{
							Color[] array5 = this.Data as Color[];
							if (array5 != null)
							{
								result = (long)(array5.Length * 4 * 4);
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x0001D665 File Offset: 0x0001B865
		public void LogMemoryUsage()
		{
			Debug.Log(string.Format("State '{0}' memory usage: {1:F2} MB", this.OperationName, (float)this.EstimateMemoryUsage() / 1048576f));
		}
	}
}
