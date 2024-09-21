using System.Threading.Tasks;
using UnityEngine;

namespace ScreenSystem
{
    /// <summary>
    /// Used to transition between two screens
    /// </summary>
    public abstract class ScreenTransition : MonoBehaviour
    {

        public abstract Task OnTransition(Screen outgoing, Screen incoming);

    }

}