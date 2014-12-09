using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace PairedGame
{
	public class CrawlerScreen: Scene
	{
		public CrawlerScreen()
		{
			//Camera.SetViewFromViewport();
			Vector2 cameraCentre = new Vector2();
			Tile.Loader("/Application/assets/level1.txt", ref cameraCentre, this);
			GameInfo.CameraHeight = 200f;
			Camera2D.SetViewFromHeightAndCenter(GameInfo.CameraHeight, cameraCentre);
		}
	}
}