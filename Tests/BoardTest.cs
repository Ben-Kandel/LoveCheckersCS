using LoveCheckers.Models;
using NUnit.Framework;

namespace Tests
{
    public class BoardTest
    {
        private Board Board;
        [SetUp]
        public void Setup()
        {
            Board = new Board();
        }

        [Test]
        public void TestCanPromote()
        {
            Assert.True(Board.CanPromote(Piece.Black | Piece.Pawn, new Point(0, 7)));
            Assert.False(Board.CanPromote(Piece.Black | Piece.Pawn, new Point(0, 0)));
            Assert.False(Board.CanPromote(Piece.Black | Piece.King, new Point(0, 7)));
        }

        [Test]
        public void TestIsValidPoint()
        {
            Assert.True(Board.IsValidPoint(new Point(0, 0)));
            Assert.True(Board.IsValidPoint(new Point(7, 7)));
            Assert.False(Board.IsValidPoint(new Point(-1, 0)));
            Assert.False(Board.IsValidPoint(new Point(0, -1)));
            Assert.False(Board.IsValidPoint(new Point(8, 0)));
            Assert.False(Board.IsValidPoint(new Point(0, 8)));
        }

        [Test]
        public void TestSquareOccupied()
        {
            // test with black piece
            Assert.True(Board.SquareOccupied(new Point(1, 0)));
            // test with red piece
            Assert.True(Board.SquareOccupied(new Point(0, 7))); 
            // test empty square
            Assert.False(Board.SquareOccupied(new Point(0, 0)));
        }

        [Test]
        public void TestSquareOccupiedByEnemy()
        {
            Assert.True(Board.SquareOccupiedByEnemy(new Point(1, 0), Piece.Red));
            Assert.False(Board.SquareOccupiedByEnemy(new Point(1, 0), Piece.Black));
            
            Assert.True(Board.SquareOccupiedByEnemy(new Point(0, 7), Piece.Black));
            Assert.False(Board.SquareOccupiedByEnemy(new Point(0, 7), Piece.Red));
            
            // test an empty square too
            Assert.False(Board.SquareOccupiedByEnemy(new Point(0, 0), Piece.Black));
            Assert.False(Board.SquareOccupiedByEnemy(new Point(0, 0), Piece.Red));
        }

        [Test]
        public void TestGetPieceAtPoint()
        {
            Assert.AreEqual(Piece.Black | Piece.Pawn, Board.GetPieceAtPoint(new Point(1, 0)));
            Assert.AreEqual(Piece.Red | Piece.Pawn, Board.GetPieceAtPoint(new Point(0, 7)));
            Assert.AreEqual(Piece.Nothing, Board.GetPieceAtPoint(new Point(0, 0)));
        }

        [Test]
        public void TestGetPiecesOfColor()
        {
            Assert.AreEqual(12, Board.GetPiecesOfColor(Piece.Black).Count);
            Assert.AreEqual(12, Board.GetPiecesOfColor(Piece.Red).Count);
        }

        [Test]
        public void TestClone()
        {
            Board clone = Board.Clone();
            Assert.AreEqual(Board.Grid, clone.Grid);
            Assert.AreEqual(Board.GetPiecesOfColor(Piece.Red), clone.GetPiecesOfColor(Piece.Red));
            Assert.AreEqual(Board.GetPiecesOfColor(Piece.Black), clone.GetPiecesOfColor(Piece.Black));
            // make sure modifying one doesnt affect the other
            clone.Grid[0, 0] = Piece.Black | Piece.Pawn;
            Assert.AreNotEqual(Board.Grid, clone.Grid);
        }
    }
}