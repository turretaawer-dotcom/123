using System;
using System.Linq;
using UnityEngine;

// Token: 0x020000C1 RID: 193
[CreateAssetMenu(menuName = "Rust/Terrain Config", fileName = "TerrainConfig")]
[Serializable]
public class TerrainConfig : ScriptableObject
{
	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060002AE RID: 686 RVA: 0x00003FF8 File Offset: 0x000021F8
	// (set) Token: 0x060002AF RID: 687 RVA: 0x00004000 File Offset: 0x00002200
	public bool CastShadows
	{
		get
		{
			return this.castShadows;
		}
		set
		{
			this.castShadows = value;
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x00004009 File Offset: 0x00002209
	// (set) Token: 0x060002B1 RID: 689 RVA: 0x00004011 File Offset: 0x00002211
	public LayerMask GroundMask
	{
		get
		{
			return this.groundMask;
		}
		set
		{
			this.groundMask = value;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000401A File Offset: 0x0000221A
	// (set) Token: 0x060002B3 RID: 691 RVA: 0x00004022 File Offset: 0x00002222
	public LayerMask WaterMask
	{
		get
		{
			return this.waterMask;
		}
		set
		{
			this.waterMask = value;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000402B File Offset: 0x0000222B
	// (set) Token: 0x060002B5 RID: 693 RVA: 0x00004033 File Offset: 0x00002233
	public PhysicMaterial GenericMaterial
	{
		get
		{
			return this.genericMaterial;
		}
		set
		{
			this.genericMaterial = value;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000403C File Offset: 0x0000223C
	// (set) Token: 0x060002B7 RID: 695 RVA: 0x00004044 File Offset: 0x00002244
	public PhysicMaterial WaterMaterial
	{
		get
		{
			return this.waterMaterial;
		}
		set
		{
			this.waterMaterial = value;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000404D File Offset: 0x0000224D
	// (set) Token: 0x060002B9 RID: 697 RVA: 0x00004055 File Offset: 0x00002255
	public Material Material
	{
		get
		{
			return this.material;
		}
		set
		{
			this.material = value;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060002BA RID: 698 RVA: 0x0000405E File Offset: 0x0000225E
	// (set) Token: 0x060002BB RID: 699 RVA: 0x00004066 File Offset: 0x00002266
	public Material MarginMaterial
	{
		get
		{
			return this.marginMaterial;
		}
		set
		{
			this.marginMaterial = value;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060002BC RID: 700 RVA: 0x0000406F File Offset: 0x0000226F
	// (set) Token: 0x060002BD RID: 701 RVA: 0x00004077 File Offset: 0x00002277
	public Texture[] AlbedoArrays
	{
		get
		{
			return this.albedoArrays;
		}
		set
		{
			this.albedoArrays = value;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060002BE RID: 702 RVA: 0x00004080 File Offset: 0x00002280
	// (set) Token: 0x060002BF RID: 703 RVA: 0x00004088 File Offset: 0x00002288
	public Texture[] NormalArrays
	{
		get
		{
			return this.normalArrays;
		}
		set
		{
			this.normalArrays = value;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060002C0 RID: 704 RVA: 0x00004091 File Offset: 0x00002291
	// (set) Token: 0x060002C1 RID: 705 RVA: 0x00004099 File Offset: 0x00002299
	public float HeightMapErrorMin
	{
		get
		{
			return this.heightMapErrorMin;
		}
		set
		{
			this.heightMapErrorMin = value;
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060002C2 RID: 706 RVA: 0x000040A2 File Offset: 0x000022A2
	// (set) Token: 0x060002C3 RID: 707 RVA: 0x000040AA File Offset: 0x000022AA
	public float HeightMapErrorMax
	{
		get
		{
			return this.heightMapErrorMax;
		}
		set
		{
			this.heightMapErrorMax = value;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060002C4 RID: 708 RVA: 0x000040B3 File Offset: 0x000022B3
	// (set) Token: 0x060002C5 RID: 709 RVA: 0x000040BB File Offset: 0x000022BB
	public float BaseMapDistanceMin
	{
		get
		{
			return this.baseMapDistanceMin;
		}
		set
		{
			this.baseMapDistanceMin = value;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060002C6 RID: 710 RVA: 0x000040C4 File Offset: 0x000022C4
	// (set) Token: 0x060002C7 RID: 711 RVA: 0x000040CC File Offset: 0x000022CC
	public float BaseMapDistanceMax
	{
		get
		{
			return this.baseMapDistanceMax;
		}
		set
		{
			this.baseMapDistanceMax = value;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060002C8 RID: 712 RVA: 0x000040D5 File Offset: 0x000022D5
	// (set) Token: 0x060002C9 RID: 713 RVA: 0x000040DD File Offset: 0x000022DD
	public float ShaderLodMin
	{
		get
		{
			return this.shaderLodMin;
		}
		set
		{
			this.shaderLodMin = value;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060002CA RID: 714 RVA: 0x000040E6 File Offset: 0x000022E6
	// (set) Token: 0x060002CB RID: 715 RVA: 0x000040EE File Offset: 0x000022EE
	public float ShaderLodMax
	{
		get
		{
			return this.shaderLodMax;
		}
		set
		{
			this.shaderLodMax = value;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060002CC RID: 716 RVA: 0x000040F7 File Offset: 0x000022F7
	// (set) Token: 0x060002CD RID: 717 RVA: 0x000040FF File Offset: 0x000022FF
	public SplatType[] Splats
	{
		get
		{
			return this.splats;
		}
		set
		{
			this.splats = value;
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060002CE RID: 718 RVA: 0x00004108 File Offset: 0x00002308
	public Texture AlbedoArray
	{
		get
		{
			return this.albedoArrays[Mathf.Clamp(QualitySettings.masterTextureLimit, 0, this.albedoArrays.Length - 1)];
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060002CF RID: 719 RVA: 0x00004126 File Offset: 0x00002326
	public Texture NormalArray
	{
		get
		{
			return this.normalArrays[Mathf.Clamp(QualitySettings.masterTextureLimit, 0, this.normalArrays.Length - 1)];
		}
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00004144 File Offset: 0x00002344
	public float GetTextureArrayWidth()
	{
		return (float)this.AlbedoArray.width;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x00027850 File Offset: 0x00025A50
	public void LoadTextureArrays()
	{
		string[] array = new string[]
		{
			"assets/content/nature/terrain/atlas/terrain4_albedo_array.asset",
			"assets/content/nature/terrain/atlas/terrain4_albedo_array_lod1.asset",
			"assets/content/nature/terrain/atlas/terrain4_albedo_array_lod2.asset"
		};
		string[] array2 = new string[]
		{
			"assets/content/nature/terrain/atlas/terrain4_normal_array.asset",
			"assets/content/nature/terrain/atlas/terrain4_normal_array_lod1.asset",
			"assets/content/nature/terrain/atlas/terrain4_normal_array_lod2.asset"
		};
		for (int i = 0; i < this.albedoArrays.Length; i++)
		{
			if (this.albedoArrays[i] == null)
			{
				this.albedoArrays[i] = AssetManager.LoadAsset<Texture2DArray>(array[i]);
				if (this.albedoArrays[i] == null)
				{
					Debug.LogError(string.Format("Failed to load Terrain4_AlbedoArray LOD {0} from AssetManager at path: {1}", i, array[i]));
				}
				else
				{
					Debug.Log(string.Format("Successfully loaded Terrain4_AlbedoArray LOD {0} from {1}.", i, array[i]));
				}
			}
		}
		for (int j = 0; j < this.normalArrays.Length; j++)
		{
			if (this.normalArrays[j] == null)
			{
				this.normalArrays[j] = AssetManager.LoadAsset<Texture2DArray>(array2[j]);
				if (this.normalArrays[j] == null)
				{
					Debug.LogError(string.Format("Failed to load Terrain4_NormalArray LOD {0} from AssetManager at path: {1}", j, array2[j]));
				}
				else
				{
					Debug.Log(string.Format("Successfully loaded Terrain4_NormalArray LOD {0} from {1}.", j, array2[j]));
				}
			}
		}
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x00004152 File Offset: 0x00002352
	private void OnEnable()
	{
		AssetManager.Callbacks.BundlesLoaded += this.OnBundlesLoaded;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x000026C7 File Offset: 0x000008C7
	private void OnBundlesLoaded()
	{
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00027988 File Offset: 0x00025B88
	public void GetSplatColorsAndVectors(int splatIndex, out Color[] colors, out Vector4[] vectors)
	{
		if (splatIndex < 0 || splatIndex >= this.splats.Length)
		{
			Debug.LogError(string.Format("Splat index {0} out of range (0-{1})", splatIndex, this.splats.Length - 1));
			colors = null;
			vectors = null;
			return;
		}
		SplatType splatType = this.splats[splatIndex];
		colors = new Color[]
		{
			splatType.AridColor,
			splatType.TemperateColor,
			splatType.TundraColor,
			splatType.ArcticColor
		};
		vectors = new Vector4[]
		{
			new Vector4(splatType.AridOverlay.Smoothness, splatType.AridOverlay.NormalIntensity, splatType.AridOverlay.BlendFactor, splatType.AridOverlay.BlendFalloff),
			new Vector4(splatType.TemperateOverlay.Smoothness, splatType.TemperateOverlay.NormalIntensity, splatType.TemperateOverlay.BlendFactor, splatType.TemperateOverlay.BlendFalloff),
			new Vector4(splatType.TundraOverlay.Smoothness, splatType.TundraOverlay.NormalIntensity, splatType.TundraOverlay.BlendFactor, splatType.TundraOverlay.BlendFalloff),
			new Vector4(splatType.ArcticOverlay.Smoothness, splatType.ArcticOverlay.NormalIntensity, splatType.ArcticOverlay.BlendFactor, splatType.ArcticOverlay.BlendFalloff)
		};
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00027AFC File Offset: 0x00025CFC
	public Color[] GetSplatColors(int splatIndex)
	{
		if (splatIndex < 0 || splatIndex >= this.splats.Length)
		{
			Debug.LogError(string.Format("Splat index {0} out of range (0-{1})", splatIndex, this.splats.Length - 1));
			return null;
		}
		SplatType splatType = this.splats[splatIndex];
		return new Color[]
		{
			splatType.AridColor,
			splatType.TemperateColor,
			splatType.TundraColor,
			splatType.ArcticColor
		};
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x00027B84 File Offset: 0x00025D84
	public PhysicMaterial[] GetSplatPhysicMaterials()
	{
		PhysicMaterial[] array = new PhysicMaterial[this.splats.Length];
		for (int i = 0; i < this.splats.Length; i++)
		{
			array[i] = (this.splats[i].Material ?? this.genericMaterial);
		}
		return array;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00027BD0 File Offset: 0x00025DD0
	public float GetSplatTiling(int splatIndex)
	{
		if (splatIndex < 0 || splatIndex >= this.splats.Length)
		{
			Debug.LogError(string.Format("Splat index {0} out of range (0-{1})", splatIndex, this.splats.Length - 1));
			return 5f;
		}
		return this.splats[splatIndex].SplatTiling;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00027C24 File Offset: 0x00025E24
	public Vector3[] GetSplatUVMixData()
	{
		Vector3[] array = new Vector3[this.splats.Length];
		for (int i = 0; i < this.splats.Length; i++)
		{
			array[i] = new Vector3(this.splats[i].UVMixMult, this.splats[i].UVMixStart, this.splats[i].UVMixDist);
		}
		return array;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00027C88 File Offset: 0x00025E88
	public TerrainConfig.GroundType GetGroundType(bool useRaycast, RaycastHit hit)
	{
		if (!useRaycast || !hit.collider)
		{
			return TerrainConfig.GroundType.None;
		}
		Vector2 textureCoord = hit.textureCoord;
		int num = Mathf.FloorToInt(textureCoord.x * (float)TerrainManager.Land.terrainData.alphamapWidth);
		int num2 = Mathf.FloorToInt(textureCoord.y * (float)TerrainManager.Land.terrainData.alphamapHeight);
		float[,,] splatMap = TerrainManager.GetSplatMap(TerrainManager.LayerType.Ground, -1);
		if (num < 0 || num >= splatMap.GetLength(0) || num2 < 0 || num2 >= splatMap.GetLength(1))
		{
			return TerrainConfig.GroundType.None;
		}
		float num3 = 0f;
		int num4 = 0;
		for (int i = 0; i < splatMap.GetLength(2); i++)
		{
			if (splatMap[num, num2, i] > num3)
			{
				num3 = splatMap[num, num2, i];
				num4 = i;
			}
		}
		TerrainConfig.GroundType result;
		switch (num4)
		{
		case 0:
			result = TerrainConfig.GroundType.Dirt;
			break;
		case 1:
			result = TerrainConfig.GroundType.Snow;
			break;
		case 2:
			result = TerrainConfig.GroundType.Sand;
			break;
		case 3:
			result = TerrainConfig.GroundType.HardSurface;
			break;
		case 4:
			result = TerrainConfig.GroundType.Grass;
			break;
		case 5:
			result = TerrainConfig.GroundType.Grass;
			break;
		case 6:
			result = TerrainConfig.GroundType.Gravel;
			break;
		case 7:
			result = TerrainConfig.GroundType.Gravel;
			break;
		default:
			result = TerrainConfig.GroundType.None;
			break;
		}
		return result;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00004165 File Offset: 0x00002365
	public float[] GetSplatTilings()
	{
		return (from s in this.Splats
		select s.SplatTiling).ToArray<float>();
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00004196 File Offset: 0x00002396
	public Vector3[] GetUVMIXParameters()
	{
		return (from s in this.Splats
		select new Vector3(s.UVMixMult, s.UVMixStart, s.UVMixDist)).ToArray<Vector3>();
	}

	// Token: 0x060002DC RID: 732 RVA: 0x000041C7 File Offset: 0x000023C7
	public Color[] GetAridColors()
	{
		return (from s in this.Splats
		select s.AridColor).ToArray<Color>();
	}

	// Token: 0x060002DD RID: 733 RVA: 0x000041F8 File Offset: 0x000023F8
	public Color[] GetTemperateColors()
	{
		return (from s in this.Splats
		select s.TemperateColor).ToArray<Color>();
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00004229 File Offset: 0x00002429
	public Color[] GetTundraColors()
	{
		return (from s in this.Splats
		select s.TundraColor).ToArray<Color>();
	}

	// Token: 0x060002DF RID: 735 RVA: 0x0000425A File Offset: 0x0000245A
	public Color[] GetArcticColors()
	{
		return (from s in this.Splats
		select s.ArcticColor).ToArray<Color>();
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x0000428B File Offset: 0x0000248B
	public Color[] GetJungleColors()
	{
		return (from s in this.Splats
		select s.JungleColor).ToArray<Color>();
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00027DA0 File Offset: 0x00025FA0
	public void GetAridOverlayData(out Color[] colors, out Vector4[] parameters)
	{
		colors = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay aridOverlay = s.AridOverlay;
			if (aridOverlay == null)
			{
				return Color.black;
			}
			return aridOverlay.Color;
		}).ToArray<Color>();
		parameters = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay aridOverlay = s.AridOverlay;
			float x = (aridOverlay != null) ? aridOverlay.Smoothness : 0.5f;
			SplatOverlay aridOverlay2 = s.AridOverlay;
			float y = (aridOverlay2 != null) ? aridOverlay2.NormalIntensity : 0f;
			SplatOverlay aridOverlay3 = s.AridOverlay;
			float z = (aridOverlay3 != null) ? aridOverlay3.BlendFactor : 0f;
			SplatOverlay aridOverlay4 = s.AridOverlay;
			return new Vector4(x, y, z, (aridOverlay4 != null) ? aridOverlay4.BlendFalloff : 0f);
		}).ToArray<Vector4>();
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00027E10 File Offset: 0x00026010
	public void GetTemperateOverlayData(out Color[] colors, out Vector4[] parameters)
	{
		colors = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay temperateOverlay = s.TemperateOverlay;
			if (temperateOverlay == null)
			{
				return Color.black;
			}
			return temperateOverlay.Color;
		}).ToArray<Color>();
		parameters = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay temperateOverlay = s.TemperateOverlay;
			float x = (temperateOverlay != null) ? temperateOverlay.Smoothness : 0.5f;
			SplatOverlay temperateOverlay2 = s.TemperateOverlay;
			float y = (temperateOverlay2 != null) ? temperateOverlay2.NormalIntensity : 0f;
			SplatOverlay temperateOverlay3 = s.TemperateOverlay;
			float z = (temperateOverlay3 != null) ? temperateOverlay3.BlendFactor : 0f;
			SplatOverlay temperateOverlay4 = s.TemperateOverlay;
			return new Vector4(x, y, z, (temperateOverlay4 != null) ? temperateOverlay4.BlendFalloff : 0f);
		}).ToArray<Vector4>();
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00027E80 File Offset: 0x00026080
	public void GetTundraOverlayData(out Color[] colors, out Vector4[] parameters)
	{
		colors = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay tundraOverlay = s.TundraOverlay;
			if (tundraOverlay == null)
			{
				return Color.black;
			}
			return tundraOverlay.Color;
		}).ToArray<Color>();
		parameters = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay tundraOverlay = s.TundraOverlay;
			float x = (tundraOverlay != null) ? tundraOverlay.Smoothness : 0.5f;
			SplatOverlay tundraOverlay2 = s.TundraOverlay;
			float y = (tundraOverlay2 != null) ? tundraOverlay2.NormalIntensity : 0f;
			SplatOverlay tundraOverlay3 = s.TundraOverlay;
			float z = (tundraOverlay3 != null) ? tundraOverlay3.BlendFactor : 0f;
			SplatOverlay tundraOverlay4 = s.TundraOverlay;
			return new Vector4(x, y, z, (tundraOverlay4 != null) ? tundraOverlay4.BlendFalloff : 0f);
		}).ToArray<Vector4>();
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00027EF0 File Offset: 0x000260F0
	public void GetArcticOverlayData(out Color[] colors, out Vector4[] parameters)
	{
		colors = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay arcticOverlay = s.ArcticOverlay;
			if (arcticOverlay == null)
			{
				return Color.black;
			}
			return arcticOverlay.Color;
		}).ToArray<Color>();
		parameters = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay arcticOverlay = s.ArcticOverlay;
			float x = (arcticOverlay != null) ? arcticOverlay.Smoothness : 0.5f;
			SplatOverlay arcticOverlay2 = s.ArcticOverlay;
			float y = (arcticOverlay2 != null) ? arcticOverlay2.NormalIntensity : 0f;
			SplatOverlay arcticOverlay3 = s.ArcticOverlay;
			float z = (arcticOverlay3 != null) ? arcticOverlay3.BlendFactor : 0f;
			SplatOverlay arcticOverlay4 = s.ArcticOverlay;
			return new Vector4(x, y, z, (arcticOverlay4 != null) ? arcticOverlay4.BlendFalloff : 0f);
		}).ToArray<Vector4>();
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00027F60 File Offset: 0x00026160
	public void GetJungleOverlayData(out Color[] colors, out Vector4[] parameters)
	{
		colors = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay jungleOverlay = s.JungleOverlay;
			if (jungleOverlay == null)
			{
				return Color.black;
			}
			return jungleOverlay.Color;
		}).ToArray<Color>();
		parameters = this.Splats.Select(delegate(SplatType s)
		{
			SplatOverlay jungleOverlay = s.JungleOverlay;
			float x = (jungleOverlay != null) ? jungleOverlay.Smoothness : 0.5f;
			SplatOverlay jungleOverlay2 = s.JungleOverlay;
			float y = (jungleOverlay2 != null) ? jungleOverlay2.NormalIntensity : 0f;
			SplatOverlay jungleOverlay3 = s.JungleOverlay;
			float z = (jungleOverlay3 != null) ? jungleOverlay3.BlendFactor : 0f;
			SplatOverlay jungleOverlay4 = s.JungleOverlay;
			return new Vector4(x, y, z, (jungleOverlay4 != null) ? jungleOverlay4.BlendFalloff : 0f);
		}).ToArray<Vector4>();
	}

	// Token: 0x04000322 RID: 802
	[SerializeField]
	private bool castShadows = true;

	// Token: 0x04000323 RID: 803
	[SerializeField]
	private LayerMask groundMask;

	// Token: 0x04000324 RID: 804
	[SerializeField]
	private LayerMask waterMask;

	// Token: 0x04000325 RID: 805
	[SerializeField]
	private PhysicMaterial genericMaterial;

	// Token: 0x04000326 RID: 806
	[SerializeField]
	private PhysicMaterial waterMaterial;

	// Token: 0x04000327 RID: 807
	[SerializeField]
	private Material material;

	// Token: 0x04000328 RID: 808
	[SerializeField]
	private Material marginMaterial;

	// Token: 0x04000329 RID: 809
	[SerializeField]
	private Texture[] albedoArrays = new Texture[3];

	// Token: 0x0400032A RID: 810
	[SerializeField]
	private Texture[] normalArrays = new Texture[3];

	// Token: 0x0400032B RID: 811
	[SerializeField]
	private float heightMapErrorMin;

	// Token: 0x0400032C RID: 812
	[SerializeField]
	private float heightMapErrorMax;

	// Token: 0x0400032D RID: 813
	[SerializeField]
	private float baseMapDistanceMin;

	// Token: 0x0400032E RID: 814
	[SerializeField]
	private float baseMapDistanceMax;

	// Token: 0x0400032F RID: 815
	[SerializeField]
	private float shaderLodMin;

	// Token: 0x04000330 RID: 816
	[SerializeField]
	private float shaderLodMax;

	// Token: 0x04000331 RID: 817
	[SerializeField]
	private SplatType[] splats = new SplatType[8];

	// Token: 0x04000332 RID: 818
	[SerializeField]
	private string[] splatNames;

	// Token: 0x04000333 RID: 819
	[SerializeField]
	private string groundMaskName;

	// Token: 0x04000334 RID: 820
	[SerializeField]
	private string waterMaskName;

	// Token: 0x04000335 RID: 821
	[SerializeField]
	private string[] topologyNames;

	// Token: 0x04000336 RID: 822
	[SerializeField]
	private string genericMaterialName;

	// Token: 0x020000C2 RID: 194
	public enum GroundType
	{
		// Token: 0x04000338 RID: 824
		None,
		// Token: 0x04000339 RID: 825
		HardSurface,
		// Token: 0x0400033A RID: 826
		Grass,
		// Token: 0x0400033B RID: 827
		Sand,
		// Token: 0x0400033C RID: 828
		Snow,
		// Token: 0x0400033D RID: 829
		Dirt,
		// Token: 0x0400033E RID: 830
		Gravel
	}
}
