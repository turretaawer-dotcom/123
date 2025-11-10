using System;
using ProtoBuf;
using UnityEngine;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D4 RID: 1236
	[ProtoContract]
	[Serializable]
	public class Colliders
	{
		// Token: 0x0600292B RID: 10539 RVA: 0x0001D762 File Offset: 0x0001B962
		public Colliders()
		{
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000B4E0C File Offset: 0x000B300C
		public Colliders(Vector3 box, Vector3 sphere, Vector3 capsule)
		{
			this.box = box;
			this.sphere = sphere;
			this.capsule = capsule;
		}

		// Token: 0x04001678 RID: 5752
		[ProtoMember(1)]
		public WorldSerialization.VectorData box = new WorldSerialization.VectorData();

		// Token: 0x04001679 RID: 5753
		[ProtoMember(2)]
		public WorldSerialization.VectorData sphere = new WorldSerialization.VectorData();

		// Token: 0x0400167A RID: 5754
		[ProtoMember(3)]
		public WorldSerialization.VectorData capsule = new WorldSerialization.VectorData();
	}
}
