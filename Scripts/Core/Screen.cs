using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using WebGame.Application;

namespace UnityScreenSystem
{
    
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Screen : MonoBehaviour
    {
        
        [SerializeField] private string _identifier;

        public string ID => _identifier;

        /// <summary>
        /// Called when a screen instance is entered
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Called when a screen instance is exited
        /// </summary>
        public abstract void OnExit();

    }
}