using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using LZ4;
using Newtonsoft.Json;
using ProtoBuf;
using RustMapEditor.Variables;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class WorldSerialization
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000046 RID: 70 RVA: 0x00002997 File Offset: 0x00000B97
	// (set) Token: 0x06000047 RID: 71 RVA: 0x0000299E File Offset: 0x00000B9E
	public static uint Version { get; private set; }

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000048 RID: 72 RVA: 0x000029A6 File Offset: 0x00000BA6
	// (set) Token: 0x06000049 RID: 73 RVA: 0x000029AE File Offset: 0x00000BAE
	public string Checksum { get; private set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600004A RID: 74 RVA: 0x000029B7 File Offset: 0x00000BB7
	// (set) Token: 0x0600004B RID: 75 RVA: 0x000029BF File Offset: 0x00000BBF
	public long Timestamp { get; private set; }

	// Token: 0x0600004C RID: 76 RVA: 0x0001E600 File Offset: 0x0001C800
	public WorldSerialization()
	{
		WorldSerialization.Version = 10U;
		this.Checksum = null;
		this.Timestamp = 0L;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000029C8 File Offset: 0x00000BC8
	public int CalculateCount()
	{
		return this.world.maps.Count + this.world.prefabs.Count + this.world.paths.Count;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x0001E678 File Offset: 0x0001C878
	public WorldSerialization.MapData GetMap(string name)
	{
		for (int i = 0; i < this.world.maps.Count; i++)
		{
			if (this.world.maps[i].name == name)
			{
				return this.world.maps[i];
			}
		}
		return null;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0001E6D4 File Offset: 0x0001C8D4
	public void AddMap(string name, byte[] data)
	{
		WorldSerialization.MapData mapData = new WorldSerialization.MapData();
		mapData.name = name;
		mapData.data = data;
		this.world.maps.Add(mapData);
	}

	// Token: 0x06000050 RID: 80 RVA: 0x0001E708 File Offset: 0x0001C908
	public void Save(string fileName)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					binaryWriter.Write(WorldSerialization.Version);
					binaryWriter.Write(DateTimeOffset.Now.ToUnixTimeMilliseconds());
					Debug.Log("saved Version: " + WorldSerialization.Version.ToString() + ", Stream Position: " + fileStream.Position.ToString());
					using (LZ4Stream lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Compress, LZ4StreamFlags.None, 1048576))
					{
						Serializer.Serialize<WorldSerialization.WorldData>(lz4Stream, this.world);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x0001E7F8 File Offset: 0x0001C9F8
	public void SaveREPrefab(string fileName)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					binaryWriter.Write(1U);
					using (LZ4Stream lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Compress, LZ4StreamFlags.None, 1048576))
					{
						Serializer.Serialize<WorldSerialization.REPrefabData>(lz4Stream, this.rePrefab);
					}
				}
			}
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream2 = File.OpenRead(fileName))
				{
					string text = BitConverter.ToString(md.ComputeHash(fileStream2)).Replace("-", "").ToLower();
					Debug.LogError(text);
					this.rePrefab.checksum = text;
				}
			}
			using (FileStream fileStream3 = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (BinaryWriter binaryWriter2 = new BinaryWriter(fileStream3))
				{
					binaryWriter2.Write(1U);
					using (LZ4Stream lz4Stream2 = new LZ4Stream(fileStream3, LZ4StreamMode.Compress, LZ4StreamFlags.None, 1048576))
					{
						Serializer.Serialize<WorldSerialization.REPrefabData>(lz4Stream2, this.rePrefab);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x0001EA08 File Offset: 0x0001CC08
	public void SaveRMPrefab(string fileName)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					binaryWriter.Write(1U);
					using (LZ4Stream lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Compress, LZ4StreamFlags.None, 1048576))
					{
						Serializer.Serialize<WorldSerialization.RMPrefabData>(lz4Stream, this.rmPrefab);
					}
				}
			}
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream2 = File.OpenRead(fileName))
				{
					string text = BitConverter.ToString(md.ComputeHash(fileStream2)).Replace("-", "").ToLower();
					Debug.Log(text);
					this.rmPrefab.checksum = text;
				}
			}
			using (FileStream fileStream3 = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				using (BinaryWriter binaryWriter2 = new BinaryWriter(fileStream3))
				{
					binaryWriter2.Write(1U);
					using (LZ4Stream lz4Stream2 = new LZ4Stream(fileStream3, LZ4StreamMode.Compress, LZ4StreamFlags.None, 1048576))
					{
						Serializer.Serialize<WorldSerialization.RMPrefabData>(lz4Stream2, this.rmPrefab);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
		Debug.Log("saved " + fileName);
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0001EC28 File Offset: 0x0001CE28
	public static byte[] SerializeTexture(Texture2D texture)
	{
		if (texture == null)
		{
			Debug.LogWarning("SerializeTexture(Texture2D): Texture is null, returning null.");
			return null;
		}
		Debug.Log(string.Format("SerializeTexture(Texture2D): Serializing texture '{0}' with resolution {1}x{2}, format: {3}", new object[]
		{
			texture.name,
			texture.width,
			texture.height,
			texture.format
		}));
		byte[] array = texture.EncodeToPNG();
		if (array == null || array.Length == 0)
		{
			Debug.LogError("SerializeTexture(Texture2D): Failed to serialize texture '" + texture.name + "'. Result is null or empty.");
		}
		else
		{
			Debug.Log(string.Format("SerializeTexture(Texture2D): Successfully serialized texture '{0}' to PNG, size: {1} bytes.", texture.name, array.Length));
		}
		return array;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x0001ECDC File Offset: 0x0001CEDC
	public static byte[] SerializeR8Texture(RenderTexture renderTexture)
	{
		if (renderTexture == null)
		{
			Debug.LogWarning("SerializeR8Texture(RenderTexture): RenderTexture is null, returning null.");
			return null;
		}
		if (renderTexture.format != RenderTextureFormat.R8)
		{
			Debug.LogWarning(string.Format("SerializeR8Texture(RenderTexture): RenderTexture '{0}' format is {1}, expected R8. Attempting to serialize anyway.", renderTexture.name, renderTexture.format));
		}
		Debug.Log(string.Format("SerializeR8Texture(RenderTexture): Serializing RenderTexture '{0}' with resolution {1}x{2}, format: {3}", new object[]
		{
			renderTexture.name,
			renderTexture.width,
			renderTexture.height,
			renderTexture.format
		}));
		Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.R8, false)
		{
			name = "TempR8TextureConversion"
		};
		Texture2D texture2D2 = null;
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		byte[] result;
		try
		{
			texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
			texture2D.Apply();
			byte[] array = texture2D.GetRawTextureData<byte>().ToArray();
			if (array == null || array.Length == 0)
			{
				Debug.LogError("SerializeR8Texture(RenderTexture): Failed to read R8 data from RenderTexture '" + renderTexture.name + "'.");
				result = null;
			}
			else
			{
				Color32[] array2 = new Color32[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = new Color32(0, 0, 0, array[i]);
				}
				texture2D2 = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false)
				{
					name = "TempRgbaTextureConversion"
				};
				texture2D2.SetPixels32(array2);
				texture2D2.Apply();
				byte[] array3 = texture2D2.GetRawTextureData<byte>().ToArray();
				if (array3 == null || array3.Length == 0)
				{
					Debug.LogError("SerializeR8Texture(RenderTexture): Failed to serialize RenderTexture '" + renderTexture.name + "' to RGBA32. Result is null or empty.");
					result = null;
				}
				else
				{
					Debug.Log(string.Format("SerializeR8Texture(RenderTexture): Successfully serialized RenderTexture '{0}' to RGBA32 raw byte array (R8 in alpha), size: {1} bytes.", renderTexture.name, array3.Length));
					result = array3;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("SerializeR8Texture(RenderTexture): Exception occurred while serializing RenderTexture '" + renderTexture.name + "': " + ex.Message);
			result = null;
		}
		finally
		{
			if (texture2D != null)
			{
				UnityEngine.Object.DestroyImmediate(texture2D);
			}
			if (texture2D2 != null)
			{
				UnityEngine.Object.DestroyImmediate(texture2D2);
			}
			RenderTexture.active = active;
		}
		return result;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x0001EF40 File Offset: 0x0001D140
	public static byte[] SerializeTexture(RenderTexture renderTexture)
	{
		if (renderTexture == null)
		{
			Debug.LogWarning("SerializeTexture(RenderTexture): RenderTexture is null, returning null.");
			return null;
		}
		Debug.Log(string.Format("SerializeTexture(RenderTexture): Serializing RenderTexture '{0}' with resolution {1}x{2}, format: {3}", new object[]
		{
			renderTexture.name,
			renderTexture.width,
			renderTexture.height,
			renderTexture.format
		}));
		Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
		texture2D.name = "TempRenderTextureConversion";
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
		texture2D.Apply();
		byte[] array = texture2D.EncodeToPNG();
		UnityEngine.Object.Destroy(texture2D);
		RenderTexture.active = active;
		if (array == null || array.Length == 0)
		{
			Debug.LogError("SerializeTexture(RenderTexture): Failed to serialize RenderTexture '" + renderTexture.name + "'. Result is null or empty.");
		}
		else
		{
			Debug.Log(string.Format("SerializeTexture(RenderTexture): Successfully serialized RenderTexture '{0}' to PNG, size: {1} bytes.", renderTexture.name, array.Length));
		}
		return array;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x0001F054 File Offset: 0x0001D254
	public static Texture2D DeserializeTexture(byte[] data, TextureFormat format)
	{
		if (data == null)
		{
			Debug.LogError("Texture data is null.");
			return null;
		}
		Texture2D texture2D = new Texture2D(2, 2, format, false);
		if (!texture2D.LoadImage(data))
		{
			UnityEngine.Object.Destroy(texture2D);
			Debug.LogError("Failed to load PNG data into Texture2D.");
			return null;
		}
		return texture2D;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0001F098 File Offset: 0x0001D298
	public static RenderTexture DeserializeTexture(byte[] data, RenderTextureFormat format)
	{
		if (data == null)
		{
			Debug.LogError("Texture data is null.");
			return null;
		}
		Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false);
		if (!texture2D.LoadImage(data))
		{
			UnityEngine.Object.Destroy(texture2D);
			Debug.LogError("Failed to load PNG data into Texture2D.");
			return null;
		}
		RenderTexture renderTexture = new RenderTexture(texture2D.width, texture2D.height, 0, format, RenderTextureReadWrite.Linear)
		{
			wrapMode = TextureWrapMode.Clamp,
			enableRandomWrite = true
		};
		renderTexture.Create();
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		Graphics.Blit(texture2D, renderTexture);
		UnityEngine.Object.Destroy(texture2D);
		RenderTexture.active = active;
		return renderTexture;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x0001F124 File Offset: 0x0001D324
	[ConsoleCommand("unpack LZ4")]
	public static void Decompress(string fileName)
	{
		Debug.Log("decompressing...");
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					WorldSerialization.Version = binaryReader.ReadUInt32();
					binaryReader.ReadUInt64();
					using (LZ4Stream lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Decompress, LZ4StreamFlags.None, 1048576))
					{
						MemoryStream memoryStream = new MemoryStream();
						lz4Stream.CopyTo(memoryStream);
						byte[] bytes = new byte[memoryStream.Length];
						bytes = memoryStream.ToArray();
						File.WriteAllBytes("protobuftest", bytes);
						Debug.LogError("Actually decompressed");
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0001F20C File Offset: 0x0001D40C
	public void Load(string fileName)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					WorldSerialization.Version = binaryReader.ReadUInt32();
					if (WorldSerialization.Version != 9U)
					{
						if (WorldSerialization.Version == 10U)
						{
							binaryReader.ReadInt64();
						}
						else
						{
							Debug.LogError("wrong version:" + WorldSerialization.Version.ToString());
						}
					}
					Debug.Log("Loaded Version: " + WorldSerialization.Version.ToString() + ", Stream Position: " + fileStream.Position.ToString());
					using (LZ4Stream lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Decompress, LZ4StreamFlags.None, 1048576))
					{
						this.world = Serializer.Deserialize<WorldSerialization.WorldData>(lz4Stream);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x0001F320 File Offset: 0x0001D520
	public void LoadREPrefab(string fileName)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					binaryReader.ReadUInt32();
					using (LZ4Stream lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Decompress, LZ4StreamFlags.None, 1048576))
					{
						this.rePrefab = Serializer.Deserialize<WorldSerialization.REPrefabData>(lz4Stream);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0001F3C8 File Offset: 0x0001D5C8
	public void LoadRMPrefab(string fileName)
	{
		try
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream))
				{
					binaryReader.ReadUInt32();
					using (LZ4Stream lz4Stream = new LZ4Stream(fileStream, LZ4StreamMode.Decompress, LZ4StreamFlags.None, 1048576))
					{
						this.rmPrefab = Serializer.Deserialize<WorldSerialization.RMPrefabData>(lz4Stream);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x0001F470 File Offset: 0x0001D670
	public void SavePrefabJSON(string fileName, List<WorldSerialization.PathData> paths)
	{
		try
		{
			if (this.rePrefab == null)
			{
				Debug.LogError("SavePrefabJSON: rePrefab is null. Initializing to default.");
				this.rePrefab = new WorldSerialization.REPrefabData();
			}
			if (paths == null)
			{
				Debug.LogWarning("SavePrefabJSON: paths parameter is null. Initializing to empty list.");
				paths = new List<WorldSerialization.PathData>();
			}
			Debug.Log(string.Format("SavePrefabJSON: Saving to {0}, rePrefab.prefabs.Count={1}, paths.Count={2}", fileName, this.rePrefab.prefabs.Count, paths.Count));
			foreach (WorldSerialization.PathData pathData in paths)
			{
				string format = "SavePrefabJSON: Path - name={0}, nodes.Length={1}, spline={2}";
				object name = pathData.name;
				WorldSerialization.VectorData[] nodes = pathData.nodes;
				Debug.Log(string.Format(format, name, (nodes != null) ? nodes.Length : 0, pathData.spline));
			}
			string contents = JsonConvert.SerializeObject(new WorldSerialization.PrefabAndPathData(this.rePrefab, paths), Formatting.Indented, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Include
			});
			File.WriteAllText(fileName, contents);
			Debug.Log(string.Format("SavePrefabJSON: Saved JSON with {0} prefabs and {1} paths to {2}", this.rePrefab.prefabs.Count, paths.Count, fileName));
		}
		catch (Exception ex)
		{
			Debug.LogError("SavePrefabJSON: Error saving JSON - " + ex.Message);
		}
	}

	// Token: 0x040000CD RID: 205
	public const uint CurrentVersion = 10U;

	// Token: 0x040000CE RID: 206
	public const uint REPrefabVersion = 1U;

	// Token: 0x040000D2 RID: 210
	public WorldSerialization.WorldData world = new WorldSerialization.WorldData
	{
		size = 4000U,
		maps = new List<WorldSerialization.MapData>(),
		prefabs = new List<WorldSerialization.PrefabData>(),
		paths = new List<WorldSerialization.PathData>()
	};

	// Token: 0x040000D3 RID: 211
	public WorldSerialization.REPrefabData rePrefab = new WorldSerialization.REPrefabData();

	// Token: 0x040000D4 RID: 212
	public WorldSerialization.RMPrefabData rmPrefab = new WorldSerialization.RMPrefabData();

	// Token: 0x02000017 RID: 23
	[ProtoContract]
	public class WorldData
	{
		// Token: 0x040000D5 RID: 213
		[ProtoMember(1)]
		public uint size = 4000U;

		// Token: 0x040000D6 RID: 214
		[ProtoMember(2)]
		public List<WorldSerialization.MapData> maps = new List<WorldSerialization.MapData>();

		// Token: 0x040000D7 RID: 215
		[ProtoMember(3)]
		public List<WorldSerialization.PrefabData> prefabs = new List<WorldSerialization.PrefabData>();

		// Token: 0x040000D8 RID: 216
		[ProtoMember(4)]
		public List<WorldSerialization.PathData> paths = new List<WorldSerialization.PathData>();
	}

	// Token: 0x02000018 RID: 24
	[ProtoContract]
	public class REPrefabData : IExtensible
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00002A30 File Offset: 0x00000C30
		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		// Token: 0x040000D9 RID: 217
		private IExtension extensionObject;

		// Token: 0x040000DA RID: 218
		[ProtoMember(1)]
		public WorldSerialization.ModifierData modifiers = new WorldSerialization.ModifierData();

		// Token: 0x040000DB RID: 219
		[ProtoMember(3)]
		public List<WorldSerialization.PrefabData> prefabs = new List<WorldSerialization.PrefabData>();

		// Token: 0x040000DC RID: 220
		[ProtoMember(5)]
		public byte[] circuits;

		// Token: 0x040000DD RID: 221
		[ProtoMember(6)]
		public byte[] emptychunk1;

		// Token: 0x040000DE RID: 222
		[ProtoMember(7)]
		public byte[] npcs;

		// Token: 0x040000DF RID: 223
		[ProtoMember(8)]
		public byte[] emptychunk3;

		// Token: 0x040000E0 RID: 224
		[ProtoMember(9)]
		public byte[] emptychunk4;

		// Token: 0x040000E1 RID: 225
		[ProtoMember(10)]
		public byte[] buildingchunk;

		// Token: 0x040000E2 RID: 226
		[ProtoMember(11)]
		public string checksum;

		// Token: 0x040000E3 RID: 227
		[ProtoMember(100)]
		public List<SocketInfo> sockets = new List<SocketInfo>();
	}

	// Token: 0x02000019 RID: 25
	[ProtoContract]
	public class RMPrefabData : IExtensible
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00002A67 File Offset: 0x00000C67
		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		// Token: 0x040000E4 RID: 228
		private IExtension extensionObject;

		// Token: 0x040000E5 RID: 229
		[ProtoMember(1)]
		public WorldSerialization.ModifierData modifiers = new WorldSerialization.ModifierData();

		// Token: 0x040000E6 RID: 230
		[ProtoMember(3)]
		public List<WorldSerialization.PrefabData> prefabs = new List<WorldSerialization.PrefabData>();

		// Token: 0x040000E7 RID: 231
		[ProtoMember(5)]
		public byte[] circuits;

		// Token: 0x040000E8 RID: 232
		[ProtoMember(6)]
		public byte[] emptychunk1;

		// Token: 0x040000E9 RID: 233
		[ProtoMember(7)]
		public byte[] npcs;

		// Token: 0x040000EA RID: 234
		[ProtoMember(8)]
		public byte[] emptychunk3;

		// Token: 0x040000EB RID: 235
		[ProtoMember(9)]
		public byte[] emptychunk4;

		// Token: 0x040000EC RID: 236
		[ProtoMember(10)]
		public byte[] buildingchunk;

		// Token: 0x040000ED RID: 237
		[ProtoMember(11)]
		public string checksum;

		// Token: 0x040000EE RID: 238
		[ProtoMember(100)]
		public List<SocketInfo> sockets = new List<SocketInfo>();

		// Token: 0x040000EF RID: 239
		[ProtoMember(12)]
		public WorldSerialization.RMMonument monument;
	}

	// Token: 0x0200001A RID: 26
	[ProtoContract]
	public class RMMonument
	{
		// Token: 0x040000F0 RID: 240
		[ProtoMember(1)]
		public WorldSerialization.VectorData size;

		// Token: 0x040000F1 RID: 241
		[ProtoMember(2)]
		public WorldSerialization.VectorData extents;

		// Token: 0x040000F2 RID: 242
		[ProtoMember(3)]
		public WorldSerialization.VectorData offset;

		// Token: 0x040000F3 RID: 243
		[ProtoMember(4)]
		public bool HeightMap = true;

		// Token: 0x040000F4 RID: 244
		[ProtoMember(5)]
		public bool AlphaMap = true;

		// Token: 0x040000F5 RID: 245
		[ProtoMember(6)]
		public bool WaterMap;

		// Token: 0x040000F6 RID: 246
		[ProtoMember(7)]
		public TerrainSplat.Enum SplatMask;

		// Token: 0x040000F7 RID: 247
		[ProtoMember(8)]
		public TerrainBiome.Enum BiomeMask;

		// Token: 0x040000F8 RID: 248
		[ProtoMember(9)]
		public TerrainTopology.Enum TopologyMask;

		// Token: 0x040000F9 RID: 249
		[ProtoMember(10)]
		public byte[] heightmap;

		// Token: 0x040000FA RID: 250
		[ProtoMember(11)]
		public byte[] splatmap0;

		// Token: 0x040000FB RID: 251
		[ProtoMember(12)]
		public byte[] splatmap1;

		// Token: 0x040000FC RID: 252
		[ProtoMember(13)]
		public byte[] alphamap;

		// Token: 0x040000FD RID: 253
		[ProtoMember(14)]
		public byte[] biomemap;

		// Token: 0x040000FE RID: 254
		[ProtoMember(15)]
		public byte[] topologymap;

		// Token: 0x040000FF RID: 255
		[ProtoMember(16)]
		public byte[] watermap;

		// Token: 0x04000100 RID: 256
		[ProtoMember(17)]
		public byte[] blendmap;
	}

	// Token: 0x0200001B RID: 27
	[ProtoContract]
	[Serializable]
	public class ModifierData
	{
		// Token: 0x04000101 RID: 257
		[ProtoMember(1)]
		public int size;

		// Token: 0x04000102 RID: 258
		[ProtoMember(2)]
		public int fade;

		// Token: 0x04000103 RID: 259
		[ProtoMember(3)]
		public int fill;

		// Token: 0x04000104 RID: 260
		[ProtoMember(4)]
		public int counter;

		// Token: 0x04000105 RID: 261
		[ProtoMember(5)]
		public uint id;
	}

	// Token: 0x0200001C RID: 28
	[ProtoContract]
	[Serializable]
	public class NPCDataHolder
	{
		// Token: 0x06000064 RID: 100 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public NPCDataHolder()
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002AC7 File Offset: 0x00000CC7
		public NPCDataHolder(List<WorldSerialization.NPCData> bots)
		{
			this.bots = bots;
		}

		// Token: 0x04000106 RID: 262
		[ProtoMember(1)]
		public List<WorldSerialization.NPCData> bots = new List<WorldSerialization.NPCData>();
	}

	// Token: 0x0200001D RID: 29
	[ProtoContract]
	public class MapData
	{
		// Token: 0x04000107 RID: 263
		[ProtoMember(1)]
		public string name = "";

		// Token: 0x04000108 RID: 264
		[ProtoMember(2)]
		public byte[] data = Array.Empty<byte>();
	}

	// Token: 0x0200001E RID: 30
	[ProtoContract]
	[Serializable]
	public class CircuitDataHolder
	{
		// Token: 0x06000067 RID: 103 RVA: 0x00002AFF File Offset: 0x00000CFF
		public CircuitDataHolder()
		{
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002B12 File Offset: 0x00000D12
		public CircuitDataHolder(List<WorldSerialization.CircuitData> circuitData)
		{
			this.circuitData = circuitData;
		}

		// Token: 0x04000109 RID: 265
		[ProtoMember(1)]
		public List<WorldSerialization.CircuitData> circuitData = new List<WorldSerialization.CircuitData>();
	}

	// Token: 0x0200001F RID: 31
	[ProtoContract]
	[Serializable]
	public class CircuitData
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00002B2C File Offset: 0x00000D2C
		public CircuitData()
		{
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0001F5E8 File Offset: 0x0001D7E8
		public CircuitData(string path, Vector3 wiring, List<WorldSerialization.Circuit> branchIn, List<WorldSerialization.Circuit> branchOut, int flow1, float setting, int flow2, string cctv, string phone)
		{
			this.path = path;
			this.wiring = wiring;
			this.branchIn = branchIn;
			this.branchOut = branchOut;
			this.flow1 = flow1;
			this.setting = setting;
			this.flow2 = flow2;
			this.cctv = cctv;
			this.phone = phone;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0001F65C File Offset: 0x0001D85C
		public CircuitData(string path, Vector3 wiring, WorldSerialization.Circuit[] connectionsIn, WorldSerialization.Circuit[] connectionsOut, int flow1, float setting, int flow2, string cctv, string phone)
		{
			this.path = path;
			this.wiring = wiring;
			this.connectionsIn = connectionsIn;
			this.connectionsOut = connectionsOut;
			this.flow1 = flow1;
			this.setting = setting;
			this.flow2 = flow2;
			this.cctv = cctv;
			this.phone = phone;
		}

		// Token: 0x0400010A RID: 266
		[ProtoMember(1)]
		public string path;

		// Token: 0x0400010B RID: 267
		[ProtoMember(2)]
		public WorldSerialization.VectorData wiring;

		// Token: 0x0400010C RID: 268
		[ProtoMember(3)]
		public List<WorldSerialization.Circuit> branchIn = new List<WorldSerialization.Circuit>();

		// Token: 0x0400010D RID: 269
		[ProtoMember(4)]
		public List<WorldSerialization.Circuit> branchOut = new List<WorldSerialization.Circuit>();

		// Token: 0x0400010E RID: 270
		[ProtoMember(5)]
		public int cardType;

		// Token: 0x0400010F RID: 271
		[ProtoMember(6)]
		public int flow1;

		// Token: 0x04000110 RID: 272
		[ProtoMember(7)]
		public float setting;

		// Token: 0x04000111 RID: 273
		[ProtoMember(14)]
		public string cctv;

		// Token: 0x04000112 RID: 274
		[ProtoMember(16)]
		public int flow2;

		// Token: 0x04000113 RID: 275
		[ProtoMember(17)]
		public string phone;

		// Token: 0x04000114 RID: 276
		public WorldSerialization.Circuit[] connectionsIn;

		// Token: 0x04000115 RID: 277
		public WorldSerialization.Circuit[] connectionsOut;
	}

	// Token: 0x02000020 RID: 32
	[ProtoContract]
	[Serializable]
	public class NPCData
	{
		// Token: 0x0600006C RID: 108 RVA: 0x00002822 File Offset: 0x00000A22
		public NPCData()
		{
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002B4A File Offset: 0x00000D4A
		public NPCData(int type, int respawnMin, int respawnMax, WorldSerialization.VectorData scientist, string category)
		{
			this.type = type;
			this.respawnMin = respawnMin;
			this.respawnMax = respawnMax;
			this.scientist = scientist;
			this.category = category;
		}

		// Token: 0x04000116 RID: 278
		[ProtoMember(1)]
		public int type;

		// Token: 0x04000117 RID: 279
		[ProtoMember(2)]
		public int respawnMin;

		// Token: 0x04000118 RID: 280
		[ProtoMember(3)]
		public int respawnMax;

		// Token: 0x04000119 RID: 281
		[ProtoMember(4)]
		public WorldSerialization.VectorData scientist;

		// Token: 0x0400011A RID: 282
		[ProtoMember(5)]
		public string category;
	}

	// Token: 0x02000021 RID: 33
	[ProtoContract]
	[Serializable]
	public class Circuit
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00002822 File Offset: 0x00000A22
		public Circuit()
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002B77 File Offset: 0x00000D77
		public Circuit(string path, Vector3 wiring, int flow1, int flow2, int fluid1)
		{
			this.path = path;
			this.wiring = wiring;
			this.flow1 = flow1;
			this.flow2 = flow2;
			this.fluid1 = fluid1;
		}

		// Token: 0x0400011B RID: 283
		[ProtoMember(1)]
		public string path;

		// Token: 0x0400011C RID: 284
		[ProtoMember(2)]
		public WorldSerialization.VectorData wiring;

		// Token: 0x0400011D RID: 285
		[ProtoMember(3)]
		public int flow1;

		// Token: 0x0400011E RID: 286
		[ProtoMember(4)]
		public int flow2;

		// Token: 0x0400011F RID: 287
		[ProtoMember(5)]
		public int fluid1;
	}

	// Token: 0x02000022 RID: 34
	[ProtoContract]
	[Serializable]
	public class PrefabData
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00002822 File Offset: 0x00000A22
		public PrefabData()
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002BA9 File Offset: 0x00000DA9
		public PrefabData(string category, uint id, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.category = category;
			this.id = id;
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		// Token: 0x04000120 RID: 288
		[ProtoMember(1)]
		public string category;

		// Token: 0x04000121 RID: 289
		[ProtoMember(2)]
		public uint id;

		// Token: 0x04000122 RID: 290
		[ProtoMember(3)]
		public WorldSerialization.VectorData position;

		// Token: 0x04000123 RID: 291
		[ProtoMember(4)]
		public WorldSerialization.VectorData rotation;

		// Token: 0x04000124 RID: 292
		[ProtoMember(5)]
		public WorldSerialization.VectorData scale;
	}

	// Token: 0x02000023 RID: 35
	[ProtoContract]
	[Serializable]
	public class PathData
	{
		// Token: 0x04000125 RID: 293
		[ProtoMember(1)]
		public string name;

		// Token: 0x04000126 RID: 294
		[ProtoMember(2)]
		public bool spline;

		// Token: 0x04000127 RID: 295
		[ProtoMember(3)]
		public bool start;

		// Token: 0x04000128 RID: 296
		[ProtoMember(4)]
		public bool end;

		// Token: 0x04000129 RID: 297
		[ProtoMember(5)]
		public float width;

		// Token: 0x0400012A RID: 298
		[ProtoMember(6)]
		public float innerPadding;

		// Token: 0x0400012B RID: 299
		[ProtoMember(7)]
		public float outerPadding;

		// Token: 0x0400012C RID: 300
		[ProtoMember(8)]
		public float innerFade;

		// Token: 0x0400012D RID: 301
		[ProtoMember(9)]
		public float outerFade;

		// Token: 0x0400012E RID: 302
		[ProtoMember(10)]
		public float randomScale;

		// Token: 0x0400012F RID: 303
		[ProtoMember(11)]
		public float meshOffset;

		// Token: 0x04000130 RID: 304
		[ProtoMember(12)]
		public float terrainOffset;

		// Token: 0x04000131 RID: 305
		[ProtoMember(13)]
		public int splat;

		// Token: 0x04000132 RID: 306
		[ProtoMember(14)]
		public int topology;

		// Token: 0x04000133 RID: 307
		[ProtoMember(15)]
		public WorldSerialization.VectorData[] nodes;

		// Token: 0x04000134 RID: 308
		[ProtoMember(16)]
		public int hierarchy;
	}

	// Token: 0x02000024 RID: 36
	[ProtoContract]
	[Serializable]
	public class VectorData
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00002822 File Offset: 0x00000A22
		public VectorData()
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002BE5 File Offset: 0x00000DE5
		public VectorData(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002C02 File Offset: 0x00000E02
		public static implicit operator WorldSerialization.VectorData(Vector3 v)
		{
			return new WorldSerialization.VectorData(v.x, v.y, v.z);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002C1B File Offset: 0x00000E1B
		public static implicit operator WorldSerialization.VectorData(Quaternion q)
		{
			return q.eulerAngles;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00002C29 File Offset: 0x00000E29
		public static implicit operator Vector3(WorldSerialization.VectorData v)
		{
			return new Vector3(v.x, v.y, v.z);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002C42 File Offset: 0x00000E42
		public static implicit operator Quaternion(WorldSerialization.VectorData v)
		{
			return Quaternion.Euler(v);
		}

		// Token: 0x04000135 RID: 309
		[ProtoMember(1)]
		public float x;

		// Token: 0x04000136 RID: 310
		[ProtoMember(2)]
		public float y;

		// Token: 0x04000137 RID: 311
		[ProtoMember(3)]
		public float z;
	}

	// Token: 0x02000025 RID: 37
	[Serializable]
	public class PrefabAndPathData
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00002C4F File Offset: 0x00000E4F
		public PrefabAndPathData(WorldSerialization.REPrefabData rePrefab, List<WorldSerialization.PathData> paths)
		{
			this.rePrefab = rePrefab;
			this.paths = paths;
		}

		// Token: 0x04000138 RID: 312
		public WorldSerialization.REPrefabData rePrefab;

		// Token: 0x04000139 RID: 313
		public List<WorldSerialization.PathData> paths;
	}
}
