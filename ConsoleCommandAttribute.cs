using System;

namespace RustMapEditor.Variables
{
	// Token: 0x020004B1 RID: 1201
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class ConsoleCommandAttribute : Attribute
	{
		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060028FB RID: 10491 RVA: 0x0001D51D File Offset: 0x0001B71D
		public string Description { get; }

		// Token: 0x060028FC RID: 10492 RVA: 0x0001D525 File Offset: 0x0001B725
		public ConsoleCommandAttribute(string description)
		{
			this.Description = description;
		}
	}
}
