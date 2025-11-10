using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200050A RID: 1290
	[SelectionBase]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class VolumetricLightBeam : MonoBehaviour
	{
		// Token: 0x060029AD RID: 10669 RVA: 0x000026C7 File Offset: 0x000008C7
		private void Awake()
		{
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000026C7 File Offset: 0x000008C7
		private void Start()
		{
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000026C7 File Offset: 0x000008C7
		private void OnEnable()
		{
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x000026C7 File Offset: 0x000008C7
		private void OnDisable()
		{
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000026C7 File Offset: 0x000008C7
		public virtual void GenerateGeometry()
		{
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x000026C7 File Offset: 0x000008C7
		public virtual void UpdateAfterManualPropertyChange()
		{
		}

		// Token: 0x060029B3 RID: 10675 RVA: 0x000026C7 File Offset: 0x000008C7
		private void UpdateLightProperties(Light light)
		{
		}

		// Token: 0x060029B4 RID: 10676 RVA: 0x000030A2 File Offset: 0x000012A2
		public float CalculateAttenuation(Vector3 point)
		{
			return 0f;
		}

		// Token: 0x060029B5 RID: 10677 RVA: 0x000B7288 File Offset: 0x000B5488
		public Bounds CalculateBeamBounds()
		{
			return default(Bounds);
		}

		// Token: 0x060029B6 RID: 10678 RVA: 0x000026C7 File Offset: 0x000008C7
		public void SetSortingOrder(int value)
		{
		}

		// Token: 0x060029B7 RID: 10679 RVA: 0x000026C7 File Offset: 0x000008C7
		public void SetSortingLayerID(int value)
		{
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x0001DA27 File Offset: 0x0001BC27
		private IEnumerator CoPlaytimeUpdate()
		{
			for (;;)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x000B72A0 File Offset: 0x000B54A0
		public void SetIntensity(float newIntensity)
		{
			if (this.associatedLight != null)
			{
				this.associatedLight.intensity = newIntensity;
				this.fadeEnd = Mathf.Lerp(this.fadeEnd, newIntensity * 10f, 0.5f);
				this.alphaInside = Mathf.Lerp(this.alphaInside, newIntensity, 0.5f);
				this.alphaOutside = Mathf.Lerp(this.alphaOutside, newIntensity / 2f, 0.5f);
				this.UpdateAfterManualPropertyChange();
				return;
			}
			Debug.LogWarning("No associated Light component found to set intensity for VolumetricLightBeam.");
		}

		// Token: 0x040017DD RID: 6109
		public bool colorFromLight = true;

		// Token: 0x040017DE RID: 6110
		public VolumetricLightBeam.ColorMode colorMode;

		// Token: 0x040017DF RID: 6111
		public Color color = Color.white;

		// Token: 0x040017E0 RID: 6112
		public Gradient colorGradient;

		// Token: 0x040017E1 RID: 6113
		public float alphaInside = 1f;

		// Token: 0x040017E2 RID: 6114
		public float alphaOutside;

		// Token: 0x040017E3 RID: 6115
		public VolumetricLightBeam.BlendingMode blendingMode;

		// Token: 0x040017E4 RID: 6116
		public bool spotAngleFromLight = true;

		// Token: 0x040017E5 RID: 6117
		public float spotAngle = 90f;

		// Token: 0x040017E6 RID: 6118
		public float coneRadiusStart;

		// Token: 0x040017E7 RID: 6119
		public VolumetricLightBeam.MeshType geomMeshType;

		// Token: 0x040017E8 RID: 6120
		public int geomCustomSides = 4;

		// Token: 0x040017E9 RID: 6121
		public int geomCustomSegments = 4;

		// Token: 0x040017EA RID: 6122
		public bool geomCap = true;

		// Token: 0x040017EB RID: 6123
		public bool fadeEndFromLight = true;

		// Token: 0x040017EC RID: 6124
		public VolumetricLightBeam.AttenuationEquation attenuationEquation = VolumetricLightBeam.AttenuationEquation.Quadratic;

		// Token: 0x040017ED RID: 6125
		public float attenuationCustomBlending = 0.5f;

		// Token: 0x040017EE RID: 6126
		public float fadeStart = 1f;

		// Token: 0x040017EF RID: 6127
		public float fadeEnd = 10f;

		// Token: 0x040017F0 RID: 6128
		public float depthBlendDistance;

		// Token: 0x040017F1 RID: 6129
		public float cameraClippingDistance = 0.1f;

		// Token: 0x040017F2 RID: 6130
		public float glareFrontal = 0.5f;

		// Token: 0x040017F3 RID: 6131
		public float glareBehind = 0.5f;

		// Token: 0x040017F4 RID: 6132
		public float boostDistanceInside;

		// Token: 0x040017F5 RID: 6133
		public float fresnelPowInside;

		// Token: 0x040017F6 RID: 6134
		public float fresnelPow = 1f;

		// Token: 0x040017F7 RID: 6135
		public bool noiseEnabled;

		// Token: 0x040017F8 RID: 6136
		public float noiseIntensity = 0.5f;

		// Token: 0x040017F9 RID: 6137
		public bool noiseScaleUseGlobal = true;

		// Token: 0x040017FA RID: 6138
		public float noiseScaleLocal = 1f;

		// Token: 0x040017FB RID: 6139
		public bool noiseVelocityUseGlobal = true;

		// Token: 0x040017FC RID: 6140
		public Vector3 noiseVelocityLocal = Vector3.zero;

		// Token: 0x040017FD RID: 6141
		private Plane clippingPlane;

		// Token: 0x040017FE RID: 6142
		private int pluginVersion;

		// Token: 0x040017FF RID: 6143
		private bool trackChangesDuringPlaytime;

		// Token: 0x04001800 RID: 6144
		private int sortingLayerID;

		// Token: 0x04001801 RID: 6145
		private int sortingOrder;

		// Token: 0x04001802 RID: 6146
		private Coroutine playtimeUpdateCoroutine;

		// Token: 0x04001803 RID: 6147
		private Light associatedLight;

		// Token: 0x0200050B RID: 1291
		public enum ColorMode
		{
			// Token: 0x04001805 RID: 6149
			Solid,
			// Token: 0x04001806 RID: 6150
			Gradient
		}

		// Token: 0x0200050C RID: 1292
		public enum BlendingMode
		{
			// Token: 0x04001808 RID: 6152
			Additive,
			// Token: 0x04001809 RID: 6153
			Alpha
		}

		// Token: 0x0200050D RID: 1293
		public enum MeshType
		{
			// Token: 0x0400180B RID: 6155
			Quad,
			// Token: 0x0400180C RID: 6156
			Cone,
			// Token: 0x0400180D RID: 6157
			Custom
		}

		// Token: 0x0200050E RID: 1294
		public enum AttenuationEquation
		{
			// Token: 0x0400180F RID: 6159
			Linear,
			// Token: 0x04001810 RID: 6160
			Quadratic,
			// Token: 0x04001811 RID: 6161
			Custom
		}
	}
}
