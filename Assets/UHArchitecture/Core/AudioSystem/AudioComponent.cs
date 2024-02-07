using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UralHedgehog
{
    public class AudioComponent : MonoBehaviour
    {
        [SerializeField] private bool isUIElement;

        public bool IsUIElement
        {
            get => isUIElement;
            set => isUIElement = value;
        }

        [SerializeField] private List<AudioLibMusic> musics;
        [SerializeField] private List<AudioLibSound> sounds;
        [SerializeField] private List<AudioLibVoice> voices;

        private Dictionary<string, AudioSource> _audioSourcesSounds;
        private AudioSource _sourceMusicCurrent;
        private AudioSource _sourceMusicPrevious;
        private AudioSource _sourceMusicNext;

        private List<AudioLibMusic> _playList;

        /// <summary>
        /// Вызвать в Launch
        /// </summary>
        public void Init()
        {
            _audioSourcesSounds = new Dictionary<string, AudioSource>();
            AddSource();
        }

        private void AddSource()
        {
            // Добавление компонентов аудиосорсов для звуков, если не UI
            if (!isUIElement)
            {
                if (!(sounds?.Count > 0)) return;

                foreach (var sound in sounds)
                {
                    SearchSound(sound.Track, out var source);
                    if (source != null)
                    {
                        _audioSourcesSounds.Add(sound.Track.ToString(), source);
                    }
                }
            }
        }

        public void Play(Sound sound)
        {
            if (!(sounds?.Count > 0)) return;

            if (isUIElement)
            {
                PlaySoundUI(sound);
            }
            else
            {
                PlayTrack(sound.ToString());
            }
        }

        public void Play(Music music, float fadeDelay = 1.5f, bool isPlayList = false)
        {
            if (_sourceMusicCurrent == null)
            {
                SearchMusic(music, out _sourceMusicPrevious);
                _sourceMusicPrevious.Play();
                StartCoroutine(isPlayList
                    ? PlayRandomAndRemoveSourceComponentM(_sourceMusicPrevious)
                    : PlayAndRemoveSourceComponentM(_sourceMusicPrevious));
                _sourceMusicCurrent = _sourceMusicPrevious;
            }
            else
            {
                if (_sourceMusicPrevious.isPlaying)
                {
                    SearchMusic(music, out _sourceMusicNext);
                    _sourceMusicNext.Play();
                    StartCoroutine(Delay(_sourceMusicNext, fadeDelay));
                    StartCoroutine(Delay(_sourceMusicPrevious, fadeDelay, false));
                    StartCoroutine(PlayAndRemoveSourceComponentM(_sourceMusicNext));
                    StartCoroutine(isPlayList
                        ? PlayRandomAndRemoveSourceComponentM(_sourceMusicNext)
                        : PlayAndRemoveSourceComponentM(_sourceMusicNext));
                    _sourceMusicCurrent = _sourceMusicNext;
                }
                else if (_sourceMusicNext.isPlaying)
                {
                    SearchMusic(music, out _sourceMusicPrevious);
                    _sourceMusicPrevious.Play();
                    StartCoroutine(Delay(_sourceMusicPrevious, fadeDelay));
                    StartCoroutine(Delay(_sourceMusicNext, fadeDelay, false));
                    StartCoroutine(isPlayList
                        ? PlayRandomAndRemoveSourceComponentM(_sourceMusicPrevious)
                        : PlayAndRemoveSourceComponentM(_sourceMusicPrevious));
                    _sourceMusicCurrent = _sourceMusicPrevious;
                }
            }
        }

        public void PlayMusicRandom()
        {
            var randomMusic = Random.Range(0, musics.Count - 1);

            Play(musics[randomMusic].Track);
        }

        public void PlayMusicRandomAllPlayList()
        {
            _playList = new List<AudioLibMusic>(musics);
            PlayRandomTrack();
        }

        public void Play(Voice voice)
        {
            AudioSource source = null;

            if (!(voices?.Count > 0)) return;

            foreach (var v in Game.Instance.AudioManager.Voices)
            {
                if (v.Track != voice || voice == Voice.NONE) continue;

                source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = Game.Instance.AudioManager.AmgVoice;
                source.playOnAwake = false;
                source.volume = v.Volume;
                source.clip = v.Clip;
                foreach (var i in voices)
                {
                    if (i.Track != voice || i.Track == Voice.NONE) continue;
                    source.loop = i.Loop;
                    break;
                }

                break;
            }

            if (source == null) return;

            source.Play();
            StartCoroutine(PlayAndRemoveSourceComponentS(source));
        }

        public void StopMusic(float fadeDelay = 0f)
        {
            AudioSource source = null;

            if (_sourceMusicCurrent.clip == _sourceMusicNext.clip)
            {
                source = _sourceMusicNext;
            }
            else if (_sourceMusicCurrent.clip == _sourceMusicPrevious.clip)
            {
                source = _sourceMusicPrevious;
            }

            if (source == null) return;

            StartCoroutine(Delay(source, fadeDelay, false));
            _sourceMusicCurrent = null;
        }

        public void StopSound(Sound sound)
        {
            if (isUIElement) return;

            if (_audioSourcesSounds.TryGetValue(sound.ToString(), out var track))
            {
                track.Stop();
            }
            else
            {
                Debug.Log($"Track {sound.ToString()} не найден");
            }
        }

        private IEnumerator PlayAndRemoveSourceComponentS(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            if (source != null && !source.loop)
            {
                Destroy(source);
            }
        }

        private IEnumerator PlayAndRemoveSourceComponentM(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            if (source != null && !source.loop)
            {
                Destroy(source);
                _sourceMusicCurrent = null;
            }
        }
        
        private IEnumerator PlayRandomAndRemoveSourceComponentM(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            if (source != null && !source.loop)
            {
                Destroy(source);
                _sourceMusicCurrent = null;
                PlayRandomTrack();
            }
        }

        private void PlayRandomTrack()
        {
            if (_playList.Count > 0)
            {
                var randomMusic = Random.Range(0, _playList.Count - 1);
                Play(_playList[randomMusic].Track, 15, true);
                _playList.RemoveAt(randomMusic);
            }
            else
            {
                PlayMusicRandomAllPlayList();
            }
        }

        private void PlaySoundUI(Sound sound)
        {
            SearchSound(sound, out var source);

            if (source == null) return;

            source.Play();
            StartCoroutine(PlayAndRemoveSourceComponentS(source));
        }

        private void PlayTrack(string trackName)
        {
            if (_audioSourcesSounds.TryGetValue(trackName, out var track))
            {
                track.Play();
            }
            else
            {
                Debug.Log($"Track {trackName} не найден");
            }
        }

        private void SearchSound(Sound sound, out AudioSource source)
        {
            source = null;

            foreach (var s in Game.Instance.AudioManager.Sounds)
            {
                if (s.Track != sound || sound == Sound.NONE) continue;

                source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = Game.Instance.AudioManager.AmgSound;
                source.playOnAwake = false;
                source.volume = s.Volume;
                source.clip = s.Clip;
                foreach (var i in sounds)
                {
                    if (i.Track != sound || i.Track == Sound.NONE) continue;
                    source.loop = i.Loop;
                    break;
                }

                break;
            }
        }

        private void SearchMusic(Music music, out AudioSource source)
        {
            source = null;

            foreach (var m in Game.Instance.AudioManager.Musics)
            {
                if (m.Track != music || music == Music.NONE) continue;

                source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = Game.Instance.AudioManager.AmgMusic;
                source.playOnAwake = false;
                source.volume = m.Volume;
                source.clip = m.Clip;
                foreach (var i in musics)
                {
                    if (i.Track != music || i.Track == Music.NONE) continue;
                    source.loop = i.Loop;
                    break;
                }

                break;
            }
        }

        private IEnumerator Delay(AudioSource source, float delay, bool increase = true)
        {
            var volume = source.volume;
            var t = 0f;

            if (increase) // +
            {
                while (t < delay)
                {
                    source.volume = Mathf.Lerp(0, volume, t);
                    t += Time.deltaTime / delay;
                    yield return null;
                }
            }
            else // -
            {
                while (t < delay)
                {
                    source.volume = Mathf.Lerp(volume, 0f, t);
                    t += Time.deltaTime / delay;
                    yield return null;
                }

                source.Stop();
                Destroy(source);
            }
        }
    }
}