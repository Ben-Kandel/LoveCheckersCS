using System.Collections.Generic;
using System.Linq;
using LoveCheckers.Commands;
using LoveCheckers.Views;

namespace LoveCheckers.Models
{
    public class Game
    {
        public Board Board { get; }
        private BoardView BView;

        private Player Player1;
        private Player Player2;
        public Player ActivePlayer { get; private set; }

        private List<ICommand> GameHistory;
        private int HistoryIndex;

        public Game()
        {
            Board = new Board();
            BView = new BoardView(Board);
            Player1 = new HumanPlayer(Piece.Red, Board);
            // Player2 = new HumanPlayer(Piece.Black, Board);
            Player2 = new AIPlayer(Piece.Black, Board);
            ActivePlayer = Player1;
            GameHistory = new List<ICommand>();
            HistoryIndex = -1;
            
            // Board.PrintBoard();
        }

        public void NextTurn()
        {
            ActivePlayer.MoveReady = false;
            ActivePlayer = (ActivePlayer == Player1) ? Player2 : Player1;
            Board.ClearHighlightedMoves();
            List<Move> suggestedMoves = Board.GetPiecesOfColor(ActivePlayer.Color).SelectMany(pair =>
            {
                MoveGenerator gen = new MoveGenerator(Board, pair.Piece, pair.Pos, false);
                return gen.GetJumps(); // actually, this might not be necessary. The generator will filter out any non-jumps
            }).ToList();
            ActivePlayer.SuggestJumps(suggestedMoves);
        }

        public bool GameOver()
        {
            List<Pair> pieces = Board.GetPiecesOfColor(ActivePlayer.Color);
            // a player loses when they have no available moves (either lost all pieces, or all pieces blocked in)
            // return false if any of the player's pieces have a move
            return !pieces.Any(pair =>
            {
                MoveGenerator gen = new MoveGenerator(Board, pair.Piece, pair.Pos, false);
                return gen.HasMoves();
            });
        }
        
        public void Draw()
        {
            BView.Draw();
        }

        public void Update(float dt)
        {
            if (GameOver())
            {
                return; // temporary
            }
            ActivePlayer.Update(dt);
            // this is where we wait for a player to...play their turn
            if (ActivePlayer.MoveReady)
            {
                MoveCommand moveCmd = ActivePlayer.MoveCmd;
                Move move = moveCmd.Move;
                ExecuteMove(moveCmd);
                ActivePlayer.SuggestJumps(new List<Move>()); // clear the previous markers
                if (move.IsJump && CheckForNextJump(move)) 
                {
                    // we need to force this player to take that available jump
                    ActivePlayer.ForceJump(move);
                }
                else
                {
                    NextTurn(); // continue on    
                }
            }
        }

        private bool CheckForNextJump(Move m)
        {
            MoveGenerator gen = new MoveGenerator(Board, m.Piece, m.Destination);
            return gen.HasJump();
        }

        private void ExecuteMove(ICommand move)
        {
            // TODO: Implement Undo() functions in the command interface, and also an undo button in the GUI
            GameHistory.Add(move);
            HistoryIndex += 1;
            Board newBoard = move.Execute(); // fix this stuff
        }
        
    }
}
