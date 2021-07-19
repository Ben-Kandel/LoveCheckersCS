using System;
using LoveCheckers.Models;
using Love;
using Point = LoveCheckers.Models.Point;

namespace LoveCheckers.Views
{
    public class BoardView
    {
        
        private Board Board;
        private int PieceRadius;

        private static Color DefaultHighlight = new Color(181f / 255, 179f / 255, 147f/ 255, 1);
        private static Color MoveHighlight = new Color(0, 0.7f, 0.3f, 1);

        public BoardView(Board b)
        {
            Board = b;
            PieceRadius = Board.TileSize / 2 - 5; // this seems to work fine
        }

        public void Draw()
        {
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
                    // TODO: Download some checker piece images and draw those instead of boring circles.
                    int piece = Board.Grid[x, y];
                    if (Piece.IsNothing(piece)) { continue; } // if there is no piece, skip over the rest of the code
                    if (Piece.IsRed(piece))
                    {
                        Graphics.SetColor(1, 0, 0); // red
                    }
                    else
                    {
                        Graphics.SetColor(0, 0, 0); // black
                    }

                    DrawMode mode = (Piece.GetType(piece) == Piece.Pawn) ? DrawMode.Fill : DrawMode.Line;
                    Graphics.Circle(mode, drawX + (Board.TileSize / 2), drawY + (Board.TileSize / 2), PieceRadius);
                }
            }
            DrawMoveHighlights();
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
    }
}