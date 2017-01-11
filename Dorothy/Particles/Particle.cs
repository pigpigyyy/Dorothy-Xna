using Microsoft.Xna.Framework;

namespace Dorothy.Particles
{
    /// <summary>
    /// 单个粒子的属性结构
    /// </summary>
    public class Particle
    {
        #region 成员
        public Vector2 Location;
        public Vector2 Velocity;
        public float Gravity;
        public float RadialAccel;
        public float TangentialAccel;
        public float Spin;
        public float SpinDelta;
        public float Size;
        public float SizeDelta;
        public Vector3 ParticleColor;
        public Vector3 ParticleColorDelta;
        public float ParticleAlpha;
        public float ParticleAlphaDelta;
        public float Age;
        public float TerminalAge;
        #endregion
    }
}
