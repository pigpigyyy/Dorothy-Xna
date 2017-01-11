namespace TestGame
{
    #if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (TestGame game = new TestGame())
            {
                game.Run();
            }
        }
    }
    #endif
}

