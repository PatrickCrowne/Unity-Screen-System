using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ScreenSystem
{

    /// <summary>
    /// Manages a system of screens based on the abstract Screen class, and transitioning to and from them.
    /// Written by Patrick Crowne
    /// </summary>
    public class ScreenManager : MonoBehaviour
    {

        private static ScreenManager _instance;
        private static Stack<Screen> _screenStack;
        private static Dictionary<string, Screen> _screenRegistry;

        [Header("Screens")] 
        [SerializeReference] private Transform screenParent;
        [Header("Transitions")]
        [SerializeReference] private ScreenTransition defaultTransition;
        [SerializeReference] private Transform transitionParent;
        [Header("Scene")]
        [SerializeReference] private Transform sceneParent;
        
        private static Screen CurrentScreen => _screenStack.Count > 0 ? _screenStack.Peek() : null;
        public static Transform SceneParent => _instance.sceneParent;
        
        private void Awake()
        {
            _instance = this;
            _screenStack = new Stack<Screen>();
            _screenRegistry = new Dictionary<string, Screen>();
        }

        /// <summary>
        /// Loads a screen bundle into the Screen Manager
        /// </summary>
        /// <param name="pScreenBundle">A reference to a Screen Bundle Prefab, do not use an instance</param>
        public static void LoadScreenBundle(ScreenBundle pScreenBundle)
        {
            ScreenBundle screenBundleInstance = Instantiate(pScreenBundle, _instance.screenParent);
            screenBundleInstance.Register();
        }

        /// <summary>
        /// Unloads a loaded screen bundle from the Screen Manager
        /// </summary>
        /// <param name="pScreenBundle">A reference to an instantiated Screen Bundle, do not use an unloaded prefab</param>
        public static void UnloadScreenBundle(ScreenBundle pScreenBundle)
        {
            pScreenBundle.Unregister();
            Destroy(pScreenBundle.gameObject);
        }
        
        /// <summary>
        /// Adds this screen to the screen registry, so it can be retrieved based on the provided identifier
        /// </summary>
        /// <param name="pScreenIdentifier">The identifier for the given screen instance</param>
        /// <param name="pScreenInstance">The instance to be registered</param>
        public static void RegisterScreen(string pScreenIdentifier, Screen pScreenInstance)
        {
            if (_screenRegistry.ContainsKey(pScreenIdentifier))
            {
                _screenRegistry[pScreenIdentifier] = pScreenInstance;
                return;
            }
            _screenRegistry.Add(pScreenIdentifier, pScreenInstance);
        }

        /// <summary>
        /// Removes the screen associated with the given identifier from the screen registry
        /// </summary>
        /// <param name="pScreenIdentifier">The identifier for the given screen instance to be removed</param>
        public static void UnregisterScreen(string pScreenIdentifier)
        {
            _screenRegistry.Remove(pScreenIdentifier);
        }

        /// <summary>
        /// Get the instance of the screen with the given screen identifier
        /// </summary>
        /// <param name="pScreenIdentifier">The identifier for the screen to get</param>
        /// <returns>The Instance of the screen associated with the identifier</returns>
        /// <exception cref="ArgumentException">If the given identifier does not correlate with a registered screen</exception>
        public static Screen ScreenFromId(string pScreenIdentifier)
        {
            if (!_screenRegistry.ContainsKey(pScreenIdentifier)) 
                throw new ArgumentException($"No screen registered with the identifier \"{pScreenIdentifier}\"!");
            return _screenRegistry[pScreenIdentifier];
        }
        
        /// <summary>
        /// Opens a screen and adds it to the screen stack.
        /// </summary>
        /// <param name="pScreenToOpen">The instance of the screen to open</param>
        /// <param name="pOpenOverExisting">Should this screen be opened on top of the existing screen, or replace it?</param>
        /// <param name="pTransition">(Optional) How should this screen be transitioned to? (uses defaultTransition if null)</param>
        /// <param name="onComplete">(Optional) Callback for once this is complete.</param>
        public static async Task OpenScreen(Screen pScreenToOpen, bool pOpenOverExisting = false, ScreenTransition pTransition = null, Action onComplete = null)
        {
            // Sanitize Inputs
            if (pScreenToOpen == null) return;
            if (pTransition == null) pTransition = _instance.defaultTransition;
            
            // Instantiate transition
            ScreenTransition transition = Instantiate(pTransition, _instance.transitionParent);

            // Handle edge cases
            if (CurrentScreen != null && !pOpenOverExisting)
            {
                CurrentScreen.OnExit();
                await transition.OnTransition(CurrentScreen, pScreenToOpen);
            }
            else
            {
                await transition.OnTransition(null, pScreenToOpen);
            }

            Debug.Log("D");
            
            // Open the screen
            _screenStack.Push(pScreenToOpen);
            CurrentScreen.OnEnter();
            
            // Destroy the transition
            Destroy(transition.gameObject);
            
            Debug.Log("Closed Transition.");
            
            // Complete
            onComplete?.Invoke();
        }

        /// <summary>
        /// Closes the top screen on the screen stack.
        /// </summary>
        /// <param name="pTransition">(Optional) How should this screen be transitioned from? (uses defaultTransition if null)</param>
        /// <param name="onComplete">(Optional) Callback for once this is complete.</param>
        public static async Task CloseScreen(ScreenTransition pTransition = null, Action onComplete = null)
        {
            // Sanitize Inputs
            if (CurrentScreen == null) return;
            if (pTransition == null) pTransition = _instance.defaultTransition;

            // Instantiate transition
            ScreenTransition transition = Instantiate(pTransition, _instance.transitionParent);
            
            // Close current screen and open last one
            Screen screenClosing = _screenStack.Pop();
            screenClosing.OnExit();
            await transition.OnTransition(screenClosing, CurrentScreen);
            CurrentScreen.OnEnter();
            
            // Destroy the transition
            Destroy(transition.gameObject);
            
            // Complete
            onComplete?.Invoke();
        }

    }
}