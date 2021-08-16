using LoveCheckers.Models;

namespace LoveCheckers.Commands
{
    public class MoveCommand : ICommand
    {
        private Board Board;
        private Board PrevBoard;
        public Move Move { get; }

        public MoveCommand(Board board, Move move)
        {
            Move = move;
            Board = board;
        }
        
        public Board Execute()
        {
            
            // okay, so maybe Execute() should return a new Board? 
            
            // remove the piece from the start point and place it at the end point
            PrevBoard = Board;
            Board.Grid[Move.Origin.X, Move.Origin.Y] = Piece.Nothing;
            Board.Grid[Move.Destination.X, Move.Destination.Y] = Move.Piece;
            // if this was a jump, then there is a captured piece that we must get rid of
            if (Move.IsJump)
            {
                Board.Grid[Move.Captured.X, Move.Captured.Y] = Piece.Nothing;
            }
            // if the piece can be promoted, do it
            if (Board.CanPromote(Move.Piece, Move.Destination))
            {
                int promotedPiece = Piece.GetColor(Move.Piece) | Piece.King;
                Board.Grid[Move.Destination.X, Move.Destination.Y] = promotedPiece;
            }
            Board.UpdatePositions();
            return new Board();
        }

        public void Undo()
        {
            Board = PrevBoard;
        }
    }
}