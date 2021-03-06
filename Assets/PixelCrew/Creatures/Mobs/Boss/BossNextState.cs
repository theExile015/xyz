using PixelCrew.Components.goBased;
using PixelCrew.Creatures.Mobs.Boss;
using UnityEngine;

public class BossNextState : StateMachineBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _stageColor;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var spawner = animator.GetComponent<CircularProjectileSpawner>();
        spawner.Stage++;

        var changeLight = animator.GetComponent<ChangeLightComponent>();
        changeLight.SetColor(_stageColor);
    }


}
