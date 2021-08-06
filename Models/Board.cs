using System;
using System.Collections.Generic;
using System.Linq;

namespace LoveCheckers.Models
{

    // public class Pair<T1, T2>
    // {
    //     public T1 First { get; }
    //     public T2 Second { get; }
    //     
    //     public Pair(T1 first, T2 second)
    //     {
    //         First = first;
    //         Second = second;
    //     }
    //     
    // }
    public record Pair(int Piece, Point Pos);
    
    public class Board : Entity
    {
        public int[,] Grid { get; private set; }
        public const int TileSize = 75;
        public List<Move> HighlightedMoves { get; private set; }
        public List<Move> MoreHighlights { get; set; }

        private List<Pair> RedPieces;
        private List<Pair> BlackPieces;

        public Board()
        {
            X = 50;
            Y = 50;
            HighlightedMoves = new List<Move>();
            MoreHighlights = new List<Move>();
            RedPieces = new List<Pair>();
            BlackPieces = new List<Pair>();
            InitializeBoard();
        }
        
        private void SetUpPiece(int x, int y, int piece)
        {
            Grid[x, y] = piece;
            Pair pair = new Pair(piece, new Point(x, y));
            if (Piece.GetColor(piece) == Piece.Black)
            {
                BlackPieces.Add(pair);
            }
            else
            { 
                RedPieces.Add(pair);
            }
        }
        
        private void InitializeBoard()
        {
            Grid = new int[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Grid[x, y] = Piece.Nothing;
                }
            }

            // setting up pieces in the starting configuration
            // writing a loop for this is kinda ugly and it's really not hard to just do it manually
            SetUpPiece(1, 0, Piece.Black | Piece.Pawn);
            SetUpPiece(3, 0, Piece.Black | Piece.Pawn);
            SetUpPiece(5, 0, Piece.Black | Piece.Pawn);
            SetUpPiece(7, 0, Piece.Black | Piece.Pawn);
            SetUpPiece(0, 1, Piece.Black | Piece.Pawn);
            SetUpPiece(2, 1, Piece.Black | Piece.Pawn);
            SetUpPiece(4, 1, Piece.Black | Piece.Pawn);
            SetUpPiece(6, 1, Piece.Black | Piece.Pawn);
            SetUpPiece(1, 2, Piece.Black | Piece.Pawn);
            SetUpPiece(3, 2, Piece.Black | Piece.Pawn);
            SetUpPiece(5, 2, Piece.Black | Piece.Pawn);
            SetUpPiece(7, 2, Piece.Black | Piece.Pawn);

            SetUpPiece(0, 5, Piece.Red | Piece.Pawn);
            SetUpPiece(2, 5, Piece.Red | Piece.Pawn);
            SetUpPiece(4, 5, Piece.Red | Piece.Pawn);
            SetUpPiece(6, 5, Piece.Red | Piece.Pawn);
            SetUpPiece(1, 6, Piece.Red | Piece.Pawn);
            SetUpPiece(3, 6, Piece.Red | Piece.Pawn);
            SetUpPiece(5, 6, Piece.Red | Piece.Pawn);
            SetUpPiece(7, 6, Piece.Red | Piece.Pawn);
            SetUpPiece(0, 7, Piece.Red | Piece.Pawn);
            SetUpPiece(2, 7, Piece.Red | Piece.Pawn);
            SetUpPiece(4, 7, Piece.Red | Piece.Pawn);
            SetUpPiece(6, 7, Piece.Red | Piece.Pawn);
        }

        public void PrintBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int piece = Grid[x, y];
                    if (Piece.GetType(piece) == Piece.Nothing)
                    {
                        continue; // don't print anything
                    }
                    string type = (Piece.GetType(piece) == Piece.Pawn) ? "pawn" : "king";
                    string color = (Piece.GetColor(piece) == Piece.Red) ? "red" : "black";
                    Console.WriteLine($"Piece at {x},{y} is a {color} {type}");
                }
            }
        }
        
        public bool WithinBounds(int x, int y)
        {
            // im using the redundant 'this' qualifier just to make it clear what's going on here
            return x > this.X && x < this.X + (TileSize * 8) && y > this.Y && y < this.Y + (TileSize * 8);
        }
        
        public Point GetPointAt(int x, int y)
        {
            // we can just divide the mouse click coordinates by the size of the tiles to get the indices of the square clicked
            // but, remember that the board starts at some position, X and Y, so we have to subtract out that offset first
            // im using the redundant 'this' qualifier just to make it clear what's going on here
            int pointX = (x - this.X) / TileSize;
            int pointY = (y - this.Y) / TileSize;
            return new Point(pointX, pointY);
        }

        public int GetPieceAtPoint(Point p)
        {
            return Grid[p.X, p.Y];
        }

        public bool SquareOccupied(Point pos)
        {
            return Grid[pos.X, pos.Y] != Piece.Nothing;
        }

        public bool SquareOccupiedByEnemy(Point pos, int friendlyColor)
        {
            int piece = Grid[pos.X, pos.Y];
            return Piece.GetColor(piece) != friendlyColor;
        }

        public bool IsValidPoint(Point p)
        {
            return p.X is >= 0 and < 8 && p.Y is >= 0 and < 8;
        }

        public void SetHighlightedMoves(List<Move> moves)
        {
            HighlightedMoves = moves;
        }

        public void ClearHighlightedMoves()
        {
            HighlightedMoves.Clear();
        }

        public static bool CanPromote(int piece, Point pos)
        {
            int color = Piece.GetColor(piece);
            return (color == Piece.Red && pos.Y == 0) || (color == Piece.Black && pos.Y == 7);
        }

        // will need this later
        public List<Pair> GetPiecesOfColor(int color)
        {
            List<Pair> answer = new List<Pair>();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int piece = Grid[x, y];
                    if (Piece.GetColor(piece) == color)
                    {
                        answer.Add(new Pair(piece, new Point(x, y)));
                    }
                }
            }
            return answer;
        }

        public void UpdatePositions()
        {
            RedPieces = GetPiecesOfColor(Piece.Red);
            BlackPieces = GetPiecesOfColor(Piece.Black);
        }

    }
}