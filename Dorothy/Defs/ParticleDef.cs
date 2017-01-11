using System;
using Microsoft.Xna.Framework;

namespace Dorothy.Particles
{
	/// <summary>
	/// 粒子系统的相关信息结构
	/// </summary>
	[Serializable]
	public class ParticleDef
	{
		/// <summary>
		/// Texture name.
		/// </summary>
		public string TexName;
		/// <summary>
		/// Texture`s draw area.
		/// </summary>
		public Rectangle DrawRectangle;
		/// <summary>
		/// 每秒粒子个数
		/// </summary>
		public int Emission = 181;
		/// <summary>
		/// 生命周期
		/// </summary>
		public float Lifetime = 100.0f;
		/// <summary>
		/// 最小生命周期
		/// </summary>
		public float ParticleLifeMin = 1.0f;
		/// <summary>
		/// 最大生命周期
		/// </summary>
		public float ParticleLifeMax = 5.0f;
		/// <summary>
		/// 方向
		/// </summary>
		public float Direction = 0;//MathHelper.PiOver2;//MathHelper.TwoPi;
		/// <summary>
		/// 偏移角度
		/// </summary>
		public float Spread = MathHelper.TwoPi;
		/// <summary>
		/// 绝对值还是相对值，用以计算生成粒子的初始速度
		/// </summary>
		public bool Relative = false;
		/// <summary>
		/// 最小速度
		/// </summary>
		public float SpeedMin = -10.0f;
		/// <summary>
		/// 最大速度
		/// </summary>
		public float SpeedMax = 0.0f;
		/// <summary>
		/// 最小重力
		/// </summary>
		public float GravityMin = -10.0f;
		/// <summary>
		/// 最大重力
		/// </summary>
		public float GravityMax = -10.0f;
		/// <summary>
		/// 最小线加速度
		/// </summary>
		public float RadialAccelMin = 0.0f;
		/// <summary>
		/// 最大线加速度
		/// </summary>
		public float RadialAccelMax = 0.0f;
		/// <summary>
		/// 最小角加速度
		/// </summary>
		public float TangentialAccelMin = 0.0f;
		/// <summary>
		/// 最大角加速度
		/// </summary>
		public float TangentialAccelMax = 0.3f;
		/// <summary>
		/// 起始大小
		/// </summary>
		public float SizeStart = 0.1f;
		/// <summary>
		/// 最终大小
		/// </summary>
		public float SizeEnd = 0.2f;
		/// <summary>
		/// 大小变化值
		/// </summary>
		public float SizeVar = 0.1f;
		/// <summary>
		/// 起始旋转角度
		/// </summary>
		public float SpinStart = 0.0f;
		/// <summary>
		/// 最终旋转角度
		/// </summary>
		public float SpinEnd = 1.0f;
		/// <summary>
		/// 旋转角度变量
		/// </summary>
		public float SpinVar = 0.1f;
		/// <summary>
		/// 起始颜色
		/// </summary>
		public Vector3 ColorStart = new Vector3(1, 1, 1);
		/// <summary>
		/// 最终颜色
		/// </summary>
		public Vector3 ColorEnd = new Vector3(0, 1, 0);
		/// <summary>
		/// 颜色变化值
		/// </summary>
		public float ColorVar = 2.5f;
		/// <summary>
		/// 起始alpha值
		/// </summary>
		public float AlphaStart = 1.0f;
		/// <summary>
		/// 最终alpha值
		/// </summary>
		public float AlphaEnd = 0.3f;
		/// <summary>
		/// alpha变化值
		/// </summary>
		public float AlphaVar = 1.1f;
	}
}
