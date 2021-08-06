using LoveCheckers.Commands;
using System.Collections.Generic;
using System.Linq;

namespace LoveCheckers.Models
{
    public abstract class Player
    {
        public int Color { get; }
        public bool MoveReady { get; set; }
        public MoveCommand MoveCmd { get; private set; }
        
        protected List<Move> SuggestedJumps { get; set; }        
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
        
        protected void ReadyUp(Move move)
        {
            MoveCmd = new MoveCommand(Board, move);
            MoveReady = true;
            Jump = false;
        }

        protected List<Move> GetAllPossibleMoves()
        {
            List<Move> moves = Board.GetPiecesOfColor(Color).SelectMany(pair =>
            {
                MoveGenerator gen = new MoveGenerator(Board, pair.Piece, pair.Pos, false);
                return gen.ValidMoves;
            }).ToList();
            return moves;
        }
        
        
    }
}