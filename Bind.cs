using System;
using UnityEngine.InputSystem;

namespace RustMapEditor.Variables
{
	// Token: 0x020004D6 RID: 1238
	[Serializable]
	public struct Bind
	{
		// Token: 0x0600292D RID: 10541 RVA: 0x0001D78B File Offset: 0x0001B98B
		public Bind(string name, InputActionType type, string input, bool ctrl = false, bool shift = false, bool alt = false)
		{
			this.bindName = name;
			this.actionType = type;
			this.primaryInput = input;
			this.requiresCtrl = ctrl;
			this.requiresShift = shift;
			this.requiresAlt = alt;
		}

		// Token: 0x04001684 RID: 5764
		public string bindName;

		// Token: 0x04001685 RID: 5765
		public InputActionType actionType;

		// Token: 0x04001686 RID: 5766
		public string primaryInput;

		// Token: 0x04001687 RID: 5767
		public bool requiresCtrl;

		// Token: 0x04001688 RID: 5768
		public bool requiresShift;

		// Token: 0x04001689 RID: 5769
		public bool requiresAlt;
	}
}
