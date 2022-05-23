using PixelCrew.Creatures.Mobs.Boss.Bombs;
using System.Collections;
using UnityEngine;

namespace Assets.PixelCrew.Creatures.Mobs.Boss
{
    public class BossBombingState : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var spawner = animator.GetComponent<BombsController>();
            spawner.StartBombing();
        }

    }
}