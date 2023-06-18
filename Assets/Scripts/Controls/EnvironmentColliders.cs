using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controls
{

    public class EnvironmentColliders : MonoBehaviour
    {
        public static EnvironmentColliders Instance { get; private set; }
        public List <Collider> colliders = new ();

        private void Awake()
        {
            Instance = this;
        }
    }

}
