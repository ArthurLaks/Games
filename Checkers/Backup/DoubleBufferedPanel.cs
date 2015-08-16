using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Breakout {
    class DoubleBufferedPanel : Panel {
        public DoubleBufferedPanel( ) {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
