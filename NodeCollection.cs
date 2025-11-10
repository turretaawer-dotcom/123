using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001F5 RID: 501
	[Serializable]
	public class NodeCollection : IList<Node>, ICollection<Node>, IEnumerable<Node>, IEnumerable
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x00009BF5 File Offset: 0x00007DF5
		public int Count
		{
			get
			{
				return this._childNodes.Count;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00009C02 File Offset: 0x00007E02
		private bool isOwnerHasTree
		{
			get
			{
				return this._owner.tree != null;
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00009C15 File Offset: 0x00007E15
		public NodeCollection(Node ownerNode)
		{
			this._owner = ownerNode;
			this._childNodes = new List<Node>();
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00075D4C File Offset: 0x00073F4C
		public void AddRange(Node[] nodeArray)
		{
			foreach (Node node in nodeArray)
			{
				this.AddInternal(node);
			}
			if (!this.isOwnerHasTree)
			{
				return;
			}
			this.RebuildTree();
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00075D84 File Offset: 0x00073F84
		public void AddRangeWithoutNotify(Node[] nodeArray)
		{
			foreach (Node node in nodeArray)
			{
				this.AddInternal(node);
			}
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00075DAC File Offset: 0x00073FAC
		public Node AddFluent(string name, int styleIndex)
		{
			Node node = new Node(name, styleIndex);
			return this.AddFluent(node);
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00075DC8 File Offset: 0x00073FC8
		public Node AddFluent(string name)
		{
			Node node = new Node(name, 0);
			return this.AddFluent(node);
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00009C2F File Offset: 0x00007E2F
		public Node AddFluent(Node node)
		{
			this.Add(node);
			return node;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00009C39 File Offset: 0x00007E39
		public void Add(Node node)
		{
			if (node == null)
			{
				return;
			}
			this.AddInternal(node);
			if (this.isOwnerHasTree && node.CheckAllParentExpanded())
			{
				this.RebuildTree();
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x00009C5C File Offset: 0x00007E5C
		public void AddWithoutNotify(Node node)
		{
			if (node == null)
			{
				return;
			}
			this.AddInternal(node);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00009C69 File Offset: 0x00007E69
		public void Clear()
		{
			this._childNodes.Clear();
			if (this.isOwnerHasTree)
			{
				this.RebuildTree();
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00075DE4 File Offset: 0x00073FE4
		private void AddInternal(Node node)
		{
			this._childNodes.Add(node);
			node.parentNode = this._owner;
			if (this.isOwnerHasTree)
			{
				this.AssignTreeToNodeAndAllChildren(node);
				if (this._owner.tree.searchable && !string.IsNullOrEmpty(node.name))
				{
					this._owner.tree.AddNodeNameReference(node);
				}
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00075E48 File Offset: 0x00074048
		private void AssignTreeToNodeAndAllChildren(Node node)
		{
			Node[] allChildrenRecursive = node.GetAllChildrenRecursive();
			for (int i = 0; i < allChildrenRecursive.Length; i++)
			{
				allChildrenRecursive[i].tree = this._owner.tree;
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00009C84 File Offset: 0x00007E84
		public bool RemoveWithoutNotify(Node node)
		{
			return node != null && this._childNodes.Remove(node);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00075E80 File Offset: 0x00074080
		public bool Remove(Node node)
		{
			if (node == null)
			{
				return false;
			}
			bool flag = this.isOwnerHasTree && node.CheckAllParentExpanded();
			if (!this._childNodes.Remove(node))
			{
				return false;
			}
			if (flag)
			{
				this.RebuildTree();
			}
			return true;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00009C9C File Offset: 0x00007E9C
		public void RemoveAt(int index)
		{
			bool flag = this.isOwnerHasTree && this._childNodes[index].CheckAllParentExpanded();
			this._childNodes.RemoveAt(index);
			if (flag)
			{
				this.RebuildTree();
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00009CCE File Offset: 0x00007ECE
		public void RemoveAtWithoutNotify(int index)
		{
			if (this.isOwnerHasTree)
			{
				this._childNodes[index].CheckAllParentExpanded();
			}
			this._childNodes.RemoveAt(index);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00009CF9 File Offset: 0x00007EF9
		private void RebuildTree()
		{
			this._owner.tree.Rebuild();
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00009D0B File Offset: 0x00007F0B
		public int IndexOf(Node node)
		{
			return this._childNodes.IndexOf(node);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00009D19 File Offset: 0x00007F19
		public void Insert(int index, Node node)
		{
			this._childNodes.Insert(index, node);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00009D28 File Offset: 0x00007F28
		public void CopyTo(Node[] array, int arrayIndex)
		{
			this._childNodes.CopyTo(array, arrayIndex);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00009D37 File Offset: 0x00007F37
		public IEnumerator<Node> GetEnumerator()
		{
			return this._childNodes.GetEnumerator();
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00009D49 File Offset: 0x00007F49
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000176 RID: 374
		public Node this[int index]
		{
			get
			{
				return this._childNodes[index];
			}
			set
			{
				this._childNodes[index] = value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x000026C4 File Offset: 0x000008C4
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00009D6E File Offset: 0x00007F6E
		public bool Contains(Node item)
		{
			return this._childNodes.Contains(item);
		}

		// Token: 0x04000A89 RID: 2697
		[SerializeReference]
		private List<Node> _childNodes;

		// Token: 0x04000A8A RID: 2698
		[SerializeReference]
		private Node _owner;
	}
}
