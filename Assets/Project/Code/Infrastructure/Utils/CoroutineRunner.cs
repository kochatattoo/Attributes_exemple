using UnityEngine;

namespace Code.Infrastructure.Utils
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
