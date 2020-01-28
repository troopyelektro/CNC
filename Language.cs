/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 06.12.2019
 * Time: 7:09
 */
using System;

namespace CNC
{
	/// <summary>
	/// Provides language translation
	/// </summary>
	public abstract class Language
	{		
		public string menuFile = "#UNTRANSLATED#";
		public string menuFileNew = "#UNTRANSLATED#";
		public string menuFileNew2DDrawing = "#UNTRANSLATED#";
		public string menuFileOpen = "#UNTRANSLATED#";
		public string menuFileSave = "#UNTRANSLATED#";
		public string menuFileSaveAs = "#UNTRANSLATED#";
		public string menuFileClose = "#UNTRANSLATED#";
		public string menuFileExit = "#UNTRANSLATED#";		
		
		public string menuSettings = "#UNTRANSLATED#";		
		public string menuSettingsConfiguration = "#UNTRANSLATED#";		
		
		public Language()
		{
		}	
	}
}
