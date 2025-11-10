using System;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001F8 RID: 504
	public class IndentBox : MonoBehaviour
	{
		// Token: 0x1700017D RID: 381
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x00075F8C File Offset: 0x0007418C
		public float indent
		{
			set
			{
				RectTransform rectTransform = (RectTransform)base.transform;
				rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
			}
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00009E3B File Offset: 0x0000803B
		private void Awake()
		{
			this._rectTransform = (RectTransform)base.transform;
		}

		// Token: 0x04000A94 RID: 2708
		private RectTransform _rectTransform;
	}
}
