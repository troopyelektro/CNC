/*
 * Created by SharpDevelop.
 * User: Ing. Petr Rösch
 * Date: 06.12.2019
 * Time: 7:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace CNC
{
	/// <summary>
	/// Czech Language Dictionarz
	/// </summary>
	public class LanguageCS : Language
	{	
		public LanguageCS() : base()
		{
			this.menuFile = "Soubor";
			this.menuFileNew = "Nový";
			this.menuFileNew2DDrawing = "2D Výkres";
			this.menuFileOpen = "Otevřít";
			this.menuFileSave = "Uložit";
			this.menuFileSaveAs = "Uložit jako";
			this.menuFileClose = "Zavřít";
			this.menuFileExit = "Ukončit";

			this.menuSettings = "Nastavení";		
			this.menuSettingsConfiguration = "Konfigurace";					
		}
	}
}
