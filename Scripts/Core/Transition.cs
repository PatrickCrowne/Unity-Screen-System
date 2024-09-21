using System.Threading.Tasks;
using UnityEngine;

namespace UnityScreenSystem
{
    /// <summary>
    /// Used to transition between two screens
    /// </summary>
    public abstract class Transition : MonoBehaviour
    {

        /// <summary>
        /// This function is called when the transition is initiated by ScreenManager
        /// </summary>
        /// <param name="outgoing">The screen that is closing</param>
        /// <param name="incoming">The screen that is opening</param>
        /// <returns>Task, can be used asynchronously</returns>
        public abstract Task OnTransition(Screen outgoing, Screen incoming);

    }

}