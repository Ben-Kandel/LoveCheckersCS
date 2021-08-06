using System;
using LoveCheckers.Models;
using Love;
using Point = LoveCheckers.Models.Point;

namespace LoveCheckers.Views
{
    public class BoardView
    {
        
        private Board Board;

        private static Color DefaultHighlight = new Color(181f / 255, 179f / 255, 147f/ 255, 1);
        private static Color MoveHighlight = new Color(0, 0.7f, 0.3f, 1);
        private static Color PieceHighlight = new Color(87f / 255, 148f / 255, 37f / 255, 1);
        
        private static Image BlackPawn = Graphics.NewImage("../../../Images/black_pawn.png");
        private static Image BlackKing = Graphics.NewImage("../../../Images/black_king.png");
        private static Image RedPawn = Graphics.NewImage("../../../Images/red_pawn.png");
        private static Image RedKing = Graphics.NewImage("../../../Images/red_king.png");
        private const int ImageSize = 64; // I know the images are 64x64 pixels

        public BoardView(Board b)
        {
            Board = b;
        }

        public void Draw()
        {
            // the drawing order matters. for example, we want to draw the mousehighlight before we draw the piece,
            // otherwise when we mouse over a piece, the highlight will cover it up.
            DrawSuggestedJumps();
            DrawMouseHighlight();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int drawX = Board.X + (Board.TileSize * x);
                    int drawY = Board.Y + (Board.TileSize * y);
                    // draw the square
                    Graphics.SetColor(1, 1, 1); // white
                    Graphics.Rectangle(DrawMode.Line, drawX, drawY, Board.TileSize, Board.TileSize);
                    
                    // draw the piece on the square
                    int piece = Board.Grid[x, y];
                    if (Piece.IsNothing(piece)) { continue; } // if there is no piece, skip over the rest of the code
                    const int offset = (Board.TileSize - ImageSize) / 2;
                    Graphics.Draw(PieceToImage(piece), drawX + offset, drawY + offset);
                }
            }
            DrawMoveHighlights();
        }

        private void DrawSuggestedJumps()
        {
            foreach(Move move in Board.MoreHighlights)
            {
                DrawHighlight(move.Origin, PieceHighlight);
            }
        }

        private void DrawHighlight(Point p, Color c)
       {
           int x = Board.X + (Board.TileSize * p.X) + 1;
           int y = Board.Y + (Board.TileSize * p.Y) + 1;
           const int size = Board.TileSize - 2;
           Graphics.SetColor(c);
           Graphics.Rectangle(DrawMode.Fill, x, y, size, size);
           Graphics.SetColor(1, 1, 1); // reset back to white
       }

       private void DrawMouseHighlight()
       {
           // if the mouse is over a square, draw a highlight
           int mouseX = (int)Mouse.GetX();
           int mouseY = (int) Mouse.GetY();
           if (Board.WithinBounds(mouseX, mouseY))
           {
               DrawHighlight(Board.GetPointAt(mouseX, mouseY), DefaultHighlight);
           }
       }

       private void DrawMoveHighlights()
       {
           foreach (Move move in Board.HighlightedMoves)
           {
               DrawHighlight(move.Destination, MoveHighlight);
           }
       }

       private static Image PieceToImage(int piece)
       {
           // a different syntax for switch
           return piece switch
           {
               Piece.Black | Piece.Pawn => BlackPawn,
               Piece.Black | Piece.King => BlackKing,
               Piece.Red | Piece.Pawn => RedPawn,
               Piece.Red | Piece.King => RedKing,
               _ => BlackPawn // return the BlackPawn image as the default one if we don't recognize the piece
           };
       }
    }
}