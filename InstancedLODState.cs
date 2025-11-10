using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Instancing
{
	// Token: 0x02000510 RID: 1296
	[Serializable]
	public class InstancedLODState
	{
		// Token: 0x060029C0 RID: 10688 RVA: 0x00002822 File Offset: 0x00000A22
		public InstancedLODState(Matrix4x4 localToWorld, MeshRenderer meshRenderer, float minimumDistance, float maximumDistance, int lodLevel, int totalLodLevels, InstancedMeshCategory meshCategory)
		{
		}

		// Token: 0x04001814 RID: 6164
		public Mesh Mesh;

		// Token: 0x04001815 RID: 6165
		public Material[] Materials;

		// Token: 0x04001816 RID: 6166
		public Matrix4x4 LocalToWorld;

		// Token: 0x04001817 RID: 6167
		public ShadowCastingMode CastShadows;

		// Token: 0x04001818 RID: 6168
		public bool RecieveShadows;

		// Token: 0x04001819 RID: 6169
		public LightProbeUsage LightProbes;

		// Token: 0x0400181A RID: 6170
		public int LodLevel;

		// Token: 0x0400181B RID: 6171
		public int TotalLodLevels;

		// Token: 0x0400181C RID: 6172
		public InstancedMeshCategory MeshCategory;

		// Token: 0x0400181D RID: 6173
		public float MinimumDistance;

		// Token: 0x0400181E RID: 6174
		public float MaximumDistance;
	}
}
