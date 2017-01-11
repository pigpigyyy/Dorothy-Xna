using System;
using Dorothy.Core;
using Dorothy.Data;
using Dorothy.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dorothy.Particles
{
    public class ParticleSet : DrawComponent
    {
        #region 成员
        private const int MAX_PARTICLES = 500;
        private bool _bRun;
        private int _particlesAlive;
        private float _duration;
        private float _emissionResidue;
        private Vector2 _prevPosition;
        private ParticleDef _psDef;
        private Particle[] _particles = new Particle[MAX_PARTICLES];
        private float _deltaTime;
        private Sprite _sprite;
        #endregion

        #region 属性
        public float Duration
        {
            set
            {
                _duration = value/oGame.TargetFrameInterval;
            }
            get { return _duration * oGame.TargetFrameInterval; }
        }
        public ParticleDef Info
        {
            set { _psDef = value; }
            get { return _psDef; }
        }
        public int ParticlesAlive
        {
            get { return _particlesAlive; }
        }
        public float MoveX
        {
            set
            {
                float prev = base.X;
                base.X = value;
                _prevPosition.X = (_bRun ? prev : base.X);
            }
            get { return this.Position.X; }
        }
        public float MoveY
        {
            set
            {
                float prev = base.Y;
                base.Y = value;
                _prevPosition.Y = (_bRun ? prev : base.Y);
            }
            get { return base.Y; }
        }
        #endregion

        #region 方法
        public ParticleSet(ParticleDef psd)
            : base(oSceneManager.CurrentScene.Controller)
        {
            _psDef = psd;
            Texture2D tex = oContent.GetTexture(psd.TexName);
            _sprite = new Sprite(tex);
            _sprite.DrawRectangle = psd.DrawRectangle;
            this.Add(_sprite);
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                _particles[i] = new Particle();
            }
            _deltaTime = oGame.TargetFrameInterval / 1000.0f;
        }
        public void MoveTo(float x, float y)
        {
            float prevX = base.X;
            float prevY = base.Y;
            _prevPosition.X = (_bRun ? prevX : base.X);
            _prevPosition.Y = (_bRun ? prevY : base.Y);
        }
        public void FireAt(float x, float y)
        {
            this.Stop(true);
            this.X = x;
            this.Y = y;
            this.Fire();
        }
        public void Fire()
        {
            _bRun = true;
            this.Enable = true;
            _duration = 0.0f;
        }
        public void Stop(bool killParticles)
        {
            _bRun = false;
            if (killParticles)
            {
                _particlesAlive = 0;
            }
        }
        public override void Update()
        {
            Vector2 location = new Vector2(base.X, base.Y);
            if (_duration >= 0)
            {
                _duration += _deltaTime;
                if (_duration >= _psDef.Lifetime)
                {
                    _bRun = false;
                }
            }
            Particle par;
            for (int i = 0; i < _particlesAlive; i++)
            {
                par = _particles[i];
                par.Age += _deltaTime;
                if (par.Age >= par.TerminalAge)
                {
                    _particlesAlive--;
                    Particle p = _particles[i];
                    _particles[i] = _particles[_particlesAlive];
                    _particles[_particlesAlive] = p;
                    i--;
                    continue;
                }
                Vector2 vecAccel = new Vector2(
                    par.Location.X - base.X,
                    par.Location.X - base.Y);
                vecAccel.Normalize();

                Vector2 vecAccel2 = vecAccel;
                vecAccel *= par.RadialAccel;

                float ang = vecAccel2.X;
                vecAccel2.X = -vecAccel2.Y;
                vecAccel2.Y = ang;

                vecAccel2 *= par.TangentialAccel;
                par.Velocity += (vecAccel + vecAccel2) * _deltaTime;
                par.Velocity.Y = par.Gravity * _deltaTime + par.Velocity.Y;

                par.Location += par.Velocity * _deltaTime;

                par.Spin += par.SpinDelta * _deltaTime;
                par.Size += par.SizeDelta * _deltaTime;
                par.ParticleColor += par.ParticleColor * _deltaTime;
            }
            if (_bRun)
            {
                float particlesNeeded = _psDef.Emission * _deltaTime + _emissionResidue;
                int particlesCreated = (int)particlesNeeded;
                _emissionResidue = particlesNeeded - particlesCreated;

                for (int i = 0; i < particlesCreated; i++)
                {
                    if (_particlesAlive >= MAX_PARTICLES)
                    {
                        break;
                    }
                    par = _particles[_particlesAlive];

                    par.Age = 0.0f;
                    par.TerminalAge = oHelper.NextFloat(_psDef.ParticleLifeMin, _psDef.ParticleLifeMax);

                    par.Location = _prevPosition + (location - _prevPosition) * oHelper.NextFloat(0.0f, 1.0f);
                    par.Location.X += oHelper.NextFloat(-2.0f, 2.0f);
                    par.Location.Y += oHelper.NextFloat(-2.0f, 2.0f);

                    float ang = _psDef.Direction - MathHelper.PiOver2 + oHelper.NextFloat(0.0f, _psDef.Spread) - _psDef.Spread / 2.0f;
                    if (_psDef.Relative)
                    {
                        ang += (float)Math.Atan2((_prevPosition - location).Y, (_prevPosition - location).X) + MathHelper.PiOver2;
                    }
                    par.Velocity.X = (float)Math.Cos(ang);
                    par.Velocity.Y = (float)Math.Sin(ang);
                    par.Velocity *= oHelper.NextFloat(_psDef.SpeedMin, _psDef.SpeedMax);

                    par.Gravity = oHelper.NextFloat(_psDef.GravityMin, _psDef.GravityMax);
                    par.RadialAccel = oHelper.NextFloat(_psDef.RadialAccelMin, _psDef.RadialAccelMax);
                    par.TangentialAccel = oHelper.NextFloat(_psDef.TangentialAccelMin, _psDef.TangentialAccelMax);

                    par.Size = oHelper.NextFloat(_psDef.SizeStart, _psDef.SizeStart + (_psDef.SizeEnd - _psDef.SizeStart) * _psDef.SizeVar);
                    par.SizeDelta = (_psDef.SizeEnd - par.Size) / par.TerminalAge;

                    par.Spin = oHelper.NextFloat(_psDef.SpinStart, _psDef.SpinStart + (_psDef.SpinEnd - _psDef.SpinStart) * _psDef.SpinVar);
                    par.SpinDelta = (_psDef.SpinEnd - par.Spin) / par.TerminalAge;

                    par.ParticleColor.X = oHelper.NextFloat(_psDef.ColorStart.X, _psDef.ColorStart.X + (_psDef.ColorEnd.X - _psDef.ColorStart.X) * _psDef.ColorVar);
                    par.ParticleColor.Y = oHelper.NextFloat(_psDef.ColorStart.Y, _psDef.ColorStart.Y + (_psDef.ColorEnd.Y - _psDef.ColorStart.Y) * _psDef.ColorVar);
                    par.ParticleColor.Z = oHelper.NextFloat(_psDef.ColorStart.Z, _psDef.ColorStart.Z + (_psDef.ColorEnd.Z - _psDef.ColorStart.Z) * _psDef.ColorVar);

                    par.ParticleAlpha = oHelper.NextFloat(_psDef.AlphaStart, _psDef.AlphaStart + (_psDef.AlphaEnd - _psDef.AlphaStart) * _psDef.AlphaVar);

                    par.ParticleColorDelta.X = oHelper.NextFloat(_psDef.ColorStart.X, _psDef.ColorStart.X + (_psDef.ColorEnd.X - _psDef.ColorStart.X) * _psDef.ColorVar);
                    par.ParticleColorDelta.Y = oHelper.NextFloat(_psDef.ColorStart.Y, _psDef.ColorStart.Y + (_psDef.ColorEnd.Y - _psDef.ColorStart.Y) * _psDef.ColorVar);
                    par.ParticleColorDelta.Z = oHelper.NextFloat(_psDef.ColorStart.Z, _psDef.ColorStart.Z + (_psDef.ColorEnd.Z - _psDef.ColorStart.Z) * _psDef.ColorVar);

                    par.ParticleAlphaDelta = oHelper.NextFloat(_psDef.AlphaStart, _psDef.AlphaStart + (_psDef.AlphaEnd - _psDef.AlphaStart) * _psDef.AlphaVar);

                    _particlesAlive++;
                }
            }
            if (_particlesAlive == 0)
            {
                this.Enable = false;
            }
            _prevPosition = location;
        }
        public override void Draw()
        {
            for (int i = 0; i < _particlesAlive; i++)
            {
                Particle par = _particles[i];
                _sprite.X = par.Location.X;
                _sprite.Y = par.Location.Y;
                _sprite.RotateZ = par.Spin * par.Age;
                _sprite.ScaleX = par.Size;
                _sprite.ScaleY = par.Size;
                _sprite.R = par.ParticleColor.X;
                _sprite.G = par.ParticleColor.Y;
                _sprite.B = par.ParticleColor.Z;
                _sprite.Alpha = par.ParticleAlpha;
                _sprite.UpdateTransform();
                _sprite.Draw();
            }
        }
        #endregion
    }

}
