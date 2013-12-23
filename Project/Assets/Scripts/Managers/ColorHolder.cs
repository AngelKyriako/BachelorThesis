using UnityEngine;
using System.Collections.Generic;

public class ColorHolder {

    private readonly Dictionary<PlayerColor, Color> colorMap = new Dictionary<PlayerColor, Color>(){
                                                                        {PlayerColor.None, Color.black },
                                                                        {PlayerColor.Red, Color.red },
                                                                        {PlayerColor.Blue, Color.blue },
                                                                        {PlayerColor.Gray, Color.gray },
                                                                        {PlayerColor.Orange, new Color32(255,70,0,255) },
                                                                        {PlayerColor.Green, Color.green },
                                                                        {PlayerColor.Pink, new Color32(240,105,180,255) },
                                                                        {PlayerColor.Yellow, new Color32(255,180,0,255) },
                                                                        {PlayerColor.Teal, new Color32(30,244,155,255) },
                                                                        {PlayerColor.White, Color.white },
                                                                        {PlayerColor.Purple, new Color32(128,0,128,255) }
                                                                   };

    private ColorHolder() { }

    private static ColorHolder instance = new ColorHolder();
    public static ColorHolder Instance {
        get { return ColorHolder.instance; }
    }

    public Color GetPlayerColor(PlayerColor _color) {
        return colorMap[_color];
    }
}
