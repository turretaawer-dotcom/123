using System;
using UnityEngine;

namespace RustMapEditor.Variables
{
	// Token: 0x020004DD RID: 1245
	public class IconTextures
	{
		// Token: 0x06002939 RID: 10553 RVA: 0x0001D890 File Offset: 0x0001BA90
		public IconTextures(Texture2D gears, Texture2D scrap, Texture2D stop, Texture2D tarp, Texture2D trash)
		{
			this.gears = gears;
			this.scrap = scrap;
			this.stop = stop;
			this.tarp = tarp;
			this.trash = trash;
		}

		// Token: 0x04001695 RID: 5781
		public Texture2D gears;

		// Token: 0x04001696 RID: 5782
		public Texture2D scrap;

		// Token: 0x04001697 RID: 5783
		public Texture2D stop;

		// Token: 0x04001698 RID: 5784
		public Texture2D tarp;

		// Token: 0x04001699 RID: 5785
		public Texture2D trash;
	}
}
