namespace LoveCheckers.Commands
{
    public class SelectCommand : ICommand
    {
        public SelectCommand()
        {
            
        }

        public void Execute()
        {
            // select the piece at the location, which means set some SelectedPiece variable and also
            // do some move generating stuff. that way we can pass around the results for other uses,
            // like highlighting the board and such
        }
    }
}