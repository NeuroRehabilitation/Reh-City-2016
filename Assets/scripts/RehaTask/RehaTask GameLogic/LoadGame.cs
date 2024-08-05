using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.scripts.UDP;
using UnityEngine;

namespace Assets.scripts.RehaTask.RehaTask_GameLogic
{
    public class LoadGame : MonoBehaviour
    {
        public GameObject LoadingPanel;
        public static bool LoadingGame = false;
        public static bool GameLoaded, StartLoadingCatImages, DistractorsSet;
        public static bool ReadyToLoadImages, ReadyToSetImages, CategoryImagesLoaded, DistractorsLoaded;
        private bool _loadStarted, _checkForDistractors, _gameWithDistractors;

        public static List<Texture> LevelCategory = new List<Texture>();
        public static List<Texture> Distractors = new List<Texture>();//list of the Distractors images
        public static List<Texture> ImagesToConstruct = new List<Texture>();
        public static List<string> DistractorsNames = new List<string>();
        public static bool CanSetRehaTask;
        private bool canLoadImages;

        private int _catArray;

        public static string SubCategory = "none";

        private void Update()
        {
            if (CanSetRehaTask && SpawnTiles.TotalLevels > 0)
            {
                for(var i = 0; i<LoadLevels.Levels.Count; i++)
                    //foreach (var t in LoadLevels.Levels.Where(t => t.UseDistractors))
                {
                    if(LoadLevels.Levels[i].UseDistractors)
                        _gameWithDistractors = true;
                }
                CanSetRehaTask = false;
                canLoadImages = true;
            }

            if (canLoadImages && LoadingGame && !_loadStarted)
            {
                LoadImages();
                _loadStarted = true;
                canLoadImages = false;
            }

            if (StartLoadingCatImages)
                LoadCategImages();

            if (SpawnTiles.StartLoadingDistrators && CategoryImagesLoaded)
                LoadDistractors();

            if (CategoryImagesLoaded && DistractorsSet && !UdpReceive.Calib)
            {
                SpawnTiles.InitTexture();
                GameLoaded = true;
            }
        }

        private void LoadImages()
        {
            if (ReadyToLoadImages)
            {
                for (var i = 0; i < SpawnTiles.TotCategories; i++)
                {
                    var tempList = new List<string>();
                    var di = new DirectoryInfo(Application.streamingAssetsPath + @"/Categories/" + SpawnTiles.Cats[i] + "/");
                    /*
                    #if UNITY_STANDALONE_OSX
                                        di = new DirectoryInfo(Application.dataPath + @"/Raw/Categories/" + SpawnTiles.Cats[i] + "/");
                    #endif
                    */
                    var smFiles = di.GetFiles();
                    
                    foreach (var fi in smFiles)
                    {
                        //exclude .meta Files
                        if (fi.Extension == ".jpg" || fi.Extension == ".png" || fi.Extension == ".JPG" || fi.Extension == ".PNG")
                        {
                            tempList.Add(fi.Name);
                        }
                    }
                    var rand = UnityEngine.Random.Range(0, tempList.Count);
                    var subCat = tempList[rand].Substring(0, 2);
                    var newTempList = new List<string>();
                    for (var z = 0; z < tempList.Count; z++)
                    {
                        if (tempList[z].StartsWith(subCat))
                        {
                            newTempList.Add(tempList[z]);
                        }
                    }

                    SpawnTiles.ImagesNames.Add(newTempList);
                }
        
                //loads Distractors -> in Reh@City only FashionStore uses distractors and are loaded from the same folder
                if (!SpawnTiles.DistractorsLoaded && _gameWithDistractors)
                {
                    var di2 = new DirectoryInfo(Application.streamingAssetsPath + @"/Categories/FashionStore/");
                    /*
                    #if UNITY_STANDALONE_OSX
                                        di2 = new DirectoryInfo(Application.dataPath + @"/Raw/Categories/FashionStore/");
                    #endif
                    */
                    var smFiles2 = di2.GetFiles();
                    foreach (var fi2 in smFiles2.Where(fi2 => fi2.Extension == ".jpg" || fi2.Extension == ".png" || fi2.Extension == ".JPG" || fi2.Extension == ".PNG"))
                    {
                        SpawnTiles.Files.Add(fi2.Name);
                    }
                    SpawnTiles.DistractorsLoaded = true;
                }
                else if (!_gameWithDistractors)
                {
                    ReadyToSetImages = true;
                }
                ChangeLevel.SetOnce = true;
                ChangeLevel.SetLevel();
                ReadyToLoadImages = false;
            }
        }

        private void LoadDistractors()
        {
            foreach (var t in Distractors)
                Destroy(t);
        
            Distractors.Clear();

            do
            {
                var rand = UnityEngine.Random.Range(0, SpawnTiles.Files.Count);

                var di2 = new DirectoryInfo(Application.streamingAssetsPath + @"/Categories/FashionStore/");
/*
#if UNITY_STANDALONE_OSX
                di2 = new DirectoryInfo(Application.dataPath + @"/Raw/Categories/Distractors/");
#endif
*/
                var smFiles2 = di2.GetFiles();
                var j = 0;
                foreach (var fi2 in smFiles2)
                {
                    //exclude .meta Files
                    if (fi2.Extension == ".jpg" || fi2.Extension == ".png" || fi2.Extension == ".JPG" || fi2.Extension == ".PNG")
                    {
                        if (fi2.Name == SpawnTiles.Files[rand])
                        {
                            var www = new WWW("File://" + Application.streamingAssetsPath + @"/Categories/" + "FashionStore/" + SpawnTiles.Files[rand]);
                            /*
                            #if UNITY_STANDALONE_OSX
                                                        www = new WWW("File://" + Application.dataPath + @"/Raw/Categories/" + "FashionStore/" + SpawnTiles.Files[rand]);

                            #endif
                            */
                            if (!DistractorsNames.Contains(fi2.Name) && !fi2.Name.StartsWith(SubCategory))
                            {
                                Distractors.Add(www.texture);
                                Distractors[j].name = SpawnTiles.Files[rand];
                                DistractorsNames.Add(fi2.Name);
                                j++;
                            }
                        }
                    }
                }
            } while (Distractors.Count < SpawnTiles.TotalToRender - SpawnTiles.Correctchoices);
            DistractorsSet = true;
            SpawnTiles.StartLoadingDistrators = false;

        }

        private void LoadCategImages()
        {
            foreach (var t in LevelCategory)
                Destroy(t);

            LevelCategory.Clear();

            var di3 = new DirectoryInfo(Application.streamingAssetsPath + @"/Categories/" + SpawnTiles.Folder + "/");
/*
#if UNITY_STANDALONE_OSX
            di3 = new DirectoryInfo(Application.dataPath + @"/Raw/Categories/" + SpawnTiles.Folder + "/");
#endif
*/
            var smFiles3 = di3.GetFiles();

            if (SpawnTiles.ImageTc >= 0)
            {
                SpawnTiles.ConstructFigure = true;
                var di4 = new DirectoryInfo(Application.streamingAssetsPath + @"/Categories/" + "ToConstruct/");
/*
#if UNITY_STANDALONE_OSX
                di4 = new DirectoryInfo(Application.dataPath + @"/Raw/Categories/" + "ToConstruct/");
#endif
*/
                var smFiles4 = di4.GetFiles();

                var c = 0;

                foreach (var fi4 in smFiles4)
                {
                    if (fi4.Extension == ".jpg" || fi4.Extension == ".png" || fi4.Extension == ".JPG" || fi4.Extension == ".PNG")
                    {
                        var www = new WWW("File://" + Application.streamingAssetsPath + @"/Categories/" + "ToConstruct/" + fi4.Name);
/*
#if UNITY_STANDALONE_OSX
                        www = new WWW("File://" + Application.dataPath + @"/Raw/Categories/" + "ToConstruct/" + fi4.Name);

#endif
*/

                        if (c == SpawnTiles.ImageTc)
                        {
                            SpawnTiles.ToConstruct = www.texture;
                            SpawnTiles.ToConstruct.name = fi4.Name;
                        }
                        c++;
                    }
                }
            }
            else
            {
                SpawnTiles.ConstructFigure = false;
            }
        
            for (var i = 0; i < SpawnTiles.Cats.Count; i++)
            {
                if (SpawnTiles.Cats[i] == SpawnTiles.Folder)
                {
                    if (!UdpReceive.Calib)
                    {
                        var b = 0;
                        
                        foreach (var fi3 in smFiles3)
                        {
                            if (fi3.Extension == ".jpg" || fi3.Extension == ".png" || fi3.Extension == ".JPG" || fi3.Extension == ".PNG")
                            {
                                var www = new WWW("File://" + Application.streamingAssetsPath + @"/Categories/" + SpawnTiles.Folder + "/" + fi3.Name);
                                /*
                                #if UNITY_STANDALONE_OSX
                                                                www = new WWW("File://" + Application.dataPath + @"/Raw/Categories/" + SpawnTiles.Folder + "/" + fi3.Name);

                                #endif
                                */
                                
                                _catArray = i;

                                ShuffleCatImages();

                                var catImagesToLoad = SpawnTiles.ImagesNames[i].Count >= 50 ? 50 : SpawnTiles.ImagesNames[i].Count;
                            
                                for (var a = 0; a < catImagesToLoad; a++)
                                {
                                    if (fi3.Name == SpawnTiles.ImagesNames[i][a])
                                    {
                                        SubCategory = fi3.Name.Substring(0, 2);
                                        LevelCategory.Add(www.texture);
                                        LevelCategory[b].name = fi3.Name;
                                        b++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            CategoryImagesLoaded = true;
            StartLoadingCatImages = false;
        } 
    
        private void ShuffleCatImages()
        {
            for (var i = 0; i < SpawnTiles.ImagesNames[_catArray].Count; i++)
            {
                var temp = SpawnTiles.ImagesNames[_catArray][i];
                var randomIndex = UnityEngine.Random.Range(i, SpawnTiles.ImagesNames[_catArray].Count);
                SpawnTiles.ImagesNames[_catArray][i] = SpawnTiles.ImagesNames[_catArray][randomIndex];
                SpawnTiles.ImagesNames[_catArray][randomIndex] = temp;
            }
        }   
    }
}

