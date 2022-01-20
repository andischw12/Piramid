﻿using UnityEngine;

namespace BlazeAISpace
{
    [System.Serializable]
    public class CoverShooterOptionsScriptable 
    {
        [Header("General")]
        [Tooltip("If set to true, the cover shooter behaviour will be set (searching for covers and going to them), if false normal ranged/melee behaviour will be set.")]
        public bool coverShooter = false;
        [Tooltip("Object layers that can take cover behind.")]
        public LayerMask coverLayers;
        [Tooltip("The lower the number the better the hiding spot. From -1 (best) to 1 (worst)")]
        public float hideSensitivity = -0.25f;
        [Tooltip("The search distance for cover. This can't be bigger than the [Distance From Enemy] property (automatically clamped if so)")]
        public float searchDistance = 5f;
        [Tooltip("The minimum height of cover obstacles. Obstacle height is measured using collider.bounds.y. Use the [GetCoverHeight] script to print cover height in console.")]
        public float minObstacleHeight = 1.25f;

        [Header("Cover options")]
        [Tooltip("If set to true, the npc will move to the center transform of the cover obstacle, if false, will be in cover the moment the obstacle hides the npc from enemy.")]
        public bool moveToCoverCenter = true;
        [Tooltip("The chance of attacking the enemy on first sight. If set to [Always Attack] the npc will always attack first. If set to [Take Cover] then the npc will always go to cover first. If set to [Randomize] there's a 50/50 chance either going to cover or attacking.")]
        public FirstSightChance firstSightChance = FirstSightChance.Randomize;
        [Tooltip("Do you want the npc to attack if it's cover is blown and an enemy can see it? If set to [Always Attack], the npc will leave it's cover and attack. If set to [Take Cover] it will refrain from attacking and find another cover. If no cover found it'll attack. If set to [Randomize] there's a 50/50 chance for either taking cover or attacking.")]
        public CoverBlownState coverBlownState = CoverBlownState.Randomize;
        [Tooltip("Should the agent open fire on the cover obstacle the enemy is hiding behind or only attack at the actual enemy. This property will only be taken to account if there's an actual cover the enemy is hiding behind.")]
        public AttackEnemyCover attackEnemyCover = AttackEnemyCover.Randomize;

        [Header("Animations")]
        [Tooltip("The cover animation names to play when NPC is hiding in low or high covers.")]
        public CoverAnimationsScriptable coverAnimations;
        [Space(2)]
        [Tooltip("The transition time to transition to this animation.")]
        public float coverAnimationTransition = 0.25f;

        public enum CoverBlownState {
            AlwaysAttack,
            TakeCover,
            Randomize
        }

        public enum FirstSightChance {
            AlwaysAttack,
            TakeCover,
            Randomize
        }

        public enum AttackEnemyCover {
            AlwaysAttackCover,
            AlwaysAttackEnemy,
            Randomize
        }
    }
}
