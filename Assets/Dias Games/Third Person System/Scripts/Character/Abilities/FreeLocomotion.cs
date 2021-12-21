using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiasGames.ThirdPersonSystem
{
    public class FreeLocomotion : ThirdPersonAbility
    {
        [SerializeField] private PhysicMaterial m_IdlePhysicMaterial = null;

        public override bool TryEnterAbility()
        {
            if (m_System.IsGrounded)
                return true;

            return false;
        }


        public override void FixedUpdateAbility()
        {
            base.FixedUpdateAbility();

            UpdatePhysicMaterial();
                       
            m_System.CalculateMoveVars();
            m_System.UpdateMovementAnimator(0.1f);
            m_System.RotateToDirection();
        }

        private void UpdatePhysicMaterial()
        {
            if (Mathf.Approximately(m_System.FreeMoveDirection.magnitude, 0))
                m_System.m_Capsule.sharedMaterial = m_IdlePhysicMaterial;
            else
                m_System.m_Capsule.sharedMaterial = m_AbilityPhysicMaterial;
        }

        public override bool TryExitAbility()
        {
            return !m_System.IsGrounded;
        }

        private void Reset()
        {
            m_EnterState = "Grounded";
            m_TransitionDuration = 0.2f;
            m_FinishOnAnimationEnd = false;
            m_UseRootMotion = true;
            m_AllowCameraZoom = true;
        }
    }
}