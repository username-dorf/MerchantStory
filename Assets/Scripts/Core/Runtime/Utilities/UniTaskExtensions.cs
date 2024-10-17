using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Core.Utils
{
    public static class UniTaskObservableExtensions
    {
        public static async UniTask<T> AwaitNextValueAsync<T>(this IObservable<T> observable, CancellationToken cancellationToken)
        {
            var tcs = new UniTaskCompletionSource<T>();

            IDisposable subscription = observable.Subscribe(
                onNext: value => tcs.TrySetResult(value),
                onError: ex => tcs.TrySetException(ex),
                onCompleted: () => tcs.TrySetCanceled());

            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                try
                {
                    T result = await tcs.Task;
                    return result;
                }
                finally
                {
                    subscription.Dispose();
                }
            }
        }

    }
}