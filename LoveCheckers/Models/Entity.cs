namespace LoveCheckers.Models
{
    public class Entity
    {

        // we will probably change this later as we might want to move things around. so we need a public setter
        public int X { get; protected init; }
        public int Y { get; protected init; }
        
        public Entity()
        {
            
        }
    }
}