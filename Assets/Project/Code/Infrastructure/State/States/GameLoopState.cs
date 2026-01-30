using Code.Infrastructure.Utils;

namespace Code.Infrastructure.State.States
{
    public class GameLoopState : IState
    {
        private const string GameScene = "DemoScene";
        private readonly SceneLoader _sceneLoader;

        public GameLoopState( SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            _sceneLoader.Load(GameScene);
        }
        public void Exit() { }
    }
}
