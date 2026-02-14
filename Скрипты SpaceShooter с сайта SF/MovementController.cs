using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Управление кораблем.
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }

        [SerializeField] private SpaceShip m_TargetShip;
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

        private ControlMode m_ControlMode;

        [SerializeField] private VirtualJoystick m_MobileJoystick;

        [SerializeField] private PointerClickHold m_MobileFirePrimary;
        [SerializeField] private PointerClickHold m_MobileFireSecondary;

        private void Start()
        {
            if (Application.isMobilePlatform)
            {
                m_ControlMode = ControlMode.Mobile;

                m_MobileJoystick.gameObject.SetActive(true);
            }
            else
            {
                m_MobileJoystick.gameObject.SetActive(false);

                m_MobileFirePrimary.gameObject.SetActive(false);
                m_MobileFireSecondary.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (m_TargetShip == null)
                return;


            switch (m_ControlMode)
            {
                case ControlMode.Keyboard:
                    ControlKeyboard();
                    break;

                case ControlMode.Mobile:
                    ControlMobile();
                    break;
            }
            
        }


        private void ControlMobile()
        {
            var dir = m_MobileJoystick.Value;

            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;

            if (m_MobileFirePrimary.IsHold)
                m_TargetShip.Fire(TurretMode.Primary);

            if (m_MobileFireSecondary.IsHold)
                m_TargetShip.Fire(TurretMode.Secondary);
        }

        private void ControlKeyboard()
        {


            float thrust = 0;
            float torque = 0;

            if (Input.GetKey(KeyCode.UpArrow))
                thrust += 1.0f;

            if (Input.GetKey(KeyCode.DownArrow))
                thrust -= 1.0f;

            if (Input.GetKey(KeyCode.LeftArrow))
                torque += 1.0f;

            if (Input.GetKey(KeyCode.RightArrow))
                torque -= 1.0f;

            m_TargetShip.ThrustControl = thrust;
            m_TargetShip.TorqueControl = torque;

            if(Input.GetKey(KeyCode.Space))
            {
                m_TargetShip.Fire(TurretMode.Primary);
            }

            if (Input.GetKey(KeyCode.X))
            {
                m_TargetShip.Fire(TurretMode.Secondary);
            }
        }
    }
}