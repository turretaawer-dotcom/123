using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B2 RID: 1202
	[AttributeUsage(AttributeTargets.Struct)]
	public class ConsoleVariableAttribute : Attribute
	{
		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060028FD RID: 10493 RVA: 0x0001D534 File Offset: 0x0001B734
		public string Description { get; }

		// Token: 0x060028FE RID: 10494 RVA: 0x0001D53C File Offset: 0x0001B73C
		public ConsoleVariableAttribute(string description)
		{
			this.Description = description;
		}
	}
}
