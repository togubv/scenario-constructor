using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC
{
    public class Menu
    {
        private static readonly List<IController> _controllers = new List<IController>();

        public void Init()
        {
            for (int i = 0; i < _controllers.Count; i++)
            {
                _controllers[i].Init();
            }
        }

        public T Create<T>(Canvas canavs)
        {
            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                throw new NullReferenceException($"Not found class with type {typeof(T).Name}");
            }

            var controller = (IController)constructor.Invoke(null);
            controller.Create(canavs);
            _controllers.Add(controller);
            return (T)controller;
        }

        public static T Get<T>()
        {
            for (int i = 0; i < _controllers.Count; i++)
            {
                if (_controllers[i] is T t)
                {
                    return t;
                }
            }

            return default;
        }

        public void Dispose()
        {
            _controllers.Clear();
        }
    }
}