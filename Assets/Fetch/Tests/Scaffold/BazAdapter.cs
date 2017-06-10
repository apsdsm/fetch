using System;
using UnityEngine;

namespace Fetch.Test {
    class BazAdapter : MonoBehaviour, IAdapter<IBazService> {

        private BazService bazService;

        void Awake() {
            bazService = new BazService();
        }

        public IBazService controller {
            get { return bazService; }
        }
    }
}
