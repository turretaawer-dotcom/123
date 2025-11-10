using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001FB RID: 507
	public class StateControl : Selectable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000E12 RID: 3602 RVA: 0x000768D4 File Offset: 0x00074AD4
		// (remove) Token: 0x06000E13 RID: 3603 RVA: 0x0007690C File Offset: 0x00074B0C
		public event Action ClickedEvent;

		// Token: 0x1700018F RID: 399
		// (set) Token: 0x06000E14 RID: 3604 RVA: 0x0000A0AA File Offset: 0x000082AA
		public ExpandIcons icons
		{
			set
			{
				this._noChild = value.noChildren;
				this._expanded = value.expanded;
				this._collapsed = value.collapsed;
			}
		}

		// Token: 0x17000190 RID: 400
		// (set) Token: 0x06000E15 RID: 3605 RVA: 0x0000A0D0 File Offset: 0x000082D0
		public float width
		{
			set
			{
				this._rectTransform.sizeDelta = new Vector2(value, this._rectTransform.sizeDelta.y);
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0000A0F3 File Offset: 0x000082F3
		// (set) Token: 0x06000E17 RID: 3607 RVA: 0x0000A105 File Offset: 0x00008305
		public Vector2 iconSize
		{
			get
			{
				return this.targetImage.rectTransform.sizeDelta;
			}
			set
			{
				this.targetImage.rectTransform.sizeDelta = value;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00009DDE File Offset: 0x00007FDE
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x00009DEB File Offset: 0x00007FEB
		public bool isActive
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
			set
			{
				if (base.gameObject.activeInHierarchy == value)
				{
					return;
				}
				base.gameObject.SetActive(value);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0000A118 File Offset: 0x00008318
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x0000A120 File Offset: 0x00008320
		public ExpandedState state
		{
			get
			{
				return this._currentState;
			}
			set
			{
				this._currentState = value;
				this.Refresh();
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0000A12F File Offset: 0x0000832F
		public void OnPointerClick(PointerEventData eventData)
		{
			Action clickedEvent = this.ClickedEvent;
			if (clickedEvent == null)
			{
				return;
			}
			clickedEvent();
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00076944 File Offset: 0x00074B44
		private void Refresh()
		{
			switch (this.state)
			{
			case ExpandedState.Expanded:
				this.targetImage.sprite = this._expanded.sprite;
				this.targetImage.color = this._expanded.color;
				return;
			case ExpandedState.Collapsed:
				this.targetImage.sprite = this._collapsed.sprite;
				this.targetImage.color = this._collapsed.color;
				return;
			case ExpandedState.NoChild:
				this.targetImage.sprite = this._noChild.sprite;
				this.targetImage.color = this._noChild.color;
				return;
			default:
				throw new Exception(string.Format("State {0} not implemented", this.state));
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0000A141 File Offset: 0x00008341
		protected override void Awake()
		{
			base.Awake();
			this._rectTransform = (RectTransform)base.transform;
		}

		// Token: 0x04000ABD RID: 2749
		[SerializeField]
		private Image targetImage;

		// Token: 0x04000ABE RID: 2750
		private Icon _noChild;

		// Token: 0x04000ABF RID: 2751
		private Icon _expanded;

		// Token: 0x04000AC0 RID: 2752
		private Icon _collapsed;

		// Token: 0x04000AC1 RID: 2753
		private RectTransform _rectTransform;

		// Token: 0x04000AC2 RID: 2754
		private ExpandedState _currentState;
	}
}
