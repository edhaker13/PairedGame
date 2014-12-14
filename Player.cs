using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace PairedGame
{
	public class Player: EntityAlive
	{
		private static int PLAYER_INDEX = 0;
		
		public Player(Vector2 position):
			base(PLAYER_INDEX, position, new Vector2i(0, 1))
		{
			IsDefending = false;
			Stats.Lives = 5;
		}
		
		override public void Update(float dt)
		{
			if(IsAlive)
				Info.TotalGameTime += dt;
	
			// Handle battle
			base.Update(dt);
			
			// Handle movement/attacks
			HandleInput();
			
			// Find current tile and apply collision
			HandleCollision();
			
			// Apply the movement
			Position = Position + MoveSpeed;
			// Make camera follow the player
			Info.CameraCentre = Position;
		}
		
		public AttackStatus Attack { get { return attackState; } }
		
		private static float MoveDelta = 2f;
		
		private void HandleInput()
		{
			var gamePadData = GamePad.GetData(0);
			// Apply direction and animation
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				MoveSpeed.X = -MoveDelta;
				// Set animation range.
				TileRangeX = new Vector2i(6, 7);
			}
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				MoveSpeed.X = MoveDelta;
				TileRangeX = new Vector2i(4, 5);
			}
			if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			{
				MoveSpeed.Y = MoveDelta;
				TileRangeX = new Vector2i(2, 3);
			}
			if((gamePadData.Buttons & GamePadButtons.Down) != 0)
			{
				MoveSpeed.Y = -MoveDelta;
				TileRangeX = new Vector2i(0, 1);
			}
			// Attacks if in battle
			if(InBattle)
			{
				if((gamePadData.ButtonsDown & GamePadButtons.Cross) != 0)
				{
					attackState = AttackStatus.MeleeNormal;
				}
				if((gamePadData.ButtonsDown & GamePadButtons.Circle) != 0)
				{
					attackState = AttackStatus.MeleeStrong;
				}
				if((gamePadData.ButtonsDown & GamePadButtons.Square) != 0)
				{
					attackState = AttackStatus.RangedNormal;
				}
				if((gamePadData.ButtonsDown & GamePadButtons.Triangle) != 0)
				{
					attackState = AttackStatus.RangedStrong;
				}
				
				if((gamePadData.ButtonsDown & GamePadButtons.L) != 0)
				{
					IsDefending = true;
				}
			}
			// Set frame to start of animation range if outside of range
			if(TileIndex2D.X < TileRangeX.X || TileIndex2D.X > TileRangeX.Y)
				TileIndex2D.X = TileRangeX.X;
		}
		
		private void HandleCollision()
		{
			if(SceneManager.CurrentScene == null)
				return;
			// Loop through tiles
			foreach(Tile t in SceneManager.CurrentScene.Children[0].Children)
			{
				if(t.Overlaps(this))
				{
					if(!MoveSpeed.IsZero())
						t.HandleCollision(Position, ref MoveSpeed);
					if(t.IsOccupied)
					{
						if(t.IsBoss && !Info.InBattle)
						{
							if(!Info.HadConversation)
							{
								SceneManager.ReplaceUIScene(new Conversation());
								SceneManager.PauseScene();
							}
						}
						else if(t.Occupier.IsAlive)
						{
							Opponent = t.Occupier;
							Opponent.Opponent = this;
							Opponent.InBattle = true;
							if(!InBattle)
								SceneManager.ReplaceUIScene(new FightUI(this, Opponent));
							InBattle = true;
							Info.InBattle = true;
						}
						else
						{
							t.IsOccupied = false;
							Opponent = null;
							t.Occupier.Opponent = null;
							t.Occupier.InBattle = false;
							InBattle = false;
							Info.InBattle = false;
						}
					}
					if(t.Key == 'Z')
						Info.LevelClear = true;
				}
			}
		}
	}
}
