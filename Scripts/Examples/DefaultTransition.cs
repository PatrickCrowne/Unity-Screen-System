using System.Threading.Tasks;
using UnityScreenSystem;
using UnityEngine;
using Screen = UnityScreenSystem.Screen;

namespace WebGame.Transitions
{
    public class DefaultTransition : Transition
    {

        private const float TransitionTime = 0.5f;
    
        [SerializeReference] private CanvasGroup canvasGroup;
    
        public override async Task OnTransition(Screen outgoing, Screen incoming)
        {

            if (outgoing != null)
            {
                // Fade out...
                canvasGroup.alpha = 0;
                for (float t = 0; t < 1.0f; t += Time.deltaTime * 4f * (2f / TransitionTime))
                {
                    canvasGroup.alpha = t;
                    await Task.Yield();
                }
                canvasGroup.alpha = 1;
            }
            
            // Swap Screens...
            if(outgoing != null) outgoing.gameObject.SetActive(false);
            if(incoming != null) incoming.gameObject.SetActive(true);
        
            // Fade in...
            canvasGroup.alpha = 1;
            for (float t = 0; t < 1.0f; t += Time.deltaTime * (2f / TransitionTime))
            {
                canvasGroup.alpha = 1f-t;
                await Task.Yield();
            }
            canvasGroup.alpha = 0;
        
        }
    }
}
