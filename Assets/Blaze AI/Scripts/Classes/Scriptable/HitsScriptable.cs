using UnityEngine;

namespace BlazeAISpace
{
    [System.Serializable]
    public class HitsScriptable
    {
        [Tooltip("The duration of being in the hit state (seconds)")]
        public float hitDuration = 0.8f;
        [Tooltip("If agent is going for an attack and got hit midway the attack will be cancelled and will return to idle-attack position. If false, after the hit the agent will continue on it's attack.")]
        public bool cancelAttackIfHit = true;

        [Header("Animations")]
        [Tooltip("Use animations on getting hit or not")]
        public bool useAnimation = true;
        [Tooltip("The animation name to play when getting hit. You can access this property dynamically and set the animation name depending on where the NPC gets hit. So for example: if the player hits the npc on the head you can access this property and set the animation name to the 'head hit' animation")]
        public string animationName;
        [Tooltip("The transition time to play this animation (seconds)")]
        public float animationTransition = 0.25f;

        [Header("Audios")]
        [Tooltip("Do you want to play audios when getting hit?")]
        public bool useAudios;
    }
}

