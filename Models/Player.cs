using LoveCheckers.Commands;

namespace LoveCheckers.Models
{
    public abstract class Player
    {
        public int Color { get; }
        public bool MoveReady { get; set; }
        protected MoveGenerator Generator;
        protected Board Board;
        
        public bool ForceJump { get; protected set; }
        // protected List<Move> Jumps;

        public Player(int color, Board board)
        {
            Color = color;
            Board = board;
        }

        public abstract MoveCommand GetMove();
        public abstract void Update(float dt);

        public void CheckForJump(Move prev)
        {
            Generator = new MoveGenerator(Board, prev.Piece, prev.Destination);
            if (Generator.HasJump()) // then we need to force the player to use this jump move. 
            {
                ForceJump = true;
            }
        }
    }
}