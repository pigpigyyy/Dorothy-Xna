using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Effects
{
	public static class oEffectManager
	{
		private static List<IDrawableEffect> _drawableEffects = new List<IDrawableEffect>();
		private static Dictionary<string, IDrawableEffect> _effectDict = new Dictionary<string, IDrawableEffect>();
		private static List<IPostEffect> _postEffects = new List<IPostEffect>();
		private static Matrix _view = Matrix.Identity;
		private static Matrix _projection = Matrix.Identity;

		public static Matrix View
		{
			get { return _view; }
		}
		public static Matrix Projection
		{
			get { return _projection; }
		}
		public static List<IPostEffect> PostProcessEffects
		{
			get { return _postEffects; }
		}

		public static void SetView(ref Matrix view)
		{
			_view = view;
			for (int i = 0; i < _drawableEffects.Count; i++)
			{
				_drawableEffects[i].View = view;
			}
		}
		public static void SetProjection(ref Matrix projection)
		{
			_projection = projection;
			for (int i = 0; i < _drawableEffects.Count; i++)
			{
				_drawableEffects[i].Projection = projection;
			}
		}
		public static IDrawableEffect GetEffect(string name)
		{
			IDrawableEffect effect = null;
			_effectDict.TryGetValue(name, out effect);
			return effect;
		}
		public static bool Add(IDrawableEffect effect)
		{
			if (!_effectDict.ContainsKey(effect.Name))
			{
				effect.View = _view;
				effect.Projection = _projection;
				_drawableEffects.Add(effect);
				_effectDict.Add(effect.Name, effect);
				return true;
			}
			return false;
		}
		public static void Add(IPostEffect effect)
		{
			if (!_postEffects.Contains(effect))
			{
				_postEffects.Add(effect);
			}
		}
		public static bool Remove(IDrawableEffect effect)
		{
			if (effect != oGame.DefaultSpriteEffect)
			{
				if (_effectDict.Remove(effect.Name))
				{
					_drawableEffects.Remove(effect);
					return true;
				}
			}
			return false;
		}
		public static bool Remove(IPostEffect effect)
		{
			return _postEffects.Remove(effect);
		}
		public static bool Remove(string name)
		{
			if (!name.Equals(oGame.DefaultSpriteEffect.Name))
			{
				IDrawableEffect effect;
				if (_effectDict.TryGetValue(name, out effect))
				{
					_effectDict.Remove(name);
					_drawableEffects.Remove(effect);
					return true;
				}
			}
			for (int i = 0; i < _postEffects.Count; i++)
			{
				if (_postEffects[i].Name.Equals(name))
				{
					_postEffects.Remove(_postEffects[i]);
					return true;
				}
			}
			return false;
		}
		public static void Clear()
		{
			_drawableEffects.Clear();
			_effectDict.Clear();
			_postEffects.Clear();
			oEffectManager.Add(oGame.DefaultSpriteEffect);
			oEffectManager.Add(oGame.PaintEffect);
		}
		public static void SetPostProcessOrder(string effectName, int order)
		{
			IPostEffect effect = _postEffects.Find(eff => { return effectName.Equals(eff.Name); });
			if (effect != null)
			{
				if (order < 0)
				{
					order = 0;
				}
				else if (order > _postEffects.Count - 1)
				{
					order = _postEffects.Count - 1;
				}
				_postEffects.Remove(effect);
				_postEffects.Insert(order, effect);
			}
		}
	}
}
