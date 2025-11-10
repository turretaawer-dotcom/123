using System;
using UnityEngine;
using UnityEngine.Events;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001EB RID: 491
	public class Example2 : MonoBehaviour
	{
		// Token: 0x06000CFF RID: 3327 RVA: 0x00009437 File Offset: 0x00007637
		private void Awake()
		{
			this._cam = Camera.main;
			this.FillTreeRecursive(this.treeView.rootNode, this.targetGameObject.transform);
			this.treeView.ExpandAll();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0000946B File Offset: 0x0000766B
		public void OnEnable()
		{
			this.treeView.onNodeCheckedChanged.AddListener(new UnityAction<Node>(this.OnNodeCheckedStateChanged));
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000738E8 File Offset: 0x00071AE8
		private void FillTreeRecursive(Node node, Transform targetTransform)
		{
			foreach (object obj in targetTransform)
			{
				Transform transform = (Transform)obj;
				Node node2 = node.nodes.AddFluent(transform.name);
				node2.data = transform;
				if (targetTransform.childCount > 0)
				{
					this.FillTreeRecursive(node2, transform);
				}
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00073960 File Offset: 0x00071B60
		private void Update()
		{
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			RaycastHit raycastHit;
			if (!Physics.Raycast(this._cam.ScreenPointToRay(Input.mousePosition), out raycastHit))
			{
				return;
			}
			Node node = this.treeView.FindFirstNodeByDataRecursive(raycastHit.transform);
			if (node != null)
			{
				node.isChecked = !node.isChecked;
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000739B8 File Offset: 0x00071BB8
		private void OnNodeCheckedStateChanged(Node node)
		{
			Transform transform;
			if (!node.TryCastData<Transform>(out transform))
			{
				return;
			}
			transform.GetComponent<Renderer>().material = (node.isChecked ? this.selectedMaterial : this.regularMaterial);
		}

		// Token: 0x04000A3A RID: 2618
		[SerializeField]
		private UIRecycleTree treeView;

		// Token: 0x04000A3B RID: 2619
		[SerializeField]
		private GameObject targetGameObject;

		// Token: 0x04000A3C RID: 2620
		[SerializeField]
		private Material selectedMaterial;

		// Token: 0x04000A3D RID: 2621
		[SerializeField]
		private Material regularMaterial;

		// Token: 0x04000A3E RID: 2622
		private Camera _cam;
	}
}
