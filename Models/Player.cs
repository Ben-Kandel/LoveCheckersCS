using LoveCheckers.Commands;
using System.Collections.Generic;

namespace LoveCheckers.Models
{
    public abstract class Player
    {
        public int Color { get; }
        public bool MoveReady { get; set; }
        public List<Move> SuggestedJumps { get; set; }
        
        protected int SelectedPiece { get; set; }
        protected bool Jump { get; set; }
        protected MoveGenerator MoveGen { get; set; }
        protected Board Board { get; }
        

        public Player(int color, Board board)
        {
            Color = color;
            Board = board;
            SuggestedJumps = new List<Move>();
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

        public void SuggestJumps(List<Move> moves)
        {
            SuggestedJumps = moves;
            Board.MoreHighlights = moves;
        }
    }
}