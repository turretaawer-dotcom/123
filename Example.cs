using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001EA RID: 490
	public class Example : MonoBehaviour
	{
		// Token: 0x06000CF0 RID: 3312 RVA: 0x00073674 File Offset: 0x00071874
		private void Start()
		{
			Application.targetFrameRate = 100;
			this.FillTree();
			this.button.onClick.AddListener(new UnityAction(this.FillTree));
			this.expand.onClick.AddListener(new UnityAction(this.Expand));
			this.collapse.onClick.AddListener(new UnityAction(this.Collapse));
			this.delete.onClick.AddListener(new UnityAction(this.DeleteSelected));
			this.fadeButton.onClick.AddListener(new UnityAction(this.FadeSelected));
			this.focusButton.onClick.AddListener(new UnityAction(this.FocusSelected));
			this.treeView.onNodeSelected.AddListener(new UnityAction<Node>(this.OnNodeSelect));
			this.treeView.onNodeDblClick.AddListener(new UnityAction<Node>(this.DoubleClicked));
			this.treeView.onSelectionChanged.AddListener(new UnityAction<Node>(this.SelectionChanged));
			this.treeView.onNodeExpandStateChanged.AddListener(new UnityAction<Node>(this.OnExpand));
			this.treeView.onNodeCheckedChanged.AddListener(new UnityAction<Node>(this.OnChecked));
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0000933B File Offset: 0x0000753B
		private void FocusSelected()
		{
			this.treeView.FocusOnSelected();
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00009348 File Offset: 0x00007548
		private void OnChecked(Node node)
		{
			Debug.Log(string.Format("{0} isChecked =  {1}", node.name, node.isChecked));
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0000936A File Offset: 0x0000756A
		private void SelectionChanged(Node node)
		{
			Debug.Log(node.name + " Selection Changed");
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00009381 File Offset: 0x00007581
		private void OnExpand(Node node)
		{
			Debug.Log(string.Format("{0} isExpanded =  {1}", node.name, node.isExpanded));
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x000093A3 File Offset: 0x000075A3
		private void FadeSelected()
		{
			if (this.treeView.hasSelected)
			{
				this.treeView.selectedNode.isFaded = !this.treeView.selectedNode.isFaded;
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000093D5 File Offset: 0x000075D5
		private void DoubleClicked(Node node)
		{
			Debug.Log(node.name + " double clicked");
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000093EC File Offset: 0x000075EC
		private void OnNodeSelect(Node node)
		{
			this.DrawPath(node);
			Debug.Log(node.name + " is selected");
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0000940A File Offset: 0x0000760A
		private void DrawPath(Node node)
		{
			this.pathTextField.text = node.fullPath;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000737C4 File Offset: 0x000719C4
		private void DeleteSelected()
		{
			if (!this.treeView.hasSelected)
			{
				return;
			}
			Node selectedNode = this.treeView.selectedNode;
			string name = selectedNode.name;
			selectedNode.RemoveYourself();
			Debug.Log(name + " node deleted");
			this.nodesCountText.text = this.treeView.nodesCount.ToString();
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0000941D File Offset: 0x0000761D
		private void Collapse()
		{
			this.treeView.CollapseAll();
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0000942A File Offset: 0x0000762A
		private void Expand()
		{
			this.treeView.ExpandAll();
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00073824 File Offset: 0x00071A24
		private void FillTree()
		{
			if (this.treeView.nodesCount != 0)
			{
				this.treeView.Clear();
			}
			this.GenerateRandomTreeContent(this.treeView.rootNode, 12, 5);
			this.nodesCountText.text = this.treeView.nodesCount.ToString();
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0007387C File Offset: 0x00071A7C
		private void GenerateRandomTreeContent(Node node, int maxChildCount, int maxDepth)
		{
			if (maxDepth <= 0)
			{
				return;
			}
			int num = UnityEngine.Random.Range(1, maxChildCount);
			for (int i = 0; i <= num; i++)
			{
				Node node2 = node.nodes.AddFluent(new Node());
				node2.name = string.Format("id{0}[depth{1}]", node2.nodeId, node.depth + 1);
				this.GenerateRandomTreeContent(node2, maxChildCount, maxDepth - 1);
			}
		}

		// Token: 0x04000A31 RID: 2609
		[SerializeField]
		private UIRecycleTree treeView;

		// Token: 0x04000A32 RID: 2610
		[SerializeField]
		protected Button button;

		// Token: 0x04000A33 RID: 2611
		[SerializeField]
		protected Button expand;

		// Token: 0x04000A34 RID: 2612
		[SerializeField]
		protected Button collapse;

		// Token: 0x04000A35 RID: 2613
		[SerializeField]
		protected Button delete;

		// Token: 0x04000A36 RID: 2614
		[SerializeField]
		protected Button fadeButton;

		// Token: 0x04000A37 RID: 2615
		[SerializeField]
		protected Button focusButton;

		// Token: 0x04000A38 RID: 2616
		[SerializeField]
		private TMP_Text pathTextField;

		// Token: 0x04000A39 RID: 2617
		[SerializeField]
		private TMP_Text nodesCountText;
	}
}
