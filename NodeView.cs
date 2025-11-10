using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001F9 RID: 505
	public class NodeView : RecycleItem, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000DDA RID: 3546 RVA: 0x00075FBC File Offset: 0x000741BC
		// (remove) Token: 0x06000DDB RID: 3547 RVA: 0x00075FF4 File Offset: 0x000741F4
		public event Action ClickedEvent;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000DDC RID: 3548 RVA: 0x0007602C File Offset: 0x0007422C
		// (remove) Token: 0x06000DDD RID: 3549 RVA: 0x00076064 File Offset: 0x00074264
		public event Action DoubleClickedEvent;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000DDE RID: 3550 RVA: 0x0007609C File Offset: 0x0007429C
		// (remove) Token: 0x06000DDF RID: 3551 RVA: 0x000760D4 File Offset: 0x000742D4
		public event Action CheckboxClickedEvent;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000DE0 RID: 3552 RVA: 0x0007610C File Offset: 0x0007430C
		// (remove) Token: 0x06000DE1 RID: 3553 RVA: 0x00076144 File Offset: 0x00074344
		public event Action ExpandClickEvent;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000DE2 RID: 3554 RVA: 0x0007617C File Offset: 0x0007437C
		// (remove) Token: 0x06000DE3 RID: 3555 RVA: 0x000761B4 File Offset: 0x000743B4
		public event Action<float> NodeWidthReadyEvent;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000DE4 RID: 3556 RVA: 0x000761EC File Offset: 0x000743EC
		// (remove) Token: 0x06000DE5 RID: 3557 RVA: 0x00076224 File Offset: 0x00074424
		public event Action<Node> DragStartEvent;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000DE6 RID: 3558 RVA: 0x0007625C File Offset: 0x0007445C
		// (remove) Token: 0x06000DE7 RID: 3559 RVA: 0x00076294 File Offset: 0x00074494
		public event Action<Node, Node> DropEvent;

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000DE8 RID: 3560 RVA: 0x000762CC File Offset: 0x000744CC
		// (remove) Token: 0x06000DE9 RID: 3561 RVA: 0x00076304 File Offset: 0x00074504
		public event Action<Node> DragEndEvent;

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00009E4E File Offset: 0x0000804E
		public override RectTransform rectTransform
		{
			get
			{
				return (RectTransform)base.transform;
			}
		}

		// Token: 0x1700017F RID: 383
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x0007633C File Offset: 0x0007453C
		public TreePrefs treePrefs
		{
			set
			{
				this._fullRectSelect = value.fullRectSelect;
				this._componentSpacing = value.spacing;
				this._childIndentPixels = value.childIndent;
				this._leftPadding = value.leftPadding;
				this._rightPadding = value.rightPadding;
				this.expandToggleControl.width = value.toggleWidth;
				this.expandToggleControl.iconSize = value.toggleIconSize;
				bool checkboxEnabled = value.checkboxEnabled;
				this.checkboxControl.isActive = checkboxEnabled;
				if (checkboxEnabled)
				{
					this.checkboxControl.width = value.checkedWidth;
					this.checkboxControl.iconSize = value.checkedIconSize;
				}
				bool iconEnabled = value.iconEnabled;
				this.imageControl.isActive = iconEnabled;
				if (iconEnabled)
				{
					this.imageControl.width = value.iconWidth;
					this.imageControl.iconSize = value.iconSize;
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (set) Token: 0x06000DEC RID: 3564 RVA: 0x00009E5B File Offset: 0x0000805B
		public float fadedAlpha
		{
			set
			{
				this._fadedAlpha = value;
			}
		}

		// Token: 0x17000181 RID: 385
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x00009E64 File Offset: 0x00008064
		public ExpandIcons toggleIcons
		{
			set
			{
				this.expandToggleControl.icons = value;
			}
		}

		// Token: 0x17000182 RID: 386
		// (set) Token: 0x06000DEE RID: 3566 RVA: 0x00009E72 File Offset: 0x00008072
		public ExpandIcons imageIcons
		{
			set
			{
				this.imageControl.icons = value;
			}
		}

		// Token: 0x17000183 RID: 387
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x00009E80 File Offset: 0x00008080
		public CheckboxIcons checkboxIcons
		{
			set
			{
				this.checkboxControl.icons = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (set) Token: 0x06000DF0 RID: 3568 RVA: 0x00009E8E File Offset: 0x0000808E
		public NodeTextStyle textStyle
		{
			set
			{
				this.textControl.style = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (set) Token: 0x06000DF1 RID: 3569 RVA: 0x00076418 File Offset: 0x00074618
		public Background backgroundStyle
		{
			set
			{
				this.imageForSelect.sprite = value.backgroundImage.sprite;
				this.imageForSelect.type = value.imageType;
				this.imageForSelect.pixelsPerUnitMultiplier = value.pixelPerUnitMultiplier;
				this.imageForSelect.color = value.backgroundImage.color;
			}
		}

		// Token: 0x17000186 RID: 390
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x00009E9C File Offset: 0x0000809C
		public float indent
		{
			set
			{
				this.indentBox.indent = value * this._childIndentPixels;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00009EB1 File Offset: 0x000080B1
		// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x00009EBE File Offset: 0x000080BE
		public string text
		{
			get
			{
				return this.textControl.text;
			}
			set
			{
				this.textControl.text = value;
			}
		}

		// Token: 0x17000188 RID: 392
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x00009ECC File Offset: 0x000080CC
		public ExpandedState state
		{
			set
			{
				this.expandToggleControl.state = value;
				this.imageControl.state = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (set) Token: 0x06000DF6 RID: 3574 RVA: 0x00009EE6 File Offset: 0x000080E6
		public bool isChecked
		{
			set
			{
				this.checkboxControl.isChecked = value;
			}
		}

		// Token: 0x1700018A RID: 394
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x00009EF4 File Offset: 0x000080F4
		public bool isFaded
		{
			set
			{
				this.contentCanvasGroup.alpha = (value ? this._fadedAlpha : 1f);
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x00076474 File Offset: 0x00074674
		private bool isDoubleClick
		{
			get
			{
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				float num = realtimeSinceStartup - this._lastClickTime;
				if (num < 0.4f && num > 0.07f)
				{
					return true;
				}
				this._lastClickTime = realtimeSinceStartup;
				return false;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x00009F11 File Offset: 0x00008111
		private Image imageForSelect
		{
			get
			{
				if (!this._fullRectSelect)
				{
					return this.contentSelectionImage;
				}
				return this.fullRectSelectionImage;
			}
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00009F28 File Offset: 0x00008128
		private IEnumerator ArrangeContent()
		{
			yield return null;
			float num = this._leftPadding;
			foreach (object obj in this.content)
			{
				RectTransform rectTransform = (RectTransform)obj;
				if (rectTransform.gameObject.activeInHierarchy)
				{
					rectTransform.anchoredPosition = new Vector2(num, rectTransform.anchoredPosition.y);
					num += rectTransform.sizeDelta.x + this._componentSpacing;
				}
			}
			num += this._rightPadding;
			this.content.sizeDelta = new Vector2(num, this.content.sizeDelta.y);
			num = 0f;
			foreach (object obj2 in this.rectTransform)
			{
				RectTransform rectTransform2 = (RectTransform)obj2;
				rectTransform2.anchoredPosition = new Vector2(num, rectTransform2.anchoredPosition.y);
				num += rectTransform2.sizeDelta.x + this._componentSpacing;
			}
			this.rectTransform.sizeDelta = new Vector2(num, this.rectTransform.sizeDelta.y);
			Action<float> nodeWidthReadyEvent = this.NodeWidthReadyEvent;
			if (nodeWidthReadyEvent != null)
			{
				nodeWidthReadyEvent(num);
			}
			if (!this._initialized)
			{
				this.SetVisible(true);
			}
			this._initialized = true;
			yield break;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00009F37 File Offset: 0x00008137
		public void ClearPreviousSubscribes()
		{
			this.ClickedEvent = null;
			this.DoubleClickedEvent = null;
			this.ExpandClickEvent = null;
			this.CheckboxClickedEvent = null;
			this.DragStartEvent = null;
			this.DropEvent = null;
			this.DragEndEvent = null;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00009F6A File Offset: 0x0000816A
		private void ClickNotify()
		{
			Action clickedEvent = this.ClickedEvent;
			if (clickedEvent == null)
			{
				return;
			}
			clickedEvent();
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x00009F7C File Offset: 0x0000817C
		private void DoubleClickNotify()
		{
			Action doubleClickedEvent = this.DoubleClickedEvent;
			if (doubleClickedEvent == null)
			{
				return;
			}
			doubleClickedEvent();
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00009F8E File Offset: 0x0000818E
		private void OnExpandToggleClick()
		{
			Action expandClickEvent = this.ExpandClickEvent;
			if (expandClickEvent == null)
			{
				return;
			}
			expandClickEvent();
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00009FA0 File Offset: 0x000081A0
		protected void OnEnable()
		{
			this.expandToggleControl.ClickedEvent += this.OnExpandToggleClick;
			this.checkboxControl.ClickedEvent += this.OnCheckedClick;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00009FD0 File Offset: 0x000081D0
		protected void OnDisable()
		{
			this.expandToggleControl.ClickedEvent -= this.OnExpandToggleClick;
			this.checkboxControl.ClickedEvent -= this.OnCheckedClick;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0000A000 File Offset: 0x00008200
		private void OnCheckedClick()
		{
			Action checkboxClickedEvent = this.CheckboxClickedEvent;
			if (checkboxClickedEvent == null)
			{
				return;
			}
			checkboxClickedEvent();
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000764AC File Offset: 0x000746AC
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (this._canvas == null || this._tree == null || this._tree.parentWindowPanel == null)
			{
				return;
			}
			this._isDragging = true;
			this._originalPosition = this.rectTransform.anchoredPosition;
			Vector2 b;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._tree.parentWindowPanel, eventData.position, eventData.pressEventCamera, out b);
			this._dragOffset = this.rectTransform.anchoredPosition - b;
			MaskableGraphic[] maskableGraphics = this._maskableGraphics;
			for (int i = 0; i < maskableGraphics.Length; i++)
			{
				maskableGraphics[i].maskable = false;
			}
			this._originalColor = this.imageForSelect.color;
			this.imageForSelect.color = new Color(this._originalColor.r, this._originalColor.g, this._originalColor.b, 0.5f);
			this.itemCanvasGroup.blocksRaycasts = false;
			Action<Node> dragStartEvent = this.DragStartEvent;
			if (dragStartEvent == null)
			{
				return;
			}
			dragStartEvent(this.node);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x000765C0 File Offset: 0x000747C0
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._isDragging || this._canvas == null || this._tree == null || this._tree.parentWindowPanel == null)
			{
				return;
			}
			Vector2 a;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._tree.parentWindowPanel, eventData.position, eventData.pressEventCamera, out a);
			this.rectTransform.anchoredPosition = a + this._dragOffset;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0007663C File Offset: 0x0007483C
		public void OnEndDrag(PointerEventData eventData)
		{
			if (!this._isDragging)
			{
				return;
			}
			this._isDragging = false;
			this.imageForSelect.color = this._originalColor;
			this.itemCanvasGroup.blocksRaycasts = true;
			MaskableGraphic[] maskableGraphics = this._maskableGraphics;
			for (int i = 0; i < maskableGraphics.Length; i++)
			{
				maskableGraphics[i].maskable = true;
			}
			NodeView nodeView = null;
			if (eventData.pointerCurrentRaycast.gameObject != null)
			{
				nodeView = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<NodeView>();
			}
			Action<Node, Node> dropEvent = this.DropEvent;
			if (dropEvent != null)
			{
				dropEvent(this.node, (nodeView != null) ? nodeView.node : null);
			}
			Action<Node> dragEndEvent = this.DragEndEvent;
			if (dragEndEvent != null)
			{
				dragEndEvent(this.node);
			}
			this.rectTransform.anchoredPosition = this._originalPosition;
			this.Refresh();
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0000A012 File Offset: 0x00008212
		private void OnDestroy()
		{
			this.ClearPreviousSubscribes();
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0000A01A File Offset: 0x0000821A
		public void Refresh()
		{
			base.StartCoroutine(this.ArrangeContent());
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0000A029 File Offset: 0x00008229
		public void Start()
		{
			this._canvas = base.GetComponentInParent<Canvas>();
			this._tree = base.GetComponentInParent<UIRecycleTree>();
			this._maskableGraphics = base.GetComponentsInChildren<MaskableGraphic>();
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0000A04F File Offset: 0x0000824F
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this._isDragging)
			{
				this.ClickNotify();
				if (this.isDoubleClick)
				{
					this.DoubleClickNotify();
				}
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0000A06D File Offset: 0x0000826D
		private void Awake()
		{
			this.SetVisible(false);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x0000A076 File Offset: 0x00008276
		private void SetVisible(bool isVisible)
		{
			this.itemCanvasGroup.alpha = (float)(isVisible ? 1 : 0);
		}

		// Token: 0x04000A95 RID: 2709
		private const float DOUBLE_TAP_MAX_DELAY = 0.4f;

		// Token: 0x04000A96 RID: 2710
		private const float DOUBLE_TAP_MIN_DELAY = 0.07f;

		// Token: 0x04000A9F RID: 2719
		public Node node;

		// Token: 0x04000AA0 RID: 2720
		private MaskableGraphic[] _maskableGraphics;

		// Token: 0x04000AA1 RID: 2721
		[SerializeField]
		private IndentBox indentBox;

		// Token: 0x04000AA2 RID: 2722
		[SerializeField]
		private StateControl expandToggleControl;

		// Token: 0x04000AA3 RID: 2723
		[SerializeField]
		private StateControl imageControl;

		// Token: 0x04000AA4 RID: 2724
		[SerializeField]
		private TextControl textControl;

		// Token: 0x04000AA5 RID: 2725
		[SerializeField]
		private CheckboxControl checkboxControl;

		// Token: 0x04000AA6 RID: 2726
		[SerializeField]
		private RectTransform content;

		// Token: 0x04000AA7 RID: 2727
		[SerializeField]
		private Image fullRectSelectionImage;

		// Token: 0x04000AA8 RID: 2728
		[SerializeField]
		private Image contentSelectionImage;

		// Token: 0x04000AA9 RID: 2729
		[SerializeField]
		private CanvasGroup itemCanvasGroup;

		// Token: 0x04000AAA RID: 2730
		[SerializeField]
		private CanvasGroup contentCanvasGroup;

		// Token: 0x04000AAB RID: 2731
		private float _childIndentPixels;

		// Token: 0x04000AAC RID: 2732
		private float _lastClickTime;

		// Token: 0x04000AAD RID: 2733
		private float _fadedAlpha;

		// Token: 0x04000AAE RID: 2734
		private bool _fullRectSelect;

		// Token: 0x04000AAF RID: 2735
		private bool _initialized;

		// Token: 0x04000AB0 RID: 2736
		private float _leftPadding;

		// Token: 0x04000AB1 RID: 2737
		private float _rightPadding;

		// Token: 0x04000AB2 RID: 2738
		private float _componentSpacing;

		// Token: 0x04000AB3 RID: 2739
		private bool _isDragging;

		// Token: 0x04000AB4 RID: 2740
		private Vector2 _originalPosition;

		// Token: 0x04000AB5 RID: 2741
		private Vector2 _dragOffset;

		// Token: 0x04000AB6 RID: 2742
		private Canvas _canvas;

		// Token: 0x04000AB7 RID: 2743
		private UIRecycleTree _tree;

		// Token: 0x04000AB8 RID: 2744
		private Color _originalColor;
	}
}
