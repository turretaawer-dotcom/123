using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001F6 RID: 502
	public class CheckboxControl : Selectable, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000DCB RID: 3531 RVA: 0x00075EC0 File Offset: 0x000740C0
		// (remove) Token: 0x06000DCC RID: 3532 RVA: 0x00075EF8 File Offset: 0x000740F8
		public event Action ClickedEvent;

		// Token: 0x17000178 RID: 376
		// (set) Token: 0x06000DCD RID: 3533 RVA: 0x00075F30 File Offset: 0x00074130
		public bool isChecked
		{
			set
			{
				this.targetImage.sprite = (value ? this._checked.sprite : this._unchecked.sprite);
				this.targetImage.color = (value ? this._checked.color : this._unchecked.color);
			}
		}

		// Token: 0x17000179 RID: 377
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x00009D7C File Offset: 0x00007F7C
		public CheckboxIcons icons
		{
			set
			{
				this._checked = value.checkedState;
				this._unchecked = value.uncheckedState;
			}
		}

		// Token: 0x1700017A RID: 378
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x00009D96 File Offset: 0x00007F96
		public float width
		{
			set
			{
				this._rectTransform.sizeDelta = new Vector2(value, this._rectTransform.sizeDelta.y);
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00009DB9 File Offset: 0x00007FB9
		// (set) Token: 0x06000DD1 RID: 3537 RVA: 0x00009DCB File Offset: 0x00007FCB
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

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00009DDE File Offset: 0x00007FDE
		// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x00009DEB File Offset: 0x00007FEB
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

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00009E08 File Offset: 0x00008008
		public void OnPointerClick(PointerEventData eventData)
		{
			Action clickedEvent = this.ClickedEvent;
			if (clickedEvent == null)
			{
				return;
			}
			clickedEvent();
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00009E1A File Offset: 0x0000801A
		protected override void Awake()
		{
			base.Awake();
			this._rectTransform = (RectTransform)base.transform;
		}

		// Token: 0x04000A8C RID: 2700
		[SerializeField]
		private Image targetImage;

		// Token: 0x04000A8D RID: 2701
		private Icon _checked;

		// Token: 0x04000A8E RID: 2702
		private Icon _unchecked;

		// Token: 0x04000A8F RID: 2703
		private RectTransform _rectTransform;
	}
}
