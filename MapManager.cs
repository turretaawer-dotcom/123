using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RustMapEditor.Maths;
using RustMapEditor.Variables;
using UnityEngine;

// Token: 0x02000163 RID: 355
public static class MapManager
{
	// Token: 0x06000880 RID: 2176 RVA: 0x00004FCD File Offset: 0x000031CD
	public static void RuntimeInit()
	{
		MapManager.CreateMap(SettingsManager.application.newSize, SettingsManager.application.newSplat, SettingsManager.application.newBiome, SettingsManager.application.newHeight * 1000f);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00050ECC File Offset: 0x0004F0CC
	public static List<int> GetEnumSelection<T>(T enumGroup)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < Enum.GetValues(typeof(T)).Length; i++)
		{
			int num = 1 << i;
			if ((Convert.ToInt32(enumGroup) & num) != 0)
			{
				list.Add(i);
			}
		}
		return list;
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00050F1C File Offset: 0x0004F11C
	public static void RotateMap(Selections.Objects objectSelection, bool CW)
	{
		foreach (int landLayerToPaint in MapManager.GetEnumSelection<Selections.Objects>(objectSelection))
		{
			switch (landLayerToPaint)
			{
			case 0:
			case 1:
			case 2:
				MapManager.RotateLayer((TerrainManager.LayerType)landLayerToPaint, CW, 0);
				break;
			case 3:
				MapManager.RotateTopologyLayers((TerrainTopology.Enum)(-1), CW);
				break;
			case 4:
				TerrainManager.RotateHeightMap(CW, TerrainManager.TerrainType.Land, null);
				break;
			case 5:
				TerrainManager.RotateHeightMap(CW, TerrainManager.TerrainType.Water, null);
				break;
			case 6:
				PrefabManager.RotatePrefabs(CW);
				break;
			case 7:
				PathManager.RotatePaths(CW);
				break;
			}
		}
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00007354 File Offset: 0x00005554
	public static void RotateLayer(TerrainManager.LayerType landLayerToPaint, bool CW, int topology = 0)
	{
		switch (landLayerToPaint)
		{
		case TerrainManager.LayerType.Ground:
		case TerrainManager.LayerType.Biome:
		case TerrainManager.LayerType.Topology:
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.Rotate(TerrainManager.GetSplatMap(landLayerToPaint, topology), CW, null), landLayerToPaint, topology);
			return;
		case TerrainManager.LayerType.Alpha:
			TerrainManager.SetAlphaMap(RustMapEditor.Maths.Array.Rotate(TerrainManager.GetAlphaMap(), CW, null));
			return;
		default:
			return;
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00050FC8 File Offset: 0x0004F1C8
	public static void RotateTopologyLayers(TerrainTopology.Enum topologyLayers, bool CW)
	{
		List<int> enumSelection = MapManager.GetEnumSelection<TerrainTopology.Enum>(topologyLayers);
		for (int i = 0; i < enumSelection.Count; i++)
		{
			MapManager.RotateLayer(TerrainManager.LayerType.Topology, CW, i);
		}
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00050FF8 File Offset: 0x0004F1F8
	public static void PaintConditional(TerrainManager.LayerType landLayerToPaint, int texture, Conditions conditions, int topology = 0)
	{
		int splatRes = TerrainManager.SplatMapRes;
		bool[,] conditionsMet = new bool[splatRes, splatRes];
		for (int l = 0; l < 8; l++)
		{
			if (conditions.GroundConditions.CheckLayer[l])
			{
				conditionsMet = RustMapEditor.Maths.Array.CheckConditions(TerrainManager.GetSplatMap(TerrainManager.LayerType.Ground, -1), conditionsMet, l, conditions.GroundConditions.Weight[l]);
			}
		}
		for (int j = 0; j < 5; j++)
		{
			if (conditions.BiomeConditions.CheckLayer[j])
			{
				conditionsMet = RustMapEditor.Maths.Array.CheckConditions(TerrainManager.GetSplatMap(TerrainManager.LayerType.Biome, -1), conditionsMet, j, conditions.BiomeConditions.Weight[j]);
			}
		}
		if (conditions.AlphaConditions.CheckAlpha)
		{
			conditionsMet = RustMapEditor.Maths.Array.CheckConditions(TerrainManager.GetAlphaMap(), conditionsMet, conditions.AlphaConditions.Texture == AlphaTextures.Visible);
		}
		for (int k = 0; k < 31; k++)
		{
			if (conditions.TopologyConditions.CheckLayer[k])
			{
				conditionsMet = RustMapEditor.Maths.Array.CheckConditions(TerrainManager.GetSplatMap(TerrainManager.LayerType.Topology, k), conditionsMet, (int)conditions.TopologyConditions.Texture[k], 0.5f);
			}
		}
		if (conditions.TerrainConditions.CheckHeights)
		{
			conditionsMet = RustMapEditor.Maths.Array.CheckConditions(TerrainManager.GetHeights(TerrainManager.TerrainType.Land), conditionsMet, conditions.TerrainConditions.Heights.HeightLow, conditions.TerrainConditions.Heights.HeightHigh);
		}
		if (conditions.TerrainConditions.CheckSlopes)
		{
			conditionsMet = RustMapEditor.Maths.Array.CheckConditions(TerrainManager.GetSlopes(), conditionsMet, conditions.TerrainConditions.Slopes.SlopeLow, conditions.TerrainConditions.Slopes.SlopeHigh);
		}
		switch (landLayerToPaint)
		{
		case TerrainManager.LayerType.Ground:
		case TerrainManager.LayerType.Biome:
		case TerrainManager.LayerType.Topology:
		{
			float[,,] splatMapToPaint = TerrainManager.GetSplatMap(landLayerToPaint, topology);
			int textureCount = TerrainManager.LayerCount(landLayerToPaint);
			Parallel.For(0, splatRes, delegate(int i)
			{
				for (int m = 0; m < splatRes; m++)
				{
					if (!conditionsMet[i, m])
					{
						for (int n = 0; n < textureCount; n++)
						{
							splatMapToPaint[i, m, n] = 0f;
						}
						splatMapToPaint[i, m, texture] = 1f;
					}
				}
			});
			TerrainManager.SetSplatMap(splatMapToPaint, landLayerToPaint, topology);
			return;
		}
		case TerrainManager.LayerType.Alpha:
		{
			bool[,] alphaMapToPaint = TerrainManager.GetAlphaMap();
			Parallel.For(0, splatRes, delegate(int i)
			{
				for (int m = 0; m < splatRes; m++)
				{
					alphaMapToPaint[i, m] = ((!conditionsMet[i, m]) ? conditionsMet[i, m] : alphaMapToPaint[i, m]);
				}
			});
			TerrainManager.SetAlphaMap(alphaMapToPaint);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00051240 File Offset: 0x0004F440
	public static void PaintHeight(TerrainManager.LayerType landLayerToPaint, float heightLow, float heightHigh, int t, int topology = 0)
	{
		switch (landLayerToPaint)
		{
		case TerrainManager.LayerType.Ground:
		case TerrainManager.LayerType.Biome:
		case TerrainManager.LayerType.Topology:
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetRange(TerrainManager.GetSplatMap(landLayerToPaint, topology), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetHeights(TerrainManager.TerrainType.Land)), t, heightLow, heightHigh, null), landLayerToPaint, topology);
			return;
		case TerrainManager.LayerType.Alpha:
		{
			bool value = t == 0;
			TerrainManager.SetAlphaMap(RustMapEditor.Maths.Array.SetRange(TerrainManager.GetAlphaMap(), TerrainManager.GetHeights(TerrainManager.TerrainType.Land), value, heightLow, heightHigh, null));
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000512AC File Offset: 0x0004F4AC
	[ConsoleCommand("Paints heights with gradients")]
	public static void PaintHeightBlend(Layers layerData, float heightLow, float heightHigh, float minBlendLow, float maxBlendHigh, int t)
	{
		TerrainManager.LayerType layer;
		if (layerData.Ground != (TerrainSplat.Enum)0)
		{
			layer = TerrainManager.LayerType.Ground;
			TerrainSplat.TypeToIndex((int)layerData.Ground);
		}
		else
		{
			if (layerData.Biome == (TerrainBiome.Enum)0)
			{
				Debug.LogError("PaintHeightBlend only supports Ground and Biome layers.");
				return;
			}
			layer = TerrainManager.LayerType.Biome;
			TerrainBiome.TypeToIndex((int)layerData.Biome);
		}
		if (minBlendLow >= heightLow || heightLow >= heightHigh || heightHigh >= maxBlendHigh)
		{
			Debug.LogError(string.Format("Invalid height range: minBlendLow ({0}) must be < heightLow ({1}) < heightHigh ({2}) < maxBlendHigh ({3}).", new object[]
			{
				minBlendLow,
				heightLow,
				heightHigh,
				maxBlendHigh
			}));
			return;
		}
		TerrainManager.SetLayerData(RustMapEditor.Maths.Array.SetRangeBlend(TerrainManager.GetSplatMap(layer, -1), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetHeights(TerrainManager.TerrainType.Land)), t, heightLow / 1000f, heightHigh / 1000f, minBlendLow / 1000f, maxBlendHigh / 1000f, null), layer, -1);
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00007394 File Offset: 0x00005594
	public static void PaintHeightBlend(TerrainManager.LayerType landLayerToPaint, float heightLow, float heightHigh, float minBlendLow, float maxBlendHigh, int t)
	{
		if (landLayerToPaint <= TerrainManager.LayerType.Biome)
		{
			TerrainManager.SetLayerData(RustMapEditor.Maths.Array.SetRangeBlend(TerrainManager.GetSplatMap(landLayerToPaint, -1), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetHeights(TerrainManager.TerrainType.Land)), t, heightLow, heightHigh, minBlendLow, maxBlendHigh, null), landLayerToPaint, -1);
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x000073C0 File Offset: 0x000055C0
	public static void PaintLayer(TerrainManager.LayerType landLayerToPaint, int t, int topology = 0)
	{
		switch (landLayerToPaint)
		{
		case TerrainManager.LayerType.Ground:
		case TerrainManager.LayerType.Biome:
		case TerrainManager.LayerType.Topology:
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetValues(TerrainManager.GetSplatMap(landLayerToPaint, topology), t, null), landLayerToPaint, topology);
			return;
		case TerrainManager.LayerType.Alpha:
			TerrainManager.SetAlphaMap(RustMapEditor.Maths.Array.SetValues(TerrainManager.GetAlphaMap(), true, null));
			return;
		default:
			return;
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0005137C File Offset: 0x0004F57C
	public static void PaintTopologyLayers(TerrainTopology.Enum topologyLayers)
	{
		List<int> enumSelection = MapManager.GetEnumSelection<TerrainTopology.Enum>(topologyLayers);
		for (int i = 0; i < enumSelection.Count; i++)
		{
			MapManager.PaintLayer(TerrainManager.LayerType.Topology, 0, i);
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00007400 File Offset: 0x00005600
	public static void ClearLayer(TerrainManager.LayerType landLayerToPaint, int topology = 0)
	{
		if (landLayerToPaint != TerrainManager.LayerType.Alpha)
		{
			if (landLayerToPaint == TerrainManager.LayerType.Topology)
			{
				TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetValues(TerrainManager.GetSplatMap(landLayerToPaint, topology), 1, null), landLayerToPaint, topology);
				return;
			}
		}
		else
		{
			TerrainManager.SetAlphaMap(RustMapEditor.Maths.Array.SetValues(TerrainManager.GetAlphaMap(), false, null));
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x000513AC File Offset: 0x0004F5AC
	public static void ClearTopologyLayers(TerrainTopology.Enum topologyLayers)
	{
		List<int> enumSelection = MapManager.GetEnumSelection<TerrainTopology.Enum>(topologyLayers);
		for (int i = 0; i < enumSelection.Count; i++)
		{
			MapManager.ClearLayer(TerrainManager.LayerType.Topology, i);
		}
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x00007431 File Offset: 0x00005631
	public static void InvertLayer(TerrainManager.LayerType landLayerToPaint, int topology = 0)
	{
		if (landLayerToPaint != TerrainManager.LayerType.Alpha)
		{
			if (landLayerToPaint == TerrainManager.LayerType.Topology)
			{
				TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.Invert(TerrainManager.GetSplatMap(landLayerToPaint, topology), null), landLayerToPaint, topology);
				return;
			}
		}
		else
		{
			TerrainManager.SetAlphaMap(RustMapEditor.Maths.Array.Invert(TerrainManager.GetAlphaMap(), null));
		}
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x000513D8 File Offset: 0x0004F5D8
	public static void InvertTopologyLayers(TerrainTopology.Enum topologyLayers)
	{
		List<int> enumSelection = MapManager.GetEnumSelection<TerrainTopology.Enum>(topologyLayers);
		for (int i = 0; i < enumSelection.Count; i++)
		{
			MapManager.InvertLayer(TerrainManager.LayerType.Topology, i);
		}
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00051404 File Offset: 0x0004F604
	public static void PaintSlope(TerrainManager.LayerType landLayerToPaint, float slopeLow, float slopeHigh, int t, int topology = 0)
	{
		switch (landLayerToPaint)
		{
		case TerrainManager.LayerType.Ground:
		case TerrainManager.LayerType.Biome:
		case TerrainManager.LayerType.Topology:
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetRange(TerrainManager.GetSplatMap(landLayerToPaint, topology), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetSlopes()), t, slopeLow, slopeHigh, null), landLayerToPaint, topology);
			return;
		case TerrainManager.LayerType.Alpha:
		{
			bool value = t == 0;
			TerrainManager.SetAlphaMap(RustMapEditor.Maths.Array.SetRange(TerrainManager.GetAlphaMap(), TerrainManager.GetSlopes(), value, slopeLow, slopeHigh, null));
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00007460 File Offset: 0x00005660
	public static void PaintSlopeBlend(TerrainManager.LayerType landLayerToPaint, float slopeLow, float slopeHigh, float minBlendLow, float maxBlendHigh, int t)
	{
		if (landLayerToPaint <= TerrainManager.LayerType.Biome)
		{
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetRangeBlend(TerrainManager.GetSplatMap(landLayerToPaint, -1), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetSlopes()), t, slopeLow, slopeHigh, minBlendLow, maxBlendHigh, null), landLayerToPaint, -1);
		}
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0000748B File Offset: 0x0000568B
	public static void PaintCurve(TerrainManager.LayerType landLayerToPaint, float curveLow, float curveHigh, int t, int topology = 0)
	{
		TerrainManager.UpdateHeightCache();
		if (landLayerToPaint == TerrainManager.LayerType.Ground)
		{
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetRange(TerrainManager.GetSplatMap(landLayerToPaint, topology), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetCurves(1000)), t, curveLow, curveHigh, null), landLayerToPaint, topology);
		}
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x000074BD File Offset: 0x000056BD
	public static float[,,] GetCurve(TerrainManager.LayerType landLayerToPaint, float curveLow, float curveHigh, int t, int topology = 0)
	{
		TerrainManager.UpdateHeightCache();
		if (landLayerToPaint == TerrainManager.LayerType.Ground)
		{
			return RustMapEditor.Maths.Array.SetRange(TerrainManager.GetSplatMap(landLayerToPaint, topology), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetCurves(1000)), t, curveLow, curveHigh, null);
		}
		return null;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x000074E9 File Offset: 0x000056E9
	public static void PaintCurveBlend(TerrainManager.LayerType landLayerToPaint, float curveLow, float curveHigh, float minBlendLow, float maxBlendHigh, int t)
	{
		TerrainManager.UpdateHeightCache();
		if (landLayerToPaint == TerrainManager.LayerType.Ground)
		{
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetRangeBlend(TerrainManager.GetSplatMap(landLayerToPaint, -1), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetCurves(1000)), t, curveLow, curveHigh, minBlendLow, maxBlendHigh, null), landLayerToPaint, -1);
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0005146C File Offset: 0x0004F66C
	public static void PaintRiver(TerrainManager.LayerType landLayerToPaint, bool aboveTerrain, int tex, int topology = 0)
	{
		switch (landLayerToPaint)
		{
		case TerrainManager.LayerType.Ground:
		case TerrainManager.LayerType.Biome:
		case TerrainManager.LayerType.Topology:
			TerrainManager.SetSplatMap(RustMapEditor.Maths.Array.SetRiver(TerrainManager.GetSplatMap(landLayerToPaint, topology), TerrainManager.GetHeights(TerrainManager.TerrainType.Land), TerrainManager.GetHeights(TerrainManager.TerrainType.Water), aboveTerrain, tex, null), landLayerToPaint, topology);
			return;
		case TerrainManager.LayerType.Alpha:
			TerrainManager.SetAlphaMap(RustMapEditor.Maths.Array.SetRiver(TerrainManager.GetAlphaMap(), TerrainManager.GetHeights(TerrainManager.TerrainType.Land), RustMapEditor.Maths.Array.HeightToSplat(TerrainManager.GetHeights(TerrainManager.TerrainType.Water)), aboveTerrain, tex == 0, null));
			return;
		default:
			return;
		}
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000514DC File Offset: 0x0004F6DC
	public static void CentreSceneObjects(WorldConverter.MapInfo mapInfo)
	{
		Vector3 position = new Vector3(mapInfo.size.x / 2f, 500f, mapInfo.size.z / 2f);
		PrefabManager.PrefabParent.GetComponent<LockObject>().SetPosition(position);
		if (PrefabManager.EditorSpace.GetComponent<LockObject>() != null)
		{
			PrefabManager.EditorSpace.GetComponent<LockObject>().SetPosition(position);
		}
		PathManager.PathParent.GetComponent<LockObject>().SetPosition(position);
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0005155C File Offset: 0x0004F75C
	public static void SaveMonument(string path)
	{
		string str = path.Split('/', StringSplitOptions.None).Last<string>().Split('.', StringSplitOptions.None)[0];
		PrefabManager.RenamePrefabCategories(PrefabManager.CurrentMapPrefabs, ":" + str + "::");
		PrefabManager.RenameNPCs(PrefabManager.CurrentMapNPCs, ":" + str + "::");
		Debug.LogError("attempting to save monument");
		MonumentManager.TerrainToRMPrefab(TerrainManager.Land, TerrainManager.Water).SaveRMPrefab(path);
		MapManager.Callbacks.OnMapSaved(path);
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0000751D File Offset: 0x0000571D
	public static void Load(WorldConverter.MapInfo mapInfo, string loadPath = "")
	{
		CoroutineManager.Instance.StartRuntimeCoroutine(MapManager.Coroutines.Load(mapInfo, loadPath));
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00007531 File Offset: 0x00005731
	public static void Save(string path)
	{
		PrefabManager.BlacklistCurrent();
		CoroutineManager.Instance.StartRuntimeCoroutine(MapManager.Coroutines.Save(path));
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x000515DC File Offset: 0x0004F7DC
	public static void SaveCustomPrefab(string path)
	{
		string str = path.Split('/', StringSplitOptions.None).Last<string>().Split('.', StringSplitOptions.None)[0];
		PrefabManager.RenamePrefabCategories(PrefabManager.CurrentMapPrefabs, ":" + str + "::");
		PrefabManager.RenameNPCs(PrefabManager.CurrentMapNPCs, ":" + str + "::");
		CoroutineManager.Instance.StartRuntimeCoroutine(MapManager.Coroutines.SaveCustomPrefab(path));
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00051648 File Offset: 0x0004F848
	[ConsoleCommand("export prefabdata into JSON")]
	public static void SaveJson(string path, List<WorldSerialization.PathData> paths = null)
	{
		string str = path.Split('/', StringSplitOptions.None).Last<string>().Split('.', StringSplitOptions.None)[0];
		PrefabManager.RenamePrefabCategories(PrefabManager.CurrentMapPrefabs, ":" + str + "::");
		PrefabManager.RenameNPCs(PrefabManager.CurrentMapNPCs, ":" + str + "::");
		if (paths == null)
		{
			paths = (from nc in PathManager.CurrentMapPaths
			where nc != null && nc.pathData != null
			select nc.pathData).ToList<WorldSerialization.PathData>();
			Debug.Log(string.Format("SaveJson: Using CurrentMapPaths, found {0} valid PathData entries.", paths.Count));
		}
		else
		{
			Debug.Log(string.Format("SaveJson: Using provided paths list with {0} entries.", paths.Count));
		}
		WorldConverter.TerrainToCustomPrefab(new ValueTuple<int, int>(-1, 0)).SavePrefabJSON(path, paths);
		MapManager.Callbacks.OnMapSaved(path);
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00051748 File Offset: 0x0004F948
	public static void LoadDumpJSON(string path)
	{
		try
		{
			string text = File.ReadAllText(path);
			Debug.Log("JSON Content: " + text.Substring(0, Math.Min(text.Length, 200)) + "...");
			MapManager.SpawnData spawnData = JsonConvert.DeserializeObject<MapManager.SpawnData>(text);
			if (((spawnData != null) ? spawnData.Spawns : null) == null || spawnData.Spawns.Length == 0)
			{
				Debug.LogError("No spawns found in JSON data or JSON is invalid");
			}
			else
			{
				for (int i = 0; i < spawnData.Spawns.Length; i++)
				{
					MapManager.SpawnEntry spawnEntry = spawnData.Spawns[i];
					if (spawnEntry == null)
					{
						Debug.LogWarning(string.Format("Spawn entry at index {0} is null", i));
					}
					else if (string.IsNullOrEmpty(spawnEntry.PrefabPath))
					{
						Debug.LogWarning(string.Format("Spawn entry at index {0} has null or empty PrefabPath", i));
					}
					else if (spawnEntry.Position == null)
					{
						Debug.LogWarning(string.Format("Spawn entry at index {0} has null Position", i));
					}
					else
					{
						Debug.Log(string.Format("Processing spawn entry {0}: PrefabPath={1}, Position=({2}, {3}, {4})", new object[]
						{
							i,
							spawnEntry.PrefabPath,
							spawnEntry.Position.x,
							spawnEntry.Position.y,
							spawnEntry.Position.z
						}));
						Vector3 position = new Vector3(spawnEntry.Position.x, spawnEntry.Position.y, spawnEntry.Position.z);
						uint num;
						if (AssetManager.PathLookup.TryGetValue(spawnEntry.PrefabPath, out num))
						{
							try
							{
								GeologyItem geologyItem = new GeologyItem(num, 0, int.MaxValue);
								if (geologyItem == null)
								{
									Debug.LogWarning(string.Format("Failed to create GeologyItem for ID {0} at index {1}", num, i));
									goto IL_241;
								}
								if (PrefabManager.PrefabParent == null)
								{
									Debug.LogWarning(string.Format("PrefabManager.PrefabParent is null for spawn entry at index {0}", i));
									goto IL_241;
								}
								GenerativeManager.spawnGeoItem(geologyItem, position, Vector3.zero, Vector3.one, PrefabManager.PrefabParent);
								goto IL_241;
							}
							catch (Exception ex)
							{
								Debug.LogError(string.Format("Failed to spawn item for entry {0} (PrefabPath={1}): {2}\n{3}", new object[]
								{
									i,
									spawnEntry.PrefabPath,
									ex.Message,
									ex.StackTrace
								}));
								goto IL_241;
							}
						}
						Debug.LogWarning(string.Format("Prefab path not found: {0} at index {1}", spawnEntry.PrefabPath, i));
					}
					IL_241:;
				}
			}
		}
		catch (JsonException ex2)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"JSON parsing error for ",
				path,
				": ",
				ex2.Message,
				"\n",
				ex2.StackTrace
			}));
		}
		catch (Exception ex3)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Failed to load or process JSON from ",
				path,
				": ",
				ex3.Message,
				"\n",
				ex3.StackTrace
			}));
		}
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x00051A84 File Offset: 0x0004FC84
	[ConsoleCommand("Creates a new map synchronously")]
	public static void NewMap(int size)
	{
		WorldConverter.MapInfo mapInfo = WorldConverter.EmptyMap(size, 503f, TerrainSplat.Enum.Grass, TerrainBiome.Enum.Temperate);
		MapManager.isLoading = true;
		PrefabManager.DeletePrefabs(PrefabManager.CurrentMapPrefabs, 0);
		PathManager.DeletePaths(PathManager.CurrentMapPaths, 0);
		MapManager.CentreSceneObjects(mapInfo);
		TerrainManager.Land.terrainData.heightmapResolution = mapInfo.terrainRes;
		TerrainManager.Land.terrainData.size = mapInfo.size;
		TerrainManager.Land.terrainData.alphamapResolution = mapInfo.splatRes;
		TerrainManager.Land.terrainData.baseMapResolution = mapInfo.splatRes;
		TerrainManager.Ocean.terrainData.heightmapResolution = mapInfo.terrainRes;
		TerrainManager.Ocean.terrainData.size = mapInfo.size;
		TerrainManager.Ocean.terrainData.alphamapResolution = mapInfo.splatRes;
		TerrainManager.Ocean.terrainData.baseMapResolution = mapInfo.splatRes;
		TerrainManager.Water.terrainData.heightmapResolution = mapInfo.terrainRes;
		TerrainManager.Water.terrainData.alphamapResolution = mapInfo.splatRes;
		TerrainManager.Water.terrainData.baseMapResolution = mapInfo.splatRes;
		TerrainManager.Water.terrainData.size = mapInfo.size;
		TerrainManager.Land.terrainData.SetHeights(0, 0, mapInfo.land.heights);
		TerrainManager.Water.terrainData.SetHeights(0, 0, mapInfo.water.heights);
		TerrainManager.Callbacks.InvokeHeightMapUpdated(TerrainManager.TerrainType.Land);
		TerrainManager.SyncTerrainResolutions();
		TerrainManager.SetSplatMap(mapInfo.splatMap, TerrainManager.LayerType.Ground, -1);
		TerrainManager.SetSplatMap(mapInfo.biomeMap, TerrainManager.LayerType.Biome, -1);
		TerrainManager.SetAlphaMap(mapInfo.alphaMap);
		TopologyData.Set(mapInfo.topology);
		for (int i = 0; i < 31; i++)
		{
			TerrainManager.SetSplatMap(TopologyData.GetTopologyLayer(TerrainTopology.IndexToType(i)), TerrainManager.LayerType.Topology, i);
		}
		TerrainManager.ChangeLayer(TerrainManager.LayerType.Ground, -1);
		PrefabManager.SpawnPrefabs(mapInfo.prefabData, 0);
		PathManager.SpawnPaths(mapInfo.pathData, 0);
		AreaManager.Reset();
		TerrainManager.ClearUndo();
		MapManager.isLoading = false;
		MapManager.Callbacks.OnMapLoaded("New Map");
		Debug.Log(string.Format("New map of size {0} created successfully.", size));
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00007549 File Offset: 0x00005749
	public static void CreateMap(int size, TerrainSplat.Enum ground, TerrainBiome.Enum biome, float landHeight = 503f)
	{
		CoroutineManager.Instance.StartCoroutine(MapManager.Coroutines.CreateMap(size, ground, biome, landHeight));
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x0000755F File Offset: 0x0000575F
	public static void MergeREPrefab(WorldConverter.MapInfo mapInfo, string loadPath = "")
	{
		PrefabManager.SpawnPrefabs(mapInfo.prefabData, 0);
		PrefabManager.SpawnCircuits(mapInfo.circuitData, 0);
		PrefabManager.SpawnNPCs(mapInfo.npcData, 0);
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00051CA0 File Offset: 0x0004FEA0
	[ConsoleCommand("Loads a map from a map file and adds only prefabs with IDs not already in CurrentMapPrefabs")]
	public static void LoadUniquePrefabs(string path)
	{
		int progressID = 0;
		try
		{
			WorldSerialization worldSerialization = new WorldSerialization();
			worldSerialization.Load(path);
			WorldConverter.MapInfo mapInfo = WorldConverter.WorldToTerrain(worldSerialization);
			HashSet<uint> existingPrefabIDs = new HashSet<uint>();
			foreach (PrefabDataHolder prefabDataHolder in PrefabManager.CurrentMapPrefabs)
			{
				if (prefabDataHolder != null)
				{
					existingPrefabIDs.Add(prefabDataHolder.prefabData.id);
				}
			}
			List<WorldSerialization.PrefabData> list = (from prefab in mapInfo.prefabData
			where !existingPrefabIDs.Contains(prefab.id)
			select prefab).ToList<WorldSerialization.PrefabData>();
			Debug.Log(string.Format("Loading {0} unique prefabs out of {1} total prefabs from {2}.", list.Count, mapInfo.prefabData.Length, path));
			PrefabManager.SpawnPrefabs(list.ToArray(), progressID);
			MapManager.Callbacks.OnMapLoaded(path);
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Failed to load or process prefabs from ",
				path,
				": ",
				ex.Message,
				"\n",
				ex.StackTrace
			}));
		}
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00007585 File Offset: 0x00005785
	public static void MergeOffsetREPrefab(WorldConverter.MapInfo mapInfo, Transform parent, string loadPath = "")
	{
		PrefabManager.SpawnCustomPrefabs(mapInfo.prefabData, 0, parent);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00007594 File Offset: 0x00005794
	public static void SaveCollectionPrefab(string path, Transform collectionRoot)
	{
		CoroutineManager.Instance.StartRuntimeCoroutine(MapManager.Coroutines.SaveCollectionPrefab(path, collectionRoot));
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x000075A8 File Offset: 0x000057A8
	public static void LoadRMPrefab(WorldSerialization world, string loadPath = "")
	{
		CoroutineManager.Instance.StartRuntimeCoroutine(MapManager.Coroutines.LoadRMPrefab(world, loadPath));
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x000075BC File Offset: 0x000057BC
	public static void LoadREPrefab(WorldConverter.MapInfo mapInfo, string loadPath)
	{
		CoroutineManager.Instance.StartRuntimeCoroutine(MapManager.Coroutines.LoadREPrefab(mapInfo, loadPath));
	}

	// Token: 0x04000748 RID: 1864
	public static bool isLoading;

	// Token: 0x02000164 RID: 356
	public static class Callbacks
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060008A4 RID: 2212 RVA: 0x00051DC4 File Offset: 0x0004FFC4
		// (remove) Token: 0x060008A5 RID: 2213 RVA: 0x00051DF8 File Offset: 0x0004FFF8
		public static event MapManager.Callbacks.MapManagerCallback MapLoaded;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060008A6 RID: 2214 RVA: 0x00051E2C File Offset: 0x0005002C
		// (remove) Token: 0x060008A7 RID: 2215 RVA: 0x00051E60 File Offset: 0x00050060
		public static event MapManager.Callbacks.MapManagerCallback MapSaved;

		// Token: 0x060008A8 RID: 2216 RVA: 0x000075D0 File Offset: 0x000057D0
		public static void OnMapLoaded(string mapName = "")
		{
			MapManager.Callbacks.MapManagerCallback mapLoaded = MapManager.Callbacks.MapLoaded;
			if (mapLoaded == null)
			{
				return;
			}
			mapLoaded(mapName);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x000075E2 File Offset: 0x000057E2
		public static void OnMapSaved(string mapName = "")
		{
			MapManager.Callbacks.MapManagerCallback mapSaved = MapManager.Callbacks.MapSaved;
			if (mapSaved == null)
			{
				return;
			}
			mapSaved(mapName);
		}

		// Token: 0x02000165 RID: 357
		// (Invoke) Token: 0x060008AB RID: 2219
		public delegate void MapManagerCallback(string mapName = "");
	}

	// Token: 0x02000166 RID: 358
	[Serializable]
	public class SpawnData
	{
		// Token: 0x0400074B RID: 1867
		public MapManager.SpawnEntry[] Spawns;
	}

	// Token: 0x02000167 RID: 359
	[Serializable]
	public class SpawnEntry
	{
		// Token: 0x0400074C RID: 1868
		public string Timestamp;

		// Token: 0x0400074D RID: 1869
		public string PlayerName;

		// Token: 0x0400074E RID: 1870
		public long PlayerId;

		// Token: 0x0400074F RID: 1871
		public string PrefabPath;

		// Token: 0x04000750 RID: 1872
		public string ShortName;

		// Token: 0x04000751 RID: 1873
		public MapManager.Position Position;

		// Token: 0x04000752 RID: 1874
		public bool IsItem;

		// Token: 0x04000753 RID: 1875
		public string Command;
	}

	// Token: 0x02000168 RID: 360
	[Serializable]
	public class Position
	{
		// Token: 0x04000754 RID: 1876
		public float x;

		// Token: 0x04000755 RID: 1877
		public float y;

		// Token: 0x04000756 RID: 1878
		public float z;
	}

	// Token: 0x02000169 RID: 361
	private class Coroutines
	{
		// Token: 0x060008B1 RID: 2225 RVA: 0x000075F4 File Offset: 0x000057F4
		public static IEnumerator LoadREPrefab(WorldConverter.MapInfo mapInfo, string loadPath)
		{
			MapManager.isLoading = true;
			yield return PrefabManager.DeletePrefabs(PrefabManager.CurrentMapPrefabs, 0);
			PrefabManager.DeleteCircuits(PrefabManager.CurrentMapElectrics, 0);
			PrefabManager.DeleteNPCs(PrefabManager.CurrentMapNPCs, 0);
			PrefabManager.DeleteModifiers(PrefabManager.CurrentModifiers);
			TerrainManager.Load(mapInfo, 0);
			MapManager.CentreSceneObjects(mapInfo);
			PrefabManager.SpawnPrefabs(mapInfo.prefabData, 0);
			PrefabManager.SpawnCircuits(mapInfo.circuitData, 0);
			PrefabManager.SpawnNPCs(mapInfo.npcData, 0);
			PrefabManager.SpawnModifiers(mapInfo.modifierData);
			MapManager.isLoading = false;
			MapManager.Callbacks.OnMapLoaded(loadPath);
			yield return null;
			yield break;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0000760A File Offset: 0x0000580A
		public static IEnumerator LoadRMPrefab(WorldSerialization world, string loadPath = "")
		{
			MapManager.isLoading = true;
			yield return PrefabManager.DeletePrefabs(PrefabManager.CurrentMapPrefabs, 0);
			PrefabManager.DeleteCircuits(PrefabManager.CurrentMapElectrics, 0);
			PrefabManager.DeleteNPCs(PrefabManager.CurrentMapNPCs, 0);
			PrefabManager.DeleteModifiers(PrefabManager.CurrentModifiers);
			WorldConverter.MapInfo mapInfo = WorldConverter.RMPrefabToTerrain(world);
			TerrainManager.Load(mapInfo, 0);
			MapManager.CentreSceneObjects(mapInfo);
			PrefabManager.SpawnPrefabs(mapInfo.prefabData, 0);
			PrefabManager.SpawnCircuits(mapInfo.circuitData, 0);
			PrefabManager.SpawnNPCs(mapInfo.npcData, 0);
			PrefabManager.SpawnModifiers(mapInfo.modifierData);
			MapManager.isLoading = false;
			MapManager.Callbacks.OnMapLoaded(loadPath);
			yield return null;
			yield break;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00007620 File Offset: 0x00005820
		public static IEnumerator SaveCollectionPrefab(string path, Transform collectionRoot)
		{
			WorldSerialization world = WorldConverter.CollectionToREPrefab(collectionRoot);
			yield return null;
			world.SaveREPrefab(path);
			yield break;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00007636 File Offset: 0x00005836
		public static IEnumerator Load(WorldConverter.MapInfo mapInfo, string path = "")
		{
			LoadScreen.Instance.Show();
			MapManager.isLoading = true;
			yield return PrefabManager.DeletePrefabs(PrefabManager.CurrentMapPrefabs, 0);
			PathManager.DeletePaths(PathManager.CurrentMapPaths, 0);
			MapManager.CentreSceneObjects(mapInfo);
			TerrainManager.Load(mapInfo, 0);
			PrefabManager.SpawnPrefabs(mapInfo.prefabData, 0);
			PathManager.SpawnPaths(mapInfo.pathData, 0);
			MapManager.Callbacks.OnMapLoaded(path);
			yield return null;
			MapManager.isLoading = false;
			yield break;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0000764C File Offset: 0x0000584C
		public static IEnumerator Save(string path)
		{
			TerrainManager.SaveLayer();
			yield return null;
			yield return MapManager.Coroutines.BlacklistCurrent();
			WorldConverter.TerrainToWorld(TerrainManager.Land, TerrainManager.Water, new ValueTuple<int, int, int>(0, 0, 0)).Save(path);
			MapManager.Callbacks.OnMapSaved(path);
			yield break;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0000765B File Offset: 0x0000585B
		private static IEnumerator BlacklistCurrent()
		{
			PrefabDataHolder[] currentMapPrefabs = PrefabManager.CurrentMapPrefabs;
			List<PrefabDataHolder> blacklistedPrefabs = new List<PrefabDataHolder>();
			foreach (PrefabDataHolder prefabDataHolder in currentMapPrefabs)
			{
				if (prefabDataHolder != null)
				{
					string key = AssetManager.ToPath(prefabDataHolder.prefabData.id).Replace("\\", "/");
					ItemSettings itemSettings;
					if (PrefabManager.ItemBlacklist.TryGetValue(key, out itemSettings) && itemSettings.blacklisted)
					{
						blacklistedPrefabs.Add(prefabDataHolder);
					}
				}
			}
			if (blacklistedPrefabs.Count == 0)
			{
				Debug.Log("No blacklisted prefabs found in current map.");
				yield break;
			}
			string message = string.Format("Found {0} blacklisted prefabs. Delete?", blacklistedPrefabs.Count);
			Task<bool> confirmationTask = ConfirmationManager.Instance.ShowConfirmationAsync("Bad Prefabs", message, "Delete", "Cancel");
			while (!confirmationTask.IsCompleted)
			{
				yield return null;
			}
			if (confirmationTask.Result)
			{
				int num = 0;
				foreach (PrefabDataHolder prefabDataHolder2 in blacklistedPrefabs)
				{
					if (prefabDataHolder2 != null && prefabDataHolder2.gameObject != null)
					{
						UnityEngine.Object.DestroyImmediate(prefabDataHolder2.gameObject);
						num++;
					}
				}
				PrefabManager.NotifyItemsChanged(true);
				Debug.Log(string.Format("Deleted {0} blacklisted prefabs.", num));
			}
			else
			{
				Debug.Log("Blacklist deletion cancelled by user.");
			}
			yield break;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00007663 File Offset: 0x00005863
		public static IEnumerator CreateMap(int size, TerrainSplat.Enum ground, TerrainBiome.Enum biome, float landHeight = 503f)
		{
			yield return CoroutineManager.Instance.StartCoroutine(MapManager.Coroutines.Load(WorldConverter.EmptyMap(size, landHeight, ground, biome), "New Map"));
			yield break;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00007687 File Offset: 0x00005887
		public static IEnumerator SaveCustomPrefab(string path)
		{
			yield return null;
			WorldConverter.TerrainToCustomPrefab(new ValueTuple<int, int>(0, 0)).SaveREPrefab(path);
			MapManager.Callbacks.OnMapSaved(path);
			yield break;
		}
	}
}
