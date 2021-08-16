using System;
using System.Collections.Generic;
using System.Data;
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
            Love.Timer.Sleep(1); // 1 second
            List<Move> moves = GetAllPossibleMoves();
            Random rand = new Random();
            Move move = moves[rand.Next(moves.Count)];
            ReadyUp(move);
        }

        private void StartMinimax()
        {
            
        }

        private int Minimax(Board board, int depth, bool maximizingPlayer, double alpha, double beta)
        {
            if (depth == 0)
            {
                return Evaluate(board, Piece.Red);
            }

            List<Move> possibleMoves = Board.GetPiecesOfColor(Piece.Red).SelectMany(pair =>
            {
                MoveGenerator gen = new MoveGenerator(board, pair.Piece, pair.Pos, false);
                return gen.ValidMoves;
            }).ToList();
            int initial = 0;
            Board tempBoard;
            if (maximizingPlayer)
            {
                initial = int.MinValue;
                foreach (Move move in possibleMoves)
                {
                    tempBoard = board.Clone();
                    tempBoard.Grid[move.Destination.X, move.Destination.Y] = move.Piece; // make the move
                    int result = Minimax(tempBoard, depth - 1, !maximizingPlayer, alpha, beta);
                    initial = Math.Max(result, initial);
                    alpha = Math.Max(alpha, initial);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }
            else
            {
                initial = int.MaxValue;
                foreach (Move move in possibleMoves)
                {
                    tempBoard = board.Clone();
                    tempBoard.Grid[move.Destination.X, move.Destination.Y] = move.Piece; // make the move
                    int result = Minimax(tempBoard, depth - 1, !maximizingPlayer, alpha, beta);
                    initial = Math.Min(result, initial);
                    alpha = Math.Min(alpha, initial);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }
            return initial;
        }

        private int Evaluate(Board board, int color)
        {
            int result = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int piece = board.Grid[x, y];
                    if (piece == Piece.Nothing) continue; // skip empty squares
                    int sign = (Piece.GetColor(piece) == color) ? 1 : -1; // positive for our color, negative for enemy
                    result += ((Piece.GetType(piece) == Piece.Pawn) ? 1 : 2) * sign; // pawns are worth 1, kings are worth 2
                }
            }
            return result;
        }
    }
}