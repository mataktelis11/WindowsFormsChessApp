using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsChessApp.ChessItems
{
    public class Tile : PictureBox
    {
        public bool IsEmpty{ get; set; } = true; 
        public ChessPiece Pointer { get; set; }
    }
}
