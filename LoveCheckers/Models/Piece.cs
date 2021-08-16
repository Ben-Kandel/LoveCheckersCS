using System;
using System.Collections.Generic;

namespace LoveCheckers.Models
{
    public static class Piece
    {
        public const int Nothing = 0;
        public const int Pawn = 1;
        public const int King = 2;
        public const int Red = 4;
        public const int Black = 8;

        private const int TypeMask = 0b0011;
        private const int RedMask = 0b0100;
        private const int BlackMask = 0b1000;
        private const int ColorMask = RedMask | BlackMask;

        public static int GetType(int piece)
        {
            return piece & TypeMask;
        }

        public static int GetColor(int piece)
        {
            return piece & ColorMask;
        }

        public static bool IsRed(int piece)
        {
            return (piece & RedMask) == Piece.Red;
        }

        public static bool IsNothing(int piece)
        {
            return (piece & TypeMask) == Piece.Nothing;
        }
    }
}