using System;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x0200020E RID: 526
	public static class UIRectTransformExtension
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x00077B98 File Offset: 0x00075D98
		public static Vector3[] GetWorldCorners(this RectTransform rectTransform)
		{
			Vector3[] array = new Vector3[4];
			rectTransform.GetWorldCorners(array);
			return array;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0000A83A File Offset: 0x00008A3A
		public static float MaxY(this RectTransform rectTransform)
		{
			return rectTransform.GetWorldCorners()[1].y;
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0000A84D File Offset: 0x00008A4D
		public static float MinY(this RectTransform rectTransform)
		{
			return rectTransform.GetWorldCorners()[0].y;
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0000A860 File Offset: 0x00008A60
		public static float MaxX(this RectTransform rectTransform)
		{
			return rectTransform.GetWorldCorners()[2].x;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0000A873 File Offset: 0x00008A73
		public static float MinX(this RectTransform rectTransform)
		{
			return rectTransform.GetWorldCorners()[0].x;
		}
	}
}
