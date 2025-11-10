using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001FD RID: 509
	[Serializable]
	public class Background
	{
		// Token: 0x04000AC4 RID: 2756
		public Icon backgroundImage = new Icon
		{
			color = Color.clear
		};

		// Token: 0x04000AC5 RID: 2757
		public Image.Type imageType;

		// Token: 0x04000AC6 RID: 2758
		public float pixelPerUnitMultiplier = 1f;
	}
}
