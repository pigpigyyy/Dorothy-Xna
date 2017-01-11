using System;
using Dorothy.Animations;
using Dorothy.Core;
using Dorothy.Data;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Defs
{
	/// <summary>
	/// Animation`s definition.
	/// </summary>
	[Serializable]
	public abstract class AnimationDef
	{
		/// <summary>
		/// Animation`s name.
		/// </summary>
		public string Name = string.Empty;
		/// <summary>
		/// Does animation play looped.
		/// </summary>
		public bool Loop;
		/// <summary>
		/// Does animation reversely play.
		/// </summary>
		public bool Reverse;
		/// <summary>
		/// Animation`s play speed.
		/// </summary>
		public float Speed = 1.0f;
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>A new animation instance.</returns>
		public abstract IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures);
	}
	/// <summary>
	/// Property animation`s definition.
	/// </summary>
	[Serializable]
	public abstract class PropertyDef : AnimationDef
	{
		/// <summary>
		/// Property from value;
		/// </summary>
		public float From;
		/// <summary>
		/// Property to value;
		/// </summary>
		public float To;
		/// <summary>
		/// Duration time in milliseconds.
		/// </summary>
		public float Duration;
		/// <summary>
		/// Easing function`s ID.
		/// </summary>
		public int EaserID = Easer.NoEasing.ID;
		/// <summary>
		/// Its target`s name.
		/// </summary>
		public string TargetName = string.Empty;
	}
	/// <summary>
	/// Fade animation`s definition.
	/// </summary>
	[Serializable]
	public class FadeDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			Fade fade = new Fade(base.From, base.To, base.Duration);
			fade.Target = (Sprite)targets[TargetName];
			fade.Speed = base.Speed;
			fade.Loop = base.Loop;
			fade.Reverse = base.Reverse;
			fade.Easer = Easer.GetEaserByID(base.EaserID);
			return fade;
		}
	}
	/// <summary>
	/// Rotate X animation`s definition.
	/// </summary>
	[Serializable]
	public class RotateXDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			RotateX rotateX = new RotateX(base.From, base.To, base.Duration);
			rotateX.Target = (Sprite)targets[TargetName];
			rotateX.Speed = base.Speed;
			rotateX.Loop = base.Loop;
			rotateX.Reverse = base.Reverse;
			rotateX.Easer = Easer.GetEaserByID(base.EaserID);
			return rotateX;
		}
	}
	/// <summary>
	/// Rotate Y animation`s definition.
	/// </summary>
	[Serializable]
	public class RotateYDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			RotateY rotateY = new RotateY(base.From, base.To, base.Duration);
			rotateY.Target = (Sprite)targets[TargetName];
			rotateY.Speed = base.Speed;
			rotateY.Loop = base.Loop;
			rotateY.Reverse = base.Reverse;
			rotateY.Easer = Easer.GetEaserByID(base.EaserID);
			return rotateY;
		}
	}
	/// <summary>
	/// Rotate Z animation`s definition.
	/// </summary>
	[Serializable]
	public class RotateZDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			RotateZ rotateZ = new RotateZ(base.From, base.To, base.Duration);
			rotateZ.Target = (Sprite)targets[TargetName];
			rotateZ.Speed = base.Speed;
			rotateZ.Loop = base.Loop;
			rotateZ.Reverse = base.Reverse;
			rotateZ.Easer = Easer.GetEaserByID(base.EaserID);
			return rotateZ;
		}
	}
	/// <summary>
	/// Move X animation`s definition.
	/// </summary>
	[Serializable]
	public class MoveXDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			MoveX moveX = new MoveX(base.From, base.To, base.Duration);
			moveX.Target = (Sprite)targets[TargetName];
			moveX.Speed = base.Speed;
			moveX.Loop = base.Loop;
			moveX.Reverse = base.Reverse;
			moveX.Easer = Easer.GetEaserByID(base.EaserID);
			return moveX;
		}
	}
	/// <summary>
	/// Move Y animation`s definition.
	/// </summary>
	[Serializable]
	public class MoveYDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			MoveY moveY = new MoveY(base.From, base.To, base.Duration);
			moveY.Target = (Sprite)targets[TargetName];
			moveY.Speed = base.Speed;
			moveY.Loop = base.Loop;
			moveY.Reverse = base.Reverse;
			moveY.Easer = Easer.GetEaserByID(base.EaserID);
			return moveY;
		}
	}
	/// <summary>
	/// Move Z animation`s definition.
	/// </summary>
	[Serializable]
	public class MoveZDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			MoveZ moveZ = new MoveZ(base.From, base.To, base.Duration);
			moveZ.Target = (Sprite)targets[TargetName];
			moveZ.Speed = base.Speed;
			moveZ.Loop = base.Loop;
			moveZ.Reverse = base.Reverse;
			moveZ.Easer = Easer.GetEaserByID(base.EaserID);
			return moveZ;
		}
	}
	/// <summary>
	/// Scale X animation`s definition.
	/// </summary>
	[Serializable]
	public class ScaleXDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			ScaleX scaleX = new ScaleX(base.From, base.To, base.Duration);
			scaleX.Target = (Sprite)targets[TargetName];
			scaleX.Speed = base.Speed;
			scaleX.Loop = base.Loop;
			scaleX.Reverse = base.Reverse;
			scaleX.Easer = Easer.GetEaserByID(base.EaserID);
			return scaleX;
		}
	}
	/// <summary>
	/// Scale Y animation`s definition.
	/// </summary>
	[Serializable]
	public class ScaleYDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			ScaleY scaleY = new ScaleY(base.From, base.To, base.Duration);
			scaleY.Target = (Sprite)targets[TargetName];
			scaleY.Speed = base.Speed;
			scaleY.Loop = base.Loop;
			scaleY.Reverse = base.Reverse;
			scaleY.Easer = Easer.GetEaserByID(base.EaserID);
			return scaleY;
		}
	}
	/// <summary>
	/// Offset X animation`s definition.
	/// </summary>
	[Serializable]
	public class OffsetXDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			OffsetX offsetX = new OffsetX(base.From, base.To, Duration);
			offsetX.Target = (Sprite)targets[TargetName];
			offsetX.Speed = base.Speed;
			offsetX.Loop = base.Loop;
			offsetX.Reverse = base.Reverse;
			offsetX.Easer = Easer.GetEaserByID(base.EaserID);
			return offsetX;
		}
	}
	/// <summary>
	/// Offset Y animation`s definition.
	/// </summary>
	[Serializable]
	public class OffsetYDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			OffsetY offsetY = new OffsetY(base.From, base.To, base.Duration);
			offsetY.Target = (Sprite)targets[TargetName];
			offsetY.Speed = base.Speed;
			offsetY.Loop = base.Loop;
			offsetY.Reverse = base.Reverse;
			offsetY.Easer = Easer.GetEaserByID(base.EaserID);
			return offsetY;
		}
	}
	/// <summary>
	/// Offset Z animation`s definition.
	/// </summary>
	[Serializable]
	public class OffsetZDef : PropertyDef
	{
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			OffsetZ offsetZ = new OffsetZ(base.From, base.To, base.Duration);
			offsetZ.Target = (Sprite)targets[TargetName];
			offsetZ.Speed = base.Speed;
			offsetZ.Loop = base.Loop;
			offsetZ.Reverse = base.Reverse;
			offsetZ.Easer = Easer.GetEaserByID(base.EaserID);
			return offsetZ;
		}
	}
	/// <summary>
	/// Idle animation`s definition.
	/// </summary>
	[Serializable]
	public class IdleDef : AnimationDef
	{
		/// <summary>
		/// Duration time in milliseconds.
		/// </summary>
		public float Duration;
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			Idle idle = new Idle(Duration);
			idle.Speed = base.Speed;
			idle.Loop = base.Loop;
			idle.Reverse = base.Reverse;
			return idle;
		}
	}
	/// <summary>
	/// Animation sequence`s definition.
	/// </summary>
	[Serializable]
	public class SequenceDef : AnimationDef
	{
		/// <summary>
		/// Animations
		/// </summary>
		public AnimationDef[] AnimationDefs;
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			Sequence seq = new Sequence();
			for (int i = 0; i < AnimationDefs.Length; i++)
			{
				seq.Add(AnimationDefs[i].ToAnimation(targets, textures));
			}
			seq.Speed = base.Speed;
			seq.Loop = base.Loop;
			seq.Reverse = base.Reverse;
			return seq;
		}
	}
	/// <summary>
	/// Animation parallel`s definition.
	/// </summary>
	[Serializable]
	public class ParallelDef : AnimationDef
	{
		/// <summary>
		/// Animations
		/// </summary>
		public AnimationDef[] AnimationDefs;
		/// <summary>
		/// Convert the definition to a new animation instance.
		/// </summary>
		/// <param name="targets">The resource contains its target.</param>
		/// <param name="textures">The resource contains textures.</param>
		/// <returns>
		/// A new animation instance.
		/// </returns>
		public override IAnimation ToAnimation(
			Resource<object> targets,
			Resource<Texture2D> textures)
		{
			Parallel par = new Parallel();
			for (int i = 0; i < AnimationDefs.Length; i++)
			{
				par.Add(AnimationDefs[i].ToAnimation(targets, textures));
			}
			par.Speed = base.Speed;
			par.Loop = base.Loop;
			par.Reverse = base.Reverse;
			return par;
		}
	}
}
