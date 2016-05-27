using System;
using UnityEngine;
using System.Collections.Generic;

public class ColorDictionary {
    
    private TextAsset colorList;
    private Dictionary<string, Dictionary<string, Color>> colorDictionary;

    //Constructor
    public ColorDictionary(TextAsset cList)
    {
        colorList = cList;
        colorDictionary = new Dictionary<string, Dictionary<string, Color>>();
        //Parse color rows
        string[] fileRows = colorList.text.Split('\n');
        

        for (int i = 0; i < fileRows.Length - 1; i++)
        {
            //Create temporary color type dictionary
            Dictionary<string, Color> tempDictionary = new Dictionary<string, Color>();
            
            //create variable to hold parsed color values
            string[] values = fileRows[i].Split(',');

            //Create temp color
            string temp_Color = values[0];

            int numberOfColors = (values.Length - 1) / 4;
            //For each _ColorType, add Color to tempDictionary
            for(int j = 0; j < numberOfColors; j++)
            {
                //Create temp_ColorType
                string temp_ColorType = values[1 + 4 * j];
                //Create tempColor
                Color tempColor = Color.HSVToRGB(float.Parse(values[2+4*j]), float.Parse(values[3+4*j]), float.Parse(values[4+4*j]));
                //Add tempColor to the tempDictionary
                tempDictionary.Add(temp_ColorType, tempColor);
            }
            //Add to colorDictionary
            colorDictionary.Add(temp_Color, tempDictionary);
        }
    }

    public Dictionary<string, Color> GetColorDictionary(string color)
    {
        return colorDictionary[color];
    }

   
}
