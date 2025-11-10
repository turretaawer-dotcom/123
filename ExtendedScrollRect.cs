using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001EE RID: 494
	[AddComponentMenu("UI/Scroll Rect", 37)]
	[SelectionBase]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public class ExtendedScrollRect : UIBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler, ICanvasElement, ILayoutElement, ILayoutGroup, ILayoutController
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x000094A8 File Offset: 0x000076A8
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x000094B0 File Offset: 0x000076B0
		public RectTransform content
		{
			get
			{
				return this.m_Content;
			}
			set
			{
				this.m_Content = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x000094B9 File Offset: 0x000076B9
		// (set) Token: 0x06000D0E RID: 3342 RVA: 0x000094C1 File Offset: 0x000076C1
		public bool horizontal
		{
			get
			{
				return this.m_Horizontal;
			}
			set
			{
				this.m_Horizontal = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x000094CA File Offset: 0x000076CA
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x000094D2 File Offset: 0x000076D2
		public bool vertical
		{
			get
			{
				return this.m_Vertical;
			}
			set
			{
				this.m_Vertical = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000094DB File Offset: 0x000076DB
		// (set) Token: 0x06000D12 RID: 3346 RVA: 0x000094E3 File Offset: 0x000076E3
		public ExtendedScrollRect.MovementType movementType
		{
			get
			{
				return this.m_MovementType;
			}
			set
			{
				this.m_MovementType = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x000094EC File Offset: 0x000076EC
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x000094F4 File Offset: 0x000076F4
		public float elasticity
		{
			get
			{
				return this.m_Elasticity;
			}
			set
			{
				this.m_Elasticity = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x000094FD File Offset: 0x000076FD
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x00009505 File Offset: 0x00007705
		public bool inertia
		{
			get
			{
				return this.m_Inertia;
			}
			set
			{
				this.m_Inertia = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0000950E File Offset: 0x0000770E
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x00009516 File Offset: 0x00007716
		public float decelerationRate
		{
			get
			{
				return this.m_DecelerationRate;
			}
			set
			{
				this.m_DecelerationRate = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0000951F File Offset: 0x0000771F
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00009527 File Offset: 0x00007727
		public float scrollSensitivity
		{
			get
			{
				return this.m_ScrollSensitivity;
			}
			set
			{
				this.m_ScrollSensitivity = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00009530 File Offset: 0x00007730
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x00009538 File Offset: 0x00007738
		public RectTransform viewport
		{
			get
			{
				return this.m_Viewport;
			}
			set
			{
				this.m_Viewport = value;
				this.SetDirtyCaching();
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00009547 File Offset: 0x00007747
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x00073BC4 File Offset: 0x00071DC4
		public Scrollbar horizontalScrollbar
		{
			get
			{
				return this.m_HorizontalScrollbar;
			}
			set
			{
				if (this.m_HorizontalScrollbar)
				{
					this.m_HorizontalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
				}
				this.m_HorizontalScrollbar = value;
				if (this.m_HorizontalScrollbar)
				{
					this.m_HorizontalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
				}
				this.SetDirtyCaching();
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x0000954F File Offset: 0x0000774F
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x00073C30 File Offset: 0x00071E30
		public Scrollbar verticalScrollbar
		{
			get
			{
				return this.m_VerticalScrollbar;
			}
			set
			{
				if (this.m_VerticalScrollbar)
				{
					this.m_VerticalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
				}
				this.m_VerticalScrollbar = value;
				if (this.m_VerticalScrollbar)
				{
					this.m_VerticalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
				}
				this.SetDirtyCaching();
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x00009557 File Offset: 0x00007757
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x0000955F File Offset: 0x0000775F
		public ExtendedScrollRect.ScrollbarVisibility horizontalScrollbarVisibility
		{
			get
			{
				return this.m_HorizontalScrollbarVisibility;
			}
			set
			{
				this.m_HorizontalScrollbarVisibility = value;
				this.SetDirtyCaching();
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0000956E File Offset: 0x0000776E
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x00009576 File Offset: 0x00007776
		public ExtendedScrollRect.ScrollbarVisibility verticalScrollbarVisibility
		{
			get
			{
				return this.m_VerticalScrollbarVisibility;
			}
			set
			{
				this.m_VerticalScrollbarVisibility = value;
				this.SetDirtyCaching();
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00009585 File Offset: 0x00007785
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x0000958D File Offset: 0x0000778D
		public float horizontalScrollbarSpacing
		{
			get
			{
				return this.m_HorizontalScrollbarSpacing;
			}
			set
			{
				this.m_HorizontalScrollbarSpacing = value;
				this.SetDirty();
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x0000959C File Offset: 0x0000779C
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x000095A4 File Offset: 0x000077A4
		public float verticalScrollbarSpacing
		{
			get
			{
				return this.m_VerticalScrollbarSpacing;
			}
			set
			{
				this.m_VerticalScrollbarSpacing = value;
				this.SetDirty();
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x000095B3 File Offset: 0x000077B3
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x000095BB File Offset: 0x000077BB
		public ExtendedScrollRect.ScrollRectEvent onValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00073CA0 File Offset: 0x00071EA0
		protected RectTransform viewRect
		{
			get
			{
				if (this.m_ViewRect == null)
				{
					this.m_ViewRect = this.m_Viewport;
				}
				if (this.m_ViewRect == null)
				{
					this.m_ViewRect = (RectTransform)base.transform;
				}
				return this.m_ViewRect;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x000095C4 File Offset: 0x000077C4
		// (set) Token: 0x06000D2D RID: 3373 RVA: 0x000095CC File Offset: 0x000077CC
		public Vector2 velocity
		{
			get
			{
				return this.m_Velocity;
			}
			set
			{
				this.m_Velocity = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000D2E RID: 3374 RVA: 0x000095D5 File Offset: 0x000077D5
		private RectTransform rectTransform
		{
			get
			{
				if (this.m_Rect == null)
				{
					this.m_Rect = base.GetComponent<RectTransform>();
				}
				return this.m_Rect;
			}
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00073CEC File Offset: 0x00071EEC
		protected ExtendedScrollRect()
		{
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000095F7 File Offset: 0x000077F7
		public virtual void Rebuild(CanvasUpdate executing)
		{
			if (executing == CanvasUpdate.Prelayout)
			{
				this.UpdateCachedData();
			}
			if (executing == CanvasUpdate.PostLayout)
			{
				this.UpdateBounds();
				this.UpdateVerticalScrollbar(Vector2.zero);
				this.UpdateHorizontalScrollbar(Vector2.zero);
				this.UpdatePrevData();
				this.m_HasRebuiltLayout = true;
			}
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000026C7 File Offset: 0x000008C7
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x000026C7 File Offset: 0x000008C7
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00073D74 File Offset: 0x00071F74
		private void UpdateCachedData()
		{
			Transform transform = base.transform;
			this.m_HorizontalScrollbarRect = ((this.m_HorizontalScrollbar == null) ? null : (this.m_HorizontalScrollbar.transform as RectTransform));
			this.m_VerticalScrollbarRect = ((this.m_VerticalScrollbar == null) ? null : (this.m_VerticalScrollbar.transform as RectTransform));
			bool flag = this.viewRect.parent == transform;
			bool flag2 = !this.m_HorizontalScrollbarRect || this.m_HorizontalScrollbarRect.parent == transform;
			bool flag3 = !this.m_VerticalScrollbarRect || this.m_VerticalScrollbarRect.parent == transform;
			bool flag4 = flag && flag2 && flag3;
			this.m_HSliderExpand = (flag4 && this.m_HorizontalScrollbarRect && this.horizontalScrollbarVisibility == ExtendedScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
			this.m_VSliderExpand = (flag4 && this.m_VerticalScrollbarRect && this.verticalScrollbarVisibility == ExtendedScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport);
			this.m_HSliderHeight = ((this.m_HorizontalScrollbarRect == null) ? 0f : this.m_HorizontalScrollbarRect.rect.height);
			this.m_VSliderWidth = ((this.m_VerticalScrollbarRect == null) ? 0f : this.m_VerticalScrollbarRect.rect.width);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00073ED4 File Offset: 0x000720D4
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.m_HorizontalScrollbar)
			{
				this.m_HorizontalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
			}
			if (this.m_VerticalScrollbar)
			{
				this.m_VerticalScrollbar.onValueChanged.AddListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
			}
			CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
			this.SetDirty();
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00073F48 File Offset: 0x00072148
		protected override void OnDisable()
		{
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
			if (this.m_HorizontalScrollbar)
			{
				this.m_HorizontalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetHorizontalNormalizedPosition));
			}
			if (this.m_VerticalScrollbar)
			{
				this.m_VerticalScrollbar.onValueChanged.RemoveListener(new UnityAction<float>(this.SetVerticalNormalizedPosition));
			}
			this.m_Dragging = false;
			this.m_Scrolling = false;
			this.m_HasRebuiltLayout = false;
			this.m_Tracker.Clear();
			this.m_Velocity = Vector2.zero;
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			base.OnDisable();
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0000962F File Offset: 0x0000782F
		public override bool IsActive()
		{
			return base.IsActive() && this.m_Content != null;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00009647 File Offset: 0x00007847
		private void EnsureLayoutHasRebuilt()
		{
			if (!this.m_HasRebuiltLayout && !CanvasUpdateRegistry.IsRebuildingLayout())
			{
				Canvas.ForceUpdateCanvases();
			}
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0000965D File Offset: 0x0000785D
		public virtual void StopMovement()
		{
			this.m_Velocity = Vector2.zero;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00073FEC File Offset: 0x000721EC
		public virtual void OnScroll(PointerEventData data)
		{
			if (!this.IsActive())
			{
				return;
			}
			this.EnsureLayoutHasRebuilt();
			this.UpdateBounds();
			Vector2 scrollDelta = data.scrollDelta;
			scrollDelta.y *= -1f;
			if (this.vertical && !this.horizontal)
			{
				if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
				{
					scrollDelta.y = scrollDelta.x;
				}
				scrollDelta.x = 0f;
			}
			if (this.horizontal && !this.vertical)
			{
				if (Mathf.Abs(scrollDelta.y) > Mathf.Abs(scrollDelta.x))
				{
					scrollDelta.x = scrollDelta.y;
				}
				scrollDelta.y = 0f;
			}
			if (data.IsScrolling())
			{
				this.m_Scrolling = true;
			}
			Vector2 vector = this.m_Content.anchoredPosition;
			vector += scrollDelta * this.m_ScrollSensitivity;
			if (this.m_MovementType == ExtendedScrollRect.MovementType.Clamped)
			{
				vector += this.CalculateOffset(vector - this.m_Content.anchoredPosition);
			}
			this.SetContentAnchoredPosition(vector);
			this.UpdateBounds();
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0000966A File Offset: 0x0000786A
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.m_Velocity = Vector2.zero;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0007410C File Offset: 0x0007230C
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateBounds();
			this.m_PointerStartLocalCursor = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out this.m_PointerStartLocalCursor);
			this.m_ContentStartPosition = this.m_Content.anchoredPosition;
			this.m_Dragging = true;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00009680 File Offset: 0x00007880
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.m_Dragging = false;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00074174 File Offset: 0x00072374
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.m_Dragging)
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.IsActive())
			{
				return;
			}
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.viewRect, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			this.UpdateBounds();
			Vector2 b = a - this.m_PointerStartLocalCursor;
			Vector2 vector = this.m_ContentStartPosition + b;
			Vector2 vector2 = this.CalculateOffset(vector - this.m_Content.anchoredPosition);
			vector += vector2;
			if (this.m_MovementType == ExtendedScrollRect.MovementType.Elastic)
			{
				if (vector2.x != 0f)
				{
					vector.x -= ExtendedScrollRect.RubberDelta(vector2.x, this.m_ViewBounds.size.x);
				}
				if (vector2.y != 0f)
				{
					vector.y -= ExtendedScrollRect.RubberDelta(vector2.y, this.m_ViewBounds.size.y);
				}
			}
			this.SetContentAnchoredPosition(vector);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00074274 File Offset: 0x00072474
		protected virtual void SetContentAnchoredPosition(Vector2 position)
		{
			if (!this.m_Horizontal)
			{
				position.x = this.m_Content.anchoredPosition.x;
			}
			if (!this.m_Vertical)
			{
				position.y = this.m_Content.anchoredPosition.y;
			}
			if (position != this.m_Content.anchoredPosition)
			{
				this.m_Content.anchoredPosition = position;
				this.UpdateBounds();
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x000742E4 File Offset: 0x000724E4
		protected virtual void LateUpdate()
		{
			if (!this.m_Content)
			{
				return;
			}
			this.EnsureLayoutHasRebuilt();
			this.UpdateBounds();
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			Vector2 vector = this.CalculateOffset(Vector2.zero);
			if (unscaledDeltaTime > 0f)
			{
				if (!this.m_Dragging && (vector != Vector2.zero || this.m_Velocity != Vector2.zero))
				{
					Vector2 vector2 = this.m_Content.anchoredPosition;
					for (int i = 0; i < 2; i++)
					{
						if (this.m_MovementType == ExtendedScrollRect.MovementType.Elastic && vector[i] != 0f)
						{
							float num = this.m_Velocity[i];
							float num2 = this.m_Elasticity;
							if (this.m_Scrolling)
							{
								num2 *= 3f;
							}
							vector2[i] = Mathf.SmoothDamp(this.m_Content.anchoredPosition[i], this.m_Content.anchoredPosition[i] + vector[i], ref num, num2, float.PositiveInfinity, unscaledDeltaTime);
							if (Mathf.Abs(num) < 1f)
							{
								num = 0f;
							}
							this.m_Velocity[i] = num;
						}
						else if (this.m_Inertia)
						{
							ref Vector2 ptr = ref this.m_Velocity;
							int index = i;
							ptr[index] *= Mathf.Pow(this.m_DecelerationRate, unscaledDeltaTime);
							if (Mathf.Abs(this.m_Velocity[i]) < 1f)
							{
								this.m_Velocity[i] = 0f;
							}
							ptr = ref vector2;
							index = i;
							ptr[index] += this.m_Velocity[i] * unscaledDeltaTime;
						}
						else
						{
							this.m_Velocity[i] = 0f;
						}
					}
					if (this.m_MovementType == ExtendedScrollRect.MovementType.Clamped)
					{
						vector = this.CalculateOffset(vector2 - this.m_Content.anchoredPosition);
						vector2 += vector;
					}
					this.SetContentAnchoredPosition(vector2);
				}
				if (this.m_Dragging && this.m_Inertia)
				{
					Vector3 b = (this.m_Content.anchoredPosition - this.m_PrevPosition) / unscaledDeltaTime;
					this.m_Velocity = Vector3.Lerp(this.m_Velocity, b, unscaledDeltaTime * 10f);
				}
			}
			if (this.m_ViewBounds != this.m_PrevViewBounds || this.m_ContentBounds != this.m_PrevContentBounds || this.m_Content.anchoredPosition != this.m_PrevPosition)
			{
				this.UpdateVerticalScrollbar(vector);
				this.UpdateHorizontalScrollbar(vector);
				UISystemProfilerApi.AddMarker("ScrollRect.value", this);
				this.OnValueChanged(this.normalizedPosition);
				this.m_OnValueChanged.Invoke(this.normalizedPosition);
				this.UpdatePrevData();
			}
			this.UpdateScrollbarVisibility();
			this.m_Scrolling = false;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x000026C7 File Offset: 0x000008C7
		protected virtual void OnValueChanged(Vector2 position)
		{
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000745D0 File Offset: 0x000727D0
		protected void UpdatePrevData()
		{
			if (this.m_Content == null)
			{
				this.m_PrevPosition = Vector2.zero;
			}
			else
			{
				this.m_PrevPosition = this.m_Content.anchoredPosition;
			}
			this.m_PrevViewBounds = this.m_ViewBounds;
			this.m_PrevContentBounds = this.m_ContentBounds;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00074624 File Offset: 0x00072824
		protected virtual void UpdateVerticalScrollbar(Vector2 offset)
		{
			if (!this.m_VerticalScrollbar)
			{
				return;
			}
			if (this.m_ContentBounds.size.y > 0f)
			{
				this.m_VerticalScrollbar.size = Mathf.Clamp01((this.m_ViewBounds.size.y - Mathf.Abs(offset.y)) / this.m_ContentBounds.size.y);
			}
			else
			{
				this.m_VerticalScrollbar.size = 1f;
			}
			this.m_VerticalScrollbar.value = this.verticalNormalizedPosition;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000746B8 File Offset: 0x000728B8
		protected virtual void UpdateHorizontalScrollbar(Vector2 offset)
		{
			if (!this.m_HorizontalScrollbar)
			{
				return;
			}
			if (this.m_ContentBounds.size.x > 0f)
			{
				this.m_HorizontalScrollbar.size = Mathf.Clamp01((this.m_ViewBounds.size.x - Mathf.Abs(offset.x)) / this.m_ContentBounds.size.x);
			}
			else
			{
				this.m_HorizontalScrollbar.size = 1f;
			}
			this.m_HorizontalScrollbar.value = this.horizontalNormalizedPosition;
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x00009692 File Offset: 0x00007892
		// (set) Token: 0x06000D45 RID: 3397 RVA: 0x000096A5 File Offset: 0x000078A5
		public Vector2 normalizedPosition
		{
			get
			{
				return new Vector2(this.horizontalNormalizedPosition, this.verticalNormalizedPosition);
			}
			set
			{
				this.SetNormalizedPosition(value.x, 0);
				this.SetNormalizedPosition(value.y, 1);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x0007474C File Offset: 0x0007294C
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x000096C1 File Offset: 0x000078C1
		public float horizontalNormalizedPosition
		{
			get
			{
				this.UpdateBounds();
				if (this.m_ContentBounds.size.x <= this.m_ViewBounds.size.x || Mathf.Approximately(this.m_ContentBounds.size.x, this.m_ViewBounds.size.x))
				{
					return (float)((this.m_ViewBounds.min.x > this.m_ContentBounds.min.x) ? 1 : 0);
				}
				return (this.m_ViewBounds.min.x - this.m_ContentBounds.min.x) / (this.m_ContentBounds.size.x - this.m_ViewBounds.size.x);
			}
			set
			{
				this.SetNormalizedPosition(value, 0);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00074814 File Offset: 0x00072A14
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x000096CB File Offset: 0x000078CB
		public float verticalNormalizedPosition
		{
			get
			{
				this.UpdateBounds();
				if (this.m_ContentBounds.size.y <= this.m_ViewBounds.size.y || Mathf.Approximately(this.m_ContentBounds.size.y, this.m_ViewBounds.size.y))
				{
					return (float)((this.m_ViewBounds.min.y > this.m_ContentBounds.min.y) ? 1 : 0);
				}
				return (this.m_ViewBounds.min.y - this.m_ContentBounds.min.y) / (this.m_ContentBounds.size.y - this.m_ViewBounds.size.y);
			}
			set
			{
				this.SetNormalizedPosition(value, 1);
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000096C1 File Offset: 0x000078C1
		private void SetHorizontalNormalizedPosition(float value)
		{
			this.SetNormalizedPosition(value, 0);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x000096CB File Offset: 0x000078CB
		protected virtual void SetVerticalNormalizedPosition(float value)
		{
			this.SetNormalizedPosition(value, 1);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000748DC File Offset: 0x00072ADC
		protected virtual void SetNormalizedPosition(float value, int axis)
		{
			this.EnsureLayoutHasRebuilt();
			this.UpdateBounds();
			float num = this.m_ContentBounds.size[axis] - this.m_ViewBounds.size[axis];
			float num2 = this.m_ViewBounds.min[axis] - value * num;
			float num3 = this.m_Content.anchoredPosition[axis] + num2 - this.m_ContentBounds.min[axis];
			Vector3 v = this.m_Content.anchoredPosition;
			if (Mathf.Abs(v[axis] - num3) > 0.01f)
			{
				v[axis] = num3;
				this.m_Content.anchoredPosition = v;
				this.m_Velocity[axis] = 0f;
				this.UpdateBounds();
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000096D5 File Offset: 0x000078D5
		private static float RubberDelta(float overStretching, float viewSize)
		{
			return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00009700 File Offset: 0x00007900
		protected override void OnRectTransformDimensionsChange()
		{
			this.SetDirty();
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00009708 File Offset: 0x00007908
		private bool hScrollingNeeded
		{
			get
			{
				return !Application.isPlaying || this.m_ContentBounds.size.x > this.m_ViewBounds.size.x + 0.01f;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x0000973B File Offset: 0x0000793B
		private bool vScrollingNeeded
		{
			get
			{
				return !Application.isPlaying || this.m_ContentBounds.size.y > this.m_ViewBounds.size.y + 0.01f;
			}
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x000026C7 File Offset: 0x000008C7
		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x000026C7 File Offset: 0x000008C7
		public virtual void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x0000976E File Offset: 0x0000796E
		public virtual float minWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x0000976E File Offset: 0x0000796E
		public virtual float preferredWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x0000976E File Offset: 0x0000796E
		public virtual float flexibleWidth
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000D56 RID: 3414 RVA: 0x0000976E File Offset: 0x0000796E
		public virtual float minHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x0000976E File Offset: 0x0000796E
		public virtual float preferredHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0000976E File Offset: 0x0000796E
		public virtual float flexibleHeight
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00009775 File Offset: 0x00007975
		public virtual int layoutPriority
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x000749C4 File Offset: 0x00072BC4
		public virtual void SetLayoutHorizontal()
		{
			this.m_Tracker.Clear();
			this.UpdateCachedData();
			if (this.m_HSliderExpand || this.m_VSliderExpand)
			{
				this.m_Tracker.Add(this, this.viewRect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY);
				this.viewRect.anchorMin = Vector2.zero;
				this.viewRect.anchorMax = Vector2.one;
				this.viewRect.sizeDelta = Vector2.zero;
				this.viewRect.anchoredPosition = Vector2.zero;
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.content);
				this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				this.m_ContentBounds = this.GetBounds();
			}
			if (this.m_VSliderExpand && this.vScrollingNeeded)
			{
				this.viewRect.sizeDelta = new Vector2(-(this.m_VSliderWidth + this.m_VerticalScrollbarSpacing), this.viewRect.sizeDelta.y);
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.content);
				this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				this.m_ContentBounds = this.GetBounds();
			}
			if (this.m_HSliderExpand && this.hScrollingNeeded)
			{
				this.viewRect.sizeDelta = new Vector2(this.viewRect.sizeDelta.x, -(this.m_HSliderHeight + this.m_HorizontalScrollbarSpacing));
				this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
				this.m_ContentBounds = this.GetBounds();
			}
			if (this.m_VSliderExpand && this.vScrollingNeeded && this.viewRect.sizeDelta.x == 0f && this.viewRect.sizeDelta.y < 0f)
			{
				this.viewRect.sizeDelta = new Vector2(-(this.m_VSliderWidth + this.m_VerticalScrollbarSpacing), this.viewRect.sizeDelta.y);
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00074C28 File Offset: 0x00072E28
		public virtual void SetLayoutVertical()
		{
			this.UpdateScrollbarLayout();
			this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
			this.m_ContentBounds = this.GetBounds();
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00009778 File Offset: 0x00007978
		protected virtual void UpdateScrollbarVisibility()
		{
			ExtendedScrollRect.UpdateOneScrollbarVisibility(this.vScrollingNeeded, this.m_Vertical, this.m_VerticalScrollbarVisibility, this.m_VerticalScrollbar);
			ExtendedScrollRect.UpdateOneScrollbarVisibility(this.hScrollingNeeded, this.m_Horizontal, this.m_HorizontalScrollbarVisibility, this.m_HorizontalScrollbar);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00074C84 File Offset: 0x00072E84
		protected static void UpdateOneScrollbarVisibility(bool xScrollingNeeded, bool xAxisEnabled, ExtendedScrollRect.ScrollbarVisibility scrollbarVisibility, Scrollbar scrollbar)
		{
			if (scrollbar)
			{
				if (scrollbarVisibility == ExtendedScrollRect.ScrollbarVisibility.Permanent)
				{
					if (scrollbar.gameObject.activeSelf != xAxisEnabled)
					{
						scrollbar.gameObject.SetActive(xAxisEnabled);
						return;
					}
				}
				else if (scrollbar.gameObject.activeSelf != xScrollingNeeded)
				{
					scrollbar.gameObject.SetActive(xScrollingNeeded);
				}
			}
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00074CD4 File Offset: 0x00072ED4
		private void UpdateScrollbarLayout()
		{
			if (this.m_VSliderExpand && this.m_HorizontalScrollbar)
			{
				this.m_Tracker.Add(this, this.m_HorizontalScrollbarRect, DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.SizeDeltaX);
				this.m_HorizontalScrollbarRect.anchorMin = new Vector2(0f, this.m_HorizontalScrollbarRect.anchorMin.y);
				this.m_HorizontalScrollbarRect.anchorMax = new Vector2(1f, this.m_HorizontalScrollbarRect.anchorMax.y);
				this.m_HorizontalScrollbarRect.anchoredPosition = new Vector2(0f, this.m_HorizontalScrollbarRect.anchoredPosition.y);
				if (this.vScrollingNeeded)
				{
					this.m_HorizontalScrollbarRect.sizeDelta = new Vector2(-(this.m_VSliderWidth + this.m_VerticalScrollbarSpacing), this.m_HorizontalScrollbarRect.sizeDelta.y);
				}
				else
				{
					this.m_HorizontalScrollbarRect.sizeDelta = new Vector2(0f, this.m_HorizontalScrollbarRect.sizeDelta.y);
				}
			}
			if (this.m_HSliderExpand && this.m_VerticalScrollbar)
			{
				this.m_Tracker.Add(this, this.m_VerticalScrollbarRect, DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaY);
				this.m_VerticalScrollbarRect.anchorMin = new Vector2(this.m_VerticalScrollbarRect.anchorMin.x, 0f);
				this.m_VerticalScrollbarRect.anchorMax = new Vector2(this.m_VerticalScrollbarRect.anchorMax.x, 1f);
				this.m_VerticalScrollbarRect.anchoredPosition = new Vector2(this.m_VerticalScrollbarRect.anchoredPosition.x, 0f);
				if (this.hScrollingNeeded)
				{
					this.m_VerticalScrollbarRect.sizeDelta = new Vector2(this.m_VerticalScrollbarRect.sizeDelta.x, -(this.m_HSliderHeight + this.m_HorizontalScrollbarSpacing));
					return;
				}
				this.m_VerticalScrollbarRect.sizeDelta = new Vector2(this.m_VerticalScrollbarRect.sizeDelta.x, 0f);
			}
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00074EDC File Offset: 0x000730DC
		protected void UpdateBounds()
		{
			this.m_ViewBounds = new Bounds(this.viewRect.rect.center, this.viewRect.rect.size);
			this.m_ContentBounds = this.GetBounds();
			if (this.m_Content == null)
			{
				return;
			}
			Vector3 size = this.m_ContentBounds.size;
			Vector3 center = this.m_ContentBounds.center;
			Vector2 pivot = this.m_Content.pivot;
			ExtendedScrollRect.AdjustBounds(ref this.m_ViewBounds, ref pivot, ref size, ref center);
			this.m_ContentBounds.size = size;
			this.m_ContentBounds.center = center;
			if (this.movementType == ExtendedScrollRect.MovementType.Clamped)
			{
				Vector2 zero = Vector2.zero;
				if (this.m_ViewBounds.max.x > this.m_ContentBounds.max.x)
				{
					zero.x = Math.Min(this.m_ViewBounds.min.x - this.m_ContentBounds.min.x, this.m_ViewBounds.max.x - this.m_ContentBounds.max.x);
				}
				else if (this.m_ViewBounds.min.x < this.m_ContentBounds.min.x)
				{
					zero.x = Math.Max(this.m_ViewBounds.min.x - this.m_ContentBounds.min.x, this.m_ViewBounds.max.x - this.m_ContentBounds.max.x);
				}
				if (this.m_ViewBounds.min.y < this.m_ContentBounds.min.y)
				{
					zero.y = Math.Max(this.m_ViewBounds.min.y - this.m_ContentBounds.min.y, this.m_ViewBounds.max.y - this.m_ContentBounds.max.y);
				}
				else if (this.m_ViewBounds.max.y > this.m_ContentBounds.max.y)
				{
					zero.y = Math.Min(this.m_ViewBounds.min.y - this.m_ContentBounds.min.y, this.m_ViewBounds.max.y - this.m_ContentBounds.max.y);
				}
				if (zero.sqrMagnitude > 1E-45f)
				{
					center = this.m_Content.anchoredPosition + zero;
					if (!this.m_Horizontal)
					{
						center.x = this.m_Content.anchoredPosition.x;
					}
					if (!this.m_Vertical)
					{
						center.y = this.m_Content.anchoredPosition.y;
					}
					ExtendedScrollRect.AdjustBounds(ref this.m_ViewBounds, ref pivot, ref size, ref center);
				}
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x000751D8 File Offset: 0x000733D8
		internal static void AdjustBounds(ref Bounds viewBounds, ref Vector2 contentPivot, ref Vector3 contentSize, ref Vector3 contentPos)
		{
			Vector3 vector = viewBounds.size - contentSize;
			if (vector.x > 0f)
			{
				contentPos.x -= vector.x * (contentPivot.x - 0.5f);
				contentSize.x = viewBounds.size.x;
			}
			if (vector.y > 0f)
			{
				contentPos.y -= vector.y * (contentPivot.y - 0.5f);
				contentSize.y = viewBounds.size.y;
			}
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00075270 File Offset: 0x00073470
		private Bounds GetBounds()
		{
			if (this.m_Content == null)
			{
				return default(Bounds);
			}
			this.m_Content.GetWorldCorners(this.m_Corners);
			Matrix4x4 worldToLocalMatrix = this.viewRect.worldToLocalMatrix;
			return ExtendedScrollRect.InternalGetBounds(this.m_Corners, ref worldToLocalMatrix);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000752C0 File Offset: 0x000734C0
		internal static Bounds InternalGetBounds(Vector3[] corners, ref Matrix4x4 viewWorldToLocalMatrix)
		{
			Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			for (int i = 0; i < 4; i++)
			{
				Vector3 lhs = viewWorldToLocalMatrix.MultiplyPoint3x4(corners[i]);
				vector = Vector3.Min(lhs, vector);
				vector2 = Vector3.Max(lhs, vector2);
			}
			Bounds result = new Bounds(vector, Vector3.zero);
			result.Encapsulate(vector2);
			return result;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x000097B4 File Offset: 0x000079B4
		private Vector2 CalculateOffset(Vector2 delta)
		{
			return ExtendedScrollRect.InternalCalculateOffset(ref this.m_ViewBounds, ref this.m_ContentBounds, this.m_Horizontal, this.m_Vertical, this.m_MovementType, ref delta);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00075338 File Offset: 0x00073538
		internal static Vector2 InternalCalculateOffset(ref Bounds viewBounds, ref Bounds contentBounds, bool horizontal, bool vertical, ExtendedScrollRect.MovementType movementType, ref Vector2 delta)
		{
			Vector2 zero = Vector2.zero;
			if (movementType == ExtendedScrollRect.MovementType.Unrestricted)
			{
				return zero;
			}
			Vector2 vector = contentBounds.min;
			Vector2 vector2 = contentBounds.max;
			if (horizontal)
			{
				vector.x += delta.x;
				vector2.x += delta.x;
				float num = viewBounds.max.x - vector2.x;
				float num2 = viewBounds.min.x - vector.x;
				if (num2 < -0.001f)
				{
					zero.x = num2;
				}
				else if (num > 0.001f)
				{
					zero.x = num;
				}
			}
			if (vertical)
			{
				vector.y += delta.y;
				vector2.y += delta.y;
				float num3 = viewBounds.max.y - vector2.y;
				float num4 = viewBounds.min.y - vector.y;
				if (num3 > 0.001f)
				{
					zero.y = num3;
				}
				else if (num4 < -0.001f)
				{
					zero.y = num4;
				}
			}
			return zero;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x000097DB File Offset: 0x000079DB
		protected void SetDirty()
		{
			if (!this.IsActive())
			{
				return;
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x000097F1 File Offset: 0x000079F1
		protected void SetDirtyCaching()
		{
			if (!this.IsActive())
			{
				return;
			}
			CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
			LayoutRebuilder.MarkLayoutForRebuild(this.rectTransform);
			this.m_ViewRect = null;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00009814 File Offset: 0x00007A14
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04000A42 RID: 2626
		[SerializeField]
		private RectTransform m_Content;

		// Token: 0x04000A43 RID: 2627
		[SerializeField]
		private bool m_Horizontal = true;

		// Token: 0x04000A44 RID: 2628
		[SerializeField]
		private bool m_Vertical = true;

		// Token: 0x04000A45 RID: 2629
		[SerializeField]
		private ExtendedScrollRect.MovementType m_MovementType = ExtendedScrollRect.MovementType.Elastic;

		// Token: 0x04000A46 RID: 2630
		[SerializeField]
		private float m_Elasticity = 0.1f;

		// Token: 0x04000A47 RID: 2631
		[SerializeField]
		private bool m_Inertia = true;

		// Token: 0x04000A48 RID: 2632
		[SerializeField]
		private float m_DecelerationRate = 0.135f;

		// Token: 0x04000A49 RID: 2633
		[SerializeField]
		private float m_ScrollSensitivity = 1f;

		// Token: 0x04000A4A RID: 2634
		[SerializeField]
		private RectTransform m_Viewport;

		// Token: 0x04000A4B RID: 2635
		[SerializeField]
		private Scrollbar m_HorizontalScrollbar;

		// Token: 0x04000A4C RID: 2636
		[SerializeField]
		protected Scrollbar m_VerticalScrollbar;

		// Token: 0x04000A4D RID: 2637
		[SerializeField]
		private ExtendedScrollRect.ScrollbarVisibility m_HorizontalScrollbarVisibility;

		// Token: 0x04000A4E RID: 2638
		[SerializeField]
		private ExtendedScrollRect.ScrollbarVisibility m_VerticalScrollbarVisibility;

		// Token: 0x04000A4F RID: 2639
		[SerializeField]
		private float m_HorizontalScrollbarSpacing;

		// Token: 0x04000A50 RID: 2640
		[SerializeField]
		private float m_VerticalScrollbarSpacing;

		// Token: 0x04000A51 RID: 2641
		[SerializeField]
		private ExtendedScrollRect.ScrollRectEvent m_OnValueChanged = new ExtendedScrollRect.ScrollRectEvent();

		// Token: 0x04000A52 RID: 2642
		private Vector2 m_PointerStartLocalCursor = Vector2.zero;

		// Token: 0x04000A53 RID: 2643
		protected Vector2 m_ContentStartPosition = Vector2.zero;

		// Token: 0x04000A54 RID: 2644
		private RectTransform m_ViewRect;

		// Token: 0x04000A55 RID: 2645
		protected Bounds m_ContentBounds;

		// Token: 0x04000A56 RID: 2646
		protected Bounds m_ViewBounds;

		// Token: 0x04000A57 RID: 2647
		private Vector2 m_Velocity;

		// Token: 0x04000A58 RID: 2648
		private bool m_Dragging;

		// Token: 0x04000A59 RID: 2649
		private bool m_Scrolling;

		// Token: 0x04000A5A RID: 2650
		private Vector2 m_PrevPosition = Vector2.zero;

		// Token: 0x04000A5B RID: 2651
		private Bounds m_PrevContentBounds;

		// Token: 0x04000A5C RID: 2652
		private Bounds m_PrevViewBounds;

		// Token: 0x04000A5D RID: 2653
		[NonSerialized]
		private bool m_HasRebuiltLayout;

		// Token: 0x04000A5E RID: 2654
		private bool m_HSliderExpand;

		// Token: 0x04000A5F RID: 2655
		private bool m_VSliderExpand;

		// Token: 0x04000A60 RID: 2656
		private float m_HSliderHeight;

		// Token: 0x04000A61 RID: 2657
		private float m_VSliderWidth;

		// Token: 0x04000A62 RID: 2658
		[NonSerialized]
		private RectTransform m_Rect;

		// Token: 0x04000A63 RID: 2659
		private RectTransform m_HorizontalScrollbarRect;

		// Token: 0x04000A64 RID: 2660
		private RectTransform m_VerticalScrollbarRect;

		// Token: 0x04000A65 RID: 2661
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x04000A66 RID: 2662
		private readonly Vector3[] m_Corners = new Vector3[4];

		// Token: 0x020001EF RID: 495
		public enum MovementType
		{
			// Token: 0x04000A68 RID: 2664
			Unrestricted,
			// Token: 0x04000A69 RID: 2665
			Elastic,
			// Token: 0x04000A6A RID: 2666
			Clamped
		}

		// Token: 0x020001F0 RID: 496
		public enum ScrollbarVisibility
		{
			// Token: 0x04000A6C RID: 2668
			Permanent,
			// Token: 0x04000A6D RID: 2669
			AutoHide,
			// Token: 0x04000A6E RID: 2670
			AutoHideAndExpandViewport
		}

		// Token: 0x020001F1 RID: 497
		[Serializable]
		public class ScrollRectEvent : UnityEvent<Vector2>
		{
		}
	}
}
