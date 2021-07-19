using LoveCheckers.Models;

namespace LoveCheckers.Commands
{
    public class MoveCommand : ICommand
    {
        private Board Board;
        public Move Move { get; }

        public MoveCommand(Board board, Move move)
        {
            Move = move;
            Board = board;
        }
        
        public void Execute()
        {
            // remove the piece from the start point and place it at the end point
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
                Board.Grid[Move.Destination.X, Move.Destination.Y] = Piece.GetColor(Move.Piece) | Piece.King;
            }
        }
    }
}