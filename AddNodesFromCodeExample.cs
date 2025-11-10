using System;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001EC RID: 492
	public class AddNodesFromCodeExample : MonoBehaviour
	{
		// Token: 0x06000D05 RID: 3333 RVA: 0x00009489 File Offset: 0x00007689
		private void Start()
		{
			this.CreateNodes();
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x000739F4 File Offset: 0x00071BF4
		private void CreateNodes()
		{
			Node node = new Node("Animations", 0);
			Node node2 = node.nodes.AddFluent(new Node("Character", 0));
			Node node3 = new Node("Mobs", 0);
			node3 = node.nodes.AddFluent(node3);
			node.nodes.AddFluent("Enemies");
			node3.nodes.AddFluent("Wolf").nodes.AddFluent("idle").isFaded = true;
			Node[] nodeArray = new Node[]
			{
				new Node("Male", 0),
				new Node("Female", 0),
				new Node("Child", 0)
			};
			node2.nodes.AddRange(nodeArray);
			this.treeView.nodes.Add(node);
		}

		// Token: 0x04000A3F RID: 2623
		[SerializeField]
		private UIRecycleTree treeView;
	}
}
