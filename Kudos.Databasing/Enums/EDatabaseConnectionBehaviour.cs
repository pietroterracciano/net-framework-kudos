﻿using System;
using Kudos.Constants;

namespace Kudos.Databasing.Enums
{
	[Flags]
	public enum EDatabaseConnectionBehaviour
	{
		None = CBinaryFlag.None,
		AutomaticOpening = CBinaryFlag._0,
		AutomaticClosing = CBinaryFlag._1
	}
}
