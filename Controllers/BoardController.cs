using LoveCheckers.Models;
using Point = LoveCheckers.Models.Point;

namespace LoveCheckers.Controllers
{
    // TODO: Get rid of this class? It doesn't do anything right now. Need to think of other ways of handling input
    public class BoardController
    {
        private Board Board;
        public BoardController(Board b)
        {
            Board = b;
        } 
        
        public void MousePressed(int x, int y, int button, bool isTouch)
        {
            // see if we clicked on the board or not
            if (WithinBounds(x, y))
            {
                Point p = GetPointAt(x, y);
                // Console.WriteLine($"Clicked on square at {p.X}, {p.Y}");
            }
        }
        
        private Point GetPointAt(int x, int y)
        {
            // we can just divide the mouse click coordinates by the size of the tiles to get the indices of the square clicked
            // but, remember that the board starts at some position, Board.X and Board.Y, so we have to subtract out that offset first
            int X = (x - Board.X) / Board.TileSize;
            int Y = (y - Board.Y) / Board.TileSize;
            return new Point(X, Y);
        }
        private bool WithinBounds(float x, float y)
        {
            return x > Board.X && x < Board.X + (Board.TileSize * 8) && y > Board.Y && y < Board.Y + (Board.TileSize * 8);
        }

    }
}