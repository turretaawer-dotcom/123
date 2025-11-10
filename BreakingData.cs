using System;
using ProtoBuf;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D5 RID: 1237
	[ProtoContract]
	[Serializable]
	public struct BreakingData
	{
		// Token: 0x0400167B RID: 5755
		[ProtoMember(1)]
		public string name;

		// Token: 0x0400167C RID: 5756
		[ProtoMember(2)]
		public uint id;

		// Token: 0x0400167D RID: 5757
		[ProtoMember(3)]
		public bool ignore;

		// Token: 0x0400167E RID: 5758
		[ProtoMember(4)]
		public int treeID;

		// Token: 0x0400167F RID: 5759
		[ProtoMember(5)]
		public Colliders colliderScales;

		// Token: 0x04001680 RID: 5760
		[ProtoMember(6)]
		public WorldSerialization.PrefabData prefabData;

		// Token: 0x04001681 RID: 5761
		[ProtoMember(7)]
		public string parent;

		// Token: 0x04001682 RID: 5762
		[ProtoMember(8)]
		public string treePath;

		// Token: 0x04001683 RID: 5763
		[ProtoMember(9)]
		public string monument;
	}
}
