using System.Collections.Generic;
using LoveCheckers.Commands;
using LoveCheckers.Controllers;
using LoveCheckers.Views;

namespace LoveCheckers.Models
{
    public class Game
    {
        public Board Board { get; }
        private BoardView BView;
        private BoardController BController;
        private GameView GameView;

        private Player Player1;
        private Player Player2;
        public Player ActivePlayer { get; private set; }

        private List<ICommand> GameHistory;
        private int HistoryIndex;

        public Game()
        {
            Board = new Board();
            BView = new BoardView(Board);
            BController = new BoardController(Board);
            GameView = new GameView(this);
            Player1 = new HumanPlayer(Piece.Red, Board);
            Player2 = new HumanPlayer(Piece.Black, Board);
            ActivePlayer = Player1;
            GameHistory = new List<ICommand>();
            HistoryIndex = -1;
            
            Board.PrintBoard();
        }

        public void NextTurn()
        {
            ActivePlayer.MoveReady = false;
            ActivePlayer = (ActivePlayer == Player1) ? Player2 : Player1;
            Board.ClearHighlightedMoves();
        }
        
        public void Draw()
        {
            BView.Draw();
            GameView.Draw();
        }
        public void MousePressed(int x, int y, int button, bool isTouch)
        {
            BController.MousePressed(x, y, button, isTouch);
        }

        public void Update(float dt)
        {
            ActivePlayer.Update(dt);
            // this is where we wait for a player to...play their turn
            if (ActivePlayer.MoveReady)
            {
                MoveCommand move = ActivePlayer.GetMove();
                ExecuteMove(move);
                // TODO: How to force jump moves? Is the implementation different for AI and Human Players? How do I do it??
                if (!ActivePlayer.ForceJump) // this is only temporary, it doesn't have all the functionality I want yet
                {
                    NextTurn();    
                }
            }
        }

        private void ExecuteMove(ICommand move)
        {
            // TODO: Implement Undo() functions in the command interface, and also an undo button in the GUI
            GameHistory.Add(move);
            HistoryIndex += 1;
            move.Execute();
        }
        
    }
}