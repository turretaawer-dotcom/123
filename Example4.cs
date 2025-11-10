using System;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001ED RID: 493
	public class Example4 : MonoBehaviour
	{
		// Token: 0x06000D08 RID: 3336 RVA: 0x00009491 File Offset: 0x00007691
		private void Start()
		{
			this.CreateNodes();
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00073AC4 File Offset: 0x00071CC4
		private void CreateNodes()
		{
			Node node = new Node("Animations", this._example3StyleIndex);
			Node node2 = node.nodes.AddFluent(new Node("Character", this._example3StyleIndex));
			Node node3 = new Node("Mobs", this._example3StyleIndex);
			node3 = node.nodes.AddFluent(node3);
			node.nodes.AddFluent("Enemies", this._example3StyleIndex);
			node3.nodes.AddFluent("Wolf", this._example3StyleIndex).nodes.AddFluent("idle", this._example3StyleIndex).isFaded = true;
			Node[] nodeArray = new Node[]
			{
				new Node("Male", this._example3StyleIndex),
				new Node("Female", this._example3StyleIndex),
				new Node("Child", this._example3StyleIndex)
			};
			node2.nodes.AddRange(nodeArray);
			this.treeView.nodes.Add(node);
		}

		// Token: 0x04000A40 RID: 2624
		[SerializeField]
		private UIRecycleTree treeView;

		// Token: 0x04000A41 RID: 2625
		private int _example3StyleIndex = 3;
	}
}
