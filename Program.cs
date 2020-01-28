/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 02.12.2019
 * Time: 20:21
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;

namespace CNC
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		private static void Main(string[] args)
		{
			GUI g = new GUI();
			g.Run();
		}		
	}
}
