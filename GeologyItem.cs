using System;
using System.Linq;

namespace RustMapEditor.Variables
{
	// Token: 0x020004E1 RID: 1249
	[Serializable]
	public class GeologyItem
	{
		// Token: 0x0600293F RID: 10559 RVA: 0x000B4E64 File Offset: 0x000B3064
		public GeologyItem Clone()
		{
			return new GeologyItem
			{
				custom = this.custom,
				customPrefab = this.customPrefab,
				prefabID = this.prefabID,
				emphasis = this.emphasis,
				maximum = this.maximum,
				sectors = this.sectors,
				minDepth = this.minDepth,
				maxDepth = this.maxDepth
			};
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000B4ED8 File Offset: 0x000B30D8
		public GeologyItem(uint prefabID, int minDepth = 0, int maxDepth = 2147483647)
		{
			this.prefabID = prefabID;
			this.custom = false;
			this.customPrefab = "";
			this.emphasis = 1;
			this.maximum = int.MaxValue;
			this.sectors = "";
			this.minDepth = Math.Max(0, minDepth);
			this.maxDepth = Math.Max(minDepth, maxDepth);
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000B4F3C File Offset: 0x000B313C
		public GeologyItem(GeologyItem geoItem)
		{
			this.prefabID = geoItem.prefabID;
			this.custom = geoItem.custom;
			this.customPrefab = geoItem.customPrefab;
			this.emphasis = geoItem.emphasis;
			this.maximum = geoItem.maximum;
			this.sectors = geoItem.sectors;
			this.minDepth = geoItem.minDepth;
			this.maxDepth = geoItem.maxDepth;
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x0001D91A File Offset: 0x0001BB1A
		public GeologyItem()
		{
			this.emphasis = 1;
			this.maximum = int.MaxValue;
			this.sectors = "";
			this.minDepth = 0;
			this.maxDepth = int.MaxValue;
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000B4FB0 File Offset: 0x000B31B0
		public GeologyItem(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				this.custom = false;
				this.prefabID = 0U;
				this.customPrefab = "";
				this.emphasis = Math.Max(1, this.emphasis);
				this.maximum = Math.Max(1, this.maximum);
				this.sectors = this.sectors;
				this.minDepth = Math.Max(0, this.minDepth);
				this.maxDepth = Math.Max(this.minDepth, this.maxDepth);
				return;
			}
			if (path[0] == '~')
			{
				path = path.Replace("~", "").Replace("\\", "/");
				this.custom = true;
				this.customPrefab = path;
				this.prefabID = 0U;
				return;
			}
			this.custom = false;
			this.prefabID = AssetManager.ToID(path + ".prefab");
			this.customPrefab = "";
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x000B50A8 File Offset: 0x000B32A8
		public GeologyItem(string path, int emphasis = 1, int maximum = 2147483647, string sectors = "", int minDepth = 0, int maxDepth = 2147483647)
		{
			if (string.IsNullOrEmpty(path))
			{
				this.custom = false;
				this.prefabID = 0U;
				this.customPrefab = "";
				this.emphasis = Math.Max(1, emphasis);
				this.maximum = Math.Max(1, maximum);
				this.sectors = sectors;
				this.minDepth = Math.Max(0, minDepth);
				this.maxDepth = Math.Max(minDepth, maxDepth);
				return;
			}
			if (path[0] == '~')
			{
				path = path.Replace("~", "").Replace("\\", "/");
				this.custom = true;
				this.customPrefab = path;
				this.prefabID = 0U;
			}
			else
			{
				this.custom = false;
				this.prefabID = AssetManager.ToID(path + ".prefab");
				this.customPrefab = "";
			}
			this.emphasis = Math.Max(1, emphasis);
			this.maximum = Math.Max(1, maximum);
			this.sectors = sectors;
			this.minDepth = Math.Max(0, minDepth);
			this.maxDepth = Math.Max(minDepth, maxDepth);
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x0001D951 File Offset: 0x0001BB51
		public bool CanConnectTo(GeologyItem other)
		{
			return string.IsNullOrEmpty(this.sectors) || string.IsNullOrEmpty(other.sectors) || this.sectors.Intersect(other.sectors).Any<char>();
		}

		// Token: 0x040016C7 RID: 5831
		public string customPrefab;

		// Token: 0x040016C8 RID: 5832
		public uint prefabID;

		// Token: 0x040016C9 RID: 5833
		public int emphasis;

		// Token: 0x040016CA RID: 5834
		public int maximum;

		// Token: 0x040016CB RID: 5835
		public string sectors;

		// Token: 0x040016CC RID: 5836
		public bool custom;

		// Token: 0x040016CD RID: 5837
		public int minDepth;

		// Token: 0x040016CE RID: 5838
		public int maxDepth;
	}
}
