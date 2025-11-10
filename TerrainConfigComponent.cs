using System;
using UnityEngine;

// Token: 0x020000C4 RID: 196
public class TerrainConfigComponent : MonoBehaviour
{
	// Token: 0x04000351 RID: 849
	[SerializeField]
	public bool CastShadows = true;

	// Token: 0x04000352 RID: 850
	[SerializeField]
	public LayerMask GroundMask;

	// Token: 0x04000353 RID: 851
	[SerializeField]
	public LayerMask WaterMask;

	// Token: 0x04000354 RID: 852
	[SerializeField]
	public PhysicMaterial GenericMaterial;

	// Token: 0x04000355 RID: 853
	[SerializeField]
	public PhysicMaterial WaterMaterial;

	// Token: 0x04000356 RID: 854
	[SerializeField]
	public Material Material;

	// Token: 0x04000357 RID: 855
	[SerializeField]
	public Material MarginMaterial;

	// Token: 0x04000358 RID: 856
	[SerializeField]
	public Texture[] AlbedoArrays = new Texture[3];

	// Token: 0x04000359 RID: 857
	[SerializeField]
	public Texture[] NormalArrays = new Texture[3];

	// Token: 0x0400035A RID: 858
	[SerializeField]
	public float HeightMapErrorMin;

	// Token: 0x0400035B RID: 859
	[SerializeField]
	public float HeightMapErrorMax;

	// Token: 0x0400035C RID: 860
	[SerializeField]
	public float BaseMapDistanceMin;

	// Token: 0x0400035D RID: 861
	[SerializeField]
	public float BaseMapDistanceMax;

	// Token: 0x0400035E RID: 862
	[SerializeField]
	public float ShaderLodMin;

	// Token: 0x0400035F RID: 863
	[SerializeField]
	public float ShaderLodMax;

	// Token: 0x04000360 RID: 864
	[SerializeField]
	public SplatType[] Splats = new SplatType[8];

	// Token: 0x020000C5 RID: 197
	[Serializable]
	public class SplatOverlay
	{
		// Token: 0x04000361 RID: 865
		public Color Color;

		// Token: 0x04000362 RID: 866
		public float Smoothness;

		// Token: 0x04000363 RID: 867
		public float NormalIntensity;

		// Token: 0x04000364 RID: 868
		public float BlendFactor;

		// Token: 0x04000365 RID: 869
		public float BlendFalloff;
	}
}
