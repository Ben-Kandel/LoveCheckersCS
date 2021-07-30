using System.Collections.Generic;
using LoveCheckers.Commands;

namespace LoveCheckers.Models
{
    public abstract class Player
    {
        public int Color { get; }
        public bool MoveReady { get; set; }

        protected int SelectedPiece { get; set; }
        protected bool Jump { get; set; }
        protected MoveGenerator MoveGen { get; set; }
        protected Board Board { get; }

        public Player(int color, Board board)
        {
            Color = color;
            Board = board;
        }

        public abstract MoveCommand GetMove();
        public abstract void Update(float dt);

        public void ForceJump(Move move)
        {
            SelectedPiece = move.Piece;
            MoveGen = new MoveGenerator(Board, SelectedPiece, move.Destination);
            MoveReady = false; // reset this
            Jump = true;
        }
    }
}