using System;
using System.Collections.Generic;
using Dorothy.Cameras;
using Dorothy.Effects;
using Dorothy.Helpers;
using Microsoft.Xna.Framework;

namespace Dorothy.Core
{
	/// <summary>
	/// Anything needs to be drew each frame should implement its Draw() method,
	/// then apply the render transform with its protected member _mWorld ( Matrix ),
	/// adjust render alpha by its protected member _finalAlpha ( float ),
	/// and carefully set its protected member _drawOrder ( float ).
	/// Things with higher _drawOrder value will be drew first,
	/// so it`s better to set _drawOrder to the distance between object and current camera.
	/// </summary>
	public abstract class Drawable
	{
		/// <summary>
		/// Indicating what kind of transform should be applied.
		/// </summary>
		enum TransformFlag
		{
			None = 0,
			Rotate = (1 << 0),
			Scale = (1 << 1),
			Translate = (1 << 2),
			World = (1 << 3)
		};
		#region Field
		private string _name = string.Empty;
		private TransformFlag _flag = TransformFlag.Rotate | TransformFlag.Scale | TransformFlag.Translate | TransformFlag.World;
		private bool _bVisible = true;
		private bool _sortChildren;
		private bool _bChildrenVisible = true;
		private float _alpha = 1.0f;
		private Vector3 _position;
		private Vector3 _offset;
		private Vector3 _rotate;
		private Vector3 _scale = Vector3.One;
		private Matrix _mRotateX = Matrix.Identity;
		private Matrix _mRotateY = Matrix.Identity;
		private Matrix _mRotateZ = Matrix.Identity;
		private Matrix _mScale;
		private Matrix _mTranslate;
		private Matrix _mLocalWorld;
		private Drawable _parent;
		private List<Drawable> _children;
		protected float _drawOrder;
		protected float _finalAlpha = 1.0f;
		protected Matrix _mWorld = Matrix.Identity;
		#endregion

		/// <summary>
		/// Gets or sets its name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name
		{
			set { _name = value; }
			get { return _name; }
		}
		/// <summary>
		/// Gets or sets the X position.
		/// It is relative to the parent`s position.
		/// </summary>
		/// <value>
		/// The X.
		/// </value>
		public float X
		{
			set
			{
				_position.X = value;
				_flag |= TransformFlag.Translate;
			}
			get { return _position.X; }
		}
		/// <summary>
		/// Gets or sets the Y position.
		/// It is relative to the parent`s position.
		/// </summary>
		/// <value>
		/// The Y.
		/// </value>
		public float Y
		{
			set
			{
				_position.Y = value;
				_flag |= TransformFlag.Translate;
			}
			get { return _position.Y; }
		}
		/// <summary>
		/// Gets or sets the Z position.
		/// It is relative to the parent`s position.
		/// </summary>
		/// <value>
		/// The Z.
		/// </value>
		public float Z
		{
			set
			{
				_position.Z = value;
				_flag |= TransformFlag.Translate;
				this.UpdateOrderByZ();
			}
			get { return _position.Z; }
		}
		/// <summary>
		/// Gets or sets its position.
		/// The origin point(0,0,0) is at parent`s position
		/// So it`s the position relative to its parent`s position.
		/// Its absolute position is ( Vector3.Transform(this.Position + this.Offset, this.World) ).
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public Vector3 Position
		{
			set
			{
				_position = value;
				_flag |= TransformFlag.Translate;
				this.UpdateOrderByZ();
			}
			get { return _position; }
		}
		/// <summary>
		/// Gets or sets the offset X.
		/// When you move the drawable item by position,
		/// offset is always applied after moving.
		/// </summary>
		/// <value>
		/// The offset X.
		/// </value>
		public float OffsetX
		{
			set
			{
				_offset.X = value;
				_flag |= TransformFlag.Translate;
			}
			get { return _offset.X; }
		}
		/// <summary>
		/// Gets or sets the offset Y.
		/// When you move the drawable item by position,
		/// offset is always applied after moving.
		/// </summary>
		/// <value>
		/// The offset Y.
		/// </value>
		public float OffsetY
		{
			set
			{
				_offset.Y = value;
				_flag |= TransformFlag.Translate;
			}
			get { return _offset.Y; }
		}
		/// <summary>
		/// Gets or sets the offset Z.
		/// When you move the drawable item by position,
		/// offset is always applied after moving.
		/// </summary>
		/// <value>
		/// The offset Z.
		/// </value>
		public float OffsetZ
		{
			set
			{
				_offset.Z = value;
				_flag |= TransformFlag.Translate;
				this.UpdateOrderByZ();
			}
			get { return _offset.Z; }
		}
		/// <summary>
		/// Gets or sets the offset.
		/// When you move the drawable item by position,
		/// offset is always applied after moving.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public Vector3 Offset
		{
			set
			{
				_offset = value;
				_flag |= TransformFlag.Translate;
				this.UpdateOrderByZ();
			}
			get { return _offset; }
		}
		/// <summary>
		/// Gets or sets the rotate angle around X axis.
		/// </summary>
		/// <value>
		/// The angle in radius.
		/// </value>
		public float RotateX
		{
			set
			{
				_rotate.X = value;
				Matrix.CreateRotationX(value, out _mRotateX);
				_flag |= TransformFlag.Rotate;
			}
			get { return _rotate.X; }
		}
		/// <summary>
		/// Gets or sets the rotate angle around Y axis.
		/// </summary>
		/// <value>
		/// The angle in radius.
		/// </value>
		public float RotateY
		{
			set
			{
				_rotate.Y = value;
				Matrix.CreateRotationY(value, out _mRotateY);
				_flag |= TransformFlag.Rotate;
			}
			get { return _rotate.Y; }
		}
		/// <summary>
		/// Gets or sets the rotate angle around Z axis.
		/// </summary>
		/// <value>
		/// The angle in radius.
		/// </value>
		public float RotateZ
		{
			set
			{
				_rotate.Z = value;
				Matrix.CreateRotationZ(value, out _mRotateZ);
				_flag |= TransformFlag.Rotate;
			}
			get { return _rotate.Z; }
		}
		/// <summary>
		/// Gets or sets the horizontal scale.
		/// </summary>
		/// <value>
		/// The horizontal scale value, 1.0f to apply no scale, 2.0f to double the size, 0.5f to half the size.
		/// </value>
		public float ScaleX
		{
			set
			{
				_scale.X = value;
				_flag |= TransformFlag.Scale;
			}
			get { return _scale.X; }
		}
		/// <summary>
		/// Gets or sets the vertical scale.
		/// </summary>
		/// <value>
		/// The vertical scale value, 1.0f to apply no scale, 2.0f to double the size, 0.5f to half the size.
		/// </value>
		public float ScaleY
		{
			set
			{
				_scale.Y = value;
				_flag |= TransformFlag.Scale;
			}
			get { return _scale.Y; }
		}
		/// <summary>
		/// Gets the draw order.
		/// Drawable item with higher draw order value is drew first.
		/// </summary>
		public float DrawOrder
		{
			get { return _drawOrder; }
		}
		/// <summary>
		/// Gets or sets a value indicating
		/// whether this drawable item and its children`s GetReady(), Draw() methods will be called each frame.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if methods for drawing to be called; otherwise, <c>false</c>.
		/// </value>
		public bool IsVisible
		{
			set { _bVisible = value; }
			get { return _bVisible; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether its children`s GetReady(), Draw() methods will be called each frame.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if children`s methods for drawing to be called; otherwise, <c>false</c>.
		/// </value>
		public bool IsChildrenVisible
		{
			set { _bChildrenVisible = value; }
			get { return _bChildrenVisible; }
		}
		/// <summary>
		/// Gets its parent.
		/// Its parent`s transform and alpha will add to its transform and alpha.
		/// </summary>
		public Drawable Parent
		{
			get { return _parent; }
		}
		/// <summary>
		/// Gets its children.
		/// </summary>
		public List<Drawable> Children
		{
			get { return _children; }
		}
		/// <summary>
		/// Gets its world.
		/// World contains its global transform.
		/// </summary>
		public Matrix World
		{
			get { return _mWorld; }
		}
		/// <summary>
		/// Gets or sets its alpha.
		/// Its real alpha is ( this._finalAlpha = this.Alpha * this.Parent.Alpha )
		/// </summary>
		/// <value>
		/// The alpha.
		/// </value>
		public float Alpha
		{
			set
			{
				if (value < 0)
				{ _alpha = 0.0f; }
				else if (value > 1.0f)
				{ _alpha = 1.0f; }
				else
				{ _alpha = value; }
				this.UpdateAlpha();
			}
			get { return _alpha; }
		}

		/// <summary>
		/// Adds a drawable item to its children.
		/// </summary>
		/// <param name="draw">The drawable item.</param>
		public void Add(Drawable draw)
		{
			if (draw._parent != null)
			{
				draw._parent.Remove(draw);
			}
			draw._parent = this;
			if (_children == null)
			{
				_children = new List<Drawable>();
			}
			_children.Add(draw);
			draw.UpdateOrderByZ();
		}
		/// <summary>
		/// Removes a drawable item from its children.
		/// </summary>
		/// <param name="draw">The drawable item.</param>
		public void Remove(Drawable draw)
		{
			if (_children != null)
			{
				if (_children.Remove(draw))
				{
					draw._parent = null;
				}
			}
		}
		/// <summary>
		/// Clears all its children.
		/// </summary>
		public void Clear()
		{
			if (_children != null)
			{
				int count = _children.Count;
				for (int i = 0; i < count; i++)
				{
					_children[i]._parent = null;
				}
				_children.Clear();
			}
		}
		/// <summary>
		/// Updates its transforms including rotation, scaling and translation.
		/// And updated transforms are stored in protected member Matrix _mWorld.
		/// </summary>
		public void UpdateTransform()
		{
			if (_flag != TransformFlag.None)
			{
				_flag &= ~TransformFlag.World;
				if (_flag != TransformFlag.None)
				{
					if ((_flag & TransformFlag.Rotate) != 0)
					{
						_flag &= ~TransformFlag.Rotate;
					}
					if ((_flag & TransformFlag.Scale) != 0)
					{
						_flag &= ~TransformFlag.Scale;
						Matrix.CreateScale(ref _scale, out _mScale);
					}
					if ((_flag & TransformFlag.Translate) != 0)
					{
						_flag &= ~TransformFlag.Translate;
						Vector3 translation = _position + _offset;
						Matrix.CreateTranslation(ref translation, out _mTranslate);
					}
					_mLocalWorld = _mScale * _mRotateX * _mRotateY * _mRotateZ * _mTranslate;
				}
				_mWorld = (_parent == null ? _mLocalWorld : _mLocalWorld * _parent._mWorld);
				if (_children != null)
				{
					for (int i = 0; i < _children.Count; i++)
					{
						_children[i]._flag |= TransformFlag.World;
					}
				}
			}
		}
		/// <summary>
		/// Updates its alpha.
		/// </summary>
		private void UpdateAlpha()
		{
			_finalAlpha = (_parent == null ? _alpha : _alpha * _parent._finalAlpha);
			if (_children != null)
			{
				for (int i = 0; i < _children.Count; i++)
				{ _children[i].UpdateAlpha(); }
			}
		}
		/// <summary>
		/// Updates the draw order by Z.
		/// </summary>
		public void UpdateOrderByZ()
		{
			if (_parent != null && oGraphic.SortMode == SortMode.ZSort)
			{
				_parent._sortChildren = true;
				_drawOrder = _parent._drawOrder - _position.Z - _offset.Z;
			}
		}
		/// <summary>
		/// Method called each frame when the item is node of tree from current scene`s root.
		/// In the method GetItselfReady() method is called first and then the GetChildrenReady() method
		/// when the item is visible and children visible.
		/// </summary>
		protected virtual void GetReady()
		{
			if (_bVisible)
			{
				this.GetItselfReady();
			}
			if (_bChildrenVisible)
			{
				this.GetChildrenReady();
			}
		}
		/// <summary>
		/// Gets the item itself ready.
		/// In the method UpdateTransform() is invoked.
		/// </summary>
		protected virtual void GetItselfReady()
		{
			this.UpdateTransform();
			if (oGraphic.SortMode != SortMode.ZSort)
			{
				_drawOrder = Math.Abs(Plane.Transform(oHelper.StandardPlane, _mWorld).DotCoordinate(oCameraManager.CurrentCamera.Position));
			}
		}
		/// <summary>
		/// Gets the children ready.
		/// Invokes all children`s GetReady() method.
		/// </summary>
		protected virtual void GetChildrenReady()
		{
			if (_children != null)
			{
				if (_sortChildren)
				{
					_sortChildren = false;
					_children.Sort(DrawOrderComparer.Comparer);
				}
				for (int i = 0; i < _children.Count; i++)
				{
					_children[i].GetReady();
				}
			}
		}
		/// <summary>
		/// Test whether a ray is running through the drawable item.
		/// </summary>
		/// <param name="ray">A ray.</param>
		/// <returns>When the ray is through the item it returns the distance between ray start point
		/// and the point where they intersect. Otherwise returns null.</returns>
		public virtual float? Intersects(Ray ray)
		{
			return null;
		}
		/// <summary>
		/// Interface for draw method.
		/// </summary>
		public abstract void Draw();
		/// <summary>
		/// Picks a drawable item from a screen point.
		/// </summary>
		/// <param name="screenX">The screen X.</param>
		/// <param name="screenY">The screen Y.</param>
		/// <param name="root">Any node from the scene tree to be the searching entry.</param>
		/// <returns>The picked drawable item.</returns>
		public static Drawable Pick(int screenX, int screenY, Drawable root)
		{
			Vector3 nearScreenPoint = new Vector3(screenX, screenY, 0);
			Vector3 farScreenPoint = new Vector3(screenX, screenY, 1);
			Vector3 nearWorldPoint = oGame.GraphicsDevice.Viewport.Unproject(nearScreenPoint, oEffectManager.Projection, oEffectManager.View, Matrix.Identity);
			Vector3 farWorldPoint = oGame.GraphicsDevice.Viewport.Unproject(farScreenPoint, oEffectManager.Projection, oEffectManager.View, Matrix.Identity);
			Ray ray = new Ray(nearWorldPoint, farWorldPoint - nearWorldPoint);
			Drawable result = null;
			float? distance = null;
			Drawable.Traverse
			(
				root,
				drawable =>
				{
					float? d = drawable.Intersects(ray);
					if (d != null)
					{
						if (distance == null || d <= distance)
						{
							distance = d;
							result = drawable;
						}
					}
				}
			);
			return result;
		}
		/// <summary>
		/// Traverses the scene tree from the specified node.
		/// </summary>
		/// <param name="root">The root node.</param>
		/// <param name="operation">The operation to each traversed node.</param>
		public static void Traverse(Drawable root, Action<Drawable> operation)
		{
			operation(root);
			if (root.Children != null)
			{
				for (int i = 0; i < root.Children.Count; i++)
				{
					Drawable.Traverse(root.Children[i], operation);
				}
			}
		}
	}
}
