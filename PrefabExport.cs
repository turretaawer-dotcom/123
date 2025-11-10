using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004EE RID: 1262
	public class PrefabExport
	{
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x0600294D RID: 10573 RVA: 0x0001D9B1 File Offset: 0x0001BBB1
		// (set) Token: 0x0600294E RID: 10574 RVA: 0x0001D9B9 File Offset: 0x0001BBB9
		public int PrefabNumber { get; set; }

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x0600294F RID: 10575 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
		// (set) Token: 0x06002950 RID: 10576 RVA: 0x0001D9CA File Offset: 0x0001BBCA
		public uint PrefabID { get; set; }

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06002951 RID: 10577 RVA: 0x0001D9D3 File Offset: 0x0001BBD3
		// (set) Token: 0x06002952 RID: 10578 RVA: 0x0001D9DB File Offset: 0x0001BBDB
		public string PrefabPath { get; set; }

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06002953 RID: 10579 RVA: 0x0001D9E4 File Offset: 0x0001BBE4
		// (set) Token: 0x06002954 RID: 10580 RVA: 0x0001D9EC File Offset: 0x0001BBEC
		public string PrefabPosition { get; set; }

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06002955 RID: 10581 RVA: 0x0001D9F5 File Offset: 0x0001BBF5
		// (set) Token: 0x06002956 RID: 10582 RVA: 0x0001D9FD File Offset: 0x0001BBFD
		public string PrefabScale { get; set; }

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06002957 RID: 10583 RVA: 0x0001DA06 File Offset: 0x0001BC06
		// (set) Token: 0x06002958 RID: 10584 RVA: 0x0001DA0E File Offset: 0x0001BC0E
		public string PrefabRotation { get; set; }
	}
}
