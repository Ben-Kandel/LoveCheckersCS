using LoveCheckers.Models;
using NUnit.Framework;

namespace Tests
{
    public class PieceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGetType()
        {
            int piece = Piece.Black | Piece.Pawn;
            Assert.AreEqual(Piece.Pawn, Piece.GetType(piece));
            piece = Piece.Red | Piece.Pawn; // changing the color should't change the type
            Assert.AreEqual(Piece.Pawn, Piece.GetType(piece));
            piece = Piece.Black | Piece.King;
            Assert.AreEqual(Piece.King, Piece.GetType(piece));
        }

        [Test]
        public void TestGetColor()
        {
            int piece = Piece.Black | Piece.Pawn;
            Assert.AreEqual(Piece.Black, Piece.GetColor(piece));
            piece = Piece.Red | Piece.Pawn;
            Assert.AreEqual(Piece.Red, Piece.GetColor(piece));
            piece = Piece.Red | Piece.King; // changing the type shouldn't change the color
            Assert.AreEqual(Piece.Red, Piece.GetColor(piece));
        }
    }
}