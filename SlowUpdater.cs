using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity_SlowUpdater
{
    public class SlowUpdater : MonoBehaviour
    {
        private static SlowUpdater instance;
        private static Dictionary<uint, RateFunctionPair> callbacks = new Dictionary<uint, RateFunctionPair>();
        private static uint nextId;
        
        private void Start() {
            if (SlowUpdater.instance) {
                Debug.LogError("More than one slow updater exists, remove one! This extra one has been disabled.");
                this.enabled = false;
                return;
            }
        
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += this.OnSceneLoaded;
            SlowUpdater.instance = this;
        }

        private void Update() {
            SlowUpdater._Update(); // Just calls the static version of update
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            SlowUpdater.reset();
        }

        private static void reset() {
            SlowUpdater.callbacks.Clear();
            SlowUpdater.nextId = 0;
        }
        
        private static void _Update() {
            foreach (RateFunctionPair pair in SlowUpdater.callbacks.Values) {
                if (Time.frameCount % pair.rate == 0) {
                    pair.function.Invoke();
                }
            }
        }
        
        /// <summary>
        /// Add a callback to the SlowUpdater
        /// </summary>
        /// <param name="pair">The rate/function pair to invoke every {rate} frames.</param>
        /// <returns>A uint ID</returns>
        public static uint add(RateFunctionPair pair) {
            if (!SlowUpdater.instance) {
                Debug.LogWarning("No instance of the SlowUpdater monobehaviour exists");
            }
            
            uint id = SlowUpdater.nextId;
            SlowUpdater.nextId++;
            
            SlowUpdater.callbacks.Add(id, pair);
            
            return id;
        }
        
        /// <summary>
        /// Add a callback to the SlowUpdater
        /// </summary>
        /// <param name="function">The action to invoke</param>
        /// <param name="rate">The rate to invoke the function, lower numbers are faster. 1 is every frame, 10 is every 10 frames.</param>
        /// <returns>A uint ID</returns>
        public static uint add(Action function, uint rate) {
            return SlowUpdater.add(new RateFunctionPair(rate, function));
        }

        /// <summary>
        /// Removes a callback from the SlowUpdater
        /// </summary>
        /// <param name="id">The number returned from SlowUpdater.add</param>
        public static void remove(uint id) {
            SlowUpdater.callbacks.Remove(id);
        }
    }
}