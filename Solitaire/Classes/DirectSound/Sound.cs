/* Solitaire
 * Version 1.0.0
 * Written by: Jason James Newland
 * ©2025 Kangasoft Software */
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using xsDirectX;

namespace Solitaire.Classes.DirectSound
{
    public class Sound
    {
        private Timer _play;
        private IBasicAudio _audio;
        private IGraphBuilder _graph;
        private IMediaControl _media;
        private int _pan;
        private IMediaPosition _position;
        private int _volume;

        public event Action<Sound> OnMediaEnded;
        public event Action<Sound, double> OnMediaPositionChanged;

        public string Clip { get; set; }

        /* Constructor/destuctor */
        public Sound()
        {
            _volume = 100;
            _pan = 50;
        }

        public Sound(string fileName)
        {
            Clip = fileName;
            _volume = 100;
            _pan = 50;
        }

        ~Sound()
        {
            Close();
        }

        /* Public properties */
        public int Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                if (_volume < 0)
                {
                    _volume = 0;
                }
                if (_volume > 100)
                {
                    _volume = 100;
                }
                if (_audio != null)
                {
                    _audio.SetVolume((_volume - 100)*50);
                }
            }
        }

        public int Pan
        {
            get { return _pan; }
            set
            {
                _pan = value;
                if (_pan < 0)
                {
                    _pan = 0;
                }
                if (_pan > 100)
                {
                    _pan = 100;
                }
                if (_audio != null)
                {
                    _audio.SetBalance((_pan - 50)*200);
                }
            }
        }

        public double Position
        {
            get
            {
                double pos;
                _position.GetCurrentPosition(out pos);
                return pos;
            }
            set
            {
                double duration;
                _position.GetDuration(out duration);
                if (value <= duration)
                {
                    _position.SetCurrentPosition(value);
                }
            }
        }

        public double Duration
        {
            get
            {
                double duration;
                _position.GetDuration(out duration);
                return duration;
            }
        }

        /* Public methods */
        public void PlayAsync()
        {
            /* This does NOT check for cross-threading, so if events are raised, it's up to the code at the other
             * end to do Invoke/BeginInvoke (if dealing with UI) */
            var t = new Thread(Play) {IsBackground = true};
            t.Start();
        }

        public void Play()
        {
            Close();
            if (!Open() || _media == null || _media.Run() < 0)
            {
                return;
            }
            try
            {
                _audio.SetVolume((_volume - 100) * 50);
                _audio.SetBalance((_pan - 50) * 200);
                _play = new Timer(TimerTick, null, 0, 100);
            }
            catch (Exception)
            {
                /* Silently ignore - this will sometimes happen when playing short files (1sec or so) in quick succession */
                Debug.Assert(true);
            }
        }

        public void Stop()
        {
            if (_media != null)
            {
                _media.Stop();
            }
            if (_play == null)
            {
                return;
            }
            _play.Dispose();
            _play = null;
        }

        public void Pause()
        {
           if (_media != null)
           {
               _media.Pause();
           }
           if (_play == null)
           {
               return;
           }
           _play.Dispose();
           _play = null;
        }

        public void Resume()
        {
            if (_media != null)
            {
                if (_media.Run() < 0)
                {
                    return;
                }
            }
            _play = new Timer(TimerTick, null, 0, 100);
        }

        /* Private helper methods */    
        private bool Open()
        {
            bool flag;
            if (string.IsNullOrEmpty(Clip) || !File.Exists(Clip))
            {
                return false;
            }
            object o = null;
            try
            {
                var typeFromClsid = Type.GetTypeFromCLSID(Clsid.FilterGraph);
                if (typeFromClsid == null)
                {
                    return false;
                }
                o = Activator.CreateInstance(typeFromClsid);
                _graph = (IGraphBuilder) o;
                o = null;
                if (_graph.RenderFile(Clip, null) < 0)
                {
                    return false;
                }
                _media = (IMediaControl)_graph;
                _position = (IMediaPosition)_graph;
                _audio = _graph as IBasicAudio;                   
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }
            finally
            {
                if (o != null)
                {
                    Marshal.ReleaseComObject(o);
                }
            }
            return flag;
        }

        private void Close()
        {
            _position = null;
            _media = null;
            _audio = null;
            if (_graph != null)
            {
                try
                {
                    Marshal.ReleaseComObject(_graph);
                }
                catch (Exception)
                {
                    /* Silently ignore - this will sometimes happen when playing short files (1sec or so) in quick succession */
                    Debug.Assert(true);
                }
                _graph = null;
            }
            GC.Collect();
        }

        /* Timer callback for getting the current position of the streaming audio */
        private void TimerTick(object sender)
        {
            double pos;
            double duration;
            try
            {
                _position.GetCurrentPosition(out pos);
                _position.GetDuration(out duration);
            }
            catch (Exception)
            {
                /* Silently ignore - this will sometimes happen when playing short files (1sec or so) in quick succession */
                return;
            }
            if (OnMediaPositionChanged != null)
            {
                OnMediaPositionChanged(this, pos);
            }
            if (pos < duration)
            {
                return;
            }
            Stop();
            if (OnMediaEnded != null)
            {
                OnMediaEnded.Invoke(this);
            }
        }
    }
}