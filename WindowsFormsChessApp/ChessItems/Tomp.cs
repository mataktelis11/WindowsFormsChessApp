using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsChessApp.ChessItems
{
    //class used when a pawn dies
    public class Tomp
    {
        public int index { get; set; }
        public Pawn pawn { get; set; }

        public Tomp(int index, Pawn pawn)
        {
            this.index = index;
            this.pawn = pawn;
        }
    }
}
