using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Util.PoolManager
{
    public static class PoolManager
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            Application.quitting += () => { OnExiting?.Invoke(); };
            
            
            SceneManager.sceneUnloaded += _ => { OnSceneUnloaded?.Invoke(); };
        }

        private static event Action OnExiting;
        private static event Action OnSceneUnloaded;

        /// <summary>
        ///     오브젝트를 풀에서 가져옵니다.
        /// </summary>
        /// <param name="prefab">프리팹</param>
        /// <param name="parent">부모</param>
        /// <typeparam name="T">컴포넌트 타입</typeparam>
        /// <returns>풀링된 오브젝트</returns>
        public static T Instantiate<T>(T prefab, Transform parent = null) where T : Component
        {
            Pool<T> pool = Pool<T>.Instance;
            T obj = pool.Get(prefab);
            obj.transform.SetParent(parent, false);
            return obj;
        }

        /// <summary>
        ///     오브젝트를 풀에 반환합니다.
        /// </summary>
        /// <param name="obj">반환할 오브젝트</param>
        /// <typeparam name="T">컴포넌트 타입</typeparam>
        public static void Destroy<T>(T obj) where T : Component
        {
            Pool<T> pool = Pool<T>.Instance;
            pool.Release(obj);
        }

        private class Pool<T> where T : Component
        {
            private readonly Dictionary<T, T> _prefabs = new();
            private readonly Dictionary<T, Stack<T>> _stacks = new();

            static Pool()
            {
                OnExiting += Instance.Clear;
                OnSceneUnloaded += Instance.Clear;
            }

            public static Pool<T> Instance { get; } = new();

            public T Get(T prefab)
            {
                if (!_stacks.TryGetValue(prefab, out Stack<T> stack))
                {
                    stack = new Stack<T>();
                    _stacks.Add(prefab, stack);
                    _prefabs.Add(prefab, prefab);
                }

                if (stack.Count > 0)
                {
                    T obj = stack.Pop();
                    obj.gameObject.SetActive(true);
                    return obj;
                }
                else
                {
                    T obj = Object.Instantiate(prefab);
                    _prefabs.Add(obj, prefab);
                    return obj;
                }
            }

            public void Release(T obj)
            {
                if (!_prefabs.TryGetValue(obj, out T prefab))
                {
                    Object.Destroy(obj.gameObject);
                    return;
                }

                obj.gameObject.SetActive(false);
                _stacks[prefab].Push(obj);
            }

            private void Clear()
            {
                foreach (T obj in _stacks.Values.SelectMany(stack => stack)) Object.Destroy(obj.gameObject);

                _stacks.Clear();
                _prefabs.Clear();
            }
        }
    }
}