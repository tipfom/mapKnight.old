using System;

using CocosSharp;

namespace mapKnightLibrary
{
	public class GroundParticle : CCNode

	{
		static CCRotateTo LeftRotation = null;
		static CCRotateTo RightRotation = null;

		Direction CurrentParticleAppearDirection;

		CCParticleSystemQuad MainParticle;

		public GroundParticle ()
		{
			string [] particleData = new [] { "particle_ground.plist" };

			MainParticle = new CCParticleSystemQuad (particleData [0]);
			this.AddChild (MainParticle);

			LeftRotation = new CCRotateTo (0f, 0f);
			RightRotation = new CCRotateTo (0f, 180f);
		}

		public Direction ParticleAppearDirection{ 
			get{return this.CurrentParticleAppearDirection; }

			set{
				this.CurrentParticleAppearDirection = value;
				switch (this.CurrentParticleAppearDirection) {
				case Direction.Left:
					this.Visible = true;
					MainParticle.RunAction (LeftRotation);
					break;
				case Direction.Right:
					this.Visible = true;
					MainParticle.RunAction (RightRotation);
					break;
				case Direction.None:
					this.Visible = false;
					break;
				default:
					this.Visible = false;
					break;
				}
			}
		}
	}
}

