/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;

namespace Solitaire.Classes.Helpers
{
    /* Use the Force... */
    public enum SoundType
    {
        Shuffle = 0,
        Deal = 1,
        Drop = 2,
        Complete = 3,
        Empty = 5,
        Win = 5
    }

    internal class AudioData
    {
        public SoundType Type { get; set; }

        public SoundPlayer Player { get; set; }
    }

    public static class AudioManager
    {
        /* Easier way to manage and play external sounds */
        private static readonly List<AudioData> Sounds = new List<AudioData>(); 
        
        static AudioManager()
        {
            Sounds.AddRange(
                new[]
                {
                    LoadSound(AppPath.MainDir(@"\data\sound\card-shuffle.wav"), SoundType.Shuffle),
                    LoadSound(AppPath.MainDir(@"\data\sound\card-deal.wav"), SoundType.Deal),
                    LoadSound(AppPath.MainDir(@"\data\sound\card-drop.wav"), SoundType.Drop),
                    LoadSound(AppPath.MainDir(@"\data\sound\card-complete.wav"), SoundType.Complete),
                    LoadSound(AppPath.MainDir(@"\data\sound\card-none.wav"), SoundType.Empty),
                    LoadSound(AppPath.MainDir(@"\data\sound\game-win.wav"), SoundType.Win)
                });
        }

        public static void Dispose()
        {
            foreach (var s in Sounds.Where(s => s.Player != null))
            {
                s.Player.Dispose();
            }
        }

        public static void Play(SoundType type)
        {
            foreach (var s in Sounds.Where(s => s.Type == type && s.Player != null))
            {
                s.Player.Play();
            }
        }

        /* Private load method */
        private static AudioData LoadSound(string file, SoundType type)
        {
            var data = new AudioData {Type = type};
            if (File.Exists(file))
            {
                data.Player = new SoundPlayer(file);
            }
            return data;
        }
    }
}
