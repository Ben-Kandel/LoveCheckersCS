using System;
using System.Collections.Generic;
using LoveCheckers.Commands;
using LoveCheckers.Views;

namespace LoveCheckers.Models
{
    public class Game
    {
        public Board Board { get; }
        private BoardView BView;
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
            GameView = new GameView(this);
            Player1 = new HumanPlayer(Piece.Red, Board);
            Player2 = new HumanPlayer(Piece.Black, Board);
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
        }
        
        public void Draw()
        {
            BView.Draw();
            GameView.Draw();
        }

        public void Update(float dt)
        {
            ActivePlayer.Update(dt);
            // this is where we wait for a player to...play their turn
            if (ActivePlayer.MoveReady)
            {
                MoveCommand move = ActivePlayer.GetMove();
                Move theMove = move.Move;
                ExecuteMove(move);
                if (theMove.IsPromotion)
                {
                    Console.WriteLine("hey this was a promotion!");
                }
                if ((theMove.IsPromotion || theMove.IsJump) && CheckForNextJump(theMove)) 
                {
                    // we need to force this player to take that available jump
                    Console.WriteLine("There is an available jump for this player.");
                    ActivePlayer.ForceJump(theMove);
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
            // Console.WriteLine($"checking for jump at point {m.Destination}");
            return gen.HasJump();
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