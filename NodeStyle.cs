using System;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x02000201 RID: 513
	[CreateAssetMenu(menuName = "UIRecycleTree/NodeStyle", fileName = "NodeStyle", order = 0)]
	public class NodeStyle : ScriptableObject
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x0000A1B1 File Offset: 0x000083B1
		public NodeTextStyle textStyle
		{
			get
			{
				return this._nodeTextStyle;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x0000A1B9 File Offset: 0x000083B9
		public Background background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x0000A1C1 File Offset: 0x000083C1
		public StateStyle selectedState
		{
			get
			{
				return this._selectedState;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x0000A1C9 File Offset: 0x000083C9
		public StateStyle subSelectedState
		{
			get
			{
				return this._subSelectedState;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x0000A1D1 File Offset: 0x000083D1
		public ExpandIcons toggleIcons
		{
			get
			{
				return this._toggleIcons;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0000A1D9 File Offset: 0x000083D9
		public ExpandIcons imageIcons
		{
			get
			{
				return this._imageIcons;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0000A1E1 File Offset: 0x000083E1
		public CheckboxIcons checkboxIcons
		{
			get
			{
				return this._checkboxIcons;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x0000A1E9 File Offset: 0x000083E9
		public float fadeAlpha
		{
			get
			{
				return this._fadedAlpha;
			}
		}

		// Token: 0x04000ACE RID: 2766
		[SerializeField]
		private ExpandIcons _toggleIcons;

		// Token: 0x04000ACF RID: 2767
		[SerializeField]
		private ExpandIcons _imageIcons;

		// Token: 0x04000AD0 RID: 2768
		[SerializeField]
		private CheckboxIcons _checkboxIcons;

		// Token: 0x04000AD1 RID: 2769
		[SerializeField]
		private Background _background;

		// Token: 0x04000AD2 RID: 2770
		[SerializeField]
		private NodeTextStyle _nodeTextStyle;

		// Token: 0x04000AD3 RID: 2771
		[SerializeField]
		private StateStyle _selectedState;

		// Token: 0x04000AD4 RID: 2772
		[SerializeField]
		private StateStyle _subSelectedState;

		// Token: 0x04000AD5 RID: 2773
		[SerializeField]
		private float _fadedAlpha = 0.3f;
	}
}
