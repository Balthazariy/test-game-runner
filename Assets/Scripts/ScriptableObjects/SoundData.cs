using System;
using System.Collections.Generic;
using TestGame.Settings;
using UnityEngine;

namespace TestGame.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "TestGame/SoundData", order = 2)]
    public class SoundData : ScriptableObject
    {
        [SerializeField]
        public List<SoundInfo> sounds;

        public float crossFadeInTime = 2f;
        public float crossFadeOutTime = 1f;

        [Serializable]
        public class SoundInfo
        {
            public Sounds type;
            public AudioClip clip;

            [Range(0, 1f)]
            public float volume = 1f;

            public bool loop;
            public bool sfx = true;
            public bool crossFade;
        }
    }
}