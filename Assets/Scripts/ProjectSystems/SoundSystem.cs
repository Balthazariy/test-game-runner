using System;
using System.Collections.Generic;
using TestGame.ScriptableObjects;
using TestGame.Settings;
using UnityEngine;
using Zenject;

namespace TestGame.ProjectSystems
{
    public class SoundSystem : MonoBehaviour, ISystem
    {
        private bool _isMute;

        public bool IsMute
        {
            get { return _isMute; }
            set
            {
                _isMute = value;
                UpdateMuteState();
            }
        }

        private List<SoundSource> _soundSources;

        private List<SoundPlayQueue> _soundPlayQueue;

        private Transform _soundContainer;

        public float SoundVolume { get; set; } = 1f;

        public float MusicVolume { get; set; } = 1f;

        public SoundData SoundData { get; private set; }

        private LoadObjectsSystem _loadObjectsSystem;

        [Inject]
        public void Construct(LoadObjectsSystem loadObjectsSystem)
        {
            Utilities.Logger.Log("SoundSystem Construct", LogTypes.Info);

            _loadObjectsSystem = loadObjectsSystem;
        }

        public void Init()
        {
            SoundData = _loadObjectsSystem.GetObjectByPath<SoundData>("Data/SoundData");

            _soundSources = new List<SoundSource>();
            _soundPlayQueue = new List<SoundPlayQueue>();

            _soundContainer = new GameObject("[Sound Container]").transform;
            _soundContainer.gameObject.AddComponent<AudioListener>();
            DontDestroyOnLoad(_soundContainer);

            SoundVolume = 1.0f;
            MusicVolume = 1.0f;

            IsMute = SoundVolume == 0 && MusicVolume == 0;
        }

        public void PlayClickSound()
        {
            PlaySound(Sounds.Click);
        }

        public void Update()
        {
            if (_soundSources == null)
            {
                return;
            }

            if (_soundPlayQueue == null)
            {
                return;
            }

            for (int i = 0; i < _soundSources.Count; i++)
            {
                _soundSources[i].Update();

                if (_soundSources[i].IsSoundEnded())
                {
                    _soundSources[i].Dispose();
                    _soundSources.RemoveAt(i--);
                }
            }

            for (int i = 0; i < _soundPlayQueue.Count; i++)
            {
                _soundPlayQueue[i].time -= Time.deltaTime;

                if (_soundPlayQueue[i].time <= 0f)
                {
                    _soundPlayQueue[i].action?.Invoke();
                    _soundPlayQueue.RemoveAt(i--);
                }
            }
        }

        public void PlaySound(Sounds soundType)
        {
            if (soundType == Sounds.Unknown)
            {
                return;
            }

            var soundInfo = SoundData.sounds.Find(item => item.type == soundType);

            if (soundInfo == null)
            {
                return;
            }

            SoundSource foundSameSource = _soundSources.Find(soundSource => soundSource.SoundType == soundType);

            if (foundSameSource != null)
            {
                if (!soundInfo.sfx)
                {
                    return;
                }
            }

            AudioClip sound = soundInfo.clip;
            SoundParameters parameters = new SoundParameters()
            {
                Loop = soundInfo.loop,
                Volume = soundInfo.volume,
                SFX = soundInfo.sfx,
                CrossFade = soundInfo.crossFade
            };

            _soundSources.Add(new SoundSource(_soundContainer, sound, soundType, parameters, this));
        }

        private void CachedDataLoadedEventHandler()
        {
            SoundVolume = 1.0f;
            MusicVolume = 1.0f;
        }

        public void StopSound(Sounds soundType)
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                if (_soundSources[i].SoundType == soundType)
                {
                    _soundSources[i].StopPlaying();
                }
            }
        }

        public void UpdateSoundStatus()
        {
            foreach (var sound in _soundSources)
            {
                sound.AudioSource.mute = _isMute;
            }
        }

        private void UpdateMuteState()
        {
            foreach (var sound in _soundSources)
            {
                sound.AudioSource.mute = _isMute;
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                _soundSources[i].Dispose();
            }

            _soundSources.Clear();
            _soundPlayQueue.Clear();
            MonoBehaviour.Destroy(_soundContainer.gameObject);
        }

        public void PlaySoundDelayed(Sounds soundType, float delay)
        {
            if (soundType == Sounds.Unknown)
            {
                return;
            }

            delay = Mathf.Clamp(delay, 0f, 999f);

            if (delay == 0f)
            {
                PlaySound(soundType);
            }
            else
            {
                _soundPlayQueue.Add(new SoundPlayQueue()
                {
                    time = delay,
                    action = () => PlaySound(soundType)
                });
            }
        }

        public void StopAllSounds()
        {
            for (int i = 0; i < _soundSources.Count; i++)
            {
                _soundSources[i].StopPlaying();
            }
        }
    }

    internal class SoundPlayQueue
    {
        public float time;
        public Action action;
    }

    internal class SoundSource
    {
        private bool _crossFadeEnded;
        private bool _crossFadeStarted;
        private bool _crossFadeInStarted;
        private bool _crossFadeInEnded;
        private float _crossFadeStep;
        private bool _prepareToEnd;

        public GameObject SoundSourceObject { get; }
        public AudioClip Sound { get; }
        public AudioSource AudioSource { get; }
        public Sounds SoundType { get; }
        public SoundParameters SoundParameters { get; }

        private SoundSystem _soundSystem;

        public SoundSource(Transform parent, AudioClip sound, Sounds soundType, SoundParameters parameters, SoundSystem soundSystem)
        {
            Sound = sound;
            SoundType = soundType;
            SoundParameters = parameters;
            _soundSystem = soundSystem;

            SoundSourceObject = new GameObject($"[Sound] - {SoundType} - {Time.time}");
            SoundSourceObject.transform.SetParent(parent);
            AudioSource = SoundSourceObject.AddComponent<AudioSource>();
            AudioSource.clip = Sound;
            AudioSource.loop = SoundParameters.Loop;

            if (!parameters.CrossFade)
            {
                AudioSource.volume = SoundParameters.SFX ? _soundSystem.SoundVolume : _soundSystem.MusicVolume;
            }

            AudioSource.Play();
        }

        public void Update()
        {
            float targetVolume = SoundParameters.Volume * (SoundParameters.SFX ? _soundSystem.SoundVolume : _soundSystem.MusicVolume);

            if (_crossFadeStarted)
            {
                AudioSource.volume -= _crossFadeStep;

                if (AudioSource.volume <= 0)
                {
                    AudioSource.volume = 0f;

                    _crossFadeStarted = false;
                    _crossFadeEnded = true;

                    if (_prepareToEnd)
                    {
                        AudioSource.Stop();
                    }
                }
            }
            else if (_crossFadeInStarted)
            {
                AudioSource.volume += _crossFadeStep;

                if (AudioSource.volume >= targetVolume)
                {
                    AudioSource.volume = targetVolume;

                    _crossFadeInEnded = true;
                    _crossFadeInStarted = false;
                }
            }
            else
            {
                AudioSource.volume = targetVolume;
            }

            if (AudioSource.isPlaying && SoundParameters.CrossFade && !_crossFadeStarted)
            {
                if (AudioSource.time >= Mathf.Max(AudioSource.clip.length * 0.9f, AudioSource.clip.length - _soundSystem.SoundData.crossFadeOutTime))
                {
                    PrepareCrossFade(_soundSystem.SoundData.crossFadeOutTime);
                }
            }

            if (AudioSource.isPlaying && SoundParameters.CrossFade && !_crossFadeInStarted && !_crossFadeInEnded)
            {
                if (AudioSource.time < Mathf.Min(AudioSource.clip.length * 0.1f, _soundSystem.SoundData.crossFadeInTime))
                {
                    AudioSource.volume = 0f;
                    _crossFadeInStarted = true;

                    CalculateCrossFadeStep(targetVolume, _soundSystem.SoundData.crossFadeInTime);
                }
            }
        }

        public bool IsSoundEnded()
        {
            return !AudioSource.loop && !AudioSource.isPlaying && ((!_crossFadeStarted && _crossFadeEnded && SoundParameters.CrossFade) || !SoundParameters.CrossFade);
        }

        public void Dispose()
        {
            AudioSource.Stop();
            MonoBehaviour.Destroy(SoundSourceObject);
        }

        public void StopPlaying()
        {
            AudioSource.loop = false;

            if (SoundParameters.CrossFade)
            {
                _prepareToEnd = true;
                PrepareCrossFade(_soundSystem.SoundData.crossFadeOutTime);
            }
            else
            {
                AudioSource.Stop();
            }
        }

        private float CalculateCrossFadeStep(float volume, float time)
        {
            _crossFadeStep = volume / time * Time.deltaTime;

            return _crossFadeStep;
        }

        private void PrepareCrossFade(float time)
        {
            CalculateCrossFadeStep(AudioSource.volume, time);
            _crossFadeStarted = true;
            _crossFadeInEnded = false;
        }
    }

    public class SoundParameters
    {
        public bool Loop { get; set; } = false;
        public bool SFX { get; set; } = true;
        public bool CrossFade { get; set; }
        public float Volume { get; set; } = 1f;
    }
}