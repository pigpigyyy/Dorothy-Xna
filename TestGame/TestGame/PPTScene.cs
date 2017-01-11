using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dorothy.Core;
using Microsoft.Xna.Framework;
using Dorothy.Cameras;
using Microsoft.Xna.Framework.Input;
using Dorothy;
using Dorothy.Defs;
using Dorothy.Game;
using Dorothy.Data;
using Microsoft.Xna.Framework.Graphics;
using Dorothy.Animations;
using Dorothy.Paints;

namespace TestGame
{
	struct Item 
	{
		public string Name;
		public float Width;
		public float Height;
		public float X;
		public float Y;
		public Item(string name, float width, float height, float x, float y)
		{
			Name = name;
			Width = width;
			Height = height;
			X = x;
			Y = y;
		}
	};

	class PPTScene : Scene
	{
		private World _world;
		private Resource<Texture2D> _res = new Resource<Texture2D>();
		private Picker _picker;
		private LinePaint _line;
		private Sprite _curcor;

		private int _current = 0;
		private Item[] _items = new Item[]
		{
			new Item("1", 496, 112, 0, 300),
			new Item("2", 208, 112, 0, 300),
			new Item("3", 300, 84, 0, 300),
			new Item("4", 300, 84, 0, 300),
			new Item("5", 156, 84, 0, 300),
			new Item("6", 300, 84, 0, 300),
			new Item("7", 208, 112, 0, 300),
			new Item("8", 156, 84, 0, 300),
			new Item("9", 156, 84, 0, 300),
			new Item("10", 156, 84, 0, 300),
			new Item("11", 156, 84, 0, 300),
			new Item("12", 300, 84, 0, 300),
			new Item("13", 208, 112, 0, 300),
			new Item("14", 156, 84, 0, 300),
			new Item("15", 300, 84, 0, 300),
			new Item("16", 208, 112, 0, 300),
			new Item("17", 300, 84, 0, 300),
			new Item("18", 228,84, 0, 300),
			new Item("19", 228,84, 0, 300),
			new Item("20", 300, 84, 0, 300),
		};

		public PPTScene(Game game)
			: base(game, "PPTScene")
		{
		}
		public override void Initialize()
		{
			oGraphic.ZWriteEnable = false;
			oGraphic.SortMode = SortMode.AlphaSort;

			_curcor = new Sprite(Game.Content.Load<Texture2D>("Miku"));
			_curcor.DrawRectangle = new Rectangle(0, 0, 16, 16);
			_curcor.Is3D = false;
			this.Root.Add(_curcor);

			_world = new World(new Vector2(0, -10));
			_world.Enable = true;
			this.Root.Add(_world);

			_picker = new Picker(_world);
			_line = new LinePaint(new Vector2[] { Vector2.Zero, Vector2.Zero }, Color.Cyan);
			_line.IsVisible = false;
			this.Root.Add(_line);

			oGroupManager.SetShouldContact(Group.PlayerOne, Group.PlayerOne, true);
			oGroupManager.SetShouldContact(Group.PlayerOne, Group.Terrain, true);
			oGroupManager.SetShouldContact(Group.Terrain, Group.Terrain, false);

			DefaultCamera camera = (DefaultCamera)oCameraManager.CurrentCamera;

			#region SaveData
			/*
			Model2DDef modDef = new Model2DDef();
			modDef.Name = "mikuMod";
			modDef.Textures = new TextureDef[]
			{
				new TextureDef("mikuTex", "miku.png"),
				new TextureDef("mikuTex2", "mikup.png")
			};
			modDef.SpriteDef = new SpriteDef();
			modDef.SpriteDef.Name = "mikuSp";
			modDef.SpriteDef.TexName = "mikuTex";
			modDef.SpriteDef.DrawRectangle = new Rectangle(0, 0, 240, 320);
			modDef.SpriteDef.Is3D = false;
			modDef.AnimationDefs = new AnimationDef[2];
			SequenceDef seqDef = new SequenceDef();
			seqDef.Name = "Zoom";
			seqDef.AnimationDefs = new AnimationDef[2];

			ParallelDef parDef1 = new ParallelDef();
			parDef1.AnimationDefs = new AnimationDef[2];
			ScaleXDef scaleXDef = new ScaleXDef();
			scaleXDef.From = 1.0f;
			scaleXDef.To = 1.2f;
			scaleXDef.Duration = 500;
			scaleXDef.EaserID = Easer.Back_Out_Cubic.ID;
			scaleXDef.TargetName = "mikuSp";
			ScaleYDef scaleYDef = new ScaleYDef();
			scaleYDef.From = 1.0f;
			scaleYDef.To = 1.2f;
			scaleYDef.Duration = 500;
			scaleYDef.EaserID = Easer.Back_Out_Cubic.ID;
			scaleYDef.TargetName = "mikuSp";
			parDef1.AnimationDefs[0] = scaleXDef;
			parDef1.AnimationDefs[1] = scaleYDef;

			ParallelDef parDef2 = new ParallelDef();
			parDef2.AnimationDefs = new AnimationDef[2];
			scaleXDef = new ScaleXDef();
			scaleXDef.From = 1.2f;
			scaleXDef.To = 1.0f;
			scaleXDef.Duration = 500;
			scaleXDef.EaserID = Easer.In_Cubic.ID;
			scaleXDef.TargetName = "mikuSp";
			scaleYDef = new ScaleYDef();
			scaleYDef.From = 1.2f;
			scaleYDef.To = 1.0f;
			scaleYDef.Duration = 500;
			scaleYDef.EaserID = Easer.In_Cubic.ID;
			scaleYDef.TargetName = "mikuSp";
			parDef2.AnimationDefs[0] = scaleXDef;
			parDef2.AnimationDefs[1] = scaleYDef;

			seqDef.AnimationDefs[0] = parDef1;
			seqDef.AnimationDefs[1] = parDef2;
			modDef.AnimationDefs[0] = seqDef;

			RotateZDef rotateDef = new RotateZDef();
			rotateDef.Name = "Rotate";
			rotateDef.From = 0;
			rotateDef.To = MathHelper.Pi*2;
			rotateDef.Duration = 2000;
			rotateDef.EaserID = Easer.Back_Out_Cubic.ID;
			rotateDef.TargetName = "mikuSp";
			rotateDef.Reverse = true;
			rotateDef.Loop = true;
			modDef.AnimationDefs[1] = rotateDef;

			using (DataSaver saver = new DataSaver("mikuMod.zip"))
			{
				saver.Save(modDef);
			}
			LevelDef levelDef = new LevelDef();
			levelDef.Name = "mikuShow";
			levelDef.Textures = new TextureDef[]
			{
				new TextureDef("mikuTex", "miku.png")
			};
			levelDef.Model2Ds = new string[] { "mikuMod.zip" };
			levelDef.UnitDefs = new UnitDef[2];
			EdgeUnitDef edgeDef = new EdgeUnitDef();
			edgeDef.Group = Group.Terrain;
			edgeDef.Name = "terrain";
			edgeDef.BodyType = Box2D.BodyType.Static;
			edgeDef.Edges = new EdgeDef[]
			{
				new EdgeDef(-512, -300, 512, -300)
			};
			levelDef.UnitDefs[0] = edgeDef;
			RoleDef roleDef = new RoleDef();
			roleDef.Group = Group.PlayerOne;
			roleDef.Name = "miku";
			roleDef.Width = 0;
			roleDef.Height = 0;
			roleDef.ModName = "mikuMod";
			roleDef.FixedRotation = false;
			levelDef.UnitDefs[1] = roleDef;
			using (DataSaver saver = new DataSaver("level.zip"))
			{
				saver.Save(levelDef);
			}
			*/
			#endregion

			oContent.LoadLevel("Level.zip");
			oContent.GetUnit("terrain", _world, 0, 0, 0);
			Role role = (Role)oContent.GetUnit("miku", _world, 0, 300, 0);
			role.AttachRectangle(16, 16, 1.0f, 0.4f, 0.4f);
			Sprite sp = (Sprite)role.Model2D.Children[0];
			sp.Texture = _curcor.Texture;
			sp.DrawRectangle = new Rectangle(0, 0, 16, 16);

			UnitDef unitDef = new UnitDef();
			unitDef.BodyType = Box2D.BodyType.Static;
			Unit sensorUnit = unitDef.ToUnit(_world, 0, 0, 0);
			Sensor sensor = sensorUnit.AttachRectangleSensor(1024, 600, Vector2.Zero, 0);
			sensor.UnitLeave += new SensorHandler(sensor_UnitLeave);
		}
		void sensor_UnitLeave(Unit sensorUnit, Unit sensedUnit)
		{
			Timer timer = new Timer(500);
			timer.EventData = sensedUnit;
			timer.OnTimer += new Action<Timer>(timer_OnTimer);
		}
		void timer_OnTimer(Timer obj)
		{
			((Unit)(obj.EventData)).Dispose();
		}

		public Role CreateItem(int index)
		{
			Texture2D tex = Game.Content.Load<Texture2D>(_items[index].Name);
			Role role = (Role)oContent.GetUnit("miku", _world, _items[index].X, _items[index].Y, 0);
			//role.AttachRectangle(_items[index].Width, _items[index].Height, 1.0f, 0.8f, 0.0f);
			role.AttachRectangle(tex.Width, tex.Height, 1.0f, 0.6f, 0.4f);
			Sprite sp = (Sprite)role.Model2D.Children[0];
			sp.Texture = tex;
			sp.DrawRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
			return role;
		}

		public override void Update()
		{
			base.Update();
			_curcor.Position = new Vector3(
				oInput.MouseX - oGame.GraphicsDevice.Viewport.Width / 2,
				oGame.GraphicsDevice.Viewport.Height / 2 - oInput.MouseY, 0);

			if (oInput.IsKeyDown(Keys.D))
			{
				//Role role = (Role)oContent.GetUnit("miku", _world, 0, 300, 0);
				CreateItem(2);
				//role.Model2D.Play("Zoom");
			}
			if (oInput.IsKeyDown(Keys.Z))
			{
				if (_current - 1 >= 0)
				{
					_current--;
					CreateItem(_current);
				}
			}
			if (oInput.IsKeyDown(Keys.X))
			{
				if (_current + 1 < _items.Length)
				{
					_current++;
					CreateItem(_current);
				}
			}
			if (oInput.IsKeyDown(Keys.Escape))
			{
				oGame.Instance.Exit();
			}

			if (!_picker.IsPicking)
			{
				if (oInput.IsLeftButtonDown)
				{
					Vector2 pos = new Vector2(
						oInput.MouseX - oGame.GraphicsDevice.Viewport.Width / 2,
						oGame.GraphicsDevice.Viewport.Height / 2 - oInput.MouseY);
					if (_picker.Pick(ref pos) != null)
					{
						_line.Vertices = new Vector2[]{pos, pos};
						_line.IsVisible = true;
					}
				}
			}
			else
			{
				Vector2 pos = new Vector2(
						oInput.MouseX - oGame.GraphicsDevice.Viewport.Width / 2,
						oGame.GraphicsDevice.Viewport.Height / 2 - oInput.MouseY);
				_picker.Move(ref pos);
				Vector2 start, end;
				_picker.GetJointLine(out start, out end);
				_line.Vertices = new Vector2[]{ start, end };
				if (oInput.IsLeftButtonUp)
				{
					_picker.Drop();
					_line.IsVisible = false;
				}
			}
		}
		public override void Dispose()
		{
		}
	}
}
