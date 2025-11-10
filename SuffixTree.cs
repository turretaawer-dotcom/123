using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SuffixTree
{
	// Token: 0x0200020F RID: 527
	public class SuffixTree
	{
		// Token: 0x06000EC4 RID: 3780 RVA: 0x00077BB4 File Offset: 0x00075DB4
		public SuffixTree()
		{
			this._root = new SuffixTree.Node
			{
				Start = 0,
				End = 0
			};
			this._structure.Add(new ValueTuple<SuffixTree.Node, char>(null, '\0'), this._root);
			this._AP = new SuffixTree.ActivePoint(this)
			{
				ActiveParent = this._root
			};
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0000A886 File Offset: 0x00008A86
		public static SuffixTree Build(string value)
		{
			SuffixTree suffixTree = new SuffixTree();
			suffixTree.AddString(value);
			return suffixTree;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00077C38 File Offset: 0x00075E38
		public void AddString(string value)
		{
			foreach (char c in value)
			{
				this.ExtendTree(c);
			}
			this._remainder = 0;
			this._AP.ResetEdge();
			this._AP.ActiveParent = this._root;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x00077C8C File Offset: 0x00075E8C
		private void ExtendTree(char c)
		{
			this._chars.Add(c);
			this._needSuffixLink = null;
			this._position++;
			this._remainder++;
			while (this._remainder > 0 && !this._AP.MoveDown(c))
			{
				if (this._AP.ActiveEdge != null)
				{
					this._AP.ActiveParent = this.InsertSplit(this._AP);
				}
				this.InsertLeaf(this._AP, c);
				this._remainder--;
				if (this._remainder > 0)
				{
					this._AP.Rescan();
				}
			}
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0000A894 File Offset: 0x00008A94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int LengthOf(SuffixTree.Node edge)
		{
			return ((edge.End == -1) ? (this._position + 1) : edge.End) - edge.Start;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0000A8B6 File Offset: 0x00008AB6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private char FirstCharOf(SuffixTree.Node edge)
		{
			return this._chars[edge.Start];
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00077D38 File Offset: 0x00075F38
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private string LabelOf(SuffixTree.Node edge)
		{
			char[] array = new char[this.LengthOf(edge)];
			this._chars.CopyTo(edge.Start, array, 0, this.LengthOf(edge));
			return new string(array);
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0000A8C9 File Offset: 0x00008AC9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool GetLinkFor(SuffixTree.Node node, out SuffixTree.Node linkedNode)
		{
			return this._suffixLinks.TryGetValue(node, out linkedNode);
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0000A8D8 File Offset: 0x00008AD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool GetEdgeFor(SuffixTree.Node n, char c, out SuffixTree.Node edge)
		{
			return this._structure.TryGetValue(new ValueTuple<SuffixTree.Node, char>(n, c), out edge);
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x00077D74 File Offset: 0x00075F74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private SuffixTree.Node InsertLeaf(SuffixTree.ActivePoint ap, char c)
		{
			SuffixTree.Node node = new SuffixTree.Node
			{
				Start = this._position,
				End = -1
			};
			this._structure.Add(new ValueTuple<SuffixTree.Node, char>(ap.ActiveParent, c), node);
			return node;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00077DB4 File Offset: 0x00075FB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private SuffixTree.Node InsertSplit(SuffixTree.ActivePoint ap)
		{
			ValueTuple<SuffixTree.Node, char> key = new ValueTuple<SuffixTree.Node, char>(ap.ActiveParent, this.FirstCharOf(ap.ActiveEdge));
			SuffixTree.Node activeEdge = ap.ActiveEdge;
			this._structure.Remove(key);
			SuffixTree.Node node = new SuffixTree.Node
			{
				Start = activeEdge.Start,
				End = activeEdge.Start + ap.ActiveLength
			};
			this._structure.Add(key, node);
			this._AP.ActiveEdge = node;
			this.AddSuffixLink(node);
			activeEdge.Start = node.End;
			this._structure.Add(new ValueTuple<SuffixTree.Node, char>(node, this.FirstCharOf(activeEdge)), activeEdge);
			return node;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0000A8ED File Offset: 0x00008AED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AddSuffixLink(SuffixTree.Node node)
		{
			if (this._needSuffixLink != null)
			{
				this._suffixLinks.Add(this._needSuffixLink, node);
			}
			this._needSuffixLink = node;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00077E5C File Offset: 0x0007605C
		public bool Contains(string value)
		{
			SuffixTree.Node root = this._root;
			int length = value.Length;
			int i = 0;
			while (i < value.Length)
			{
				if (!this.GetEdgeFor(root, value[i++], out root))
				{
					return false;
				}
				int num = root.IsLeaf ? (this._position + 1) : root.End;
				int num2 = root.Start + 1;
				while (num2 < num && i < length)
				{
					if (this._chars[num2] != value[i])
					{
						return false;
					}
					num2++;
					i++;
				}
			}
			return true;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00077EF0 File Offset: 0x000760F0
		public string PrintTree()
		{
			SuffixTree.<>c__DisplayClass24_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.sb = new StringBuilder();
			CS$<>8__locals1.sb.AppendLine(string.Format("Content length: {0}{1}", this._chars.Count, Environment.NewLine));
			this.<PrintTree>g__Print|24_0(0, this._root, ref CS$<>8__locals1);
			return CS$<>8__locals1.sb.ToString();
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00077F58 File Offset: 0x00076158
		[CompilerGenerated]
		private void <PrintTree>g__Print|24_0(int depth, SuffixTree.Node node, ref SuffixTree.<>c__DisplayClass24_0 A_3)
		{
			string text = "";
			string text2 = this.LabelOf(node);
			string text3 = "";
			string text4 = "";
			if (node == this._AP.ActiveParent)
			{
				text = ">";
			}
			if (node == this._AP.ActiveEdge)
			{
				text2 = text2.Insert(this._AP.ActiveLength, " | ");
			}
			if (node.IsLeaf)
			{
				text3 = "...";
			}
			SuffixTree.Node edge;
			if (this.GetLinkFor(node, out edge))
			{
				text4 = " -> " + this.FirstCharOf(edge).ToString();
			}
			A_3.sb.AppendLine(string.Concat(new string[]
			{
				new string(' ', depth + 1 - text.Length),
				text,
				depth.ToString(),
				":",
				text2,
				text3,
				text4
			}));
			for (char c = 'a'; c <= 'z'; c += '\u0001')
			{
				SuffixTree.Node node2;
				if (this._structure.TryGetValue(new ValueTuple<SuffixTree.Node, char>(node, c), out node2))
				{
					this.<PrintTree>g__Print|24_0(depth + 1, node2, ref A_3);
				}
			}
		}

		// Token: 0x04000B26 RID: 2854
		private const int BOUNDLESS = -1;

		// Token: 0x04000B27 RID: 2855
		private SuffixTree.ActivePoint _AP;

		// Token: 0x04000B28 RID: 2856
		private int _remainder;

		// Token: 0x04000B29 RID: 2857
		private int _position = -1;

		// Token: 0x04000B2A RID: 2858
		private SuffixTree.Node _root;

		// Token: 0x04000B2B RID: 2859
		private SuffixTree.Node _needSuffixLink;

		// Token: 0x04000B2C RID: 2860
		private List<char> _chars = new List<char>();

		// Token: 0x04000B2D RID: 2861
		private Dictionary<ValueTuple<SuffixTree.Node, char>, SuffixTree.Node> _structure = new Dictionary<ValueTuple<SuffixTree.Node, char>, SuffixTree.Node>();

		// Token: 0x04000B2E RID: 2862
		private Dictionary<SuffixTree.Node, SuffixTree.Node> _suffixLinks = new Dictionary<SuffixTree.Node, SuffixTree.Node>();

		// Token: 0x02000210 RID: 528
		private class Node
		{
			// Token: 0x170001C6 RID: 454
			// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x0000A910 File Offset: 0x00008B10
			public bool IsLeaf
			{
				get
				{
					return this.End == -1;
				}
			}

			// Token: 0x04000B2F RID: 2863
			public int Start;

			// Token: 0x04000B30 RID: 2864
			public int End;
		}

		// Token: 0x02000211 RID: 529
		private class ActivePoint
		{
			// Token: 0x170001C7 RID: 455
			// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0000A91B File Offset: 0x00008B1B
			// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x0000A923 File Offset: 0x00008B23
			public SuffixTree.Node ActiveEdge
			{
				get
				{
					return this._activeEdge;
				}
				set
				{
					this._activeEdge = value;
				}
			}

			// Token: 0x170001C8 RID: 456
			// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0000A92C File Offset: 0x00008B2C
			// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x0000A934 File Offset: 0x00008B34
			public SuffixTree.Node ActiveParent
			{
				get
				{
					return this._activeParent;
				}
				set
				{
					if (value.IsLeaf)
					{
						throw new Exception("Leaf node cannot be parent.");
					}
					this._activeParent = value;
				}
			}

			// Token: 0x170001C9 RID: 457
			// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0000A950 File Offset: 0x00008B50
			// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0000A958 File Offset: 0x00008B58
			public int ActiveLength { get; private set; }

			// Token: 0x06000EDB RID: 3803 RVA: 0x0000A961 File Offset: 0x00008B61
			public ActivePoint(SuffixTree tree)
			{
				this._tree = tree;
			}

			// Token: 0x06000EDC RID: 3804 RVA: 0x0000A970 File Offset: 0x00008B70
			public void ResetEdge()
			{
				this.ActiveLength = 0;
				this.ActiveEdge = null;
			}

			// Token: 0x06000EDD RID: 3805 RVA: 0x00078070 File Offset: 0x00076270
			public bool MoveDown(char c)
			{
				if (this.ActiveEdge == null && !this._tree.GetEdgeFor(this.ActiveParent, c, out this._activeEdge))
				{
					return false;
				}
				if (this._tree._chars[this.ActiveEdge.Start + this.ActiveLength] != c)
				{
					return false;
				}
				int activeLength = this.ActiveLength;
				this.ActiveLength = activeLength + 1;
				if (!this.ActiveEdge.IsLeaf && this.ActiveLength == this._tree.LengthOf(this.ActiveEdge))
				{
					this.ActiveParent = this.ActiveEdge;
					this.ResetEdge();
				}
				return true;
			}

			// Token: 0x06000EDE RID: 3806 RVA: 0x00078114 File Offset: 0x00076314
			public void Rescan()
			{
				if (!this._tree.GetLinkFor(this._activeParent, out this._activeParent))
				{
					this.ActiveEdge = null;
					this.ActiveParent = this._tree._root;
					this.ActiveLength = this._tree._remainder - 1;
				}
				if (this.ActiveLength == 0)
				{
					return;
				}
				while (this._tree.GetEdgeFor(this.ActiveParent, this._tree._chars[this._tree._position - this.ActiveLength], out this._activeEdge) && this.ActiveLength >= this._tree.LengthOf(this._activeEdge))
				{
					this.ActiveLength -= this._tree.LengthOf(this._activeEdge);
					this.ActiveParent = this._activeEdge;
				}
			}

			// Token: 0x04000B32 RID: 2866
			private SuffixTree.Node _activeEdge;

			// Token: 0x04000B33 RID: 2867
			private SuffixTree.Node _activeParent;

			// Token: 0x04000B34 RID: 2868
			private SuffixTree _tree;
		}
	}
}
