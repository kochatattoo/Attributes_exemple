using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Code.Infrastructure.Factory
{
    public interface IMenuFactory
    {
        UniTask<GameObject> CreateMenu(CancellationToken ct = default);
    }
}