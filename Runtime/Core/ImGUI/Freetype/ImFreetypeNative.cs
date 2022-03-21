using System;
using System.Runtime.InteropServices;

namespace Espionage.Engine.ImGUI
{
	internal static unsafe partial class ImFreetypeNative
	{
		[DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr GetBuilderForFreeType();

		[DllImport("cimgui", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetAllocatorFunctions(IntPtr alloc_func, IntPtr free_func, IntPtr user_data);
	}
}