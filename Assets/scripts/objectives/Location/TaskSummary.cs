using UnityEngine;
using System.IO;
using Assets.scripts.RehaTask.RehaTaskGUI;
using System;
using System.Globalization;
using Assets.scripts.Manager;
using Assets.scripts.objectives;

public class TaskSummary : MonoBehaviour {

    private string[] _taskHeader = new string[11];
    private static readonly string[] TaskData = new string[11];
    private string _taskPath, _taskFilepath;
    private static TextWriter _taskFile;

    private static bool _canCloseFile;

    private void Start()
    {
        _taskHeader = new[] {
                "Tasks_Set",
                "Task_Location",
                "ObjectivePathNodes",
                "PlayerPathNodes",
                "Correct",
                "Incorrect",
                "CorrectOutOfTime",
                "IncorrectOutOfTime",
                "Performance",
                "TaskTime",
                "ExceededTime"
            };

        LogInit();
    }

    private void Update()
    {
        if (_canCloseFile)
        {
            CloseFile();
        }
    }

    public void CloseFile()
    {
        Debug.Log("Closing summary file");
        _taskFile.Close();
        _canCloseFile = false;
    }

    private void LogInit()
    {
        _taskPath = Application.dataPath + "/RehaCity_Log/" + Main_Menu.Uid + "/" + Main_Menu.Uid + "_" + DateTime.Now.ToString("yyyyMMdd") + "/";

        if (!Directory.Exists(_taskPath))
        {
            Directory.CreateDirectory(_taskPath);
        }
        
        _taskFilepath = _taskPath  + "tasks_summary.csv";
        
        //creates the string that will be the header of the csv File
        var taskHeader = _taskHeader[0];

        for (var i = 1; i < _taskHeader.Length; i++)
        {
            taskHeader = taskHeader + "," + _taskHeader[i];
        }

        _taskFile = new StreamWriter(_taskFilepath, false);
        _taskFile.WriteLine(taskHeader);

    }

    public static void SaveTaskSummary(string taskName, int path, int playerPath, int correct, int incorrect, int correctOutOfTime, int incorrectOutOfTime, float performance, float taskTime, float timeExceeded)
    {
        TaskData[0] = ObjectiveManager.Instance.Level.ToString();
        TaskData[1] = taskName;
        TaskData[2] = path.ToString();
        TaskData[3] = playerPath.ToString();
        TaskData[4] = correct.ToString();
        TaskData[5] = incorrect.ToString();
        TaskData[6] = correctOutOfTime.ToString();
        TaskData[7] = incorrectOutOfTime.ToString();
        TaskData[8] = performance.ToString(CultureInfo.InvariantCulture);
        TaskData[9] = taskTime.ToString(CultureInfo.InvariantCulture);
        TaskData[10] = timeExceeded.ToString(CultureInfo.InvariantCulture);

        var newLine = TaskData[0];

        for (var i = 1; i < TaskData.Length; i++)
        {
            newLine = newLine + "," + TaskData[i];
        }
       
        _taskFile.WriteLine(newLine);
    }

    private void OnApplicationQuit()
    {
        _taskFile.Close();
    }
}
