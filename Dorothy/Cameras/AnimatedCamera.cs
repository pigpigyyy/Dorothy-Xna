using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony.Core;
using Harmony.Animations;
using Microsoft.Xna.Framework;

namespace Harmony.Cameras
{
    public class AnimatedCamera : Animation
    {
        #region 成员
        private Vector3 _from;
        private Vector3 _change;
        private float _count;
        private Camera _cam = new Camera();
        private EaseFunction _ease = EaseFunction.NoEasing;
        #endregion

        #region 属性
        public EaseFunction EaseFunction
        {
            set { _ease = value; }
            get { return _ease; }
        }
        public Vector3 From
        {
            set { _from = value; }
            get { return _from; }
        }
        public Vector3 To
        {
            set { _change = value - _from; }
            get { return _from + _change; }
        }
        public new float Duration
        {
            set
            {
                _duration = value;
                _count = value / HIHCore.Instance().TargetFrameMilliseconds;
            }
            get { return _duration; }
        }
        #endregion

        public AnimatedCamera()
        {
            _cam.Set(new Vector3(0, 0, 200), Vector3.Zero);
        }
        public AnimatedCamera(Vector3 position, Vector3 target)
        {
            _cam.Set(ref position, ref target);
        }
        public AnimatedCamera(Vector3 position, Vector3 target, Vector3 from, Vector3 to, uint duration)
        {
            _cam.Set(ref position, ref target);
            _from = from;
            _change = to - from;
            this.Duration = duration;
        }
        public void Set(Vector3 position, Vector3 target)
        {
            _cam.Set(ref position, ref target);
        }
        protected override void PreStart()
        {
            _current = 0.0f;
            _cam.Move(_from);
            _cam.Apply();
        }
        protected override bool UpdateFrame()
        {
            if (_bReverse)
            {
                if (_bIsReversing)
                {
                    if (_current >= 0.0f)
                    {
                        Vector3 pos;
                        pos.X = _ease.Func(_current, _from.X, _change.X, _count);
                        pos.Y = _ease.Func(_current, _from.Y, _change.Y, _count);
                        pos.Z = _ease.Func(_current, _from.Z, _change.Z, _count);
                        _cam.Move(ref pos);
                        _cam.Apply();
                        _current -= _speed;
                    }
                    else
                    {
                        _bIsReversing = false;
                        return true;
                    }
                }
                else
                {
                    if (_current < _count)
                    {
                        Vector3 pos;
                        pos.X = _ease.Func(_current, _from.X, _change.X, _count);
                        pos.Y = _ease.Func(_current, _from.Y, _change.Y, _count);
                        pos.Z = _ease.Func(_current, _from.Z, _change.Z, _count);
                        _cam.Move(ref pos);
                        _cam.Apply();
                        _current += _speed;
                    }
                    else
                    {
                        
                        _cam.Move(this.To);
                        _cam.Apply();
                        _current = _count;
                        _bIsReversing = true;
                    }
                }
            }
            else
            {
                if (_current < _count)
                {
                    Vector3 pos;
                    pos.X = _ease.Func(_current, _from.X, _change.X, _count);
                    pos.Y = _ease.Func(_current, _from.Y, _change.Y, _count);
                    pos.Z = _ease.Func(_current, _from.Z, _change.Z, _count);
                    _cam.Move(ref pos);
                    _cam.Apply();
                    _current += _speed;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        protected override void PreEnd()
        {
            if (base.Reverse)
            {
                _current = 0.0f;
                _bIsReversing = false;
                _cam.Move(ref _from);
                _cam.Apply();
            }
            else
            {
                _current = _count;
                _cam.Move(this.To);
                _cam.Apply();
            }
        }
    }
}
