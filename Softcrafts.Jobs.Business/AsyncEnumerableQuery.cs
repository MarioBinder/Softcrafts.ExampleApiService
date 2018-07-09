using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Softcrafts.Jobs.Business
{
    /// <summary>
    /// copied from https://stackoverflow.com/a/44815307/119109
    /// </summary>
    public static class AsyncQueryableExtensions
    {
        public static IQueryable<TElement> AsAsyncQueryable<TElement>(this IEnumerable<TElement> source)
        {
            return new AsyncEnumerableQuery<TElement>(source);
        }

        public static IDbAsyncEnumerable<TElement> AsDbAsyncEnumerable<TElement>(this IEnumerable<TElement> source)
        {
            return new AsyncEnumerableQuery<TElement>(source);
        }

        public static EnumerableQuery<TElement> AsAsyncEnumerableQuery<TElement>(this IEnumerable<TElement> source)
        {
            return new AsyncEnumerableQuery<TElement>(source);
        }

        public static IQueryable<TElement> AsAsyncQueryable<TElement>(this Expression expression)
        {
            return new AsyncEnumerableQuery<TElement>(expression);
        }

        public static IDbAsyncEnumerable<TElement> AsDbAsyncEnumerable<TElement>(this Expression expression)
        {
            return new AsyncEnumerableQuery<TElement>(expression);
        }

        public static EnumerableQuery<TElement> AsAsyncEnumerableQuery<TElement>(this Expression expression)
        {
            return new AsyncEnumerableQuery<TElement>(expression);
        }
    }

    /// <summary>
    /// copied from https://stackoverflow.com/a/48744037/119109
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncEnumerableQuery<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>
    {
        public AsyncEnumerableQuery(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public AsyncEnumerableQuery(Expression expression) : base(expression)
        {
        }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new InMemoryDbAsyncEnumerator<T>(((IEnumerable<T>)this).GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        private sealed class InMemoryDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;

            public InMemoryDbAsyncEnumerator(IEnumerator<T> enumerator)
            {
                _enumerator = enumerator;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                // Cleanup
            }

            ~InMemoryDbAsyncEnumerator()
            {
                Dispose(false);
            }

            public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_enumerator.MoveNext());
            }

            public T Current => _enumerator.Current;

            object IDbAsyncEnumerator.Current => Current;
        }
    }
}
