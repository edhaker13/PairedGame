using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace PairedGame
{
	public class AppMain
	{		
		public static void Main(string[] args)
		{
			Initialize();
			bool quitGame = false;
			GamePadData gamePadData = GamePad.GetData(0);
			
			while(!quitGame)
			{
				SystemEvents.CheckEvents();	// We check system events (such as pressing PS button, pressing power button to sleep, major and unknown crash!!)				
				Director.Instance.Update();
				UISystem.Update(Touch.GetData(0), ref gamePadData); // Update UI Manager
				Director.Instance.Render();
				UISystem.Render(); // Render UI Manager
				Director.Instance.GL.Context.SwapBuffers(); // Swap between back and front buffer
				Director.Instance.PostSwap(); // Must be called after swap buffers - not 100% sure, imagine it resets back buffer to black/white, unallocates tied resources for next swap
			}
			TextureManager.Dispose();
			AudioManager.StopMusic();
			AudioManager.StopSounds();
			Director.Terminate();	// Kill (terminate) the director, hence ending 2D scene program, once we are done with the scene (clicking red X button)
			Sce.PlayStation.HighLevel.UI.UISystem.Terminate();
		}

		public static void Initialize()
		{
			// Initialises the GameEngine2D supplied by Sony.
			Director.Initialize();
			// Initialises the UI Framework supplied by Sony.
			UISystem.Initialize(Director.Instance.GL.Context);
			// Load and store textures
			TextureManager.AddAsset("tiles", new TextureInfo(new Texture2D("/Application/assets/dungeon_tiles.png", false),
			                                                 new Vector2i(21, 5)));
			TextureManager.AddAsset("entities", new TextureInfo(new Texture2D("/Application/assets/dungeon_objects.png", false),
			                                                 new Vector2i(9, 14)));
			
			//Load and store sounds
			AudioManager.AddSound("/Application/assets/draw-knife.wav", "fight");
			AudioManager.AddMusic("/Application/assets/dungeon-ambience.mp3", "ambient");
			
			// Initial Values;
			Info.TotalGameTime = 0f;
			Info.LevelNumber = 1;
			
			// Tell the UISystem to run an empty scene
			UISystem.SetScene(new GameUI(), null);
			Director.Instance.RunWithScene(new CrawlerScreen(), true);
		}
	}
}
