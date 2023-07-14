using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using System;
using System.Reflection;
using UnityEditor;

[InitializeOnLoad]

public class Startup
{

    public static bool needRead = false;
    static Startup()
    {
        if (!needRead)
        {
            return;
        }
        string path = Application.dataPath + "/Editor/Level.xlsx";
        string assetName = "Level";

        FileInfo fileInfo = new FileInfo(path);

        LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));

        using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Zombie"];

            for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)
            {
                LevelItem levelItem = new LevelItem();
                
                Type type = typeof(LevelItem);

                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    FieldInfo variable = type.GetField(worksheet.GetValue(2, j).ToString());
                    string tableValue = worksheet.GetValue(i, j).ToString();
                    variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
                }

                levelData.levelDataList.Add(levelItem);
            }
        }

        AssetDatabase.CreateAsset(levelData, "Assets/Resources/" + assetName + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
    
}
