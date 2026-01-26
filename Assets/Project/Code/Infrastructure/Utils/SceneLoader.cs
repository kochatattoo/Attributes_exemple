using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Utils
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        public SceneLoader(ICoroutineRunner coroutineRunner) =>
             _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoaded = null, CancellationToken ct = default) =>
             _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded, ct));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null, CancellationToken ct = default)
        {
            if (ct.IsCancellationRequested)
                yield break;

            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
            {
                if (ct.IsCancellationRequested)
                    yield break;
                yield return null;
            }

            if (!ct.IsCancellationRequested)
                onLoaded?.Invoke();
        }
    }
}
