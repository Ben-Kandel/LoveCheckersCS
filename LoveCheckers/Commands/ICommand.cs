using LoveCheckers.Models;

namespace LoveCheckers.Commands
{
    public interface ICommand
    {
        Board Execute();
        void Undo();
    }
}