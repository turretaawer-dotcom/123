using System;
using System.Collections.Generic;
using System.Text;
using SuffixTree;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001F4 RID: 500
	[Serializable]
	public class Node
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00009916 File Offset: 0x00007B16
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x0000991E File Offset: 0x00007B1E
		public SuffixTree SuffixTree
		{
			get
			{
				return this._suffixTree;
			}
			set
			{
				this._suffixTree = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00009927 File Offset: 0x00007B27
		public bool hasChildren
		{
			get
			{
				return this.nodeCollection.Count > 0;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x00009937 File Offset: 0x00007B37
		public int childCount
		{
			get
			{
				return this.nodeCollection.Count;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x00009944 File Offset: 0x00007B44
		public int depth
		{
			get
			{
				if (this.parent != null)
				{
					return this.parent.depth + 1;
				}
				return -1;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0000995D File Offset: 0x00007B5D
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x00009965 File Offset: 0x00007B65
		public bool isExpanded
		{
			get
			{
				return this._isExpanded;
			}
			set
			{
				if (value == this._isExpanded)
				{
					return;
				}
				this._isExpanded = value;
				if (this.tree != null)
				{
					this.tree.Rebuild();
				}
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00009991 File Offset: 0x00007B91
		// (set) Token: 0x06000D7C RID: 3452 RVA: 0x00009999 File Offset: 0x00007B99
		public bool isSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value == this._isSelected)
				{
					return;
				}
				if (this.tree != null)
				{
					this.tree.OnNodeClicked(this);
				}
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x000099BF File Offset: 0x00007BBF
		// (set) Token: 0x06000D7E RID: 3454 RVA: 0x000099C7 File Offset: 0x00007BC7
		public bool isSubSelected { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x000099D0 File Offset: 0x00007BD0
		// (set) Token: 0x06000D80 RID: 3456 RVA: 0x000099D8 File Offset: 0x00007BD8
		public bool isChecked
		{
			get
			{
				return this._isChecked;
			}
			set
			{
				if (value == this._isChecked)
				{
					return;
				}
				this._isChecked = value;
				if (this.tree != null)
				{
					this.tree.UpdateNodeCheckedState(this);
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00009A05 File Offset: 0x00007C05
		// (set) Token: 0x06000D82 RID: 3458 RVA: 0x00009A0D File Offset: 0x00007C0D
		public bool isFaded
		{
			get
			{
				return this._isFaded;
			}
			set
			{
				this._isFaded = value;
				if (this.tree != null)
				{
					this.tree.Repaint();
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00009A2F File Offset: 0x00007C2F
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x00009A37 File Offset: 0x00007C37
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				if (this.tree != null)
				{
					this.tree.Repaint();
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00009A59 File Offset: 0x00007C59
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x00009A61 File Offset: 0x00007C61
		public int styleIndex
		{
			get
			{
				return this._styleIndex;
			}
			set
			{
				this._styleIndex = value;
				if (this.tree != null)
				{
					this.tree.Repaint();
				}
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00009A83 File Offset: 0x00007C83
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x00009A8B File Offset: 0x00007C8B
		public object data { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00009A94 File Offset: 0x00007C94
		public NodeCollection nodes
		{
			get
			{
				return this.nodeCollection;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00009A9C File Offset: 0x00007C9C
		public int indexInCollection
		{
			get
			{
				return this.parent.nodes.IndexOf(this);
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00009AAF File Offset: 0x00007CAF
		public int nodeId
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00009AB7 File Offset: 0x00007CB7
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00009ABF File Offset: 0x00007CBF
		public UIRecycleTree tree
		{
			get
			{
				return this.treeView;
			}
			set
			{
				this.treeView = value;
				this._id = this.treeView.GetNextId();
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00009AD9 File Offset: 0x00007CD9
		// (set) Token: 0x06000D8F RID: 3471 RVA: 0x00009AE1 File Offset: 0x00007CE1
		public Node parentNode
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x00075668 File Offset: 0x00073868
		public string fullPath
		{
			get
			{
				if (this.treeView == null)
				{
					throw new Exception("Tree Node Has No Parent");
				}
				StringBuilder stringBuilder = new StringBuilder();
				this.GetFullPath(stringBuilder, this.treeView.separator);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x00009AEA File Offset: 0x00007CEA
		public Node(string name, int styleIndex = 0) : this()
		{
			this._styleIndex = styleIndex;
			this._name = name;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x00009B00 File Offset: 0x00007D00
		public Node(UIRecycleTree treeView, Node[] children, string name = null) : this()
		{
			this._name = name;
			this.tree = treeView;
			this.nodeCollection.AddRange(children);
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00009B22 File Offset: 0x00007D22
		public Node(UIRecycleTree treeView, string name = null) : this()
		{
			this._name = name;
			this.tree = treeView;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00009B38 File Offset: 0x00007D38
		public Node()
		{
			this.nodeCollection = new NodeCollection(this);
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x000756AC File Offset: 0x000738AC
		public virtual void ExpandAll()
		{
			foreach (Node node in this.nodeCollection)
			{
				node.ExpandAllWithoutNotify();
			}
			this.isExpanded = true;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00075700 File Offset: 0x00073900
		public virtual void CollapseAll()
		{
			foreach (Node node in this.nodeCollection)
			{
				node.CollapseAllWithoutNotify();
			}
			this.isExpanded = false;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00075754 File Offset: 0x00073954
		public void ExpandAllWithoutNotify()
		{
			foreach (Node node in this.nodeCollection)
			{
				node.ExpandAllWithoutNotify();
			}
			this._isExpanded = true;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00009B4C File Offset: 0x00007D4C
		public void ExpandWithoutNotify()
		{
			this._isExpanded = true;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00009B55 File Offset: 0x00007D55
		public void CollapseWithoutNotify()
		{
			this._isExpanded = false;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x000757A8 File Offset: 0x000739A8
		public void CollapseAllWithoutNotify()
		{
			foreach (Node node in this.nodeCollection)
			{
				node.CollapseAllWithoutNotify();
			}
			this._isExpanded = false;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000757FC File Offset: 0x000739FC
		public Node[] GetAllChildrenRecursive()
		{
			List<Node> list = new List<Node>
			{
				this
			};
			this.GetAllChildrenRecursive(list);
			return list.ToArray();
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00075824 File Offset: 0x00073A24
		public void GetAllChildrenRecursive(List<Node> childList)
		{
			foreach (Node node in this.nodes)
			{
				childList.Add(node);
				node.GetAllChildrenRecursive(childList);
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00075878 File Offset: 0x00073A78
		public Node[] GetAllParentsRecursive()
		{
			List<Node> list = new List<Node>();
			this.GetAllParentsRecursive(list);
			return list.ToArray();
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00075898 File Offset: 0x00073A98
		public void GetAllParentsRecursive(List<Node> parentsList)
		{
			if (this.parent == null || this.tree == null)
			{
				return;
			}
			if (this.parent == this.tree.rootNode)
			{
				return;
			}
			parentsList.Add(this.parent);
			this.parent.GetAllParentsRecursive(parentsList);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x000758E8 File Offset: 0x00073AE8
		public void GetAllExpandedChildrenRecursive(List<Node> childList)
		{
			foreach (Node node in this.nodes)
			{
				childList.Add(node);
				if (node.isExpanded)
				{
					node.GetAllExpandedChildrenRecursive(childList);
				}
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00075944 File Offset: 0x00073B44
		public bool TryCastData<T>(out T castedData) where T : class
		{
			castedData = default(T);
			if (this.data == null)
			{
				return false;
			}
			bool result;
			try
			{
				castedData = (T)((object)this.data);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00009B5E File Offset: 0x00007D5E
		public void RemoveYourself()
		{
			if (this.parent != null)
			{
				this.parent.nodes.Remove(this);
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00075990 File Offset: 0x00073B90
		public Node FindNodeByIdRecursive(int id, Node item)
		{
			if (item == null)
			{
				return null;
			}
			if (item.nodeId == id)
			{
				return item;
			}
			if (!item.hasChildren)
			{
				return null;
			}
			foreach (Node item2 in item.nodes)
			{
				Node node = this.FindNodeByIdRecursive(id, item2);
				if (node != null)
				{
					return node;
				}
			}
			return null;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00075A04 File Offset: 0x00073C04
		public Node FindNodeByDataRecursive(object searchedData)
		{
			foreach (Node node in this.nodes)
			{
				if (node.data == searchedData)
				{
					return node;
				}
				if (node.hasChildren)
				{
					Node node2 = node.FindNodeByDataRecursive(searchedData);
					if (node2 != null)
					{
						return node2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00075A74 File Offset: 0x00073C74
		public void FindNodesByDataRecursive(object searchObject, List<Node> foundedItems)
		{
			foreach (Node node in this.nodes)
			{
				if (node.data == searchObject)
				{
					foundedItems.Add(node);
				}
				if (node.hasChildren)
				{
					node.FindNodesByDataRecursive(searchObject, foundedItems);
				}
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00075ADC File Offset: 0x00073CDC
		public Node FindNodeByNameRecursive(string searchedName)
		{
			foreach (Node node in this.nodes)
			{
				if (node.name == searchedName)
				{
					return node;
				}
				if (node.hasChildren)
				{
					Node node2 = node.FindNodeByDataRecursive(searchedName);
					if (node2 != null)
					{
						return node2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00075B50 File Offset: 0x00073D50
		public void FindNodesByNameRecursive(string searchName, List<Node> foundedItems)
		{
			foreach (Node node in this.nodes)
			{
				if (node.name == searchName)
				{
					foundedItems.Add(node);
				}
				if (node.hasChildren)
				{
					node.FindNodesByNameRecursive(searchName, foundedItems);
				}
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00075BBC File Offset: 0x00073DBC
		public void FindAllChildrenWithIsCheckedStateRecursive(List<Node> foundedItems)
		{
			foreach (Node node in this.nodes)
			{
				if (node._isChecked)
				{
					foundedItems.Add(node);
				}
				if (node.hasChildren)
				{
					node.FindAllChildrenWithIsCheckedStateRecursive(foundedItems);
				}
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00009B7A File Offset: 0x00007D7A
		public bool CheckAllParentExpanded()
		{
			return this.parent == null || (this.parent.isExpanded && this.parent.CheckAllParentExpanded());
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00075C20 File Offset: 0x00073E20
		public int GetAllChildrenCountRecursive()
		{
			int num = 0;
			foreach (Node node in this.nodes)
			{
				num += node.GetAllChildrenCountRecursive() + 1;
			}
			return num;
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00075C74 File Offset: 0x00073E74
		public void ChangeIsCheckedStateForAllChildren(bool isCheck)
		{
			foreach (Node node in this.nodes)
			{
				node.SetCheckedWithoutNotify(isCheck);
				if (this.tree != null)
				{
					this.tree.NodeCheckedStateChangedNotify(node);
				}
				if (node.hasChildren)
				{
					node.ChangeIsCheckedStateForAllChildren(this.isChecked);
				}
			}
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00075CF0 File Offset: 0x00073EF0
		public void ChangeIsSubSelectedStateForAllChildren(bool nodeIsSubSelected)
		{
			foreach (Node node in this.nodes)
			{
				node.isSubSelected = nodeIsSubSelected;
				if (node.hasChildren)
				{
					node.ChangeIsSubSelectedStateForAllChildren(nodeIsSubSelected);
				}
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00009BA0 File Offset: 0x00007DA0
		public void SetCheckedWithoutNotify(bool nodeIsChecked)
		{
			this._isChecked = nodeIsChecked;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00009BA9 File Offset: 0x00007DA9
		public void SetExpandedStateWithoutNotify(bool nodeIsExpanded)
		{
			this._isExpanded = nodeIsExpanded;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00009BB2 File Offset: 0x00007DB2
		public void SetSelectedWithoutNotify(bool nodeIsSelected)
		{
			this._isSelected = nodeIsSelected;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00009BBB File Offset: 0x00007DBB
		private void GetFullPath(StringBuilder path, string pathSeparator)
		{
			if (this.parent == null)
			{
				return;
			}
			this.parent.GetFullPath(path, pathSeparator);
			if (this.parent.parent != null)
			{
				path.Append(pathSeparator);
			}
			path.Append(this._name);
		}

		// Token: 0x04000A7B RID: 2683
		private const short ROOT_INDENT = -1;

		// Token: 0x04000A7C RID: 2684
		[NonSerialized]
		private SuffixTree _suffixTree;

		// Token: 0x04000A7D RID: 2685
		[SerializeReference]
		protected NodeCollection nodeCollection;

		// Token: 0x04000A7E RID: 2686
		[SerializeReference]
		protected UIRecycleTree treeView;

		// Token: 0x04000A7F RID: 2687
		[SerializeReference]
		protected Node parent;

		// Token: 0x04000A80 RID: 2688
		[SerializeReference]
		private string _name;

		// Token: 0x04000A81 RID: 2689
		[SerializeField]
		private int _id;

		// Token: 0x04000A82 RID: 2690
		[SerializeField]
		private bool _isExpanded;

		// Token: 0x04000A83 RID: 2691
		[SerializeField]
		private bool _isChecked;

		// Token: 0x04000A84 RID: 2692
		[SerializeField]
		private int _styleIndex;

		// Token: 0x04000A87 RID: 2695
		private bool _isSelected;

		// Token: 0x04000A88 RID: 2696
		private bool _isFaded;
	}
}
