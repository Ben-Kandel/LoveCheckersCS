using System;
using System.Collections.Generic;
using System.Linq;

namespace LoveCheckers.Models
{
    public class AIPlayer : Player
    {
        
        public AIPlayer(int color, Board board) : base(color, board)
        {
            
        }

        public override void Update(float dt)
        {
            List<Move> moves = GetAllPossibleMoves();
            if (moves.Any(move => move.IsJump))
            {
                moves.RemoveAll(move => !move.IsJump);
            }
            Random rand = new Random();
            Move move = moves[rand.Next(moves.Count)];
            ReadyUp(move);
        }
    }
}