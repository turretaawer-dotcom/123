using System;
using System.Collections.Generic;
using UnityEngine;

namespace RustMapEditor.Variables
{
	// Token: 0x020004E8 RID: 1256
	[Serializable]
	public struct GeologyPreset
	{
		// Token: 0x0600294B RID: 10571 RVA: 0x000B5284 File Offset: 0x000B3484
		public GeologyPreset(string title)
		{
			this = default(GeologyPreset);
			this.title = (title ?? "DefaultPreset");
			this.filename = (title ?? "DefaultPreset");
			this.geologyItems = new List<GeologyItem>();
			this.geologyCollisions = new List<GeologyCollisions>();
			this.newCollisions = new GeologyCollisions();
			this.heights = new HeightSelector
			{
				slopeLow = 0f,
				slopeHigh = 90f,
				heightMin = 0f,
				heightMax = 1000f,
				curveMin = 0f,
				curveMax = 1f,
				slopeWeight = 1f,
				curveWeight = 1f
			};
		}

		// Token: 0x040016E0 RID: 5856
		public List<GeologyItem> geologyItems;

		// Token: 0x040016E1 RID: 5857
		public List<GeologyCollisions> geologyCollisions;

		// Token: 0x040016E2 RID: 5858
		public GeologyCollisions newCollisions;

		// Token: 0x040016E3 RID: 5859
		public string filename;

		// Token: 0x040016E4 RID: 5860
		public string title;

		// Token: 0x040016E5 RID: 5861
		public int density;

		// Token: 0x040016E6 RID: 5862
		public int frequency;

		// Token: 0x040016E7 RID: 5863
		public int floor;

		// Token: 0x040016E8 RID: 5864
		public int ceiling;

		// Token: 0x040016E9 RID: 5865
		public int biomeIndex;

		// Token: 0x040016EA RID: 5866
		public int seed;

		// Token: 0x040016EB RID: 5867
		public int spawns;

		// Token: 0x040016EC RID: 5868
		public TerrainBiome.Enum biomeLayer;

		// Token: 0x040016ED RID: 5869
		public ColliderLayer colliderLayer;

		// Token: 0x040016EE RID: 5870
		public ColliderLayer closeColliderLayer;

		// Token: 0x040016EF RID: 5871
		public bool avoidTopo;

		// Token: 0x040016F0 RID: 5872
		public bool flipping;

		// Token: 0x040016F1 RID: 5873
		public bool tilting;

		// Token: 0x040016F2 RID: 5874
		public bool normalizeX;

		// Token: 0x040016F3 RID: 5875
		public bool normalizeY;

		// Token: 0x040016F4 RID: 5876
		public bool normalizeZ;

		// Token: 0x040016F5 RID: 5877
		public bool biomeExclusive;

		// Token: 0x040016F6 RID: 5878
		public bool cliffTest;

		// Token: 0x040016F7 RID: 5879
		public bool overlap;

		// Token: 0x040016F8 RID: 5880
		public bool closeOverlap;

		// Token: 0x040016F9 RID: 5881
		public bool temperate;

		// Token: 0x040016FA RID: 5882
		public bool arid;

		// Token: 0x040016FB RID: 5883
		public bool arctic;

		// Token: 0x040016FC RID: 5884
		public bool tundra;

		// Token: 0x040016FD RID: 5885
		public bool jungle;

		// Token: 0x040016FE RID: 5886
		public bool road;

		// Token: 0x040016FF RID: 5887
		public bool monument;

		// Token: 0x04001700 RID: 5888
		public bool dither;

		// Token: 0x04001701 RID: 5889
		public bool useSeed;

		// Token: 0x04001702 RID: 5890
		public bool slopeRange;

		// Token: 0x04001703 RID: 5891
		public bool curveRange;

		// Token: 0x04001704 RID: 5892
		public bool heightRange;

		// Token: 0x04001705 RID: 5893
		public bool hAscend;

		// Token: 0x04001706 RID: 5894
		public bool hDescend;

		// Token: 0x04001707 RID: 5895
		public bool sAscend;

		// Token: 0x04001708 RID: 5896
		public bool sDescend;

		// Token: 0x04001709 RID: 5897
		public bool cAscend;

		// Token: 0x0400170A RID: 5898
		public bool cDescend;

		// Token: 0x0400170B RID: 5899
		public bool featureMenu;

		// Token: 0x0400170C RID: 5900
		public bool rotationMenu;

		// Token: 0x0400170D RID: 5901
		public bool scaleMenu;

		// Token: 0x0400170E RID: 5902
		public bool placementMenu;

		// Token: 0x0400170F RID: 5903
		public bool collisionMenu;

		// Token: 0x04001710 RID: 5904
		public bool presetMenu;

		// Token: 0x04001711 RID: 5905
		public bool jitterMenu;

		// Token: 0x04001712 RID: 5906
		public bool preview;

		// Token: 0x04001713 RID: 5907
		public Vector3 scalesLow;

		// Token: 0x04001714 RID: 5908
		public Vector3 scalesHigh;

		// Token: 0x04001715 RID: 5909
		public Vector3 rotationsLow;

		// Token: 0x04001716 RID: 5910
		public Vector3 rotationsHigh;

		// Token: 0x04001717 RID: 5911
		public Vector3 jitterLow;

		// Token: 0x04001718 RID: 5912
		public Vector3 jitterHigh;

		// Token: 0x04001719 RID: 5913
		public Vector3 slideHigh;

		// Token: 0x0400171A RID: 5914
		public Vector3 slideLow;

		// Token: 0x0400171B RID: 5915
		public float zOffset;

		// Token: 0x0400171C RID: 5916
		public float colliderDistance;

		// Token: 0x0400171D RID: 5917
		public float closeColliderDistance;

		// Token: 0x0400171E RID: 5918
		public float balance;

		// Token: 0x0400171F RID: 5919
		public float slopeLow;

		// Token: 0x04001720 RID: 5920
		public float slopeHigh;

		// Token: 0x04001721 RID: 5921
		public HeightSelector heights;

		// Token: 0x04001722 RID: 5922
		public Topologies topologies;
	}
}
