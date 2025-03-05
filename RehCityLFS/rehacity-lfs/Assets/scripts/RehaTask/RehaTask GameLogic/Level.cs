namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class Level {

        public string LevelName;
        public int Columns;
        public int Rows;
        public int Targets;
        public int CorrectChoices;
        public float PicTimer;
        public string Category;
        public string ImagesToDisplay;
        public bool UseDistractors;
        public bool UseMemory;
        public bool ShowCorrect;
        public bool Completed;
        public int ImageToConstruct;
        public bool Sequence;
        public bool KeepThumbnail;
        public int Pairs;

        public Level(string ln, int col, int row, int tar, int cc, float pt, string cat, string itd, bool ud, bool um, bool sc, bool comp, int imgConstruct, bool seq, bool kt, int pairs)
        {
            LevelName = ln;
            Columns = col;
            Rows = row;
            Targets = tar;
            CorrectChoices = cc;
            PicTimer = pt;
            Category = cat;
            ImagesToDisplay = itd;
            UseDistractors = ud;
            UseMemory = um;
            ShowCorrect = sc;
            Completed = comp;
            ImageToConstruct = imgConstruct;
            Sequence = seq;
            KeepThumbnail = kt;
            Pairs = pairs;
        }
    }
}
