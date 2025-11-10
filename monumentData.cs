using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004C8 RID: 1224
	public struct monumentData
	{
		// Token: 0x06002925 RID: 10533 RVA: 0x0001D703 File Offset: 0x0001B903
		public monumentData(int X, int Y, int Width, int Height)
		{
			this.x = X;
			this.y = Y;
			this.width = Width;
			this.height = Height;
		}

		// Token: 0x04001640 RID: 5696
		public int x;

		// Token: 0x04001641 RID: 5697
		public int y;

		// Token: 0x04001642 RID: 5698
		public int width;

		// Token: 0x04001643 RID: 5699
		public int height;
	}
}
