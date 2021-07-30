using System;
using System.Collections.Generic;
using System.Linq;

namespace LoveCheckers.Models
{
    public class MoveGenerator
    {
        private Board Board;
        private int SelectedPiece;
        private Point Start;
        public List<Move> ValidMoves { get; }
        private bool Highlight;
        
        public MoveGenerator(Board board, int piece, Point start, bool highlight = true)
        {
            Board = board;
            SelectedPiece = piece;
            Start = start;
            ValidMoves = new List<Move>();
            Highlight = highlight;
            GenerateValidMoves();
        }

        private enum Direction
        {
            NorthEast, SouthEast, NorthWest, SouthWest
        }

        private static Dictionary<Direction, int> ChangeX = new Dictionary<Direction, int>
        {
            {Direction.NorthEast, -1},
            {Direction.SouthEast, -1},
            {Direction.NorthWest, 1},
            {Direction.SouthWest, 1}
        };

        private static Dictionary<Direction, int> ChangeY = new Dictionary<Direction, int>
        {
            {Direction.NorthEast, -1},
            {Direction.SouthEast, 1},
            {Direction.NorthWest, -1},
            {Direction.SouthWest, 1}
        };

        private static Point GoInDir(Point start, Direction dir)
        {
            return new Point(start.X + ChangeX[dir], start.Y + ChangeY[dir]);
        }

        public bool IsValidMove(Point destination)
        {
            // return true if one of our moves has the same end point as the one passed in
            return ValidMoves.Any(m => m.Destination == destination);
        }

        public Move GetMoveFromDestination(Point destination)
        {
            // return the move where the end point is the same as the one provided
            // this will never break if we call IsValidMove() beforehand
            return ValidMoves.Find(m => m.Destination == destination);
        }

        public bool HasMoves()
        {
            return ValidMoves.Count != 0;
        }

        public bool HasJump()
        {
            // return true if any of our valid moves is a jump move
            return ValidMoves.Any(m => m.IsJump);
        }
        
        private void GenerateValidMoves()
        {
            ValidMoves.Clear(); // clear out previous moves just in case
            Direction[] dirs = {Direction.NorthEast, Direction.NorthWest, Direction.SouthEast, Direction.SouthWest};
            if (Piece.GetType(SelectedPiece) == Piece.King)
            {
                GenerateMovesInDirs(dirs);
            }else if (Piece.GetColor(SelectedPiece) == Piece.Red)
            {
                GenerateMovesInDirs(dirs.Take(2)); // the first two directions, northeast and northwest
            }
            else
            {
                GenerateMovesInDirs(dirs.Skip(2)); // the last two directions, southeast and southwest
            }
            ForceJumpMoves(); // if there is a jump move, remove all the other ones
            if (Highlight)
            {
                Board.SetHighlightedMoves(ValidMoves);    
            }
        }

        private void GenerateMovesInDirs(IEnumerable<Direction> dirs)
        {
            foreach(Direction dir in dirs)
            {
                Point next = GoInDir(Start, dir);
                if (!Board.IsValidPoint(next)) { continue; } // if it's not a valid point, skip this direction
                if (!Board.SquareOccupied(next)) // if the square is not occupied, this is a good move
                {
                    CreateMove(next, false, null);
                    // ValidMoves.Add(new Move(SelectedPiece, Start, next, false));    
                }else if (Board.SquareOccupiedByEnemy(next, Piece.GetColor(SelectedPiece)))
                {
                    // if the square is occupied by the enemy, we check the next point again and see if it is open
                    Point nextNext = GoInDir(next, dir);
                    if (!Board.IsValidPoint(nextNext)) { continue; }
                    if (!Board.SquareOccupied(nextNext))
                    {
                        CreateMove(nextNext, true, next);
                        // ValidMoves.Add(new Move(SelectedPiece, Start, nextNext, true, next));
                    }
                }
            }
        }

        private void ForceJumpMoves()
        {
            // LINQ makes this really easy
            if (ValidMoves.Any(m => m.IsJump)) // if any of these are a jump
            {
                // we need to remove all non-jumps from the list
                ValidMoves.RemoveAll(m => !m.IsJump);
            }
        }

        private void CreateMove(Point destination, bool jump, Point captured)
        {
            bool promotion = Board.CanPromote(SelectedPiece, destination);
            Move m = new Move(SelectedPiece, Start, destination, jump, captured, promotion);
            ValidMoves.Add(m);
        }


    }
}