using Dorothy;
using Dorothy.Core;
using TestGame.Effects;

namespace TestGame
{
    public class TestGame : oGame
    {
        public TestGame()
        {
            //base.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 30);
            //oGame.GraphicsDeviceManager.PreferredBackBufferWidth = 800;
            //oGame.GraphicsDeviceManager.PreferredBackBufferHeight = 580;
        }
        protected override void Initialize()
        {
            base.Initialize();
            //oGame.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
			oGame.TargetFrameInterval = 1000.0f / 60;
            oGame.IsFullScreen = true;
			oSceneManager.Add(new PPTScene(this));
			oSceneManager.SwitchScene("PPTScene");
            //oSceneManager.Add(new TestScene(this));
            //oSceneManager.Add(new ZipTestScene(this));
            //oSceneManager.SwitchScene("ZipTest");
        }
    }
}
