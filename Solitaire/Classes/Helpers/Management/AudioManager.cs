/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Solitaire.Classes.Data;
using Solitaire.Classes.DirectSound;

namespace Solitaire.Classes.Helpers.Management
{
    /* Use the Force... */
    public enum SoundType
    {
        Shuffle = 0,
        Deal = 1,
        Drop = 2,
        Complete = 3,
        Empty = 5,
        Win = 6
    }

    internal class AudioData
    {
        public SoundType Type { get; set; }

        public Sound Player { get; set; }
    }

    public static class AudioManager
    {
        /* Easier way to manage and play external sounds */
        private static readonly List<AudioData> Sounds = new List<AudioData>();
 
        private static readonly List<string> Music = new List<string>();
        private static int _musicIndex ;

        private static Sound _music;

        private static int _effectsVolume;
        private static int _musicVolume;

        static AudioManager()
        {
            Sounds.AddRange(
                new[]
                {
                    LoadSound(Utils.MainDir(@"\data\sound\card-shuffle.wav"), SoundType.Shuffle),
                    LoadSound(Utils.MainDir(@"\data\sound\card-deal.wav"), SoundType.Deal),
                    LoadSound(Utils.MainDir(@"\data\sound\card-drop.wav"), SoundType.Drop),
                    LoadSound(Utils.MainDir(@"\data\sound\card-complete.wav"), SoundType.Complete), 
                    LoadSound(Utils.MainDir(@"\data\sound\card-none.wav"), SoundType.Empty),
                    LoadSound(Utils.MainDir(@"\data\sound\game-win.mp3"), SoundType.Win)
                });

            SetEffectsVolume(SettingsManager.Settings.Options.Sound.EffectsVolume);
            SetMusicVolume(SettingsManager.Settings.Options.Sound.MusicVolume);

            var musicSearch = new FolderSearch();
            musicSearch.OnFileSearchCompleted += MusicSearchCompleted;
            musicSearch.OnFileFound += MusicSearchFileFound;

            var d = new DirectoryInfo(Utils.MainDir(@"data\sound\music\", false));
            musicSearch.BeginSearch(d, "*.mp3", "*", false);
        }

        public static void SetEffectsVolume(int volume)
        {
            _effectsVolume = volume;
            if (Sounds.Count == 0)
            {
                return;
            }
            foreach (var s in Sounds)
            {
                s.Player.Volume = volume;
            }
        }

        public static void SetMusicVolume(int volume)
        {
            _musicVolume = volume;
            if (Sounds.Count == 0 || _music == null)
            {
                return;
            }
            _music.Volume = volume;
        }

        public static void Play(SoundType type)
        {
            if (!SettingsManager.Settings.Options.Sound.EnableEffects)
            {
                return;
            }
            foreach (var s in Sounds.Where(s => s.Type == type))
            {
                if (s.Player != null)
                {
                    s.Player.PlayAsync(true);    
                }
                return;
            }
        }

        public static void PlayMusic(bool next = false)
        {
            if (!SettingsManager.Settings.Options.Sound.EnableMusic || Music.Count == 0)
            {
                return;
            }
            if (next)
            {
                _musicIndex++;
                if (_musicIndex > Music.Count - 1)
                {
                    _musicIndex = 0;
                    Music.Shuffle();
                }
            }
            _music = new Sound(Music[_musicIndex]) { Volume = _musicVolume };
            _music.OnMediaEnded += OnMusicEnd;
            _music.PlayAsync();
        }

        public static void PauseMusic()
        {
            if (_music == null)
            {
                return;
            }
            _music.Pause();            
        }

        public static void ResumeMusic()
        {
            if (_music == null)
            {
                return;
            }
            _music.Resume(); 
        }

        public static void StopMusic()
        {
            if (_music == null)
            {
                return;
            }
            _music.Stop();
        }

        /* FolderSearch callback */
        private static void MusicSearchFileFound(string file)
        {
            Music.Add(file);
        }

        private static void MusicSearchCompleted(FolderSearch search)
        {
            Music.Shuffle();
            /* Begin music playback */
            PlayMusic();
        }

        /* Music playback callback */
        private static void OnMusicEnd(Sound sound)
        {
            _music.OnMediaEnded -= OnMusicEnd;
            /* Get next track */
            _musicIndex++;
            if (_musicIndex > Music.Count - 1)
            {
                _musicIndex = 0;
                Music.Shuffle();
            }
            _music = new Sound(Music[_musicIndex]) { Volume = _musicVolume };
            _music.OnMediaEnded += OnMusicEnd;
            _music.PlayAsync();
        }

        /* Private load method */
        private static AudioData LoadSound(string file, SoundType type)
        {
            var data = new AudioData {Type = type};
            if (File.Exists(file))
            {
                data.Player = new Sound(file) {Volume = _effectsVolume};
            }
            return data;
        }
    }
}
