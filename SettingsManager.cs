using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RustMapEditor.Variables;
using UIRecycleTreeNamespace;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public static class SettingsManager
{
	// Token: 0x06000A92 RID: 2706 RVA: 0x00008342 File Offset: 0x00006542
	public static string AppDataPath()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RustMapper");
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00060B58 File Offset: 0x0005ED58
	public static void RuntimeInit()
	{
		try
		{
			SettingsManager.SettingsPath = Path.Combine(SettingsManager.AppDataPath(), "EditorSettings.json");
			if (!File.Exists(SettingsManager.SettingsPath))
			{
				Debug.Log("no EditorSettings.json found in appdata, copying default configuration from home directory");
				SettingsManager.CopyDirectory("Presets", Path.Combine(SettingsManager.AppDataPath(), "Presets"));
				SettingsManager.CopyDirectory("Custom", Path.Combine(SettingsManager.AppDataPath(), "Custom"));
			}
			SettingsManager.CopyEditorSettings(SettingsManager.AppDataPath());
		}
		catch (Exception ex)
		{
			Debug.LogError("Error initializing directories: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
		}
		SettingsManager.EnsureDefaultBrushes();
		SettingsManager.LoadFragmentLookup();
		SettingsManager.LoadSettings();
		SettingsManager.ManageSkins();
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x00060C14 File Offset: 0x0005EE14
	private static void ManageSkins()
	{
		try
		{
			string path = Path.Combine("Custom", "Skins");
			string text = Path.Combine(SettingsManager.AppDataPath(), "Custom", "Skins");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
				Debug.Log("Created Skins directory at " + text);
			}
			string[] array = new string[]
			{
				"classic.png",
				"cabinet.png",
				"darkmode.png"
			};
			string text2 = Path.Combine(path, "darkmode.png");
			string text3 = Path.Combine(text, "darkmode.png");
			bool flag = false;
			if (File.Exists(text2))
			{
				if (!File.Exists(text3))
				{
					flag = true;
					Debug.Log("No darkmode.png found in AppData, will copy all default skins");
					goto IL_1CF;
				}
				try
				{
					if (!File.Exists(text2))
					{
						Debug.LogWarning("Source file not found: " + text2 + ". Cannot compare modification times.");
						return;
					}
					DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(text2);
					DateTime lastWriteTimeUtc2 = File.GetLastWriteTimeUtc(text3);
					DateTime creationTimeUtc = File.GetCreationTimeUtc(text2);
					DateTime creationTimeUtc2 = File.GetCreationTimeUtc(text3);
					if (lastWriteTimeUtc > lastWriteTimeUtc2 || creationTimeUtc > creationTimeUtc2)
					{
						flag = true;
						Debug.Log(string.Format("Newer darkmode.png detected in source directory (modification: {0:yyyy-MM-dd HH:mm:ss} UTC, creation: {1:yyyy-MM-dd HH:mm:ss} UTC) ", lastWriteTimeUtc, creationTimeUtc) + string.Format("compared to app data (modification: {0:yyyy-MM-dd HH:mm:ss} UTC, creation: {1:yyyy-MM-dd HH:mm:ss} UTC). Updating all skins.", lastWriteTimeUtc2, creationTimeUtc2));
					}
					else
					{
						Debug.Log(string.Format("No update needed. Source file (modification: {0:yyyy-MM-dd HH:mm:ss} UTC, creation: {1:yyyy-MM-dd HH:mm:ss} UTC) ", lastWriteTimeUtc, creationTimeUtc) + string.Format("is not newer than app data file (modification: {0:yyyy-MM-dd HH:mm:ss} UTC, creation: {1:yyyy-MM-dd HH:mm:ss} UTC).", lastWriteTimeUtc2, creationTimeUtc2));
					}
					goto IL_1CF;
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Error comparing file times for ",
						text2,
						" and ",
						text3,
						": ",
						ex.Message
					}));
					goto IL_1CF;
				}
			}
			Debug.LogWarning("Source darkmode.png not found at " + text2 + ", skipping skin update");
			IL_1CF:
			if (flag)
			{
				foreach (string text4 in array)
				{
					string text5 = Path.Combine(path, text4);
					string destFileName = Path.Combine(text, text4);
					if (File.Exists(text5))
					{
						File.Copy(text5, destFileName, true);
						Debug.Log("Updated " + text4 + " in " + text);
					}
					else
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"Source skin ",
							text4,
							" not found at ",
							text5,
							", skipped"
						}));
					}
				}
			}
			if (File.Exists(SettingsManager.application.startupSkin))
			{
				ModManager.LoadSkin(SettingsManager.application.startupSkin);
				Debug.Log("Loaded skin: " + SettingsManager.application.startupSkin);
			}
			else
			{
				Debug.LogWarning("Failed to load startupSkin");
			}
		}
		catch (Exception ex2)
		{
			Debug.LogError("Error managing skins: " + ex2.Message + "\nStackTrace: " + ex2.StackTrace);
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00060F2C File Offset: 0x0005F12C
	private static void EnsureDefaultBrushes()
	{
		string text = Path.Combine(Application.dataPath, "..", "Custom", "Brushes");
		string text2 = Path.Combine(SettingsManager.AppDataPath(), "Custom", "Brushes");
		if (!Directory.Exists(text2))
		{
			if (Directory.Exists(text))
			{
				SettingsManager.CopyDirectory(text, text2);
				Debug.Log("Populated " + text2 + " with default brushes from " + text);
				return;
			}
			Debug.LogError("Default brushes directory not found at " + text + ". Creating empty Brushes folder.");
			Directory.CreateDirectory(text2);
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00060FB4 File Offset: 0x0005F1B4
	public static List<string> GetScriptFiles()
	{
		string text = Path.Combine(SettingsManager.AppDataPath(), "Presets", "Scripts");
		List<string> list = new List<string>();
		try
		{
			if (!Directory.Exists(text))
			{
				Debug.LogWarning("Scripts directory not found at: " + text);
				return list;
			}
			foreach (string path in Directory.EnumerateFiles(text, "*.rmml", SearchOption.TopDirectoryOnly))
			{
				list.Add(Path.GetFileName(path));
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			Debug.LogWarning("Access denied to scripts directory: " + ex.Message);
		}
		catch (IOException ex2)
		{
			Debug.LogWarning("IO error accessing scripts directory: " + ex2.Message);
		}
		return list;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x00061098 File Offset: 0x0005F298
	public static List<string> GetScriptCommands(string scriptName)
	{
		List<string> list = new List<string>();
		string text = Path.Combine(SettingsManager.AppDataPath(), "Presets", "Scripts", scriptName);
		try
		{
			if (!File.Exists(text))
			{
				Debug.LogWarning("Script file not found at: " + text);
				return list;
			}
			string[] array = File.ReadAllLines(text);
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i].Trim();
				if (!string.IsNullOrEmpty(text2) && !text2.StartsWith("#") && !text2.StartsWith("//"))
				{
					list.Add(text2);
				}
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			Debug.LogWarning("Access denied to script file " + scriptName + ": " + ex.Message);
		}
		catch (IOException ex2)
		{
			Debug.LogWarning("IO error reading script file " + scriptName + ": " + ex2.Message);
		}
		catch (Exception ex3)
		{
			Debug.LogWarning("Unexpected error reading script file " + scriptName + ": " + ex3.Message);
		}
		return list;
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x000611C0 File Offset: 0x0005F3C0
	private static void UpdateBreakerFragmentsIfNewer()
	{
		string text = "Presets/breakerFragments.json";
		string text2 = Path.Combine(SettingsManager.AppDataPath(), "Presets", "breakerFragments.json");
		string destFileName = Path.Combine(SettingsManager.AppDataPath(), "Presets", "autosaveFragments.json");
		if (File.Exists(text))
		{
			if (File.Exists(text2))
			{
				File.Copy(text2, destFileName, true);
				Debug.Log("Saved current breakerFragments as autosaveFragments.json.");
			}
			FileInfo fileInfo = new FileInfo(text);
			FileInfo fileInfo2 = new FileInfo(text2);
			if (!File.Exists(text2) || fileInfo.LastWriteTimeUtc > fileInfo2.LastWriteTimeUtc)
			{
				File.Copy(text, text2, true);
				Debug.Log("Updated breakerFragments.json with the default version.");
				return;
			}
		}
		else
		{
			Debug.LogWarning("Default breakerFragments.json not found.");
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00061268 File Offset: 0x0005F468
	public static void CopyDirectory(string sourceDir, string destinationDir)
	{
		if (!Directory.Exists(destinationDir))
		{
			Directory.CreateDirectory(destinationDir);
		}
		foreach (string text in Directory.GetFiles(sourceDir))
		{
			string fileName = Path.GetFileName(text);
			string destFileName = Path.Combine(destinationDir, fileName);
			File.Copy(text, destFileName, true);
		}
		foreach (string text2 in Directory.GetDirectories(sourceDir))
		{
			string fileName2 = Path.GetFileName(text2);
			SettingsManager.CopyDirectory(text2, Path.Combine(destinationDir, fileName2));
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x000612E0 File Offset: 0x0005F4E0
	public static void CopyFile(string sourcePath, string destinationPath, bool overwrite = true)
	{
		try
		{
			if (File.Exists(sourcePath))
			{
				File.Copy(sourcePath, destinationPath, overwrite);
				Debug.Log("File copied from: " + sourcePath + " to: " + destinationPath);
			}
			else
			{
				Debug.LogWarning("Source file not found at: " + sourcePath);
			}
		}
		catch (IOException ex)
		{
			Debug.LogError("Error copying file: " + ex.Message);
		}
		catch (UnauthorizedAccessException ex2)
		{
			Debug.LogError("Permission denied when copying file: " + ex2.Message);
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00061374 File Offset: 0x0005F574
	public static void CopyEditorSettings(string destinationDirectory)
	{
		foreach (string text in new string[]
		{
			"EditorSettings.json",
			"blacklist.json",
			"VolumesList.txt"
		})
		{
			string text2 = text;
			string text3 = Path.Combine(destinationDirectory, text);
			if (File.Exists(text2) && !File.Exists(text3))
			{
				File.Copy(text2, text3, true);
				Debug.Log("File not found. Copied default " + text + " to active directory: " + text3);
			}
			else
			{
				Debug.Log(text3 + " exists or default has been removed");
			}
		}
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00061404 File Offset: 0x0005F604
	public static List<string> AddFilePaths(string path, string extension)
	{
		List<string> list = new List<string>();
		string fullPath = Path.GetFullPath(path);
		if (string.IsNullOrWhiteSpace(fullPath) || !Directory.Exists(fullPath))
		{
			Debug.LogWarning("Invalid path: " + fullPath);
			return list;
		}
		try
		{
			foreach (string path2 in Directory.EnumerateDirectories(fullPath, "*", SearchOption.TopDirectoryOnly))
			{
				list.Add(Path.GetFullPath(path2) + Path.DirectorySeparatorChar.ToString());
			}
			foreach (string path3 in Directory.EnumerateFiles(fullPath, "*." + extension, SearchOption.TopDirectoryOnly))
			{
				list.Add(Path.GetFullPath(path3));
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Error at " + fullPath + ": " + ex.Message);
		}
		return list;
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00061520 File Offset: 0x0005F720
	public static List<string> GetBrushPaths()
	{
		string text = Path.Combine(SettingsManager.AppDataPath(), "Custom/Brushes");
		if (!Directory.Exists(text))
		{
			Debug.LogWarning("Brush directory not found at: " + text);
			return new List<string>();
		}
		return Directory.GetFiles(text, "*.png").Concat(Directory.GetFiles(text, "*.jpg")).ToList<string>();
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0006157C File Offset: 0x0005F77C
	public static List<string> GetDataPaths(string path, string root, string extension = ".prefab")
	{
		List<string> list = new List<string>();
		string[] directories = Directory.GetDirectories(path);
		string[] files = Directory.GetFiles(path);
		int num = path.IndexOf(root, StringComparison.Ordinal);
		if (num != -1)
		{
			list.Add("~" + path.Substring(num));
		}
		foreach (string path2 in directories)
		{
			list.AddRange(SettingsManager.GetDataPaths(path2, root, ".prefab"));
		}
		foreach (string text in files)
		{
			int num2 = text.IndexOf(root, StringComparison.Ordinal);
			if (num2 != -1)
			{
				list.Add("~" + text.Substring(num2));
			}
		}
		return list;
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x00061630 File Offset: 0x0005F830
	public static void AddPathsAsNodes(UIRecycleTree tree, List<string> paths)
	{
		Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
		foreach (Node node in tree.rootNode.nodes)
		{
			SettingsManager.PopulateNodeMap(node, dictionary, string.Empty);
		}
		foreach (string text in paths)
		{
			string[] array = text.Replace("\\", "/", StringComparison.Ordinal).Split('/', StringSplitOptions.None);
			Node node2 = null;
			for (int i = 0; i < array.Length; i++)
			{
				string name = array[i];
				string key = string.Join("/", array, 0, i + 1);
				if (!dictionary.TryGetValue(key, out node2))
				{
					node2 = new Node(name, 0);
					dictionary[key] = node2;
					if (i == 0)
					{
						tree.rootNode.nodes.AddWithoutNotify(node2);
					}
					else
					{
						string key2 = string.Join("/", array, 0, i);
						Node node3;
						if (dictionary.TryGetValue(key2, out node3))
						{
							node3.nodes.AddWithoutNotify(node2);
							node2.parentNode = node3;
						}
					}
					node2.tree = tree;
				}
			}
		}
		tree.Rebuild();
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0006178C File Offset: 0x0005F98C
	private static void PopulateNodeMap(Node node, Dictionary<string, Node> nodeMap, string parentPath)
	{
		string text = string.IsNullOrEmpty(parentPath) ? node.name : (parentPath + "/" + node.name);
		nodeMap[text] = node;
		foreach (Node node2 in node.nodes)
		{
			SettingsManager.PopulateNodeMap(node2, nodeMap, text);
		}
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00008355 File Offset: 0x00006555
	public static void UpdateFavorite(Node node)
	{
		if (node.isChecked)
		{
			SettingsManager.AddFavorite(node);
			return;
		}
		SettingsManager.RemoveFavorite(node);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00061804 File Offset: 0x0005FA04
	public static void AddFavorite(Node node)
	{
		string item = node.fullPath;
		if (node.data != null)
		{
			item = (string)node.data;
		}
		SettingsManager.faves.favoriteCustoms.Add(item);
		SettingsManager.SaveSettings();
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00061844 File Offset: 0x0005FA44
	public static void RemoveFavorite(Node node)
	{
		string item = node.fullPath;
		if (node.data != null)
		{
			item = (string)node.data;
		}
		SettingsManager.faves.favoriteCustoms.Remove(item);
		SettingsManager.SaveSettings();
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0000836C File Offset: 0x0000656C
	public static void CheckFavorites(UIRecycleTree tree)
	{
		SettingsManager.CheckNode(tree.rootNode);
		tree.Rebuild();
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x00061884 File Offset: 0x0005FA84
	public static int GetNodeStyleIndex(Node node, string fullPath)
	{
		if (node.hasChildren)
		{
			return 1;
		}
		string key = fullPath + ".prefab";
		ItemSettings itemSettings;
		if (PrefabManager.ItemBlacklist.TryGetValue(key, out itemSettings))
		{
			if (itemSettings.blacklisted)
			{
				return 3;
			}
			if (itemSettings.hidden)
			{
				return 2;
			}
		}
		return 0;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x000618CC File Offset: 0x0005FACC
	private static void CheckNode(Node node)
	{
		string text = node.fullPath;
		if (node.data != null)
		{
			text = (string)node.data;
		}
		if (node.fullPath != "~Favorites")
		{
			bool flag = SettingsManager.faves.favoriteCustoms.Contains(text);
			node.SetCheckedWithoutNotify(flag);
			node.styleIndex = SettingsManager.GetNodeStyleIndex(node, text);
			if (node.fullPath.StartsWith("~Favorites/", StringComparison.Ordinal) && !flag)
			{
				Node parentNode = node.parentNode;
				if (parentNode != null)
				{
					parentNode.nodes.Remove(node);
					return;
				}
			}
		}
		foreach (Node node2 in new List<Node>(node.nodes))
		{
			SettingsManager.CheckNode(node2);
		}
		if (node.fullPath == "~Favorites")
		{
			node.SetCheckedWithoutNotify(true);
			node.styleIndex = 1;
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x000619C4 File Offset: 0x0005FBC4
	public static void ConvertPathsToNodes(UIRecycleTree tree, List<string> paths, string extension, string searchQuery = "")
	{
		tree.Clear();
		Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
		Node node = new Node("~Favorites", 0);
		tree.rootNode.nodes.AddWithoutNotify(node);
		node.tree = tree;
		foreach (string text in paths)
		{
			if (text.EndsWith(extension, StringComparison.Ordinal) || extension.Equals("override", StringComparison.Ordinal) || text.StartsWith("~Favorites/", StringComparison.Ordinal))
			{
				string text2 = text.Replace(extension, "", StringComparison.Ordinal).Replace("\\", "/", StringComparison.Ordinal);
				if (text2.StartsWith("~Geology/", StringComparison.Ordinal))
				{
					text2 = text2.Substring("~Geology/".Length);
				}
				if (string.IsNullOrEmpty(searchQuery) || text2.Contains(searchQuery, StringComparison.Ordinal))
				{
					if (text.StartsWith("~Favorites/", StringComparison.Ordinal))
					{
						Node node2 = new Node(Path.GetFileName(text), 0);
						string data = text.Substring("~Favorites/".Length);
						node2.data = data;
						node.nodes.AddWithoutNotify(node2);
						node2.parentNode = node;
						node2.tree = tree;
					}
					else
					{
						string[] array = text2.Split('/', StringSplitOptions.None);
						Node node3 = null;
						for (int i = 0; i < array.Length; i++)
						{
							string name = array[i];
							string key = string.Join("/", array, 0, i + 1);
							if (!dictionary.TryGetValue(key, out node3))
							{
								node3 = new Node(name, 0);
								dictionary[key] = node3;
								if (i == 0)
								{
									tree.rootNode.nodes.AddWithoutNotify(node3);
								}
								else
								{
									string key2 = string.Join("/", array, 0, i);
									Node node4;
									if (dictionary.TryGetValue(key2, out node4))
									{
										node4.nodes.AddWithoutNotify(node3);
										node3.parentNode = node4;
									}
								}
								node3.tree = tree;
							}
						}
					}
				}
			}
		}
		tree.Rebuild();
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00061BE4 File Offset: 0x0005FDE4
	public static void ConvertPathsToNodes(UIRecycleTree tree, List<string> paths, string extension1, string extension2, string searchQuery = "", bool showAll = true)
	{
		tree.Clear();
		Dictionary<string, Node> dictionary = new Dictionary<string, Node>();
		Node node = new Node("~Favorites", 0);
		tree.rootNode.nodes.AddWithoutNotify(node);
		node.tree = tree;
		foreach (string text in paths)
		{
			if (text.EndsWith(extension1, StringComparison.Ordinal) || text.EndsWith(extension2, StringComparison.Ordinal) || extension1.Equals("override", StringComparison.Ordinal) || text.StartsWith("~Favorites/", StringComparison.Ordinal))
			{
				string text2 = text;
				if (text.EndsWith(extension1, StringComparison.Ordinal))
				{
					text2 = text.Replace(extension1, "", StringComparison.Ordinal);
				}
				text2 = text2.Replace("\\", "/", StringComparison.Ordinal);
				bool flag = false;
				bool flag2 = false;
				ItemSettings itemSettings;
				if (PrefabManager.ItemBlacklist.TryGetValue(text, out itemSettings))
				{
					flag = itemSettings.blacklisted;
					flag2 = itemSettings.hidden;
				}
				if (showAll || (!flag && !flag2))
				{
					if (text2.StartsWith("~Geology/", StringComparison.Ordinal))
					{
						text2 = text2.Substring("~Geology/".Length);
					}
					if (string.IsNullOrEmpty(searchQuery) || text2.Contains(searchQuery, StringComparison.Ordinal))
					{
						if (text.StartsWith("~Favorites/", StringComparison.Ordinal))
						{
							Node node2 = new Node(Path.GetFileName(text), 0);
							string data = text.Substring("~Favorites/".Length);
							node2.data = data;
							node.nodes.AddWithoutNotify(node2);
							node2.parentNode = node;
							node2.tree = tree;
						}
						else
						{
							string[] array = text2.Split('/', StringSplitOptions.None);
							Node node3 = null;
							for (int i = 0; i < array.Length; i++)
							{
								string name = array[i];
								string key = string.Join("/", array, 0, i + 1);
								if (!dictionary.TryGetValue(key, out node3))
								{
									node3 = new Node(name, 0);
									dictionary[key] = node3;
									if (i == 0)
									{
										tree.rootNode.nodes.AddWithoutNotify(node3);
									}
									else
									{
										string key2 = string.Join("/", array, 0, i);
										Node node4;
										if (dictionary.TryGetValue(key2, out node4))
										{
											node4.nodes.AddWithoutNotify(node3);
											node3.parentNode = node4;
										}
									}
									node3.tree = tree;
								}
							}
						}
					}
				}
			}
		}
		tree.Rebuild();
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0000837F File Offset: 0x0000657F
	// (set) Token: 0x06000AAA RID: 2730 RVA: 0x00008386 File Offset: 0x00006586
	public static bool style { get; set; }

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000AAB RID: 2731 RVA: 0x0000838E File Offset: 0x0000658E
	// (set) Token: 0x06000AAC RID: 2732 RVA: 0x00008395 File Offset: 0x00006595
	public static string RustDirectory { get; set; }

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000AAD RID: 2733 RVA: 0x0000839D File Offset: 0x0000659D
	// (set) Token: 0x06000AAE RID: 2734 RVA: 0x000083A4 File Offset: 0x000065A4
	public static float PrefabRenderDistance { get; set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000AAF RID: 2735 RVA: 0x000083AC File Offset: 0x000065AC
	// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x000083B3 File Offset: 0x000065B3
	public static float PathRenderDistance { get; set; }

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x000083BB File Offset: 0x000065BB
	// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x000083C2 File Offset: 0x000065C2
	public static float WaterTransparency { get; set; }

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x000083CA File Offset: 0x000065CA
	// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x000083D1 File Offset: 0x000065D1
	public static bool LoadBundleOnLaunch { get; set; }

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x000083D9 File Offset: 0x000065D9
	// (set) Token: 0x06000AB6 RID: 2742 RVA: 0x000083E0 File Offset: 0x000065E0
	public static bool TerrainTextureSet { get; set; }

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x000083E8 File Offset: 0x000065E8
	// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x000083EF File Offset: 0x000065EF
	public static Favorites faves { get; set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x000083F7 File Offset: 0x000065F7
	// (set) Token: 0x06000ABA RID: 2746 RVA: 0x000083FE File Offset: 0x000065FE
	public static FilePreset application { get; set; }

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00008406 File Offset: 0x00006606
	// (set) Token: 0x06000ABC RID: 2748 RVA: 0x0000840D File Offset: 0x0000660D
	public static CrazingPreset crazing { get; set; }

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00008415 File Offset: 0x00006615
	// (set) Token: 0x06000ABE RID: 2750 RVA: 0x0000841C File Offset: 0x0000661C
	public static PerlinSplatPreset perlinSplat { get; set; }

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00008424 File Offset: 0x00006624
	// (set) Token: 0x06000AC0 RID: 2752 RVA: 0x0000842B File Offset: 0x0000662B
	public static RipplePreset ripple { get; set; }

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x00008433 File Offset: 0x00006633
	// (set) Token: 0x06000AC2 RID: 2754 RVA: 0x0000843A File Offset: 0x0000663A
	public static OceanPreset ocean { get; set; }

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x00008442 File Offset: 0x00006642
	// (set) Token: 0x06000AC4 RID: 2756 RVA: 0x00008449 File Offset: 0x00006649
	public static TerracingPreset terracing { get; set; }

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x00008451 File Offset: 0x00006651
	// (set) Token: 0x06000AC6 RID: 2758 RVA: 0x00008458 File Offset: 0x00006658
	public static PerlinPreset perlin { get; set; }

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x00008460 File Offset: 0x00006660
	// (set) Token: 0x06000AC8 RID: 2760 RVA: 0x00008467 File Offset: 0x00006667
	public static GeologyPreset geology { get; set; }

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0000846F File Offset: 0x0000666F
	// (set) Token: 0x06000ACA RID: 2762 RVA: 0x00008476 File Offset: 0x00006676
	public static ReplacerPreset replacer { get; set; }

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0000847E File Offset: 0x0000667E
	// (set) Token: 0x06000ACC RID: 2764 RVA: 0x00008485 File Offset: 0x00006685
	public static string[] breakerPresets { get; set; }

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0000848D File Offset: 0x0000668D
	// (set) Token: 0x06000ACE RID: 2766 RVA: 0x00008494 File Offset: 0x00006694
	public static string[] geologyPresets { get; set; }

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0000849C File Offset: 0x0000669C
	// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x000084A3 File Offset: 0x000066A3
	public static string[] geologyPresetLists { get; set; }

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x000084AB File Offset: 0x000066AB
	// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x000084B2 File Offset: 0x000066B2
	public static string[] PrefabPaths { get; private set; }

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x000084BA File Offset: 0x000066BA
	// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x000084C1 File Offset: 0x000066C1
	public static List<string> macro { get; set; } = new List<string>();

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x000084C9 File Offset: 0x000066C9
	// (set) Token: 0x06000AD6 RID: 2774 RVA: 0x000084D0 File Offset: 0x000066D0
	public static bool macroSources { get; set; }

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x000084D8 File Offset: 0x000066D8
	// (set) Token: 0x06000AD8 RID: 2776 RVA: 0x000084DF File Offset: 0x000066DF
	public static RustCityPreset city { get; set; }

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x000084E7 File Offset: 0x000066E7
	// (set) Token: 0x06000ADA RID: 2778 RVA: 0x000084EE File Offset: 0x000066EE
	public static BreakerPreset breaker { get; set; }

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000ADB RID: 2779 RVA: 0x000084F6 File Offset: 0x000066F6
	// (set) Token: 0x06000ADC RID: 2780 RVA: 0x000084FD File Offset: 0x000066FD
	public static FragmentLookup fragmentIDs { get; set; }

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00008505 File Offset: 0x00006705
	// (set) Token: 0x06000ADE RID: 2782 RVA: 0x0000850C File Offset: 0x0000670C
	public static WindowState[] windowStates { get; set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00008514 File Offset: 0x00006714
	// (set) Token: 0x06000AE0 RID: 2784 RVA: 0x0000851B File Offset: 0x0000671B
	public static MenuState menuState { get; set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00008523 File Offset: 0x00006723
	// (set) Token: 0x06000AE2 RID: 2786 RVA: 0x0000852A File Offset: 0x0000672A
	public static List<Bind> binds { get; set; }

	// Token: 0x06000AE3 RID: 2787 RVA: 0x00061E54 File Offset: 0x00060054
	public static void SaveSettings()
	{
		try
		{
			BindManager.GetBinds();
			string directoryName = Path.GetDirectoryName(SettingsManager.SettingsPath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
				Debug.Log("Created directory: " + directoryName);
			}
			if (File.Exists(SettingsManager.SettingsPath))
			{
				File.SetAttributes(SettingsManager.SettingsPath, FileAttributes.Normal);
			}
			using (StreamWriter streamWriter = new StreamWriter(SettingsManager.SettingsPath, false))
			{
				EditorSettings editorSettings = new EditorSettings(SettingsManager.RustDirectory, SettingsManager.PrefabRenderDistance, SettingsManager.PathRenderDistance, SettingsManager.WaterTransparency, SettingsManager.LoadBundleOnLaunch, SettingsManager.TerrainTextureSet, SettingsManager.style, SettingsManager.crazing, SettingsManager.perlinSplat, SettingsManager.ripple, SettingsManager.ocean, SettingsManager.terracing, SettingsManager.perlin, SettingsManager.geology, SettingsManager.replacer, SettingsManager.city, SettingsManager.breaker, SettingsManager.macroSources, SettingsManager.application, SettingsManager.faves, SettingsManager.windowStates, SettingsManager.menuState, SettingsManager.binds);
				JsonSerializerSettings settings = new JsonSerializerSettings
				{
					ContractResolver = new Vector3ContractResolver(),
					Formatting = Formatting.Indented
				};
				string value = JsonConvert.SerializeObject(editorSettings, settings);
				streamWriter.Write(value);
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Access denied to ",
				SettingsManager.SettingsPath,
				": ",
				ex.Message,
				"\nStackTrace: ",
				ex.StackTrace
			}));
		}
		catch (IOException ex2)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"IO error for ",
				SettingsManager.SettingsPath,
				": ",
				ex2.Message,
				"\nStackTrace: ",
				ex2.StackTrace
			}));
		}
		catch (Exception ex3)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"General error saving settings to ",
				SettingsManager.SettingsPath,
				": ",
				ex3.Message,
				"\nStackTrace: ",
				ex3.StackTrace
			}));
		}
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x000620AC File Offset: 0x000602AC
	public static Dictionary<string, uint> ListToDict(List<FragmentPair> fragmentPairs)
	{
		Dictionary<string, uint> dictionary = new Dictionary<string, uint>();
		foreach (FragmentPair fragmentPair in fragmentPairs)
		{
			dictionary.Add(fragmentPair.fragment, fragmentPair.id);
		}
		return dictionary;
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0006210C File Offset: 0x0006030C
	public static List<FragmentPair> DictToList(Dictionary<string, uint> fragmentNamelist)
	{
		List<FragmentPair> list = new List<FragmentPair>();
		FragmentPair item = default(FragmentPair);
		foreach (KeyValuePair<string, uint> keyValuePair in fragmentNamelist)
		{
			item.fragment = keyValuePair.Key;
			item.id = keyValuePair.Value;
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x00062188 File Offset: 0x00060388
	public static void SaveFragmentLookup()
	{
		using (StreamWriter streamWriter = new StreamWriter(Path.Combine(SettingsManager.AppDataPath(), "Presets", "breakerFragments.json"), false))
		{
			string value = JsonConvert.SerializeObject(SettingsManager.fragmentIDs, Formatting.Indented);
			streamWriter.Write(value);
			SettingsManager.fragmentIDs.Deserialize();
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x000621EC File Offset: 0x000603EC
	public static void LoadFragmentLookup()
	{
		try
		{
			SettingsManager.fragmentIDs = new FragmentLookup();
			string text = Path.Combine(SettingsManager.AppDataPath(), "Presets", "breakerFragments.json");
			if (!File.Exists(text))
			{
				Debug.LogError("Fragment lookup file not found at: " + text);
			}
			else
			{
				using (StreamReader streamReader = new StreamReader(text))
				{
					SettingsManager.fragmentIDs = JsonConvert.DeserializeObject<FragmentLookup>(streamReader.ReadToEnd());
					if (SettingsManager.fragmentIDs == null)
					{
						Debug.LogError("Failed to deserialize breakerFragments.json");
					}
					else
					{
						SettingsManager.fragmentIDs.Deserialize();
					}
				}
			}
		}
		catch (FileNotFoundException ex)
		{
			Debug.LogError("Fragment lookup file not found: " + ex.Message);
		}
		catch (JsonException ex2)
		{
			Debug.LogError("JSON deserialization error in breakerFragments.json: " + ex2.Message);
		}
		catch (Exception ex3)
		{
			Debug.LogError("Unexpected error in LoadFragmentLookup: " + ex3.Message + "\n" + ex3.StackTrace);
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00008532 File Offset: 0x00006732
	public static void SaveBreakerPreset(string filename)
	{
		SettingsManager.breakerSerializer.breaker = SettingsManager.breaker;
		SettingsManager.breakerSerializer.Save(Path.Combine(SettingsManager.AppDataPath(), "Presets", "Breaker", filename + ".breaker"));
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0000856C File Offset: 0x0000676C
	public static void LoadBreakerPreset(string filename)
	{
		SettingsManager.breaker = SettingsManager.breakerSerializer.Load(Path.Combine(new string[]
		{
			SettingsManager.AppDataPath() + "Presets/Breaker/" + filename + ".breaker"
		}));
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x00062304 File Offset: 0x00060504
	public static void SaveGeologyPreset()
	{
		using (StreamWriter streamWriter = new StreamWriter(Path.Combine(SettingsManager.AppDataPath(), "Presets", "Geology", SettingsManager.geology.title + ".json"), false))
		{
			string value = JsonConvert.SerializeObject(SettingsManager.geology, Formatting.Indented);
			streamWriter.Write(value);
		}
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x00062374 File Offset: 0x00060574
	public static void DeleteGeologyPreset()
	{
		string path = Path.Combine(SettingsManager.AppDataPath(), "Presets", "Geology", SettingsManager.geology.title + ".json");
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x000623B8 File Offset: 0x000605B8
	public static void SaveReplacerPreset()
	{
		using (StreamWriter streamWriter = new StreamWriter(Path.Combine(SettingsManager.AppDataPath(), "Presets", "Geology", SettingsManager.geology.title + ".json"), false))
		{
			string value = JsonConvert.SerializeObject(SettingsManager.replacer, Formatting.Indented);
			streamWriter.Write(value);
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00062428 File Offset: 0x00060628
	public static void LoadGeologyPreset(string filename)
	{
		using (StreamReader streamReader = new StreamReader(Path.Combine(SettingsManager.AppDataPath(), "Presets", "Geology", SettingsManager.geology.title + ".json")))
		{
			SettingsManager.geology = JsonConvert.DeserializeObject<GeologyPreset>(streamReader.ReadToEnd());
		}
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00062490 File Offset: 0x00060690
	public static GeologyPreset GetGeologyPreset(string filename)
	{
		if (File.Exists(filename))
		{
			using (StreamReader streamReader = new StreamReader(filename))
			{
				return JsonConvert.DeserializeObject<GeologyPreset>(streamReader.ReadToEnd());
			}
		}
		return new GeologyPreset("file not found");
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x000624E0 File Offset: 0x000606E0
	public static void LoadReplacerPreset(string filename)
	{
		using (StreamReader streamReader = new StreamReader(SettingsManager.AppDataPath() + "Presets/Replacer/" + filename + ".json"))
		{
			SettingsManager.replacer = JsonConvert.DeserializeObject<ReplacerPreset>(streamReader.ReadToEnd());
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00062534 File Offset: 0x00060734
	public static void LoadGeologyMacro(string filename)
	{
		SettingsManager.macro = new List<string>();
		using (StreamReader streamReader = new StreamReader(Path.Combine(new string[]
		{
			SettingsManager.AppDataPath(),
			"Presets",
			"Geology",
			"Macros",
			filename + ".macro"
		})))
		{
			GeologyMacroWrapper geologyMacroWrapper = JsonConvert.DeserializeObject<GeologyMacroWrapper>(streamReader.ReadToEnd());
			if (geologyMacroWrapper != null && geologyMacroWrapper.macroList != null)
			{
				SettingsManager.macro = geologyMacroWrapper.macroList;
			}
		}
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x000625C8 File Offset: 0x000607C8
	public static void SaveGeologyMacro(string macroTitle)
	{
		string value = JsonConvert.SerializeObject(new GeologyMacroWrapper
		{
			macroList = SettingsManager.macro
		}, Formatting.Indented);
		using (StreamWriter streamWriter = new StreamWriter(Path.Combine(new string[]
		{
			SettingsManager.AppDataPath(),
			"Presets",
			"Geology",
			"Macros",
			macroTitle + ".macro"
		}), false))
		{
			streamWriter.Write(value);
		}
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x000085A0 File Offset: 0x000067A0
	public static void RemovePreset(int index)
	{
		if (index >= 0 && index < SettingsManager.macro.Count)
		{
			SettingsManager.macro.RemoveAt(index);
		}
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x000085BE File Offset: 0x000067BE
	public static bool MacroExists(string macroTitle)
	{
		return File.Exists(Path.Combine(new string[]
		{
			SettingsManager.AppDataPath(),
			"Presets",
			"Geology",
			"Macros",
			macroTitle + ".macro"
		}));
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x00062650 File Offset: 0x00060850
	public static void AddToMacro(string macroTitle)
	{
		string item = Path.Combine(new string[]
		{
			SettingsManager.AppDataPath(),
			"Presets",
			"Geology",
			"Macros",
			macroTitle + ".macro"
		});
		SettingsManager.macro.Add(item);
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x000626A4 File Offset: 0x000608A4
	public static void LoadSettings()
	{
		try
		{
			string settingsPath = SettingsManager.SettingsPath;
			if (string.IsNullOrEmpty(settingsPath))
			{
				Debug.LogError("Settings path is null or empty");
				return;
			}
			if (!File.Exists(settingsPath))
			{
				Debug.LogError("Config file not found at: " + settingsPath);
				return;
			}
			using (StreamReader streamReader = new StreamReader(settingsPath))
			{
				string value = streamReader.ReadToEnd();
				if (string.IsNullOrEmpty(value))
				{
					Debug.LogError("Config file is empty at: " + settingsPath);
					return;
				}
				Debug.Log("loading settings from " + settingsPath);
				EditorSettings editorSettings = JsonConvert.DeserializeObject<EditorSettings>(value);
				SettingsManager.RustDirectory = editorSettings.rustDirectory;
				SettingsManager.PrefabRenderDistance = editorSettings.prefabRenderDistance;
				SettingsManager.PathRenderDistance = editorSettings.pathRenderDistance;
				SettingsManager.WaterTransparency = editorSettings.waterTransparency;
				SettingsManager.LoadBundleOnLaunch = editorSettings.loadbundleonlaunch;
				SettingsManager.PrefabPaths = editorSettings.prefabPaths;
				SettingsManager.style = editorSettings.style;
				SettingsManager.crazing = editorSettings.crazing;
				SettingsManager.perlinSplat = editorSettings.perlinSplat;
				SettingsManager.ripple = editorSettings.ripple;
				SettingsManager.ocean = editorSettings.ocean;
				SettingsManager.terracing = editorSettings.terracing;
				SettingsManager.perlin = editorSettings.perlin;
				SettingsManager.geology = editorSettings.geology;
				SettingsManager.replacer = editorSettings.replacer;
				SettingsManager.city = editorSettings.city;
				SettingsManager.macroSources = editorSettings.macroSources;
				SettingsManager.application = editorSettings.application;
				SettingsManager.faves = editorSettings.faves;
				SettingsManager.windowStates = editorSettings.windowStates;
				SettingsManager.menuState = editorSettings.menuState;
				SettingsManager.binds = editorSettings.binds;
				Debug.Log(SettingsManager.binds.Count.ToString() + " binds loaded from disk");
			}
		}
		catch (FileNotFoundException ex)
		{
			Debug.LogError("Settings file not found: " + ex.Message);
			SettingsManager.SetDefaultSettings();
		}
		catch (JsonException ex2)
		{
			Debug.LogError("JSON deserialization error in settings file: " + ex2.Message + " ... File may have been corrupted");
			SettingsManager.SetDefaultSettings();
		}
		catch (IOException ex3)
		{
			Debug.LogError("IO error while reading settings file: " + ex3.Message);
			SettingsManager.SetDefaultSettings();
		}
		catch (Exception ex4)
		{
			Debug.LogError("Unexpected error in LoadSettings: " + ex4.Message + "\n" + ex4.StackTrace);
			SettingsManager.SetDefaultSettings();
		}
		SettingsManager.LoadPresets();
		SettingsManager.LoadMacros();
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00062964 File Offset: 0x00060B64
	public static void LoadPresets()
	{
		SettingsManager.geologyPresets = Directory.GetFiles(Path.Combine(SettingsManager.AppDataPath(), "Presets", "Geology"), "*.json");
		SettingsManager.breakerPresets = Directory.GetFiles(Path.Combine(SettingsManager.AppDataPath(), "Presets", "Breaker"));
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x000085FE File Offset: 0x000067FE
	public static void LoadMacros()
	{
		SettingsManager.geologyPresetLists = SettingsManager.GetPresetTitles(Path.Combine(SettingsManager.AppDataPath(), "Presets", "Geology", "Macros"));
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x000629B4 File Offset: 0x00060BB4
	public static string[] GetPresetTitles(string path)
	{
		char[] separator = new char[]
		{
			'/',
			'.'
		};
		string[] files = Directory.GetFiles(path);
		string[] array = new string[files.Length];
		for (int i = 0; i < files.Length; i++)
		{
			string[] array2 = files[i].Split(separator);
			int num = array2.Length - 2;
			array[i] = array2[num];
		}
		return array;
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00008623 File Offset: 0x00006823
	public static string[] GetDirectoryTitles(string path)
	{
		return Directory.GetDirectories(path);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00062A10 File Offset: 0x00060C10
	public static void SetDefaultSettings()
	{
		SettingsManager.RustDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Rust";
		ToolTips.rustDirectoryPath.text = SettingsManager.RustDirectory;
		SettingsManager.PrefabRenderDistance = 700f;
		SettingsManager.PathRenderDistance = 250f;
		SettingsManager.WaterTransparency = 0.2f;
		SettingsManager.LoadBundleOnLaunch = true;
		SettingsManager.TerrainTextureSet = false;
		SettingsManager.style = true;
		SettingsManager.application = new FilePreset
		{
			rustDirectory = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Rust",
			prefabRenderDistance = 5000f,
			pathRenderDistance = 5000f,
			waterTransparency = 0f,
			loadBatch = 128,
			newSize = 1000,
			newHeight = 0.5f,
			newBiome = TerrainBiome.Enum.Temperate,
			newSplat = TerrainSplat.Enum.Grass
		};
		SettingsManager.crazing = default(CrazingPreset);
		SettingsManager.perlinSplat = default(PerlinSplatPreset);
		SettingsManager.ripple = default(RipplePreset);
		SettingsManager.ocean = default(OceanPreset);
		SettingsManager.terracing = default(TerracingPreset);
		SettingsManager.perlin = default(PerlinPreset);
		SettingsManager.geology = default(GeologyPreset);
		SettingsManager.replacer = default(ReplacerPreset);
		SettingsManager.city = default(RustCityPreset);
		SettingsManager.breaker = default(BreakerPreset);
		SettingsManager.macroSources = false;
		SettingsManager.faves = default(Favorites);
		SettingsManager.windowStates = new WindowState[0];
		SettingsManager.menuState = default(MenuState);
		SettingsManager.PrefabPaths = new string[0];
		SettingsManager.macro = new List<string>();
		SettingsManager.binds = new List<Bind>();
		Debug.Log("Default Settings set.");
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0000862B File Offset: 0x0000682B
	public static void SetDefaultGeology()
	{
		SettingsManager.geology = new GeologyPreset("Default");
	}

	// Token: 0x04000844 RID: 2116
	public static string SettingsPath;

	// Token: 0x04000845 RID: 2117
	public const string BundlePathExt = "\\Bundles\\Bundles";

	// Token: 0x04000860 RID: 2144
	public static BreakerSerialization breakerSerializer = new BreakerSerialization();
}
