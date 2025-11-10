using System;
using System.Collections.Generic;
using SuffixTree;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x02000207 RID: 519
	public class UIRecycleTree : RecycleView, IRecycleDataSource
	{
		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000E50 RID: 3664 RVA: 0x00076A74 File Offset: 0x00074C74
		// (remove) Token: 0x06000E51 RID: 3665 RVA: 0x00076AAC File Offset: 0x00074CAC
		public event Action<Node> onNodeDragStart;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000E52 RID: 3666 RVA: 0x00076AE4 File Offset: 0x00074CE4
		// (remove) Token: 0x06000E53 RID: 3667 RVA: 0x00076B1C File Offset: 0x00074D1C
		public event Action<Node, Node> onNodeDrop;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x06000E54 RID: 3668 RVA: 0x00076B54 File Offset: 0x00074D54
		// (remove) Token: 0x06000E55 RID: 3669 RVA: 0x00076B8C File Offset: 0x00074D8C
		public event Action<Node> onNodeDragEnd;

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0000A307 File Offset: 0x00008507
		public NodeCollection nodes
		{
			get
			{
				return this.root.nodes;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0000A314 File Offset: 0x00008514
		public Node rootNode
		{
			get
			{
				return this.root;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0000A31C File Offset: 0x0000851C
		public int nodesCount
		{
			get
			{
				return this.root.GetAllChildrenCountRecursive() - 1;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0000A32B File Offset: 0x0000852B
		public bool isCheckboxesEnabled
		{
			get
			{
				return this.checkboxEnabled;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0000A333 File Offset: 0x00008533
		// (set) Token: 0x06000E5B RID: 3675 RVA: 0x0000A33B File Offset: 0x0000853B
		public string separator
		{
			get
			{
				return this.pathSeparator;
			}
			set
			{
				this.pathSeparator = value;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0000A344 File Offset: 0x00008544
		public NodeStyle[] nodeStyles
		{
			get
			{
				return this.nodeStylesArray;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0000A34C File Offset: 0x0000854C
		public int expandedCount
		{
			get
			{
				return this.expandedNodes.Count;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0000A359 File Offset: 0x00008559
		public Node selectedNode
		{
			get
			{
				return this._selectedNode;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0000A361 File Offset: 0x00008561
		public bool hasSelected
		{
			get
			{
				return this._selectedNode != null;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x0000A36C File Offset: 0x0000856C
		// (set) Token: 0x06000E61 RID: 3681 RVA: 0x0000A374 File Offset: 0x00008574
		public bool isRecursiveChecked
		{
			get
			{
				return this.recursiveChecked;
			}
			set
			{
				this.recursiveChecked = value;
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00076BC4 File Offset: 0x00074DC4
		private new void Start()
		{
			if (AppManager.Instance != null && AppManager.Instance.RecycleTrees != null)
			{
				int num = AppManager.Instance.RecycleTrees.IndexOf(this);
				if (num >= 0 && num < AppManager.Instance.windowPanels.Count && AppManager.Instance.windowPanels[num] != null)
				{
					this.parentWindowPanel = AppManager.Instance.windowPanels[num].GetComponent<RectTransform>();
				}
			}
			if (this.parentWindowPanel == null)
			{
				Debug.LogWarning("No parent windowPanel found for UIRecycleTree " + base.gameObject.name + ". Drag-and-drop may not work correctly.");
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00076C74 File Offset: 0x00074E74
		public void ExpandAll()
		{
			foreach (Node node in this.nodes)
			{
				node.ExpandAllWithoutNotify();
			}
			this.Rebuild();
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00076CC4 File Offset: 0x00074EC4
		public void CollapseAll()
		{
			foreach (Node node in this.nodes)
			{
				node.CollapseAllWithoutNotify();
			}
			this.Rebuild();
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0000A37D File Offset: 0x0000857D
		public void Clear()
		{
			this.nodes.Clear();
			this.lastNodeId = this.root.nodeId;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00076D14 File Offset: 0x00074F14
		public void OnNodeClicked(Node node)
		{
			if (this._selectedNode == null)
			{
				this.SelectAndNotify(node);
			}
			else if (this._selectedNode == node)
			{
				this.DeselectAndNotify(node);
			}
			else
			{
				this.DeselectAndNotify(this._selectedNode);
				this.SelectAndNotify(node);
			}
			base.Repaint();
			NodeEvent nodeEvent = this.onSelectionChanged;
			if (nodeEvent == null)
			{
				return;
			}
			nodeEvent.Invoke(node);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0000A39B File Offset: 0x0000859B
		public void UpdateNodeCheckedState(Node node)
		{
			this.NodeCheckedStateChangedNotify(node);
			base.Repaint();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0000A3AA File Offset: 0x000085AA
		public void NodeCheckedStateChangedNotify(Node node)
		{
			NodeEvent nodeEvent = this.onNodeCheckedChanged;
			if (nodeEvent == null)
			{
				return;
			}
			nodeEvent.Invoke(node);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0000A3BD File Offset: 0x000085BD
		public void Rebuild()
		{
			if (!Application.isPlaying || !base.isActiveAndEnabled)
			{
				return;
			}
			this.expandedNodes = new List<Node>();
			this.root.GetAllExpandedChildrenRecursive(this.expandedNodes);
			base.StartCoroutine(base.Reload());
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x00076D70 File Offset: 0x00074F70
		public void MergeDataWithView(RecycleItem recycleItem, int indexInExpandedNodes)
		{
			Node node = this.expandedNodes[indexInExpandedNodes];
			NodeView nodeView = (NodeView)recycleItem;
			nodeView.ClearPreviousSubscribes();
			int styleIndex = node.styleIndex;
			if (this.nodeStylesArray.Length == 0 || styleIndex >= this.nodeStylesArray.Length)
			{
				throw new Exception(string.Format("NodeStylesArray is empty or The Node {0} has an styleIndex {1} that is not in the UIRecycleTree stylesArray.", node.name, styleIndex));
			}
			NodeStyle nodeStyle = this.nodeStylesArray[styleIndex];
			if (nodeStyle == null)
			{
				throw new Exception(string.Format("Tree not contain nodeStyle with index {0}. Please add Node Style or change styleIndex in node named {1} id {2}", styleIndex, node.name, node.nodeId));
			}
			this.SetNodeStyle(nodeStyle, node, nodeView);
			nodeView.node = node;
			nodeView.text = node.name;
			nodeView.indent = (float)node.depth;
			nodeView.isChecked = node.isChecked;
			nodeView.isFaded = node.isFaded;
			if (!node.hasChildren)
			{
				nodeView.state = ExpandedState.NoChild;
			}
			else
			{
				nodeView.state = (node.isExpanded ? ExpandedState.Expanded : ExpandedState.Collapsed);
			}
			nodeView.ClickedEvent += delegate()
			{
				this.OnNodeClicked(node);
			};
			nodeView.ExpandClickEvent += delegate()
			{
				this.OnNodeExpandClicked(node);
			};
			nodeView.CheckboxClickedEvent += delegate()
			{
				this.NodeCheckedClicked(node);
			};
			nodeView.DoubleClickedEvent += delegate()
			{
				this.OnNodeDoubleClick(node);
			};
			nodeView.DragStartEvent += delegate(Node draggedNode)
			{
				Action<Node> action = this.onNodeDragStart;
				if (action == null)
				{
					return;
				}
				action(draggedNode);
			};
			nodeView.DropEvent += delegate(Node draggedNode, Node targetNode)
			{
				Action<Node, Node> action = this.onNodeDrop;
				if (action == null)
				{
					return;
				}
				action(draggedNode, targetNode);
			};
			nodeView.DragEndEvent += delegate(Node draggedNode)
			{
				Action<Node> action = this.onNodeDragEnd;
				if (action == null)
				{
					return;
				}
				action(draggedNode);
			};
			nodeView.Refresh();
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00076F40 File Offset: 0x00075140
		private void SetNodeStyle(NodeStyle style, Node node, NodeView view)
		{
			view.fadedAlpha = style.fadeAlpha;
			view.toggleIcons = style.toggleIcons;
			view.imageIcons = style.imageIcons;
			view.checkboxIcons = style.checkboxIcons;
			if (node.isSelected)
			{
				StateStyle selectedState = style.selectedState;
				view.backgroundStyle = selectedState.background;
				view.textStyle = (selectedState.overrideFont ? selectedState.textStyle : style.textStyle);
				return;
			}
			if (node.isSubSelected)
			{
				StateStyle subSelectedState = style.subSelectedState;
				view.backgroundStyle = subSelectedState.background;
				view.textStyle = (subSelectedState.overrideFont ? subSelectedState.textStyle : style.textStyle);
				return;
			}
			view.backgroundStyle = style.background;
			view.textStyle = style.textStyle;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00077008 File Offset: 0x00075208
		public int GetNextId()
		{
			int result = this.lastNodeId + 1;
			this.lastNodeId = result;
			return result;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0000A3F8 File Offset: 0x000085F8
		public Node FindNodeByIdRecursive(int id)
		{
			return this.root.FindNodeByIdRecursive(id, this.root);
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00077028 File Offset: 0x00075228
		public Node[] FindNodesByNameRecursive(string searchName)
		{
			List<Node> list = new List<Node>();
			this.root.FindNodesByNameRecursive(searchName, list);
			return list.ToArray();
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x0000A40C File Offset: 0x0000860C
		public void FocusOnSelected()
		{
			if (!this.hasSelected)
			{
				return;
			}
			this.FocusOn(this._selectedNode);
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00077050 File Offset: 0x00075250
		public void FocusOn(Node node)
		{
			if (node == null || node.tree == null)
			{
				return;
			}
			if (node.tree != this)
			{
				return;
			}
			Node[] allParentsRecursive = node.GetAllParentsRecursive();
			for (int i = 0; i < allParentsRecursive.Length; i++)
			{
				allParentsRecursive[i].ExpandWithoutNotify();
			}
			this.Rebuild();
			this._focusedNodeIndexInExpandedNodesList = this.GetIndexInExpandedNodesList(node);
			this._isAwaitReloadForFocusOn = true;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0000A423 File Offset: 0x00008623
		public Node FindFirstNodeByDataRecursive(object searchedData)
		{
			return this.rootNode.FindNodeByDataRecursive(searchedData);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x000770B8 File Offset: 0x000752B8
		private int GetIndexInExpandedNodesList(Node node)
		{
			for (int i = 0; i < this.expandedNodes.Count; i++)
			{
				if (node == this.expandedNodes[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0000A431 File Offset: 0x00008631
		private void SelectAndNotify(Node node)
		{
			this._selectedNode = node;
			this._selectedNode.SetSelectedWithoutNotify(true);
			if (this.highlightSubSelected)
			{
				node.ChangeIsSubSelectedStateForAllChildren(true);
			}
			NodeEvent nodeEvent = this.onNodeSelected;
			if (nodeEvent == null)
			{
				return;
			}
			nodeEvent.Invoke(node);
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0000A466 File Offset: 0x00008666
		private void DeselectAndNotify(Node node)
		{
			this._selectedNode = null;
			node.SetSelectedWithoutNotify(false);
			if (this.highlightSubSelected)
			{
				node.ChangeIsSubSelectedStateForAllChildren(false);
			}
			NodeEvent nodeEvent = this.onNodeDeselected;
			if (nodeEvent == null)
			{
				return;
			}
			nodeEvent.Invoke(node);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0000A496 File Offset: 0x00008696
		private void OnNodeExpandClicked(Node node)
		{
			if (!node.hasChildren)
			{
				return;
			}
			node.SetExpandedStateWithoutNotify(!node.isExpanded);
			this.Rebuild();
			NodeEvent nodeEvent = this.onNodeExpandStateChanged;
			if (nodeEvent == null)
			{
				return;
			}
			nodeEvent.Invoke(node);
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x000770F0 File Offset: 0x000752F0
		private void NodeCheckedClicked(Node node)
		{
			bool flag = !node.isChecked;
			node.SetCheckedWithoutNotify(flag);
			if (this.isRecursiveChecked)
			{
				node.ChangeIsCheckedStateForAllChildren(flag);
			}
			this.UpdateNodeCheckedState(node);
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0000A4C7 File Offset: 0x000086C7
		private void OnNodeDoubleClick(Node node)
		{
			NodeEvent nodeEvent = this.onNodeDblClick;
			if (nodeEvent == null)
			{
				return;
			}
			nodeEvent.Invoke(node);
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0000A4DA File Offset: 0x000086DA
		protected override RecycleItem CreateItem()
		{
			NodeView nodeView = UnityEngine.Object.Instantiate<NodeView>(Resources.Load<NodeView>("UINodeView_template"), base.content, false);
			nodeView.treePrefs = this.GetTreePrefs();
			return nodeView;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0000A4FE File Offset: 0x000086FE
		protected override void OnPoolIncrease(RecycleItem item)
		{
			((NodeView)item).NodeWidthReadyEvent += this.UpdateContentWidth;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0000A517 File Offset: 0x00008717
		protected override void BeforePoolDecrease(RecycleItem item)
		{
			((NodeView)item).NodeWidthReadyEvent -= this.UpdateContentWidth;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0000A530 File Offset: 0x00008730
		protected override void BeforeReload()
		{
			this._maxItemWidth = 0f;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00077124 File Offset: 0x00075324
		protected override void AfterReload()
		{
			if (this._isAwaitReloadForFocusOn)
			{
				base.SetTopmostIndexForFocusedNode(this._focusedNodeIndexInExpandedNodesList);
				base.UpdateVerticalScrollbar(default(Vector2));
				this._isAwaitReloadForFocusOn = false;
			}
			if (!this.fullRowNodes)
			{
				return;
			}
			float width = base.viewport.rect.width;
			this._maxItemWidth = width;
			this.SetContentWidth(this._maxItemWidth);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0000A53D File Offset: 0x0000873D
		private void UpdateContentWidth(float itemWidth)
		{
			if (itemWidth < this._maxItemWidth)
			{
				return;
			}
			this._maxItemWidth = itemWidth;
			this.SetContentWidth(this._maxItemWidth + (float)base.contentPadding.left + (float)base.contentPadding.right);
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0000A576 File Offset: 0x00008776
		private void SetContentWidth(float width)
		{
			base.content.sizeDelta = new Vector2(width, base.content.sizeDelta.y);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0000A599 File Offset: 0x00008799
		protected override void Awake()
		{
			if (this.root != null)
			{
				return;
			}
			this.lastNodeId = -1;
			this.root = new Node
			{
				tree = this,
				isExpanded = true
			};
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0007718C File Offset: 0x0007538C
		public void AddNodeNameReference(Node node)
		{
			if (this.nameToSuffixTree == null)
			{
				Debug.LogError("suffixtree map not found. initializing");
				this.nameToSuffixTree = new Dictionary<string, SuffixTree>();
			}
			string text = node.name.ToLower();
			SuffixTree suffixTree;
			if (!this.nameToSuffixTree.TryGetValue(text, out suffixTree))
			{
				suffixTree = new SuffixTree();
				suffixTree.AddString(text);
				this.nameToSuffixTree[text] = suffixTree;
			}
			node.SuffixTree = suffixTree;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0000A5C4 File Offset: 0x000087C4
		protected override void OnEnable()
		{
			base.OnEnable();
			base.recycleDataSource = this;
			this.Rebuild();
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x000771F4 File Offset: 0x000753F4
		private TreePrefs GetTreePrefs()
		{
			return new TreePrefs
			{
				fullRectSelect = this.fullRowNodes,
				childIndent = this.childIndent,
				toggleWidth = this.toggleWidth,
				toggleIconSize = this.toggleIconSize,
				iconEnabled = this.imageEnabled,
				iconWidth = this.imageWidth,
				iconSize = this.imageIconSize,
				checkboxEnabled = this.checkboxEnabled,
				checkedWidth = this.checkboxWidth,
				checkedIconSize = this.checkboxIconSize,
				leftPadding = this.leftPadding,
				rightPadding = this.rightPadding,
				spacing = this.contentSpacing
			};
		}

		// Token: 0x04000AEE RID: 2798
		private const string ITEM_RESOURCE_NAME = "UINodeView_template";

		// Token: 0x04000AEF RID: 2799
		public NodeEvent onNodeSelected = new NodeEvent();

		// Token: 0x04000AF0 RID: 2800
		public NodeEvent onNodeDeselected = new NodeEvent();

		// Token: 0x04000AF1 RID: 2801
		public NodeEvent onNodeCheckedChanged = new NodeEvent();

		// Token: 0x04000AF2 RID: 2802
		public NodeEvent onNodeDblClick = new NodeEvent();

		// Token: 0x04000AF3 RID: 2803
		public NodeEvent onNodeExpandStateChanged = new NodeEvent();

		// Token: 0x04000AF4 RID: 2804
		public NodeEvent onSelectionChanged = new NodeEvent();

		// Token: 0x04000AF8 RID: 2808
		public Dictionary<string, SuffixTree> nameToSuffixTree;

		// Token: 0x04000AF9 RID: 2809
		[SerializeField]
		public bool searchable;

		// Token: 0x04000AFA RID: 2810
		[SerializeField]
		private bool fullRowNodes;

		// Token: 0x04000AFB RID: 2811
		[SerializeField]
		private bool highlightSubSelected;

		// Token: 0x04000AFC RID: 2812
		[SerializeField]
		private float childIndent = 55f;

		// Token: 0x04000AFD RID: 2813
		[SerializeField]
		private float leftPadding = 1f;

		// Token: 0x04000AFE RID: 2814
		[SerializeField]
		private float rightPadding = 1f;

		// Token: 0x04000AFF RID: 2815
		[SerializeField]
		private float contentSpacing = 1f;

		// Token: 0x04000B00 RID: 2816
		[SerializeField]
		private float toggleWidth = 40f;

		// Token: 0x04000B01 RID: 2817
		[SerializeField]
		private Vector2 toggleIconSize = new Vector2(30f, 30f);

		// Token: 0x04000B02 RID: 2818
		[SerializeField]
		private bool imageEnabled = true;

		// Token: 0x04000B03 RID: 2819
		[SerializeField]
		private float imageWidth = 40f;

		// Token: 0x04000B04 RID: 2820
		[SerializeField]
		private Vector2 imageIconSize = new Vector2(35f, 35f);

		// Token: 0x04000B05 RID: 2821
		[SerializeField]
		private bool checkboxEnabled;

		// Token: 0x04000B06 RID: 2822
		[SerializeField]
		private bool recursiveChecked;

		// Token: 0x04000B07 RID: 2823
		[SerializeField]
		private float checkboxWidth = 40f;

		// Token: 0x04000B08 RID: 2824
		[SerializeField]
		private Vector2 checkboxIconSize = new Vector2(35f, 35f);

		// Token: 0x04000B09 RID: 2825
		[SerializeReference]
		private Node root;

		// Token: 0x04000B0A RID: 2826
		[SerializeField]
		private string pathSeparator = "/";

		// Token: 0x04000B0B RID: 2827
		[SerializeField]
		private List<Node> expandedNodes;

		// Token: 0x04000B0C RID: 2828
		[SerializeField]
		private NodeStyle[] nodeStylesArray;

		// Token: 0x04000B0D RID: 2829
		[SerializeField]
		private int lastNodeId;

		// Token: 0x04000B0E RID: 2830
		private Node _selectedNode;

		// Token: 0x04000B0F RID: 2831
		private float _maxItemWidth;

		// Token: 0x04000B10 RID: 2832
		private bool _isAwaitReloadForFocusOn;

		// Token: 0x04000B11 RID: 2833
		private int _focusedNodeIndexInExpandedNodesList;

		// Token: 0x04000B12 RID: 2834
		public RectTransform parentWindowPanel;
	}
}
