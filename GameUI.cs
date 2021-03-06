using System;
using Sce.PlayStation.HighLevel.UI;

namespace PairedGame
{
	public class GameUI: Scene
	{
		private static Label level = new Label();
		private static Label score = new Label();
		
		public GameUI(): base()
		{
			Title = "GameUI";
			
			level = new Label();
			level.SetPosition(20, 5);
			level.TextColor = new UIColor(1f, 1f, 1f, 1f);
			level.TextShadow = new TextShadowSettings();
			level.TextShadow.Color = new UIColor(0.2f, 0.2f, 0.2f, 1f);
			level.TextTrimming = TextTrimming.None;
			RootWidget.AddChildLast(level);
			
			score = new Label();
			score.SetPosition(800, 5);
			score.TextColor = new UIColor(1f, 1f, 1f, 1f);
			score.TextShadow = new TextShadowSettings();
			score.TextShadow.Color = new UIColor(0.2f, 0.2f, 0.2f, 1f);
			score.TextTrimming = TextTrimming.None;
			RootWidget.AddChildLast(score);
		}

		override protected void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			level.Text = "Level " + Info.LevelNumber.ToString();
			score.Text = "Reputation " + Info.Reputation.ToString();
		}
	}
}

