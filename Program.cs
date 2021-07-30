using Love;
using LoveCheckers.Models;
using LoveCheckers.Views;

namespace LoveCheckers
{
    class Program : Scene
    {
        private static Game TheGame;
        private static GameView GameView;

        static void Main(string[] args)
        {
            BootConfig config = new BootConfig
            {
                WindowWidth = 750,
                WindowHeight = 750,
                WindowCentered = true,
                WindowTitle = "Yep This Works!"
            };
            TheGame = new Game();
            GameView = new GameView(TheGame);
            Boot.Init(config);;
            Boot.Run(new Program());
        }
        
        /*
            ---------------------------------------------------------------------------------------------------------
            These are the callbacks provided by Love2D.
            We will use them to call the appropriate functions in our views and controllers. 
            ---------------------------------------------------------------------------------------------------------
        */
        
        public override void Draw()
        {
            Graphics.SetBackgroundColor(70f / 255, 70f / 255, 70f / 255);
            TheGame.Draw();
            GameView.Draw();
        }

        public override void MouseMoved(float x, float y, float dx, float dy, bool isTouch)
        {
            // cast the floats to ints now just to get it out of the way
            // TheGame.MouseMoved((int)x, (int)y, (int)dx, (int)dy, isTouch);
            // GController.MouseMoved((int)x, (int)y, (int)dx, (int)dy, isTouch);
        }

        public override void Update(float dt)
        {
            TheGame.Update(dt);
        }
    }
}