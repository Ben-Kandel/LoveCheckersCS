using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace LoveCheckers.Models
{
    public class MoveGenerator
    {
        private Board Board;
        private int SelectedPiece;
        private Point Start;
        private List<Move> ValidMoves;
        
        public MoveGenerator(Board board, int piece, Point start)
        {
            Board = board;
            SelectedPiece = piece;
            Start = start;
            ValidMoves = new List<Move>();
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
            if (Piece.GetType(SelectedPiece) == Piece.King) // kings can move in all directions
            {
                GenerateUpMoves();
                GenerateDownMoves();
            }
            else
            {
                if (Piece.GetColor(SelectedPiece) == Piece.Red)
                {
                    GenerateUpMoves();
                }
                else
                {
                    GenerateDownMoves();
                }                
            }
            ForceJumpMoves(); // if there is a jump move, remove all the other ones
            Board.UpdateHighlightedMoves(ValidMoves); // tell the board so it can draw the highlights for the moves
        }
        
        // TODO: refactor GenerateUpMoves() and GenerateDownMoves() into one function. It's not a huge deal, but I don't like repeating myself
        private void GenerateUpMoves()
        {
            Direction[] dirs = {Direction.NorthEast, Direction.NorthWest};
            for (int i = 0; i < 2; i++)
            {
                Point next = GoInDir(Start, dirs[i]);
                if (!Board.IsValidPoint(next)) { continue; } // if it's not a valid point, skip this direction
                if (!Board.SquareOccupied(next)) // if the square is not occupied, this is a good move
                {
                    ValidMoves.Add(new Move(SelectedPiece, Start, next, false));    
                }else if (Board.SquareOccupiedByEnemy(next, Piece.GetColor(SelectedPiece)))
                {
                    // if the square is occupied by the enemy, we check the next point again and see if it is open
                    Point nextNext = GoInDir(next, dirs[i]);
                    if (!Board.IsValidPoint(nextNext)) { continue; }
                    if (!Board.SquareOccupied(nextNext))
                    {
                        ValidMoves.Add(new Move(SelectedPiece, Start, nextNext, true, next));
                    }
                }
            }
        }

        private void GenerateDownMoves()
        {
            Direction[] dirs = {Direction.SouthEast, Direction.SouthWest};
            for (int i = 0; i < 2; i++)
            {
                Point next = GoInDir(Start, dirs[i]);
                if (!Board.IsValidPoint(next)) { continue; } // if it's not a valid point, skip this direction
                if (!Board.SquareOccupied(next)) // if the square is not occupied, this is a good move
                {
                    ValidMoves.Add(new Move(SelectedPiece, Start, next, false));    
                }else if (Board.SquareOccupiedByEnemy(next, Piece.GetColor(SelectedPiece)))
                {
                    // if the square is occupied by the enemy, we check the next point again and see if it is open
                    Point nextNext = GoInDir(next, dirs[i]);
                    if (!Board.IsValidPoint(nextNext)) { continue; }
                    if (!Board.SquareOccupied(nextNext))
                    {
                        ValidMoves.Add(new Move(SelectedPiece, Start, nextNext, true, next));
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
        

    }
}