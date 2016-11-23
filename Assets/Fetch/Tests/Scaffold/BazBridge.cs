using System;
using UnityEngine;

namespace Fetch.Test {
    class BazBridge : MonoBehaviour, IBridge<IBazService> {

        private BazService _controller;

        void Awake() {
            _controller = new BazService();
        }

        public IBazService bridged {
            get {
                return _controller;
            }
        }
    }
}
