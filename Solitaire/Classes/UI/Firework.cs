/* Fireworks generator
 * By: Thao Meo - 2021
 * https://laptrinhvb.net/bai-viet/chuyen-de-csharp/---csharp----chia-se-source-code-firework-effect-trong-winform/5481a0c82f1b246a.html */
using System;
using System.Drawing;

namespace Solitaire.Classes.UI
{
    public class FireWork
    {
        /* Draw some simple pretty fireworks on the background :) */
        private const double Step = 0.5;
        private const int MaxRays = 10;

        private readonly double _start;
        private readonly double _stop;
        private readonly double _len;
        private double _curpos;

        private readonly SolidBrush _brush;

        private readonly int _nrays;
        private readonly int _cx;
        private readonly int _cy;
        private readonly double[] _sintab;
        private readonly double[] _costab;
        private readonly double _descent;

        private static readonly Random Rand = new Random();       

        /* Constructor */
        public FireWork(int xsize, int ysize)
        {
            _cx = Rand.Next(xsize);
            _cy = Rand.Next(ysize);
            _descent = Rand.NextDouble() * 0.1 + 0.05;

            _start = Rand.NextDouble() * 10 + 5;
            _stop = Rand.NextDouble() * 50 + 50;
            _len = Rand.NextDouble() * 50 + 25;

            _curpos = _start;

            _brush = new SolidBrush(Color.FromArgb(Rand.Next(128, 256),
                                  Rand.Next(128, 256),
                                  Rand.Next(128, 256)));

            _nrays = Rand.Next(5, MaxRays + 1);

            var angleInc = 2 * Math.PI / _nrays;
            var angle = Rand.NextDouble() * angleInc;

            _costab = new double[_nrays];
            _sintab = new double[_nrays];
            for (var i = 0; i < _nrays; ++i, angle += angleInc)
            {
                _costab[i] = Math.Cos(angle);
                _sintab[i] = Math.Sin(angle);
            }
        }

        /* Public methods */
        public bool Update()
        {
            _curpos += Step;
            return _curpos <= _stop + _len;
        }

        public void Paint(Graphics g)
        {
            var lower = Math.Max(_curpos - _len, _start);
            var upper = Math.Min(_curpos, _stop);

            for (var pos = lower; pos < upper; pos += Step)
            {
                var quad = _descent * pos;
                var quadsq = quad * quad;

                for (var i = 0; i < _nrays; ++i)
                {
                    g.FillRectangle(_brush,
                        (int)(_cx + pos * _costab[i]),
                        (int)(_cy + pos * _sintab[i] + quadsq),
                        1, 1);
                }
            }
        }
    }
}