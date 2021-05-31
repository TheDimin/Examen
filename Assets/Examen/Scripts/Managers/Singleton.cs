using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Examen.Managers
{
    public abstract class Singelton<T> where T : class, new()
    {
        private static T _Instance;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                    try
                    {
                        _Instance = new T();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }


                return _Instance;
            }
        }

        public Singelton()
        {
            Awake();
        }

        public static void Dispose()
        {
            _Instance = null;
        }

        public abstract void Awake();

        public virtual void OnDrawGizmos() { }
    }
}
