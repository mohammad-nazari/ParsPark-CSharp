﻿using System;
using System.Windows.Forms;

namespace ParsPark
{
	public static class Delegates
	{
		public static void InvokeIfRequired<T>(this T c, Action<T> action)
			where T : Control
		{
			if (c.InvokeRequired)
			{
				c.Invoke(new Action(() => action(c)));
			}
			else
			{
				action(c);
			}
		}
	}
}
