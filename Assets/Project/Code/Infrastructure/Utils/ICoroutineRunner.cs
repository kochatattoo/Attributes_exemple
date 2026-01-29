using System.Collections;
using UnityEngine;

namespace Code.Infrastructure.Utils
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
