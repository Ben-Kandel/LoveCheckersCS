using Love;
using LoveCheckers.Commands;

namespace LoveCheckers.Models
{
    public class HumanPlayer : Player
    {
        private MoveCommand MoveCmd;

        public HumanPlayer(int color, Board board) : base(color, board)
        {
            
        }

        public override MoveCommand GetMove()
        {
            return MoveCmd;
        }

        private void CreateMove(Move move)
        {
            MoveCmd = new MoveCommand(Board, move);
            MoveReady = true;
            Jump = false;
        }

        public override void Update(float dt)
        {
            // actually, does the HumanPlayer need the update() at all? can we do it all with callbacks and shit?
            if (Mouse.IsPressed(0)) // if we left clicked
            {
                int mouseX = (int)Mouse.GetX();
                int mouseY = (int) Mouse.GetY();
                if (Board.WithinBounds(mouseX, mouseY)) // if we clicked on the board somewhere
                {
                    Point clicked = Board.GetPointAt(mouseX, mouseY); // the point we clicked on the board
                    if (SelectedPiece == Piece.Nothing) // if we don't have a piece selected yet
                    {
                        int piece = Board.GetPieceAtPoint(clicked);
                        if (piece == Piece.Nothing) { return; } // if we didn't click on a piece, don't do anything
                        if (Piece.GetColor(piece) != Color) { return; } // can't click on a piece that isn't ours!
                        SelectedPiece = piece;
                        MoveGen = new MoveGenerator(Board, SelectedPiece, clicked);
                        if (!MoveGen.HasMoves()) // if the piece we clicked doesn't have anywhere to go
                        {
                            SelectedPiece = Piece.Nothing; // reset this
                        }
                    }
                    else // we have a piece selected already. that means we are trying to place it down
                    {
                        if (MoveGen.IsValidMove(clicked)) // if the move was valid
                        {
                            CreateMove(MoveGen.GetMoveFromDestination(clicked));
                        }
                        if (!Jump)
                        {
                            SelectedPiece = Piece.Nothing; // reset
                            Board.ClearHighlightedMoves();    
                        }
                    }
                }
            }else if (!Jump && Mouse.IsPressed(1)) // if we right clicked, but only when we are not forced to jump
            {
                // cancel any moves
                SelectedPiece = Piece.Nothing;
                Board.ClearHighlightedMoves();
            }
        }
    }
}