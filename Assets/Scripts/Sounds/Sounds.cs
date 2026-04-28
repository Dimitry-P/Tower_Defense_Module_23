using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefense
{
    [CreateAssetMenu()]
    public class Sounds : ScriptableObject
    {
        [SerializeField] private AudioClip[] m_Sounds;
        public AudioClip this[Sound s] => m_Sounds[(int)s]; //Наш объект со звуками теперь индексируется

#if UNITY_EDITOR

        [CustomEditor(typeof(Sounds))]
        public class SoundsInspector: Editor
        {
            private static readonly int soundCount = Enum.GetValues(typeof(Sound)).Length;
            private new Sounds target => base.target as Sounds; //Теперь в этом и инспекторе тип target-а НЕ Object, а  Sounds. 
            public override void OnInspectorGUI()
            {
                if(target.m_Sounds.Length < soundCount)
                {
                    Array.Resize(ref target.m_Sounds, soundCount);
                }
                for(int i = 0; i < target.m_Sounds.Length; i++) //Так как этот класс является внутренним классом класса Sounds, то он имеет доступ к его приватным переменным. 
                {
                    target.m_Sounds[i] = EditorGUILayout.ObjectField($"{(Sound)i}:", 
                        target.m_Sounds[i], typeof(AudioClip), false) as AudioClip;
                        //ObjectField - это поле в инспекторе, в которое можно вводить объекты
                }
            }
        }
        #endif
    }
  
}

