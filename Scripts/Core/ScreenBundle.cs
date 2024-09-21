using System.Collections.Generic;
using UnityEngine;

namespace UnityScreenSystem
{
    
    /// <summary>
    /// Helper class used to load and unload a bundle of screens at one time
    /// </summary>
    public class ScreenBundle : MonoBehaviour
    {

        [SerializeReference] private List<Screen> _screens;

        /// <summary>
        /// Registers all the screens in this bundle to the Screen Manager
        /// </summary>
        public void Register()
        {
            for (int i = 0; i < _screens.Count; i++)
            {
                ScreenManager.RegisterScreen(_screens[i].ID, _screens[i]);
            }
        }

        /// <summary>
        /// Unregisters all the screens in this bundle from the Screen Manager
        /// </summary>
        public void Unregister()
        {
            for (int i = 0; i < _screens.Count; i++)
            {
                ScreenManager.UnregisterScreen(_screens[i].ID);
            }
        }

    }
}