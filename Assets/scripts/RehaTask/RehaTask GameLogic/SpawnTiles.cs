using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Assets.scripts.objectives;
using Assets.scripts.RehaTask.RehaTaskGUI;
using Assets.scripts.Settings;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class SpawnTiles : MonoBehaviour
    {
        public static int Columns, Rows, Targets, Correctchoices, TotalToRender, ImageTc, Pairs;
        public static bool Showcorrect, UseDistractors, UseMemory, Valence, Completed, Sequence, KeepThumbnail;
        public static float Timer, PicTimer;
        public static string ImagesToDisplay, ObjectiveText;
    
        public Transform tile, mediumTile, bigTile, xlTile, xxlTile;
        public GameObject calibPrefab;
        private Transform _table;

        public static float Spacing = 0.35f;//space between the tiles
        public static float Xoffset = -1.9f;
        public static float Zoffset = -6;
        private static bool _spawn;

        public static List<Texture> SelectionList = new List<Texture>();//list with Targets
        public static List<GameObject> Hints = new List<GameObject>();//list that stores the cubes with the correct answers
        public static List<GameObject> Incorrect = new List<GameObject>();//list that stores the Distractors cubes
        public static List<Texture> TexturesToRender = new List<Texture>();//list that stores all the images that will be displayed on the current level
        public static List<string> TexturesToExport = new List<string>();//list with names of textures being displayed on the current level
    
        public static List<string> Cats = new List<string>();//list of categories
        public static List<string> Files = new List<string>();//list of Distractors Files
    
        public static List<List<string>> ImagesNames = new List<List<string>>();

        public static string LevelName = "L1";
        public static int TotalLevels = 0;

        public static bool Big;
        public static bool Medium;
        public static bool Xl, Xxl;

        public static string Folder = "NaN";
        public static int CatNumber = 0;
        private float _height = 0.5f;

        //private static string _filepath = string.Empty;
        private XmlDocument _xmlDoc;
        private static XmlWriter _writer;
        public static string Path;
        public static TextWriter File;
        public static bool DistractorsLoaded = false;
        public static bool ReadyToSpawn;

        public static Texture Randtxt;
        public static bool SpawnOnce;

        public static int TotCategories;
    
        public static bool ImagesLoaded;
    

        public static bool CategoriesLoaded;
        public static bool StartLoadingDistrators, PracticeStarted;

        public static int ActualLevel = 1;

        public static Texture ToConstruct;
        public static bool ConstructFigure;
        public static bool RenderPointer;
        private GameObject _number;
        
        //load the categories by reading folders
        private IEnumerator Start()
        {
            if (!CategoriesLoaded)
            {
                var di = new DirectoryInfo(Application.streamingAssetsPath + @"/Categories/");
/*
#if UNITY_STANDALONE_OSX
                di = new DirectoryInfo(Application.dataPath + @"/Raw/Categories/");
#endif
*/
                var folders = di.GetDirectories();

                foreach (var fi in folders)
                {
                    yield return fi.Name;

                    if (fi.Name != "Distractors")
                    {
                        Cats.Add(fi.Name);
                        TotCategories++;
                    }
                }

                RenderPointer = false;
                
                CategoriesLoaded = true;
                LoadGame.ReadyToLoadImages = true;
            }
        }
    
        private void Update()
        {
            if (ReadyToSpawn && _spawn)//allows level to be set on the _table
            {
                
                _table = GameObject.FindGameObjectWithTag("Table").transform;
                _spawn = false;
                Spawn();
                ReadyToSpawn = false;  
            }
        
            if (UdpReceive.Calib)//only allows _spawn when calibration is not being done
                _spawn = false;

            else if (!UdpReceive.Calib && SpawnOnce)
            {
                _spawn = true;
                SpawnOnce = false;  
            }
        }

        //returns a random texture from the selected Category Folder
        public static Texture RandTexture()
        {
            Texture tp = null;

            var rand = UnityEngine.Random.Range(0, LoadGame.LevelCategory.Count);
            tp = LoadGame.LevelCategory[rand];
            tp.name = LoadGame.LevelCategory[rand].name;
            return tp;  
        }
    
        //populate the lists with Target textures and textures to render on cubes
        public static void InitTexture()
        {
            StartLoadingDistrators = false;

            ObjectiveText = LoadGoals.SetGoal();

            if (Folder != "NaN")//First select the Targets which will be stored inside the SelectionList
            {
                if (ImagesToDisplay == "")//if ImagesToDisplay is empty Targets will be chosen randomly
                {
                    do
                    {
                        Randtxt = RandTexture();

                        if (!SelectionList.Contains(Randtxt))
                        {
                            SelectionList.Add(Randtxt);
                        }
                    } while (SelectionList.Count < Targets);
                }
                else//else if ImagesToDisplay are not empty Targets are the ones specified on the field split by commas
                {
                    if (Sequence)
                    {
                        if(Targets == 2)
                            ImagesToDisplay = "0,5";
                        else if(Targets == 3)
                            ImagesToDisplay = "0,3,5";
                        else if (Targets == 4)
                            ImagesToDisplay = "0,2,4,5";
                        else if (Targets == 5)
                            ImagesToDisplay = "0,1,3,4,5";
                    }
                    var words = ImagesToDisplay.Split(new char[] { ',' });

                    foreach (var t in words)
                    {
                        var tempImage = int.Parse(t);
                        SelectionList.Add(LoadGame.LevelCategory[tempImage]);
                    }
                }

                //starts populating the array TexturesToRender that will store all the images will be rendered on that level
                for (var i = 0; i < Targets; i++)
                {
                    var tp = SelectionList[i];
                    TexturesToRender.Add(tp);
                }

                //chooses amongst Targets the enough to match the _number of correct choices
                if (Correctchoices > Targets)
                {
                    if (Pairs > 0)
                    {
                        for (var i = 1; i < Pairs; i ++)
                        {
                            for (var b = 0; b < SelectionList.Count; b++)
                            {
                                TexturesToRender.Add(SelectionList[b]);
                            }
                        }
                    }
                    else
                    {
                        for (var j = Targets; j < Correctchoices; j++)
                        {
                            var rand = UnityEngine.Random.Range(0, SelectionList.Count);
                            var tp = SelectionList[rand];
                            TexturesToRender.Add(tp);
                        }
                    }
                }
                
                //populates the array testuresToRender with the images that are not Targets - > considered as Distractors
                if (TotalToRender > Correctchoices)
                {
                    //Debug.Log(LoadGame.SubCategory);
                    do
                    {
                        Texture randTxt2;
                        if (UseDistractors)//if UseDistractors is activated then Distractors will be loaded from Distractors Folder otherwise will be from the same Category Folder
                        {
                            //if (!_distractorsChecked)
                            //{
                            //    for (var s = 0; s < LoadGame.DistractorsNames.Count; s ++)
                            //    {
                            //        if (LoadGame.DistractorsNames[s].StartsWith(LoadGame.SubCategory))
                            //        {
                            //            Debug.Log("removing " + LoadGame.DistractorsNames[s]);
                            //            LoadGame.Distractors.RemoveAt(s);
                            //            LoadGame.DistractorsNames.RemoveAt(s);
                            //        }
                            //    }
                            //    _distractorsChecked = true;
                            //}
                            randTxt2 = LoadGame.Distractors[0];
                            randTxt2.name = LoadGame.DistractorsNames[0];
                        }
                        else
                            randTxt2 = RandTexture();

                        if (!TexturesToRender.Contains(randTxt2) && !SelectionList.Contains(randTxt2))
                        {
                            TexturesToRender.Add(randTxt2);
                            if (UseDistractors)
                            {
                                LoadGame.Distractors.RemoveAt(0);
                                LoadGame.DistractorsNames.RemoveAt(0);
                            }
                        }
                    
                        else if (TexturesToRender.Contains(randTxt2) && !SelectionList.Contains(randTxt2))
                        {
                            for (var i = 0; i < Cats.Count; i++)
                            {
                                if (Cats[i] == Folder)
                                {
                                    if (TexturesToRender.Count >= ImagesNames[i].Count)
                                    {
                                        //Debug.Log("TexturesToRender.Count >= ImagesNames[i].Count: " + TexturesToRender.Count + ">" + ImagesNames[i].Count);
                                        TexturesToRender.Add(randTxt2);
                                     
                                    }
                                }
                            }
                        }
                        //else if (UseDistractors && randTxt2.name.StartsWith(LoadGame.SubCategory))
                        //{
                        //    LoadGame.Distractors.RemoveAt(0);
                        //    LoadGame.DistractorsNames.RemoveAt(0);
                        //}
                    
                    } while (TexturesToRender.Count < TotalToRender);   
                }

                //shuffles testuresToRender to offer a more random display on the _table
                ShuffleList();

                //saves the level info
                SaveLevelInfo();

                //Debug.Log(TexturesToRender.Count);
                if (TexturesToRender.Count == TotalToRender && !MainGUI.SettingLevel && !ImagesLoaded)
                {
                    ImagesLoaded = true;
                    LoadGame.LoadingGame = false;
                    GUIChangeLevel.LevelStarted = 1;
                    if (SpawnTiles.LevelName != "L0")
                    {
                        Main_Menu.GameStarted = true;
                    }
                    else
                    {
                        PracticeStarted = true;
                    }
                    ChangeLevel.ResetOnce = true;
                    ChangeLevel.ResetLevel();
                }

                if(!Main_Menu.GameStarted && LevelName != "L0")
                {
                    Main_Menu.GameStarted = true;
                    ChangeLevel.ResetOnce = true;
                    ChangeLevel.ResetLevel();
                }
            }
            LoadGame.DistractorsSet = false;
            LoadGame.CategoryImagesLoaded = false;
        
        }

        //Shuffles the TexturesToRender List
        public static void ShuffleList()
        {
            LoadGame.LevelCategory.Clear();

            for (var i = 0; i < TexturesToRender.Count; i++)
            {
                var temp = TexturesToRender[i];
                var randomIndex = UnityEngine.Random.Range(i, TexturesToRender.Count);
                TexturesToRender[i] = TexturesToRender[randomIndex];
                TexturesToRender[randomIndex] = temp;
            }

            foreach (var t in TexturesToRender)
            {
                string temp = t.name;
                TexturesToExport.Add(temp);
            }
        }

        //Create Grid
        private void Spawn()
        {
            if (!UdpReceive.Calib && Main_Menu.GameStarted || PracticeStarted)
            {
                //Debug.Log("Started to _spawn");
                var i = 1;//increment for name
                //var count = 0;
                var j = 0;//increment for TexturesToRender
                for (var z = 0; z < Rows; z++)
                { //Rows
                    for (var x = 0; x < Columns; x++)
                    { 	
                        Spacing = 0.35f;
                        Xoffset = 0;
                        Zoffset = -6;

                        SpacingAndOffSet();

                        _height = 0.6f;
                        
                        Transform tileToUse;
                        if (Xxl)
                        {
                            tileToUse = xxlTile;
                            _height = 0.3f;
                        }
                        else if (Xl)
                        {
                            tileToUse = xlTile;
                            _height = 0.4f;
                        }
                        else if (Big)
                            tileToUse = bigTile;
                        else if (Medium)
                            tileToUse = mediumTile;
                        else
                            tileToUse = tile;

                        if (Application.loadedLevelName == "Park")
                            _height = 0.64f;

                        var t = Instantiate(tileToUse, new Vector3((Xoffset + x) * Spacing, _height, (Zoffset + z) * Spacing), Quaternion.identity) as Transform;//objects to transform
                        t.gameObject.name = "Tile" + i;//set the name of the object
                        t.gameObject.tag = "Tile";

                        var go = t.Find("ImageDisplay").gameObject; //convert transforms into gameobjects

                        if (Sequence)
                        {
                            _number = t.Find("NumberDisplay").gameObject;
                        }

                        i++;

                        var temp = TexturesToRender[j];

                        if (SelectionList.Contains(temp))
                        {
                            go.GetComponent<Renderer>().material.mainTexture = temp; //assign random texture -----> it has to change into a texture from a list (size = Correctchoices)
                            go.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-1, -1);//flip the texture
                            go.GetComponent<Renderer>().material.color = Color.white;
                            if (Sequence)
                            {
                                for (var s = 0; s < SelectionList.Count; s++)
                                {
                                    if (temp == SelectionList[s])
                                    {
                                        var tempNumber = s + 1;
                                        _number.GetComponent<TextMesh>().text = tempNumber.ToString();
                                    }
                                }
                            }
                            if(Showcorrect)
                                Hints.Add(t.gameObject);

                            // count++;
                        }
                        else
                        {
                            go.GetComponent<Renderer>().material.mainTexture = temp;
                            go.GetComponent<Renderer>().material.color = Color.white;
                            go.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-1, -1);//flip the texture

                            if (Showcorrect)
                                Incorrect.Add(t.gameObject);
                        }

                        j++;
                    }
                }
                tile.transform.position = new Vector3(0, 20000, 0);
                _spawn = false;
                RehaTask_GameLogic.Hints.Ordered = false;
                RenderPointer = true;
            }

        }//end of Spawn

        //calculates the offsets and Spacing according to _number of Columns and Rows
        void SpacingAndOffSet()
        {
            for (var i = 2; i <= Rows; i++)
            {
                Zoffset = Zoffset - 0.87f;  
            }
  
            for (var i = 2; i <= Columns; i++)
            {
                Xoffset = Xoffset - 0.5f;
            }

            if (Columns >= 6 || Rows >= 3)
                Spacing = 0.33f;
        
            if (Columns >= 8 || Rows >= 4)
                Spacing = 0.31f;

            if (Columns == 9 || Rows == 5)
                Spacing = 0.29f;

            if (Columns < 3 && Rows <= 2)
            {
                Xxl = true;
                if (Application.loadedLevelName != "Park")
                    _table.position = new Vector3(_table.position.x, _table.position.y - 0.05f, _table.position.z);

                Spacing = 0.8f;

                if (Rows == 2)
                    Zoffset = -3.15f;
                else
                    Zoffset = -2.8f;
            }
            else if (Columns <= 4 && Rows <= 2)
            {
                Xl = true;
                Xxl = false;
                if (Application.loadedLevelName != "Park")
                    _table.position = new Vector3(_table.position.x, _table.position.y - 0.03f, _table.position.z);
                Spacing = 0.62f;
            
                if (Rows == 2)
                    Zoffset = -4.1f;
                else
                    Zoffset = -3.5f;
            
            }
            else if (Columns >=3 && Columns <6 && Rows <= 3)
            {
                Xl = false;
                Xxl = false;
                Big = true;
                if (Application.loadedLevelName != "Park")
                    _table.position = new Vector3(_table.position.x, _table.position.y - 0.01f, _table.position.z);
                Spacing = 0.45f;
                Zoffset = -4.9f;

                if (Columns > 4)
                    Xoffset = -2f;

                if (Rows == 2)
                    Zoffset = -5.3f;

                if (Rows == 3)
                    Zoffset = -5.8f;
            }
            else if (Columns >= 6 && Columns < 8 && Rows <= 3)
            {
                Xl = false;
                Xxl = false;
                Big = false;
                Medium = true;
                Spacing = 0.39f;

                if (Rows == 1)
                    Zoffset = -5.5f;

                if (Rows == 2)
                    Zoffset = -5.9f;

                if (Rows == 3)
                    Zoffset = -6.6f;
            }
            else
            {
                Medium = false;
                Big = false;
                Xl = false;
                Xxl = false;
            }

            if (Columns == 7 && Rows <= 3)
            {
                Spacing = 0.35f;
                Xoffset = -3f;

                if(Rows == 1)
                    Zoffset = -6.4f;
                if (Rows == 2)
                    Zoffset = -6.7f;
                if (Rows == 3)
                    Zoffset = -7.2f;      
            }

            if (Columns == 8)
                Xoffset = -3.5f;

            if (Columns == 8 && Rows <= 3)
            {
                if (Rows == 1)
                    Zoffset = -7.2f;
                if (Rows == 2)
                    Zoffset = -7.7f;
                if (Rows == 3)
                    Zoffset = -8.2f;
            }

            if (Columns == 9)
            {
                if (Rows == 1)
                    Zoffset = -7.7f;
                else if (Rows == 2)
                    Zoffset = -8.2f;
                else if (Rows == 3)
                    Zoffset = -8.7f;
                else if (Rows == 4)
                    Zoffset = -9.1f;
                else if (Rows == 5)
                    Zoffset = -9.4f;
                else
                    Xoffset = -4f;
            }  
        }

        //export cubes textures and calibration values to csv File
        public static void SaveLevelInfo()
        {/*
            if (Main_Menu.Uid != "")
            {
                Path = Application.dataPath + "/RehaCity_Log/" + Main_Menu.Uid + "/" + DateTime.Now.ToString("yyyyMMdd") + "/RehaTask_LevelsInfo/";

                if (!Directory.Exists(Path))
                {
                    System.IO.Directory.CreateDirectory(Path);
                }

                _filepath = Path + Main_Menu.Uid + DateTime.Now.ToString("yyyyMMddHHmmss") + LevelName + "_Level_Details" + ".csv";
                File = new StreamWriter(_filepath, true);

                var newLine = TexturesToExport[0];

                var targetsLine = SelectionList[0].name;

                for (var i = 1; i < TexturesToExport.Count; i++)
                {
                    newLine = newLine + "," + TexturesToExport[i];
                }

                for(var i = 1; i< SelectionList.Count; i++)
                {
                    targetsLine = targetsLine + "," + SelectionList[i].name;
                }

                File.WriteLine(LevelName + "," + newLine);
                File.WriteLine("Targets," + targetsLine);
                File.WriteLine("");
                File.Close();
            }*/
        }

        public static void SaveActualLevelNumber()
        {
            var actualLevelNumber = LevelName;
            var numberL = actualLevelNumber[1].ToString();//level in SpawnTiles script is a string so needs to be "split" to parse it into an int, the First _number corresponds to the position 1 of the array of characters

            if (actualLevelNumber.Length > 2)//if current level is higher than 9, or higher than 99 it adds the next characters posiotened on the array of characters
            {
                for (var i = 2; i < actualLevelNumber.Length; i++)
                {
                    numberL = numberL + actualLevelNumber[i].ToString();
                }
            }

            //parses the obtained string _number into an int
            ActualLevel = int.Parse(numberL);
        }
  
    }
}