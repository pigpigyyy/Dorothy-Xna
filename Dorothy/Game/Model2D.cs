using System.Collections.Generic;
using Dorothy.Animations;
using Dorothy.Cameras;
using Dorothy.Core;
using Dorothy.Defs;
using Microsoft.Xna.Framework;
using Dorothy.Helpers;
using System;

namespace Dorothy.Game
{
	public class SpriteState
	{
		private int _count;
		private Sprite[] _sprites;
		private SpriteDef[] _defs;

		public SpriteState(int count)
		{
			_count = count;
			_sprites = new Sprite[count];
			_defs = new SpriteDef[count];
		}
		public void Set(int subscript, Sprite sprite, SpriteDef def)
		{
			_sprites[subscript] = sprite;
			_defs[subscript] = def;
		}
		public void Restore()
		{
			for (int i = 0; i < _count; i++)
			{
				_sprites[i].OffsetX = _defs[i].OffsetX;
				_sprites[i].OffsetY = _defs[i].OffsetY;
				_sprites[i].OffsetZ = _defs[i].OffsetZ;
				_sprites[i].RotateX = _defs[i].RotateX;
				_sprites[i].RotateY = _defs[i].RotateY;
				_sprites[i].RotateZ = _defs[i].RotateZ;
				_sprites[i].ScaleX = _defs[i].ScaleX;
				_sprites[i].ScaleY = _defs[i].ScaleY;
				_sprites[i].Alpha = _defs[i].Alpha;
			}
		}
	}

	public class Model2D : Drawable
	{
		#region 成员
		private IAnimation _currentAnimation;
		private SpriteState _spriteState;
		private Dictionary<string, IAnimation> _animations = new Dictionary<string, IAnimation>();
		#endregion

		public IAnimation CurrentAnimation
		{
			get { return _currentAnimation; }
		}
		public Dictionary<string, IAnimation> Animations
		{
			get { return _animations; }
		}

		public Model2D(Sprite sprite, SpriteState state)
		{
			_spriteState = state;
			base.Add(sprite);
		}
		public void Play(string name)
		{
			if (_currentAnimation != null)
			{
				if (_currentAnimation.IsPlaying)
				{
					_currentAnimation.Stop();
				}
			}
			_spriteState.Restore();
			_currentAnimation = _animations[name];
			_currentAnimation.Play();
		}
		public void Stop(bool end = false)
		{
			if (_currentAnimation != null)
			{
				if (_currentAnimation.IsPlaying)
				{
					_currentAnimation.Stop(end);
				}
			}
			_currentAnimation = null;
		}
		public override void Draw()
		{
			List<Drawable> children = this.Children;
			if (children != null)
			{
				foreach (var child in children)
				{
					Drawable.Traverse
					(
						child,
						drawable =>
						{
							drawable.Draw();
						}
					);
				}
			}
		}
	}
}
