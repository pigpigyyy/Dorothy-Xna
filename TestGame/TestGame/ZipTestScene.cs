using System.IO;
using Dorothy;
using Dorothy.Animations;
using Dorothy.Cameras;
using Dorothy.Core;
using Dorothy.Data;
using Dorothy.Defs;
using Dorothy.Game;
using Dorothy.Paints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestGame.Debugs;
using Dorothy.Effects;

namespace TestGame
{
    class ZipTestScene : Scene
    {
        bool _bInatialized = false;
        World _world;
        Unit _terrain;
        Role _role;
        MyDebugDraw _debugDraw;
        DebugString _s = new DebugString();
        Mirror _mirror;
        QuakeCamera _flCamera;
        MouseState _originMs;
        Sprite sp;

        public ZipTestScene(Game game)
            : base(game, "ZipTest")
        { }
        public override void Initialize()
        {
            //BasicCamera cam = new BasicCamera();
            //cam.Set(new Vector3(0, 0, -500), new Vector3(0, 0, -499));
            //oCameraManager.Apply(cam);
            if (_bInatialized)
            {
                oCameraManager.Apply("Cam1");
                return;
            }
            _bInatialized = true;
            oGraphic.SortMode = SortMode.AllSort;
            _world = new World(new Vector2(0, -10));
            //_world.Enable = false;
            _debugDraw = new MyDebugDraw();
            _debugDraw.Flags = Box2D.DebugDrawFlags.Shape | Box2D.DebugDrawFlags.AABB;
            _world.B2World.DebugDraw = _debugDraw;
            _debugDraw.Is3D = false;
            _world.Add(_debugDraw);
            //_debugDraw.OffsetZ = 50;
            oGame.Instance.IsMouseVisible = true;
			//oGame.IsFullScreen = false;
            oGroupManager.SetShouldContact(Group.PlayerOne, Group.PlayerOne, true);

            oGraphic.IsPostProcessed = false;
            BlurEffect blurEffect = new BlurEffect(oGame.GraphicsDevice);
            blurEffect.BlurSize = 0.0f;
            oEffectManager.Add(blurEffect);
            SaturationEffect saturationEffect = new SaturationEffect(oGame.GraphicsDevice);
            saturationEffect.Saturation = 0.0f;
            oEffectManager.Add(saturationEffect);

            _mirror = new Mirror(400, 300);
            _mirror.Name = "Mirror0";
            _mirror.RotateY = MathHelper.PiOver4;
            _mirror.Is3D = true;
            _mirror.X = -200;
            _mirror.Z = -50;
            Root.Add(_mirror);

            _mirror = new Mirror(400, 300);
            _mirror.Name = "Mirror1";
            _mirror.Is3D = true;
            _mirror.Z = -250;
			Root.Add(_mirror);

			_flCamera = new QuakeCamera();
			_flCamera.Name = "Cam1";
			_flCamera.Position = new Vector3(0, 0, 200);
			oCameraManager.Add(_flCamera);
			oCameraManager.Apply("Cam1");
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            _originMs = Mouse.GetState();
        }
        public override void Update()
        {
			_flCamera.RotateX += (oInput.MouseY - _originMs.Y) * 0.01f;
							_flCamera.RotateY += (oInput.MouseX - _originMs.X) * 0.01f;
							Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
						
			if (oInput.IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }
            if (oInput.IsKeyDown(Keys.U))
            {
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
                RepeatSpriteDef spChildDef = new RepeatSpriteDef();
                spChildDef.Name = "mikuSpChild";
                spChildDef.TexName = "mikuTex2";
                spChildDef.RepeatX = spChildDef.RepeatY = 3;
                spChildDef.Alpha = 1.0f;
                spChildDef.OffsetZ = 5.0f;
                spChildDef.Is3D = false;
                modDef.SpriteDef.Children = new SpriteDef[1];
                modDef.SpriteDef.Children[0] = spChildDef;
                modDef.AnimationDefs = new AnimationDef[1];
                RotateZDef rotateDef = new RotateZDef();
                rotateDef.Name = "Rotate";
                rotateDef.From = 0;
                rotateDef.To = MathHelper.PiOver2;
                rotateDef.Duration = 800;
                rotateDef.EaserID = Easer.Back_Out_Cubic.ID;
                rotateDef.TargetName = "mikuSp";
                rotateDef.Reverse = true;
                rotateDef.Loop = true;
                modDef.AnimationDefs[0] = rotateDef;
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
                levelDef.SpriteDefs = new SpriteDef[1];
                levelDef.SpriteDefs[0] = new SpriteDef();
                levelDef.SpriteDefs[0].ScaleX = levelDef.SpriteDefs[0].ScaleY = 5.0f;
                levelDef.SpriteDefs[0].Name = "bk";
                levelDef.SpriteDefs[0].TexName = "mikuTex";
                levelDef.SpriteDefs[0].DrawRectangle = new Rectangle(0, 0, 240, 320);
                levelDef.SpriteDefs[0].Is3D = true;
                levelDef.Model2Ds = new string[1];
                levelDef.Model2Ds[0] = "mikuMod.zip";
                levelDef.UnitDefs = new UnitDef[2];
                EdgeUnitDef edgeDef = new EdgeUnitDef();
                edgeDef.Name = "terrain";
                edgeDef.BodyType = Box2D.BodyType.Static;
                edgeDef.Edges = new EdgeDef[4];
                edgeDef.Edges[0] = new EdgeDef(-800, 800, 800, 800);
                edgeDef.Edges[1] = new EdgeDef(800, 800, 800, -400);
                edgeDef.Edges[2] = new EdgeDef(800, -400, -800, -400);
                edgeDef.Edges[3] = new EdgeDef(-800, -400, -800, 800);
                levelDef.UnitDefs[0] = edgeDef;
                RoleDef roleDef = new RoleDef();
                roleDef.Name = "miku";
                roleDef.Width = 240;
                roleDef.Height = 320;
                roleDef.ModName = "mikuMod";
                roleDef.FixedRotation = true;
                levelDef.UnitDefs[1] = roleDef;
                using (DataSaver saver = new DataSaver("level.zip"))
                {
                    saver.Save(levelDef);
                }
            }
            if (oInput.IsKeyDown(Keys.L))
            {
                oContent.LoadLevel("Level.zip");
                if (_terrain != null)
                {
                    _terrain.Dispose();
                }
                if (_role != null)
                {
                    _role.Dispose();
                }
                _terrain = oContent.GetUnit("terrain", _world, 0, 0, 0);
                _role = (Role)oContent.GetUnit("miku", _world, 0, 300, 0);
                sp = oContent.GetSprite("bk");
                sp.Is3D = false;
                sp.Z = -300;
                sp.R = 0.5f;
                sp.G = 0.5f;
                sp.B = 0.5f;
                Root.Add(sp);
                RectanglePaint paint = new RectanglePaint(200, 200, Color.Cyan, Color.Green);
                paint.Z = 50;
                //paint.Alpha = 0.8f;
                paint.IsSolid = true;
                paint.Is3D = false;
                //_role.Model2D.Children[0].Children[0].IsVisible = false;
                //_role.Model2D.Add(paint);
                //_role.Model2D.Play("Rotate");
            }
            if (_role != null)
            {
                if (oInput.GetKeyState(Keys.Up))
                {
                    if (_role.IsOnSurface)
                    {
                        _role.VelocityY = 800;
                    }
                }
                if (oInput.GetKeyState(Keys.Down))
                {
                    _role.VelocityY = -200;
                }
                if (_role.IsOnSurface)
                {
                    if (oInput.GetKeyState(Keys.Left))
                    {
                        _role.ApplyForce(MathHelper.Pi, 200);
                    }
                    if (oInput.GetKeyState(Keys.Right))
                    {
                        _role.ApplyForce(0, 200);
                    }
                }
            }
            if (oInput.GetKeyState(Keys.N))
            {
                _world.RotateX += 0.01f;
                _world.Alpha += 0.01f;
            }
            if (oInput.GetKeyState(Keys.Y))
            {
                _world.RotateX -= 0.01f;
                _world.Alpha -= 0.01f;
            }
            if (oInput.IsKeyDown(Keys.M))
            {
                oGame.IsFullScreen = !oGame.IsFullScreen;
                //oGraphic.IsPostProcessed = !oGraphic.IsPostProcessed;
                Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
                _originMs = Mouse.GetState();
            }
            if (oInput.GetKeyState(Keys.W))
            {
                _flCamera.MoveForwardBy(1.0f);
            }
            if (oInput.GetKeyState(Keys.S))
            {
                _flCamera.MoveForwardBy(-1.0f);
            }
            if (oInput.GetKeyState(Keys.A))
            {
                _flCamera.MoveRightBy(-1.0f);
            }
            if (oInput.GetKeyState(Keys.D))
            {
                _flCamera.MoveRightBy(1.0f);
            }
            if (oInput.GetKeyState(Keys.Q))
            {
                //_flCamera.RotateHorizontalBy(0.01f);
            }
            if (oInput.GetKeyState(Keys.E))
            {
                //_flCamera.RotateHorizontalBy(-0.01f);
            }
            if (oInput.GetKeyState(Keys.R))
            {
                //_flCamera.RotateVerticalBy(0.01f);
            }
            if (oInput.GetKeyState(Keys.T))
            {
                //_flCamera.RotateVerticalBy(-0.01f);
            }
            if (oInput.IsKeyDown(Keys.P))
            {
                _debugDraw.IsVisible = !_debugDraw.IsVisible;
            }
            if (oInput.IsKeyDown(Keys.V))
            {
                oGame.Instance.IsMouseVisible = false;
                oSceneManager.SwitchScene("Test");
            }
            if (oInput.IsKeyDown(Keys.C))
            {
                using (FileStream stream = new FileStream("mirror.png",FileMode.Create))
                {
                    _mirror._face.SaveAsPng(stream, _mirror._face.Width, _mirror._face.Height);
                }
            }
            //_s.Text = string.Empty;
            Drawable result = Drawable.Pick(oInput.MouseX, oInput.MouseY, Root);
            if (result != null)
            {
               _s.Text = result.Name;
            }
            base.Update();
        }

        public override void Dispose()
        { }
    }
}
