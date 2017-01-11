using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Box2D;
using Harmony.Core;
using Harmony.Cameras;
using Harmony.Animations;
using Harmony.Game;
using Harmony.Helpers;
using Harmony.Particles;
using Harmony.Game.Actions;
using HIHGame.Debugs;
using HIHGame.AISystem;
using HIHGame.Sounds;
using HIHGame.Misc;

namespace HIHGame
{
    class GameScene : Scene
    {
        bool _bInitialized = false;
        bool _bPauseGame = false;
        bool _bStartGame = false;
        DrawUnit _terrain;
        UnitCamera _unitCam;
        Harmony.Game.World _world;
        Character _char;
        Character _char1;
        AIManager _aiManager;

        Texture2D _soundTex;
        Texture2D _huaWalkTex;
        Texture2D _huaAttackTex;
        Texture2D _danWalkTex;
        Texture2D _danAttackTex;
        Texture2D _hpFrameTex;
        Texture2D _hpTex;
        Texture2D _dan1WalkTex;
        Texture2D _dan1AttackTex;

        FrameDef _soundDef;
        FrameDef _huaWalkDef;
        FrameDef _huaAttackDef;
        FrameDef _danWalkDef;
        FrameDef _danAttackDef;
        FrameDef _dan1WalkDef;
        FrameDef _dan1AttackDef;

        Sprite _clearScreen;
        Fade _fadeScreen;

        Timer _startTimer;
        Board _board;
        Texture2D[] _boardTexs;
        Timer _introTimer;
        int _countBoard = 0;
        DebugString s;

        public bool PauseGame
        {
            set
            {
                _bPauseGame = value;
                _world.Enable = !value;
            }
            get { return _bPauseGame; }
        }

        public GameScene(Game game)
            : base(game, "Game")
        {
        }

        public override void Initialize()
        {
            #region Initialize Part1
            if (_bInitialized)
            {
                return;
            }
            _bInitialized = true;
            //Root.World = Matrix.CreateRotationY(0.1f);

            HIHGroupManager gm = HIHGroupManager.Instance();

            #region should contact
            gm.SetShouldContact(Group.PlayerOne, Group.Terrain, true);
            gm.SetShouldContact(Group.PlayerOne, Group.PlayerOne, false);
            gm.SetShouldContact(Group.PlayerOne, Group.Destructable, true);
            gm.SetShouldContact(Group.PlayerOne, Group.JiangYou, false);

            gm.SetShouldContact(Group.PlayerTwo, Group.Terrain, true);
            gm.SetShouldContact(Group.PlayerTwo, Group.PlayerTwo, true);
            gm.SetShouldContact(Group.PlayerTwo, Group.Destructable, true);
            gm.SetShouldContact(Group.PlayerTwo, Group.JiangYou, false);

            gm.SetShouldContact(Group.PlayerThree, Group.Terrain, true);
            gm.SetShouldContact(Group.PlayerThree, Group.PlayerThree, true);
            gm.SetShouldContact(Group.PlayerThree, Group.Destructable, true);
            gm.SetShouldContact(Group.PlayerThree, Group.JiangYou, false);
            gm.SetShouldContact(Group.PlayerThree, Group.PlayerTwo, true);
            gm.SetShouldContact(Group.PlayerThree, Group.PlayerOne, false);

            gm.SetShouldContact(Group.PlayerOne, Group.PlayerTwo, false);

            gm.SetShouldContact(Group.Destructable, Group.Terrain, true);

            gm.SetShouldContact(Group.JiangYou, Group.Destructable, false);
            gm.SetShouldContact(Group.JiangYou, Group.Terrain, true);

            gm.SetShouldContact(Group.JiangYou, Group.JiangYou, false);
            gm.SetShouldContact(Group.Destructable, Group.Destructable, true);
            #endregion
            #region relation ship
            gm.SetRelationShip(Group.PlayerOne, Group.PlayerTwo, RelationShip.Enemy);
            gm.SetRelationShip(Group.PlayerTwo, Group.PlayerOne, RelationShip.Enemy);
            gm.SetRelationShip(Group.PlayerOne, Group.PlayerOne, RelationShip.Friend);

            gm.SetRelationShip(Group.PlayerThree, Group.PlayerTwo, RelationShip.Friend);
            gm.SetRelationShip(Group.PlayerTwo, Group.PlayerThree, RelationShip.Friend);
            gm.SetRelationShip(Group.PlayerThree, Group.PlayerOne, RelationShip.Enemy);
            gm.SetRelationShip(Group.PlayerOne, Group.PlayerThree, RelationShip.Enemy);
            gm.SetRelationShip(Group.PlayerThree, Group.PlayerThree, RelationShip.Friend);

            gm.SetRelationShip(Group.PlayerTwo, Group.Terrain, RelationShip.Neutral);
            gm.SetRelationShip(Group.Terrain, Group.PlayerTwo, RelationShip.Neutral);
            gm.SetRelationShip(Group.PlayerOne, Group.Terrain, RelationShip.Neutral);
            gm.SetRelationShip(Group.Terrain, Group.PlayerOne, RelationShip.Neutral);
            gm.SetRelationShip(Group.PlayerThree, Group.Terrain, RelationShip.Neutral);
            gm.SetRelationShip(Group.Terrain, Group.PlayerThree, RelationShip.Neutral);

            gm.SetRelationShip(Group.JiangYou, Group.PlayerTwo, RelationShip.Neutral);
            gm.SetRelationShip(Group.PlayerTwo, Group.JiangYou, RelationShip.Neutral);
            gm.SetRelationShip(Group.JiangYou, Group.PlayerOne, RelationShip.Neutral);
            gm.SetRelationShip(Group.PlayerOne, Group.JiangYou, RelationShip.Neutral);
            gm.SetRelationShip(Group.JiangYou, Group.PlayerThree, RelationShip.Neutral);
            gm.SetRelationShip(Group.PlayerThree, Group.JiangYou, RelationShip.Neutral);
            #endregion

            _world = new Harmony.Game.World(new Vector2(0, -10.0f), this.Controller);
            _world.IsRunning = true;
            _aiManager = new AIManager();

            #region TexLoad
            _huaWalkTex = Game.Content.Load<Texture2D>("huawalk");
            _huaAttackTex = Game.Content.Load<Texture2D>("huaattack");
            _danWalkTex = Game.Content.Load<Texture2D>("danwalk");
            _danAttackTex = Game.Content.Load<Texture2D>("danattack");
            _hpFrameTex = Game.Content.Load<Texture2D>("bloodframe");
            _hpTex = Game.Content.Load<Texture2D>("blood");
            _soundTex = Game.Content.Load<Texture2D>("sound");
            _dan1WalkTex = Game.Content.Load<Texture2D>("dan1walk");
            _dan1AttackTex = Game.Content.Load<Texture2D>("dan1attack");
            #endregion
            #region huaFrameDef
            _huaWalkDef = new FrameDef();
            _huaWalkDef.Name = "Walk";
            _huaWalkDef.SetUp(_huaWalkTex, 11);
            _huaWalkDef.SetFrame(0, new Rectangle((int)(186.18 * 0), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(1, new Rectangle((int)(186.18 * 1), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(2, new Rectangle((int)(186.18 * 2), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(3, new Rectangle((int)(186.18 * 3), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(4, new Rectangle((int)(186.18 * 4), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(5, new Rectangle((int)(186.18 * 5), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(6, new Rectangle((int)(186.18 * 6), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(7, new Rectangle((int)(186.18 * 7), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(8, new Rectangle((int)(186.18 * 8), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(9, new Rectangle((int)(186.18 * 9), 0, 186, 181), 200);
            _huaWalkDef.SetFrame(10, new Rectangle((int)(186.18 * 10), 0, 186, 181), 200);
            _huaWalkDef.Speed = 3.0f;

            _huaAttackDef = new FrameDef();
            _huaAttackDef.Name = "Attack";
            _huaAttackDef.SetUp(_huaAttackTex, 9);
            _huaAttackDef.SetFrame(0, new Rectangle((int)(186.22 * 0), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(1, new Rectangle((int)(186.22 * 1), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(2, new Rectangle((int)(186.22 * 2), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(3, new Rectangle((int)(186.22 * 3), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(4, new Rectangle((int)(186.22 * 4), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(5, new Rectangle((int)(186.22 * 5), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(6, new Rectangle((int)(186.22 * 6), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(7, new Rectangle((int)(186.22 * 7), 0, 186, 181), 200);
            _huaAttackDef.SetFrame(8, new Rectangle((int)(186.22 * 8), 0, 186, 181), 200);
            _huaAttackDef.Reverse = true;
            _huaAttackDef.Speed = 12.0f;
            #endregion
            #region danFrameDef
            _danWalkDef = new FrameDef();
            _danWalkDef.Name = "Walk";
            _danWalkDef.SetUp(_danWalkTex, 3);
            _danWalkDef.SetFrame(0, new Rectangle((int)(132.33 * 0), 0, 132, 170), 200);
            _danWalkDef.SetFrame(1, new Rectangle((int)(132.33 * 1), 0, 132, 170), 200);
            _danWalkDef.SetFrame(2, new Rectangle((int)(132.33 * 2), 0, 132, 170), 200);
            _danWalkDef.Reverse = true;
            _danWalkDef.Speed = 3.0f;

            _danAttackDef = new FrameDef();
            _danAttackDef.Name = "Attack";
            _danAttackDef.SetUp(_danAttackTex, 6);
            _danAttackDef.SetFrame(0, new Rectangle((int)(133.666f * 0), 0, 134, 170), 200);
            _danAttackDef.SetFrame(1, new Rectangle((int)(133.666f * 1), 0, 134, 170), 200);
            _danAttackDef.SetFrame(2, new Rectangle((int)(133.666f * 2), 0, 134, 170), 200);
            _danAttackDef.SetFrame(3, new Rectangle((int)(133.666f * 3), 0, 134, 170), 200);
            _danAttackDef.SetFrame(4, new Rectangle((int)(133.666f * 4), 0, 134, 170), 200);
            _danAttackDef.SetFrame(5, new Rectangle((int)(133.666f * 5), 0, 134, 170), 200);
            _danAttackDef.Speed = 1.0f;
            #endregion
            #region dan1FrameDef
            _dan1WalkDef = new FrameDef();
            _dan1WalkDef.Name = "Walk";
            _dan1WalkDef.SetUp(_dan1WalkTex, 7);
            _dan1WalkDef.SetFrame(0, new Rectangle((int)(292.57 * 0), 0, 292, 228), 200);
            _dan1WalkDef.SetFrame(1, new Rectangle((int)(292.57 * 1), 0, 292, 228), 200);
            _dan1WalkDef.SetFrame(2, new Rectangle((int)(292.57 * 2), 0, 292, 228), 200);
            _dan1WalkDef.SetFrame(3, new Rectangle((int)(292.57 * 3), 0, 292, 228), 200);
            _dan1WalkDef.SetFrame(4, new Rectangle((int)(292.57 * 4), 0, 292, 228), 200);
            _dan1WalkDef.SetFrame(5, new Rectangle((int)(292.57 * 5), 0, 292, 228), 200);
            _dan1WalkDef.SetFrame(6, new Rectangle((int)(292.57 * 6), 0, 292, 228), 200);
            _dan1WalkDef.Speed = 3.0f;

            _dan1AttackDef = new FrameDef();
            _dan1AttackDef.Name = "Attack";
            _dan1AttackDef.SetUp(_dan1AttackTex, 7);
            _dan1AttackDef.SetFrame(0, new Rectangle((int)(292.57 * 0), 0, 292, 228), 200);
            _dan1AttackDef.SetFrame(1, new Rectangle((int)(292.57 * 1), 0, 292, 228), 200);
            _dan1AttackDef.SetFrame(2, new Rectangle((int)(292.57 * 2), 0, 292, 228), 200);
            _dan1AttackDef.SetFrame(3, new Rectangle((int)(292.57 * 3), 0, 292, 228), 200);
            _dan1AttackDef.SetFrame(4, new Rectangle((int)(292.57 * 4), 0, 292, 228), 200);
            _dan1AttackDef.SetFrame(5, new Rectangle((int)(292.57 * 5), 0, 292, 228), 200);
            _dan1AttackDef.SetFrame(6, new Rectangle((int)(292.57 * 6), 0, 292, 228), 200);
            _dan1AttackDef.Reverse = true;
            _dan1AttackDef.Speed = 3.0f;
            #endregion
            #region soundDef
            _soundDef = new FrameDef();
            _soundDef.Name = "Sound";
            _soundDef.SetUp(_soundTex, 7);
            _soundDef.SetFrame(0, new Rectangle((int)(176 * 0), 0, 176, 169), 200);
            _soundDef.SetFrame(1, new Rectangle((int)(176 * 1), 0, 176, 169), 200);
            _soundDef.SetFrame(2, new Rectangle((int)(176 * 2), 0, 176, 169), 200);
            _soundDef.SetFrame(3, new Rectangle((int)(176 * 3), 0, 176, 169), 200);
            _soundDef.SetFrame(4, new Rectangle((int)(176 * 4), 0, 176, 169), 200);
            _soundDef.SetFrame(5, new Rectangle((int)(176 * 5), 0, 176, 169), 200);
            _soundDef.SetFrame(6, new Rectangle((int)(176 * 6), 0, 176, 169), 200);
            #endregion

            #region characters
            _char = CreateHua(Group.PlayerOne, -9450, 700, 101.0f, 20, 200, 600, 1.3f, 1.3f);
            _aiManager.Player = _char;
            _char1 = CreateDan1(Group.PlayerOne, -9600, 600, 100.9f, 20, 200, 600, 1.2f, 1.2f);
            _aiManager.Player1 = _char1;

            for (int i = 0; i < 30;i++ )
            {
                Character enemy;
                int rand = RandomHelper.NextInt(0, 3);
                if(rand == 1)
                {
                    float size = RandomHelper.NextFloat(1.0f, 1.5f);
                    enemy = CreateDan(Group.PlayerTwo, RandomHelper.NextFloat(-8000, 9500), 200, 100, 2, 120, 400, size, size, RandomHelper.NextFloat(0.9f, 1.3f), RandomHelper.NextFloat(0.5f, 1.5f), RandomHelper.NextFloat(0.9f, 1.3f));
                }
                else if(rand == 2)
                {
                    float size = RandomHelper.NextFloat(1.0f, 1.5f);
                    enemy = CreateDan1(Group.PlayerTwo, RandomHelper.NextFloat(-8000, 9500), 200, 100, 2, 150, 500, size, size, RandomHelper.NextFloat(0.9f, 1.3f), RandomHelper.NextFloat(0.5f, 1.5f), RandomHelper.NextFloat(0.9f, 1.3f));
                }
                else //if (rand == 2)
                {
                    float size = RandomHelper.NextFloat(1.0f, 1.5f);
                    enemy = CreateHua(Group.PlayerTwo, RandomHelper.NextFloat(-8000, 9500), 200, 100, 2, 100, 450, size, size, RandomHelper.NextFloat(-0.1f, 0.3f), RandomHelper.NextFloat(0.5f, 1.5f), RandomHelper.NextFloat(0.9f, 1.3f));
                }
                _aiManager.Add(enemy);
            }
            Character boss = CreateHua(Group.PlayerThree, 10000, 300, 99, 30, 300, 600, 2.0f, 2.0f, 2.0f, 1.0f, 1.0f);
            boss.Actions["Quit"].ActionEnd += new ActionHandler(GameScene_ActionEnd);
            _aiManager.Add(boss);

            #endregion

            #region terrain camera
            {
                DrawUnitDef def = new DrawUnitDef();
                def.BodyType = BodyType.Static;
                def.TexName = "floor";
                def.ZWriteEnable = true;
                def.RepeatX = 40;
                def.ScaleX = 40;
                def.RotateX = MathHelper.PiOver2;
                _terrain = _world.CreateUnit(def);
                _terrain.AttachEdge(-10240, 1024,10240, 1024, 0.0f, 0.4f);
                _terrain.AttachEdge(10240, 1024, 10240, 0, 0.0f, 0.4f);
                _terrain.AttachEdge(10240, 0, -10240, 0, 0.4f, 0.4f);
                _terrain.AttachEdge(-10240, 0, -10240, 1024, 0.0f, 0.4f);
                _terrain.Group = Group.Terrain;
                this.Root.Add(_terrain.Sprite);
            }

            _unitCam = new UnitCamera(
                _char,
                new Vector2(40.0f, 40.0f),
                new Vector2(80.0f, 80.0f),
                new Vector2(-9550, 465),
                new Vector2(9550, -9550),
                new Vector2(190, 190),
                new Vector3(0.0f, 120.0f, 500.0f));
            _unitCam.Start();
            #endregion
            #endregion

            #region bg
            CreateDecoration("bg1", -9216, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg2", -7168, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg3", -5120, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg4", -3072, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg5", -1024, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg6", 1024, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg7", 3072, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg8", 5120, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg9", 7168, 512, -5).ZWriteEnable = true;
            CreateDecoration("bg10", 9216, 512, -5).ZWriteEnable = true;
            Texture2D tex1 = HIHGame.Instance().Content.Load<Texture2D>("bg1");
            Sprite sprite = new Sprite(tex1);
            sprite.RotateY = MathHelper.PiOver2;
            sprite.DrawRectangle = new Rectangle(0, 0, tex1.Width, tex1.Height);
            sprite.Position = new Vector3(-10240, 512, -5);
            Root.Add(sprite);
            sprite = new Sprite(tex1);
            sprite.RotateY = MathHelper.PiOver2;
            sprite.DrawRectangle = new Rectangle(0, 0, tex1.Width, tex1.Height);
            sprite.Position = new Vector3(10240, 512, -5);
            Root.Add(sprite);
            #endregion

            #region front
            for (int i = -7; i <= 7; i++)
            {
                CreateDecoration("gallery", i * 1427, 570, 200 + i * 0.1f);
            }
            for (int i = -10; i <= 10; i++)
            {
                CreateDecoration("lantern1", i * 951.333333f, 470, 180);
            }
            for (int i = -11; i <= 11; i++)
            {
                CreateDecoration("lantern2", 475.666666666f + i * 951.333333f, 470, 160);
            }
            for (int i = -22; i <= 22; i++)
            {
                CreateDecoration("pillar", 238 + i * 475.666666f, 287, 200.1f);
            }
            for (int i = -5; i <= 5; i++)
            {
                CreateDecoration("rail", i * 2048, 64, 310);
            }
            for (int i = -15; i < 15; i++ )
            {
                CreateDecoration("roof", i * 705, 700, 210);
            }
            #endregion

            #region chair table vase
            CreateContactableDecoration("chair3", -9271, 150 * 0.27f, 99.5f, 100, 70, 0.5f, 0.77f, BodyType.Dynamic);
            CreateContactableDecoration("table4", -8444, 67, 99, 151, 115, 0.5f, 0.57f);
            CreateContactableDecoration("chair2", -7000, 150 * 0.28f, 99, 108, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("table3", -6857, 63, 99.5f, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", -6677, 150 * 0.28f, 99, 108, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("table1", -5000, 76, 99, 153, 120, 0.5f, 0.6f);
            CreateContactableDecoration("chair4", -4600, 150 * 0.28f, 99, 190, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("chair1", -3000, 150 * 0.25f, 99, 98, 75, 0.5f, 0.75f, BodyType.Dynamic);
            CreateContactableDecoration("chair1", -2860, 150 * 0.25f, 99.5f, 98, 75, 0.5f, 0.75f, BodyType.Dynamic);
            CreateContactableDecoration("chair3",-1100, 150 * 0.27f, 99, 100, 70, 0.5f, 0.77f, BodyType.Dynamic);
            CreateContactableDecoration("table2", -943, 64, 99, 152, 106, 0.5f, 0.58f);
            CreateContactableDecoration("chair3", -773, 150 * 0.27f, 99, 100, 70, 0.5f, 0.77f, BodyType.Dynamic);
            CreateContactableDecoration("chair1", -30, 150 * 0.25f, 99, 98, 75, 0.5f, 0.75f, BodyType.Dynamic);
            CreateContactableDecoration("chair1", 100, 150 * 0.25f, 99.5f, 98, 75, 0.5f, 0.75f, BodyType.Dynamic);
            CreateContactableDecoration("chair2", 1100, 150 * 0.28f, 99, 108, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("table3", 1255, 63, 99.5f, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", 1429, 150 * 0.28f, 99, 108, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("table1", -2600, 76, 99, 153, 120, 0.5f, 0.6f);
            CreateContactableDecoration("chair3", 3700, 150 * 0.27f, 99, 100, 70, 0.5f, 0.77f, BodyType.Dynamic);
            CreateContactableDecoration("chair3", 3830, 150 * 0.27f, 99.5f, 100, 70, 0.5f, 0.77f, BodyType.Dynamic);
            CreateContactableDecoration("table2", 4700, 64, 99, 152, 106, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", 5350, 150 * 0.28f, 99, 108, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("chair2", 5470, 150 * 0.28f, 99.5f, 108, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("table3", 1255, 63, 99, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair2", 5945, 150 * 0.28f, 99.5f, 108, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("chair4", 6500, 150 * 0.28f, 99, 190, 65, 0.5f, 0.78f, BodyType.Dynamic);
            CreateContactableDecoration("table3", 7777, 63, 99, 180, 104, 0.5f, 0.58f);
            CreateContactableDecoration("chair1", 8700, 150 * 0.25f, 99, 98, 75, 0.5f, 0.75f, BodyType.Dynamic);
            CreateContactableDecoration("table4", 8850, 67, 99.5f, 151, 115, 0.5f, 0.57f);
            CreateContactableDecoration("chair1", 9029, 150 * 0.25f, 99, 98, 75, 0.5f, 0.75f, BodyType.Dynamic);
            CreateContactableDecoration("chair3", 9758, 150 * 0.27f, 99, 100, 70, 0.5f, 0.77f, BodyType.Dynamic);
            CreateContactableDecoration("chair3", 9888, 150 * 0.27f, 99.5f, 100, 70, 0.5f, 0.77f, BodyType.Dynamic);

            CreateDecoration("vase1", -9000, 75, 99);
            CreateDecoration("vase1", -8900, 75, 99);
            CreateDecoration("vase2", -6000, 91, 99);
            CreateDecoration("vase3", -2120, 94, 99);
            CreateDecoration("vase3", -2000, 94, 99);
            CreateDecoration("vase4", 500, 116, 99);
            CreateDecoration("vase1", 1500, 75, 99.5f);
            CreateDecoration("vase2", 2400, 91, 99);
            CreateDecoration("vase2", 3520, 91, 99);
            CreateDecoration("vase1", 6900, 75, 99);
            CreateDecoration("vase3", 8200, 94, 99);
            #endregion

            #region dragonfly
            CreateDragonfly("dragonfly1", 50, -4000,-3000, 200,250);
            CreateDragonfly("dragonfly2", 50, -3930, -4300, 200, 140);
            CreateDragonfly("dragonfly2", 150, 0,400,  280, 130, 4000);
            CreateDragonfly("dragonfly1",150, 1400, 1000, 340, 270, 5000);
            CreateDragonfly("dragonfly2", 50, 2900, 3800,  200, 470, 5000);
            CreateDragonfly("dragonfly1", 50, 3050, 3200, 200, 500, 3000);
            CreateDragonfly("dragonfly2", 150, 7000, 6300, 130, 240, 4000);
            CreateDragonfly("dragonfly1", 150, 9000, 10200, 270, 300, 4000);
            CreateDecoration("dragonfly2", 9070, 200, 50);
            #endregion

            #region Plant
            CreateDecoration("plant1", -9980, 100, 50);
            CreateDecoration("plant2", -8372, 100, 50);
            CreateDecoration("plant3", -7263, 100, 50);
            CreateDecoration("plant10", -7000, 100, 50);
            CreateDecoration("plant4", -6573, 100, 50);
            CreateDecoration("plant3", -6000, 100, 50);
            CreateDecoration("plant5", -5555, 100, 50);
            CreateDecoration("plant6", -4238, 100, 50);
            CreateDecoration("plant7", -3997, 100, 50);
            CreateDecoration("plant8", -2345, 100, 50);
            CreateDecoration("plant9", -1234, 100, 50);
            CreateDecoration("plant1", 0, 100, 50);
            CreateDecoration("plant2", 1000, 100, 50);
            CreateDecoration("plant8", -1345, 100, 50);
            CreateDecoration("plant3", 2000, 100, 50);
            CreateDecoration("plant4", 3000, 100, 50);
            CreateDecoration("plant7", 3200, 100, 50);
            CreateDecoration("plant5", 4000, 100, 50);
            CreateDecoration("plant5", -4855, 100, 50);
            CreateDecoration("plant6", 5000, 100, 50);
            CreateDecoration("plant7", 6000, 100, 50);
            CreateDecoration("plant8", 7000, 100, 50);
            CreateDecoration("plant9", 8000, 100, 50);
            CreateDecoration("plant10", 9000, 100, 50);

           CreateDecoration("plant3", -9843, 100, 200);
           CreateDecoration("plant5", -7855, 100, 200);
           CreateDecoration("plant8", -4438, 100, 200);
           CreateDecoration("plant7", -3997, 100, 200);
           CreateDecoration("plant8", -1345, 100, 200);
           CreateDecoration("plant4", -234, 100, 200);
           CreateDecoration("plant1", 1433, 100, 200);
           CreateDecoration("plant7", 1800, 100, 200);
           CreateDecoration("plant5", 3345, 100, 200);
           CreateDecoration("plant3", 4700, 100, 200);
           CreateDecoration("plant4", 5300, 100, 200);
           CreateDecoration("plant7", 6800, 100, 200);
           CreateDecoration("plant10", 7000, 100, 200);
           CreateDecoration("plant4", 873, 100, 200);
           CreateDecoration("plant3", 9350, 100, 200);

            CreateDecoration("plant5", -9740, 100, 300);
            CreateDecoration("plant5", -6200, 100, 300);
            CreateDecoration("plant3", -3300, 100, 300);
            CreateDecoration("plant7", 1300, 100, 300);
            CreateDecoration("plant5", 3300, 100, 300);
            CreateDecoration("plant8", 7000, 100, 300);
            CreateDecoration("plant10", 8000, 100, 300);
            #endregion

            _clearScreen = new Sprite(Game.Content.Load<Texture2D>("black1x1"));
            _clearScreen.DrawRectangle = new Rectangle(0, 0, 1, 1);
            _clearScreen.Position = _char.Sprite.Position;
            _clearScreen.Z = 315;
            _clearScreen.ScaleX = 4096.0f;
            _clearScreen.ScaleY = 4096.0f;
            Root.Add(_clearScreen);

            _fadeScreen = new Fade(1.0f, 0.0f, 2000);
            _fadeScreen.EaseFunction = EaseFunction.In_Cubic;
            _fadeScreen.Target = _clearScreen;
            _fadeScreen.OnEnd += new AnimationHandler(_fadeScreen_OnEnd);
            //SoundManager.GetInstance().PlayBGM();
            this.PauseGame = true;

            _boardTexs = new Texture2D[4];
            _boardTexs[0] = Game.Content.Load<Texture2D>("intro0");
            _boardTexs[1] = Game.Content.Load<Texture2D>("intro1");
            _boardTexs[2] = Game.Content.Load<Texture2D>("intro2");
            _boardTexs[3] = Game.Content.Load<Texture2D>("intro3");
            _board = new Board(_boardTexs[0], _unitCam);
            _board.Distance = 285;
            _board.OnHide += new EventHandler(_board_OnHide);
            Root.Add(_board.Sprite);

            Texture2D logoTex = Game.Content.Load<Texture2D>("logo");
            Logo logo = new Logo(logoTex, _unitCam);
            logo.Distance = 284;
            logo.Show();
            Root.Add(logo.Sprite);

            _startTimer = new Timer(5500);
            _startTimer.OnTimer += new TimerHandler(_startTimer_OnTimer);
            _startTimer.Start();

            _introTimer = new Timer(1000);
            _introTimer.OnTimer += new TimerHandler(_introTimer_OnTimer);

            this.PauseGame = true;

            s = new DebugString();
            s.String = "A: ";
            s.Position = Vector2.Zero;
            s.DrawOrder = 10000;
            Root.Add(s);

            UnitDef unitdef = new UnitDef();
            unitdef.BodyType = BodyType.Static;
            unitdef.Group = Group.Destructable;
            unitdef.X = -9000;
            unitdef.Y = 128;
            Unit u = _world.CreateUnit(unitdef);
            SensorInfo sensorInfo = u.AttachRectangleSensor(50, 256, Vector2.Zero, 0);
            sensorInfo.UnitEnter += new SensorHandler(sensorInfo_UnitEnter);
            sensorInfo.UnitLeave += new SensorHandler(sensorInfo_UnitLeave);
            //Root.Add(u.Sprite);

            base.Initialize();
        }

        void sensorInfo_UnitLeave(Unit sensorUnit, Unit sensedUnit)
        {
            s.String = "Leaved";
        }

        void sensorInfo_UnitEnter(Unit sensorUnit, Unit sensedUnit)
        {
            s.String = "Entered";
        }

        void GameScene_ActionEnd(IAction sender)
        {
            _board.Sprite.Texture = _boardTexs[_countBoard];
            _board.Show();
        }

        void _board_OnHide(object sender, EventArgs e)
        {
            this.PauseGame = false;
        }

        void _introTimer_OnTimer(Timer sender)
        {
            _board.Sprite.Texture = _boardTexs[_countBoard];
            _board.Show();
            this.PauseGame = true;
            _bStartGame = true;
        }

        void _startTimer_OnTimer(Timer sender)
        {
            _fadeScreen.Start();
            this.PauseGame = false;
            _introTimer.Start();
        }

        void _fadeScreen_OnEnd(Animation sender)
        {
            _clearScreen.Visible = false;
        }

        public override void Update()
        {
            /*
            s.String = "A: ";
                        if (_char.CurrentAction != null)
                        {
                            s.String += _char.CurrentAction.Name;
                        }*/
            
            if (!_bPauseGame)
            {
                #region Update Part 1
                _aiManager.Update();
                if (HIHInput.Instance().IsKeyDown(Keys.Escape))
                {
                    HIHGame.Instance().Exit();
                }
                if (HIHInput.Instance().IsKeyUp(Keys.Left) ||
                    HIHInput.Instance().IsKeyUp(Keys.Right))
                {
                    _char.Do("Stop");
                }
                if (HIHInput.Instance().GetKeyState(Keys.F))
                {
                    _char.Do("Attack");
                }
                if (HIHInput.Instance().GetKeyState(Keys.Right))
                {
                    if (_char.Direction != Direction.Right)
                    {
                        _char.Do("TurnRight");
                    }
                    _char.Do("Walk");
                }
                if (HIHInput.Instance().GetKeyState(Keys.Left))
                {
                    if (_char.Direction != Direction.Left)
                    {
                        _char.Do("TurnLeft");
                    }
                    _char.Do("Walk");
                }
                if (HIHInput.Instance().GetKeyState(Keys.Space) ||
                    HIHInput.Instance().GetKeyState(Keys.Up))
                {
                    _char.Do("Jump");
                }
                if (HIHInput.Instance().IsKeyDown(Keys.Q))
                {
                    HIHGame.Instance().FullScreen = true;
                }
                if (HIHInput.Instance().IsKeyDown(Keys.W))
                {
                    HIHGame.Instance().FullScreen = false;
                }
                if (HIHInput.Instance().IsKeyDown(Keys.V))
                {
                    _char.HP = _char.MaxHP;
                    _char1.HP = _char1.MaxHP;
                    HIHSceneManager.Instance().SwitchScene("Test");
                }
                if (HIHInput.Instance().IsKeyDown(Keys.P))
                {
                    this.PauseGame = !this.PauseGame;
                }
                #endregion
            }
            else if (_bStartGame)
            {
                if (!_board.IsFading && HIHInput.Instance().IsKeyDown(Keys.F))
                {
                    _countBoard++;
                    if (_countBoard == 3)
                    {
                        _board.Hide();
                    }
                    else
                    {
                        _board.Sprite.Texture = _boardTexs[_countBoard];
                        _board.Show();
                    }
                }
            }
            base.Update();
        }

        private Sprite CreateDecoration(
            string texName,
            float x, float y, float z,
            float scaleX = 1.0f, float scaleY = 1.0f)
        {
            Texture2D tex = HIHGame.Instance().Content.Load<Texture2D>(texName);
            Sprite sprite = new Sprite(tex);
            sprite.DrawRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            sprite.Position = new Vector3(x, y, z);
            Root.Add(sprite);
            return sprite;
        }

        private void CreateContactableDecoration(
             string texName,
            float x, float y, float z, 
            float w, float h, float transX = 0.5f, float transY = 0.5f, 
            BodyType bodyType = BodyType.Static,
            float scaleX = 1.0f, float scaleY = 1.0f)
        {
            DrawUnitDef def = new DrawUnitDef();
            def.TexName = texName;
            def.X = x;
            def.Y = y;
            def.Z = z;
            def.BodyType = bodyType;
            def.TransformOrigin = new Vector2(transX, transY);
            DrawUnit unit = _world.CreateRectangle(def, w, h);
            Texture2D tex = unit.Sprite.Texture;
            unit.Sprite.DrawRectangle = new Rectangle(0, 0, tex.Width, tex.Height);
            unit.Group = Group.Destructable;
            Root.Add(unit.Sprite);
        }

        private void CreateDragonfly(
            string texName,
            float z,
            float xFrom, float xTo, float yFrom, float yTo,
            uint duration = 2000)
        {
            Sprite sprite = CreateDecoration(texName, xTo, yTo, z);

            MoveXY moveDragonfly = new MoveXY(xFrom, xTo, yFrom, yTo, duration);
            moveDragonfly.Loop = true;
            moveDragonfly.Reverse = true;
            moveDragonfly.EaseFunction = EaseFunction.In_Out_Cubic;
            moveDragonfly.Target = sprite;
            moveDragonfly.Start();

            RotateZ rotateDragonfly = new RotateZ(0.3f, -0.5f, 1000);
            rotateDragonfly.Loop = true;
            rotateDragonfly.Reverse = true;
            rotateDragonfly.Target = sprite;
            rotateDragonfly.Start();
        }

        Character CreateHua(Group group, float x, float y, float z,
            int HP = 10, float speedX = 200, float speedY = 400,
            float scaleX = 1.0f ,float scaleY = 1.0f,
            float r = 1.0f, float g = 1.0f, float b = 1.0f)
        {
            CharacterDef charDef = new CharacterDef();
            charDef.TexName = "huawalk";
            charDef.R = r;
            charDef.G = g;
            charDef.B = b;
            charDef.FixedRotation = true;
            charDef.X = x;
            charDef.Y = y;
            charDef.Z = z;
            charDef.DrawRectangle = new Rectangle(0, 0, 186, 181);
            charDef.ScaleX = scaleX;
            charDef.ScaleY = scaleY;
            charDef.Width = 90.0f * scaleX;
            charDef.Height = 181.0f * scaleY;
            charDef.FrameAnimationDefs = new FrameDef[2];
            charDef.FrameAnimationDefs[0] = _huaWalkDef;
            charDef.FrameAnimationDefs[1] = _huaAttackDef;
            charDef.MoveSpeed = new Vector2(speedX, speedY);
            charDef.Group = group;
            Character hua = _world.CreateCharacter(charDef);
            hua.AttachAction(new Jump());
            hua.AttachAction(new Walk());
            hua.AttachAction(new TurnLeft());
            hua.AttachAction(new TurnRight());
            Attack attack = new Attack();
            attack.Sound = SoundManager.GetInstance().Mao;
            hua.AttachAction(attack);
            hua.AttachAction(new Stop());
            hua.AttachAction(new Hit());
            hua.AttachAction(new Quit());
            hua.MaxHP = HP;
            hua.HP = HP;

            this.Root.Add(hua.Sprite);
            HPBar hpBar = new HPBar(_hpFrameTex, _hpTex);
            hpBar.Width = 186;
            hpBar.Height = 20;
            hpBar.X = -93;
            hpBar.Y = 100;
            hpBar.Target = hua;
            return hua;
        }

        Character CreateDan1(Group group, float x, float y, float z,
            int HP = 10, float speedX = 200, float speedY = 400,
            float scaleX = 1.0f, float scaleY = 1.0f,
            float r = 1.0f, float g = 1.0f, float b = 1.0f)
        {
            CharacterDef charDef = new CharacterDef();
            charDef.TexName = "dan1walk";
            charDef.FixedRotation = true;
            charDef.X = x;
            charDef.Y = y;
            charDef.Z = z;
            charDef.R = r;
            charDef.G = g;
            charDef.B = b;
            charDef.Restitution = 0.4f;
            charDef.DrawRectangle = new Rectangle(0, 0, 292, 228);
            charDef.ScaleX = scaleX;
            charDef.ScaleY = scaleY;
            charDef.Width = 90.0f * scaleX;
            charDef.Height = 228.0f * scaleY;
            charDef.FrameAnimationDefs = new FrameDef[2];
            charDef.FrameAnimationDefs[0] = _dan1WalkDef;
            charDef.FrameAnimationDefs[1] = _dan1AttackDef;
            charDef.MoveSpeed = new Vector2(speedX,speedY);
            charDef.Group = group;
            Character dan = _world.CreateCharacter(charDef);
            dan.AttachAction(new Jump());
            dan.AttachAction(new Walk());
            dan.AttachAction(new TurnLeft());
            dan.AttachAction(new TurnRight());
            Attack attack = new Attack();
            attack.Sound = SoundManager.GetInstance().Pipa;
            dan.AttachAction(attack);
            dan.AttachAction(new Stop());
            dan.AttachAction(new Hit());
            dan.AttachAction(new Quit());
            dan.MaxHP = HP;
            dan.HP = HP;
            this.Root.Add(dan.Sprite);
            HPBar hpBar = new HPBar(_hpFrameTex, _hpTex);
            hpBar.Width = 186;
            hpBar.Height = 20;
            hpBar.X = -93;
            hpBar.Y = 100;
            hpBar.Target = dan;
            return dan;
        }

        Character CreateDan(Group group, float x, float y, float z,
            int HP = 10, float speedX = 200, float speedY = 400,
            float scaleX = 1.0f ,float scaleY = 1.0f,
            float r = 1.0f, float g = 1.0f, float b = 1.0f)
        {
            CharacterDef charDef = new CharacterDef();
            charDef.TexName = "danwalk";
            charDef.FixedRotation = true;
            charDef.X = x;
            charDef.Y = y;
            charDef.Z = z;
            charDef.R = r;
            charDef.G = g;
            charDef.B = b;
            charDef.Restitution = 0.4f;
            charDef.DrawRectangle = new Rectangle(0, 0, 136, 170);
            charDef.ScaleX = scaleX;
            charDef.ScaleY = scaleY;
            charDef.Width = 80.0f * scaleX;
            charDef.Height = 170.0f * scaleY;
            charDef.FrameAnimationDefs = new FrameDef[2];
            charDef.FrameAnimationDefs[0] = _danWalkDef;
            charDef.FrameAnimationDefs[1] = _danAttackDef;
            charDef.MoveSpeed = new Vector2(speedX, speedY);
            charDef.Group = group;
            Character dan = _world.CreateCharacter(charDef);
            dan.AttachAction(new Jump());
            dan.AttachAction(new Walk());
            dan.AttachAction(new TurnLeft());
            dan.AttachAction(new TurnRight());
            Attack attack = new Attack();
            attack.Sound = SoundManager.GetInstance().PipaLike;
            attack.ActionStart += new ActionHandler(dan_AttackStart);
            dan.AttachAction(attack);
            dan.AttachAction(new Stop());
            dan.AttachAction(new Hit());
            dan.AttachAction(new Quit());
            dan.MaxHP = HP;
            dan.HP = HP;
            this.Root.Add(dan.Sprite);
            HPBar hpBar = new HPBar(_hpFrameTex, _hpTex);
            hpBar.Width = 186;
            hpBar.Height = 20;
            hpBar.X = -93;
            hpBar.Y = 100;
            hpBar.Target = dan;
            return dan;
        }
        void _frame_OnEnd(Animation sender)
        {
            Frame frame = (Frame)sender;
            frame.Target.Parent.Remove(frame.Target);
        }
        void _timer_OnTimer(Timer sender)
        {
            Character dan = (Character)sender.EventData;
            if (dan.Actions["Attack"].IsDoing)
            {
                Sprite sprite = new Sprite(_soundTex);
                sprite.DrawRectangle = new Rectangle(0, 0, _soundTex.Width, _soundTex.Height);
                sprite.Position = dan.Sprite.Position;
                sprite.ScaleX = dan.Sprite.ScaleX;
                sprite.ScaleY = dan.Sprite.ScaleY;
                sprite.R = dan.Sprite.R;
                sprite.G = dan.Sprite.G;
                sprite.B = dan.Sprite.B;
                sprite.Z += 2.0f;
                Root.Add(sprite);
                Frame soundFrame = new Frame(_soundDef);
                soundFrame.Speed = 2.0f;
                soundFrame.OnEnd += new AnimationHandler(_frame_OnEnd);
                soundFrame.Target = sprite;
                soundFrame.Start();
            }
        }
        void dan_AttackStart(IAction sender)
        {
            Timer timer = new Timer(500);
            timer.EventData = sender.Character;
            timer.OnTimer += new TimerHandler(_timer_OnTimer);
            timer.Start();
        }
    }

   
}
