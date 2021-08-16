using LoveCheckers.Models;
using NUnit.Framework;

namespace Tests
{
    public class MoveGeneratorTest
    {

        private Board Board;
        [SetUp]
        public void Setup()
        {
            Board = new Board();
            
        }

        [Test]
        public void TestSomething()
        {
            // i need to set the board up in some configuration so I can easily test stuff
            MoveGenerator gen = new MoveGenerator(Board, Piece.Black | Piece.Pawn, new Point(0, 0));
        } 
    }
}