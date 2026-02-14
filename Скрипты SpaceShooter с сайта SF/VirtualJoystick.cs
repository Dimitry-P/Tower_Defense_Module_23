using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceShooter
{

    public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Image m_JoyBack;
        [SerializeField] private Image m_Joystick;

        public Vector3 Value { get; private set; }

        public void OnDrag(PointerEventData ped)
        {
            Vector2 position = Vector2.zero;

            //To get InputDirection
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                    (m_JoyBack.rectTransform,
                    ped.position,
                    ped.pressEventCamera,
                    out position);

            position.x = (position.x / m_JoyBack.rectTransform.sizeDelta.x);
            position.y = (position.y / m_JoyBack.rectTransform.sizeDelta.y);

            float x = (m_JoyBack.rectTransform.pivot.x == 1f) ? position.x * 2 + 1 : position.x * 2 - 1;
            float y = (m_JoyBack.rectTransform.pivot.y == 1f) ? position.y * 2 + 1 : position.y * 2 - 1;

            Value = new Vector3(x, y, 0);
            Value = (Value.magnitude > 1) ? Value.normalized : Value;

            //to define the area in which joystick can move around
            m_Joystick.rectTransform.anchoredPosition = new Vector3(Value.x * (m_JoyBack.rectTransform.sizeDelta.x / 3)
                                                                   , Value.y * (m_JoyBack.rectTransform.sizeDelta.y) / 3);

        }

        public void OnPointerDown(PointerEventData ped)
        {
            OnDrag(ped);
        }

        public void OnPointerUp(PointerEventData ped)
        {
            Value = Vector3.zero;
            m_Joystick.rectTransform.anchoredPosition = Vector3.zero;
        }
    }
}