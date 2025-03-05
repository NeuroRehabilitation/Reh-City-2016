namespace Assets.scripts.Manager
{
    public static class GameStartEventManager {

        public delegate void StartAction();
        public static event StartAction OnGameStart;


        public static void StartGame()
        {
            if (OnGameStart != null)
                OnGameStart();
        }
    }
}
