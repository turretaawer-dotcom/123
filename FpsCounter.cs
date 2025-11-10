using System;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001F3 RID: 499
	public class FpsCounter : MonoBehaviour
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x00009824 File Offset: 0x00007A24
		// (set) Token: 0x06000D6A RID: 3434 RVA: 0x0000982C File Offset: 0x00007A2C
		public int averageFPS { get; private set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x00075454 File Offset: 0x00073654
		private Rect rect
		{
			get
			{
				switch (this.screenPos)
				{
				case ScreenPos.LeftTop:
					return new Rect(0f, 0f, 230f, 100f);
				case ScreenPos.RightTop:
					return new Rect((float)(Screen.width - 230), 0f, 230f, 100f);
				case ScreenPos.Center:
					return new Rect((float)((Screen.width - 200) / 2), (float)((Screen.height - 100) / 2), 200f, 100f);
				}
				throw new ArgumentOutOfRangeException(this.screenPos.ToString() + " not exist");
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00075510 File Offset: 0x00073710
		private GUIStyle guiStyle
		{
			get
			{
				switch (this.screenPos)
				{
				case ScreenPos.LeftTop:
					return new GUIStyle
					{
						fontSize = this.fontSize,
						fontStyle = FontStyle.Bold,
						normal = 
						{
							textColor = this.color
						},
						alignment = TextAnchor.UpperLeft
					};
				case ScreenPos.RightTop:
					return new GUIStyle
					{
						fontSize = this.fontSize,
						fontStyle = FontStyle.Bold,
						normal = 
						{
							textColor = this.color
						},
						alignment = TextAnchor.UpperRight
					};
				case ScreenPos.Center:
					return new GUIStyle
					{
						fontSize = this.fontSize,
						fontStyle = FontStyle.Bold,
						normal = 
						{
							textColor = this.color
						},
						alignment = TextAnchor.MiddleCenter
					};
				}
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00009835 File Offset: 0x00007A35
		private void Awake()
		{
			this._guiStyle = new GUIStyle
			{
				fontSize = this.fontSize,
				fontStyle = FontStyle.Bold,
				normal = 
				{
					textColor = this.color
				}
			};
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00009866 File Offset: 0x00007A66
		private void Update()
		{
			if (this._fpsBuffer == null || this.frameRange != this._fpsBuffer.Length)
			{
				this.InitializeBuffer();
			}
			this.UpdateBuffer();
			this.CalculateAverageFps();
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00009892 File Offset: 0x00007A92
		private void InitializeBuffer()
		{
			if (this.frameRange <= 0)
			{
				this.frameRange = 1;
			}
			this._fpsBuffer = new int[this.frameRange];
			this._fpsBufferIndex = 0;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000755E4 File Offset: 0x000737E4
		private void UpdateBuffer()
		{
			int[] fpsBuffer = this._fpsBuffer;
			int fpsBufferIndex = this._fpsBufferIndex;
			this._fpsBufferIndex = fpsBufferIndex + 1;
			fpsBuffer[fpsBufferIndex] = (int)(1f / Time.unscaledDeltaTime);
			if (this._fpsBufferIndex >= this.frameRange)
			{
				this._fpsBufferIndex = 0;
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0007562C File Offset: 0x0007382C
		private void CalculateAverageFps()
		{
			int num = 0;
			for (int i = 0; i < this.frameRange; i++)
			{
				int num2 = this._fpsBuffer[i];
				num += num2;
			}
			this.averageFPS = num / this.frameRange;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000098BC File Offset: 0x00007ABC
		private void OnGUI()
		{
			GUILayout.BeginArea(this.rect);
			GUILayout.Label(string.Format("{0} fps", this.averageFPS), this.guiStyle, Array.Empty<GUILayoutOption>());
			GUILayout.EndArea();
		}

		// Token: 0x04000A73 RID: 2675
		[SerializeField]
		private int frameRange = 60;

		// Token: 0x04000A74 RID: 2676
		[SerializeField]
		private int fontSize = 15;

		// Token: 0x04000A75 RID: 2677
		[SerializeField]
		private Color color = Color.green;

		// Token: 0x04000A76 RID: 2678
		[SerializeField]
		private ScreenPos screenPos;

		// Token: 0x04000A78 RID: 2680
		private GUIStyle _guiStyle;

		// Token: 0x04000A79 RID: 2681
		private int[] _fpsBuffer;

		// Token: 0x04000A7A RID: 2682
		private int _fpsBufferIndex;
	}
}
