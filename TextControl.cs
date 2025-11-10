using System;
using TMPro;
using UnityEngine;

namespace UIRecycleTreeNamespace
{
	// Token: 0x020001FC RID: 508
	public class TextControl : MonoBehaviour
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x0000A15A File Offset: 0x0000835A
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x0000A167 File Offset: 0x00008367
		public string text
		{
			get
			{
				return this.textField.text;
			}
			set
			{
				this.textField.text = value;
			}
		}

		// Token: 0x17000195 RID: 405
		// (set) Token: 0x06000E22 RID: 3618 RVA: 0x00076A10 File Offset: 0x00074C10
		public NodeTextStyle style
		{
			set
			{
				this.textField.fontSize = value.fontSize;
				this.textField.font = value.fontAsset;
				this.textField.fontStyle = value.fontStyle;
				this.textField.color = value.color;
				this.textField.wordSpacing = value.wordSpacing;
			}
		}

		// Token: 0x04000AC3 RID: 2755
		[SerializeField]
		private TMP_Text textField;
	}
}
