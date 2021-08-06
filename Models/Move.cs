namespace LoveCheckers.Models
{
    public class Move
    {
        public int Piece { get; }
        public Point Origin { get; }
        public Point Destination { get; }
        public bool IsJump { get; }
        public bool IsPromotion { get; }
        public Point Captured { get; }
        
        public Move(int piece, Point origin, Point destination, bool jump = false, Point captured = null, bool promotion = false)
        {
            Piece = piece;
            Origin = origin;
            Destination = destination;
            IsJump = jump;
            Captured = captured;
            IsPromotion = promotion;
        }
    }
}