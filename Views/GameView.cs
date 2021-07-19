using Love;
using LoveCheckers.Models;

namespace LoveCheckers.Views
{
    public class GameView
    {
        public Game Game;

        public GameView(Game game)
        {
            Game = game;
        }

        public void Draw()
        {
            int centerx = Game.Board.X + (Board.TileSize * 4);
            string test = $"It is {ColorToString(Game.ActivePlayer.Color)}'s turn to move.";
            int width = Graphics.GetFont().GetWidth(test);
            int height = Graphics.GetFont().GetHeight();
            Graphics.Print(test, centerx - (width / 2), Game.Board.Y - height, 0);
        }

        private string ColorToString(int color)
        {
            return (color == Piece.Black) ? "black" : "red";
        }
    }
}