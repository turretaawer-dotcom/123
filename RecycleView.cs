using System;
using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIRecycleTreeNamespace
{
	// Token: 0x0200020C RID: 524
	public class RecycleView : ExtendedScrollRect
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0000A66E File Offset: 0x0000886E
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x00077398 File Offset: 0x00075598
		public float spacing
		{
			get
			{
				if (!(this.contentLayoutGroup == null))
				{
					return this.contentLayoutGroup.spacing;
				}
				return 0f;
			}
			set
			{
				if (this.contentLayoutGroup == null)
				{
					return;
				}
				if (Math.Abs(this.contentLayoutGroup.spacing - value) <= 0.01f)
				{
					return;
				}
				this.contentLayoutGroup.spacing = value;
				base.StartCoroutine(this.Reload());
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0000A68F File Offset: 0x0000888F
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x0000A69C File Offset: 0x0000889C
		public RectOffset contentPadding
		{
			get
			{
				return this.contentLayoutGroup.padding;
			}
			set
			{
				if (this.contentLayoutGroup.padding == value)
				{
					return;
				}
				this.contentLayoutGroup.padding = value;
				base.StartCoroutine(this.Reload());
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x0000A6C6 File Offset: 0x000088C6
		// (set) Token: 0x06000E95 RID: 3733 RVA: 0x0000A6CE File Offset: 0x000088CE
		public IRecycleDataSource recycleDataSource { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0000A6D7 File Offset: 0x000088D7
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x0000A6E4 File Offset: 0x000088E4
		private Vector2 contentPosition
		{
			get
			{
				return base.content.anchoredPosition;
			}
			set
			{
				base.content.anchoredPosition = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0000A6F2 File Offset: 0x000088F2
		private RectTransform firstRectTransform
		{
			get
			{
				return this._recycleItemsPool[0].rectTransform;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000E99 RID: 3737 RVA: 0x000773E8 File Offset: 0x000755E8
		private RectTransform lastRectTransform
		{
			get
			{
				ObservableCollection<RecycleItem> recycleItemsPool = this._recycleItemsPool;
				int index = recycleItemsPool.Count - 1;
				return recycleItemsPool[index].rectTransform;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0000A705 File Offset: 0x00008905
		private int lowestRecyclingIndex
		{
			get
			{
				return this._topmostRecyclingIndex + this._visibleItemsPoolSize;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000E9B RID: 3739 RVA: 0x0000A714 File Offset: 0x00008914
		private float scaledItemHeight
		{
			get
			{
				return (this.itemHeight + this.spacing) * base.transform.lossyScale.x;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0000A734 File Offset: 0x00008934
		private float recycleVerticalThreshold
		{
			get
			{
				return this.scaledItemHeight * 4f;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0000A742 File Offset: 0x00008942
		private float extraContentSize
		{
			get
			{
				return (this.itemHeight + this.spacing) * 6f;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0000A757 File Offset: 0x00008957
		private float verticalPaddingValue
		{
			get
			{
				return (float)(this.contentPadding.top + this.contentPadding.bottom);
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0000A771 File Offset: 0x00008971
		private Vector2 contentOffset
		{
			get
			{
				return new Vector2(0f, this.itemHeight + this.spacing);
			}
		}

		// Token: 0x170001C3 RID: 451
		// (set) Token: 0x06000EA0 RID: 3744 RVA: 0x00077410 File Offset: 0x00075610
		private float contentNormalizedPositionY
		{
			set
			{
				float y = Mathf.Clamp01(value);
				base.normalizedPosition = new Vector2(base.normalizedPosition.x, y);
			}
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0000A78A File Offset: 0x0000898A
		public IEnumerator Reload()
		{
			if (this.recycleDataSource == null)
			{
				yield break;
			}
			this._isReloading = true;
			this.BeforeReload();
			yield return null;
			this.template.rectTransform.sizeDelta = new Vector2(this.template.rectTransform.sizeDelta.x, this.itemHeight);
			this._requiredItemsCountOnScreen = this.GetRequiredItemsCountOnScreen();
			int num = Mathf.Min(this._requiredItemsCountOnScreen, this.recycleDataSource.expandedCount);
			float y = (float)num * (this.itemHeight + this.spacing) - this.spacing + this.verticalPaddingValue;
			base.content.sizeDelta = new Vector2(base.content.sizeDelta.x, y);
			if (num > this._visibleItemsPoolSize)
			{
				this.IncreasePool(num);
			}
			else if (num < this._visibleItemsPoolSize)
			{
				this.DecreasePool(num);
			}
			this._visibleItemsPoolSize = num;
			this.OnPoolReady();
			this.StopMoving();
			this._isReloading = false;
			this.AfterReload();
			this.Repaint();
			yield break;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0007743C File Offset: 0x0007563C
		public void Repaint()
		{
			if (!base.isActiveAndEnabled || this._isReloading)
			{
				return;
			}
			bool flag = this.recycleDataSource.expandedCount - this._topmostRecyclingIndex < this._visibleItemsPoolSize;
			int num = flag ? (this.recycleDataSource.expandedCount - this._visibleItemsPoolSize) : this._topmostRecyclingIndex;
			if (flag)
			{
				this.contentNormalizedPositionY = 0f;
			}
			this._topmostRecyclingIndex = num;
			foreach (RecycleItem item in this._recycleItemsPool)
			{
				this.GetDataFromSource(item, num);
				num++;
			}
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0000A799 File Offset: 0x00008999
		private void GetDataFromSource(RecycleItem item, int index)
		{
			this.recycleDataSource.MergeDataWithView(item, index);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000774EC File Offset: 0x000756EC
		private void IncreasePool(int newPoolSize)
		{
			for (int i = this._visibleItemsPoolSize; i < newPoolSize; i++)
			{
				RecycleItem item = this.CreateItem();
				this._recycleItemsPool.Add(item);
				this.OnPoolIncrease(item);
			}
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0000A7A8 File Offset: 0x000089A8
		protected virtual RecycleItem CreateItem()
		{
			return UnityEngine.Object.Instantiate<RecycleItem>(this.template, base.content, true);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00077524 File Offset: 0x00075724
		private void DecreasePool(int newPoolSize)
		{
			for (int i = this._visibleItemsPoolSize - 1; i >= newPoolSize; i--)
			{
				RecycleItem recycleItem = this._recycleItemsPool[i];
				this.BeforePoolDecrease(recycleItem);
				UnityEngine.Object.Destroy(recycleItem.gameObject);
				this._recycleItemsPool.RemoveAt(i);
			}
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x000026C7 File Offset: 0x000008C7
		protected virtual void OnPoolIncrease(RecycleItem item)
		{
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x000026C7 File Offset: 0x000008C7
		protected virtual void BeforePoolDecrease(RecycleItem item)
		{
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x000026C7 File Offset: 0x000008C7
		protected virtual void BeforeReload()
		{
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x000026C7 File Offset: 0x000008C7
		protected virtual void AfterReload()
		{
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0000A7BC File Offset: 0x000089BC
		protected virtual void OnPoolReady()
		{
			if (this._visibleItemsPoolSize < this._requiredItemsCountOnScreen)
			{
				this._topmostRecyclingIndex = 0;
			}
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00077570 File Offset: 0x00075770
		private int GetRequiredItemsCountOnScreen()
		{
			return (int)Mathf.Ceil((this.extraContentSize + base.viewport.rect.height) / (this.itemHeight + this.spacing));
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x000775AC File Offset: 0x000757AC
		protected override void OnValueChanged(Vector2 position)
		{
			if (!Application.isPlaying || this.recycleDataSource.expandedCount == 0)
			{
				return;
			}
			if (this._isReloading)
			{
				return;
			}
			this.SetRecyclingBounds();
			bool flag = base.velocity.y > 0f && this.lastRectTransform.MaxY() > this._recyclableViewBounds.min.y;
			bool flag2 = base.velocity.y < 0f && this.firstRectTransform.MinY() < this._recyclableViewBounds.max.y;
			if (flag)
			{
				this.RecycleFromTopToBottom();
			}
			if (flag2)
			{
				this.RecycleFromBottomToTop();
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00077654 File Offset: 0x00075854
		protected void SetTopmostIndexForFocusedNode(int index)
		{
			int num = (int)Mathf.Ceil(3f);
			if (this._visibleItemsPoolSize < this._requiredItemsCountOnScreen - num)
			{
				return;
			}
			int num2 = (int)Mathf.Ceil((float)this._visibleItemsPoolSize / 2f) - num;
			this._topmostRecyclingIndex = ((index > num2) ? (index - num2) : 0);
			this.contentNormalizedPositionY = 1f;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x000776B0 File Offset: 0x000758B0
		public override void OnScroll(PointerEventData data)
		{
			if (this._isReloading)
			{
				return;
			}
			this.StopMoving();
			this.SetRecyclingBounds();
			this.contentPosition -= data.scrollDelta * base.scrollSensitivity;
			bool flag = this.lastRectTransform.MaxY() > this._recyclableViewBounds.min.y;
			bool flag2 = this.firstRectTransform.MinY() < this._recyclableViewBounds.max.y;
			if (flag)
			{
				this.RecycleFromTopToBottom();
			}
			if (flag2)
			{
				this.RecycleFromBottomToTop();
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00077740 File Offset: 0x00075940
		private void RecycleFromTopToBottom()
		{
			while (this.firstRectTransform.MinY() > this._recyclableViewBounds.max.y && this.lowestRecyclingIndex < this.recycleDataSource.expandedCount)
			{
				this.contentPosition -= this.contentOffset;
				this.m_ContentStartPosition -= this.contentOffset;
				this.firstRectTransform.SetAsLastSibling();
				this._recycleItemsPool.Move(0, this._recycleItemsPool.Count - 1);
				ObservableCollection<RecycleItem> recycleItemsPool = this._recycleItemsPool;
				int index = recycleItemsPool.Count - 1;
				this.GetDataFromSource(recycleItemsPool[index], this.lowestRecyclingIndex);
				this._topmostRecyclingIndex++;
			}
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00077808 File Offset: 0x00075A08
		private void RecycleFromBottomToTop()
		{
			while (this.lastRectTransform.MaxY() < this._recyclableViewBounds.min.y && this._topmostRecyclingIndex > 0)
			{
				this.contentPosition += this.contentOffset;
				this.m_ContentStartPosition += this.contentOffset;
				this.lastRectTransform.SetAsFirstSibling();
				this._recycleItemsPool.Move(this._recycleItemsPool.Count - 1, 0);
				this._topmostRecyclingIndex--;
				this.GetDataFromSource(this._recycleItemsPool[0], this._topmostRecyclingIndex);
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x000778B8 File Offset: 0x00075AB8
		protected override void UpdateVerticalScrollbar(Vector2 offset)
		{
			if (this.recycleDataSource == null)
			{
				return;
			}
			if (!base.verticalScrollbar)
			{
				return;
			}
			base.verticalScrollbar.size = ((this._visibleItemsPoolSize > 0) ? Mathf.Clamp01((float)this._visibleItemsPoolSize / (float)this.recycleDataSource.expandedCount) : 1f);
			int num = this.recycleDataSource.expandedCount - this._visibleItemsPoolSize;
			float valueWithoutNotify = (num != 0) ? (1f - ((float)this.lowestRecyclingIndex - (float)this._visibleItemsPoolSize) / (float)num) : 0f;
			base.verticalScrollbar.SetValueWithoutNotify(valueWithoutNotify);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00077954 File Offset: 0x00075B54
		protected override void SetVerticalNormalizedPosition(float value)
		{
			if (this.recycleDataSource == null)
			{
				return;
			}
			value = Mathf.Clamp01(1f - value);
			int num = this.recycleDataSource.expandedCount - this._visibleItemsPoolSize;
			int num2 = (int)(value * (float)num) + this._visibleItemsPoolSize - 1 - this._visibleItemsPoolSize + 1;
			if (this._topmostRecyclingIndex != num2)
			{
				int num3 = (this._topmostRecyclingIndex > num2) ? 1 : 0;
				this.contentNormalizedPositionY = (float)num3;
			}
			this._topmostRecyclingIndex = num2;
			this.Repaint();
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x000779D0 File Offset: 0x00075BD0
		private void SetRecyclingBounds()
		{
			base.viewport.GetWorldCorners(this._corners);
			this._recyclableViewBounds.min = new Vector3(this._corners[0].x, this._corners[0].y - this.recycleVerticalThreshold);
			this._recyclableViewBounds.max = new Vector3(this._corners[2].x, this._corners[2].y + this.recycleVerticalThreshold);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0000A7D5 File Offset: 0x000089D5
		protected override void Start()
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.Reload());
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0000A7D5 File Offset: 0x000089D5
		protected override void OnRectTransformDimensionsChange()
		{
			if (base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.Reload());
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0000A7EC File Offset: 0x000089EC
		private void StopMoving()
		{
			base.velocity = Vector2.zero;
		}

		// Token: 0x04000B15 RID: 2837
		private const int EXTRA_ITEMS_COUNT = 6;

		// Token: 0x04000B16 RID: 2838
		private const int RECYCLE_BOUNDS_THRESHOLD_IN_ITEMS = 4;

		// Token: 0x04000B17 RID: 2839
		private const int DEFAULT_ITEM_HEIGHT = 30;

		// Token: 0x04000B18 RID: 2840
		[SerializeField]
		private RecycleItem template;

		// Token: 0x04000B19 RID: 2841
		[SerializeField]
		private VerticalLayoutGroup contentLayoutGroup;

		// Token: 0x04000B1A RID: 2842
		[SerializeField]
		private float itemHeight = 30f;

		// Token: 0x04000B1C RID: 2844
		private readonly Vector3[] _corners = new Vector3[4];

		// Token: 0x04000B1D RID: 2845
		private ObservableCollection<RecycleItem> _recycleItemsPool = new ObservableCollection<RecycleItem>();

		// Token: 0x04000B1E RID: 2846
		private Bounds _recyclableViewBounds;

		// Token: 0x04000B1F RID: 2847
		private int _topmostRecyclingIndex;

		// Token: 0x04000B20 RID: 2848
		private int _visibleItemsPoolSize;

		// Token: 0x04000B21 RID: 2849
		private int _requiredItemsCountOnScreen;

		// Token: 0x04000B22 RID: 2850
		private bool _isReloading;
	}
}
