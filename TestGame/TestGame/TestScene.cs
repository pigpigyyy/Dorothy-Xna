using Dorothy;
using Dorothy.Animations;
using Dorothy.Cameras;
using Dorothy.Core;
using Dorothy.Defs;
using Dorothy.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestGame.Debugs;

namespace TestGame
{
    class TestScene : Scene
    {
        QuakeCamera _flCamera;
        MouseState _originMs;
        Sprite _sprite;
        Sequence _seq;
        Parallel _par;
        Frame _frame;
        bool _bInitialized = false;
        EventListener _listenerUp;
        EventListener _listenerDown;
        DebugString _s;

        public TestScene(Game game)
            : base(game, "Test")
        { }

        public override void Initialize()
        {
            if (_bInitialized)
            {
                oCameraManager.Apply("Cam1");
                return;
            }
            _bInitialized = true;
            oEventManager.AddEventType(new EventType("Up"));
            oEventManager.AddEventType(new EventType("Down"));

            _listenerUp = new EventListener("Up", ObjectMoveUp);
			_listenerUp.Register = true;
            _listenerDown = new EventListener("Down", ObjectMoveDown);
			_listenerDown.Register = true;

            _flCamera = new QuakeCamera();
            _flCamera.Name = "Cam1";
            _flCamera.Position = new Vector3(0, 0, 200);
            oCameraManager.Add(_flCamera);
            oCameraManager.Apply("Cam1");

            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            _originMs = Mouse.GetState();

            Texture2D tex = Game.Content.Load<Texture2D>("bunny");
            _sprite = new Sprite(tex);
            _sprite.DrawRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            Root.Add(_sprite);

            FrameDef frameDef = new FrameDef("bunny", 3);
            frameDef.SetFrame(0, new Rectangle(1, 0, 28, 40), 100);
            frameDef.SetFrame(1, new Rectangle(29, 0, 28, 40), 100);
            frameDef.SetFrame(2, new Rectangle(57, 0, 27, 40), 100);
            frameDef.Loop = true;
            //frameDef.Reverse = true;

            _frame = new Frame(frameDef, tex);
            _frame.Target = _sprite;
            _frame.Loop = true;
            _frame.Reverse = true;
            _frame.Play();

            _par = new Parallel();
            MoveY moveY = new MoveY(-100, 100, 8000);
            moveY.Target = _sprite;
            _par.Add(moveY);
            _seq = new Sequence();
            MoveZ moveZ = new MoveZ(-100, 100, 3000);
            moveZ.Target = _sprite;
            _seq.Add(moveZ);
            RotateZ rotateZ = new RotateZ(0, MathHelper.TwoPi, 3000);
            rotateZ.Target = _sprite;
            _seq.Add(rotateZ);
            ScaleX scaleX = new ScaleX(1.0f, 1.5f, 2000);
            scaleX.Target = _sprite;
            _seq.Add(scaleX);
            _seq.Loop = true;
            _seq.Reverse = true;
            //_seq.Play();
            //_seq.Pause();
            _par.Loop = true;
            _par.Reverse = true;
            _par.Add(_seq);
            //_par.Play();
            _s = new DebugString();
            _s.Text = _par.Current.ToString("F2") + "/" + _par.Duration.ToString("F2");
            Root.Add(_s);
        }

        public void ObjectMoveUp(Event e)
        {
            _sprite.Y++;
        }
        public void ObjectMoveDown(Event e)
        {
            _sprite.Y--;
        }
        public void ObjectMoveLeft(Event e)
        {
            _sprite.X--;
        }
        public void ObjectMoveRight(Event e)
        {
            _sprite.X++;
        }

        public override void Update()
        {
            _s.Text = _par.Current.ToString("F2") + "/" + _par.Duration.ToString("F2");

            MouseState ms = Mouse.GetState();
            if (ms != _originMs)
            {
                _flCamera.RotateX += (ms.Y - _originMs.Y) * 0.01f;
                _flCamera.RotateY += (ms.X - _originMs.X) * 0.01f;
                Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            }
            if (oInput.IsKeyDown(Keys.Escape))
            {
                oGame.Instance.Exit();
            }
            if (oInput.IsKeyDown(Keys.V))
            {
                //HIHSceneManager.Instance.SwitchScene("Game");
            }
            if (oInput.IsKeyDown(Keys.P))
            {
                if (_frame.IsPaused)
                {
                    _frame.Resume();
                }
                else
                {
                    _frame.Pause();
                }
            }
            if (oInput.GetKeyState(Keys.Up))
            {
                //oEventManager.SendEvent(new Event("Up"));
                _par.Current += 20.0f;
                //_flCamera.RotateVertical(0.01f);
            }
            if (oInput.GetKeyState(Keys.Down))
            {
                oEventManager.SendEvent(new Event("Down"));
                _par.Current -= 20.0f;
                //_flCamera.RotateVertical(-0.01f);
            }
            if (oInput.GetKeyState(Keys.Left))
            {
                //_flCamera.RotateHorizontal(0.01f);
            }
            if (oInput.GetKeyState(Keys.Right))
            {
                //_flCamera.RotateHorizontal(-0.01f);
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
                //_flCamera.MoveUp(1.0f);
            }
            if (oInput.GetKeyState(Keys.E))
            {
                //_flCamera.MoveUp(-1.0f);
            }
            if (oInput.IsKeyDown(Keys.R))
            {
                _listenerUp.Register = !_listenerUp.Register;
            }
            if (oInput.IsKeyDown(Keys.T))
            {
                _listenerDown.Register = !_listenerDown.Register;
            }
            if (oInput.IsKeyDown(Keys.V))
            {
                oGame.Instance.IsMouseVisible = true;
                oSceneManager.SwitchScene("ZipTest");
            }
            base.Update();
        }
        public override void Dispose()
        {
            _listenerUp.Register = false;
            _listenerDown.Register = false;
        }
    }
}
