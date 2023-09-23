
using System;
using System.Collections.Generic;
using UnityEngine;

namespace core.general.datamodels
{
    public class DataModels : MonoBehaviour
    {

    }

    #region Downloaded Images

    public enum DownloadedImagestypes
    {

    }


    #endregion

    #region UI
    #region Popups

    public enum PopupsType
    {
        NoInternet,
        InputBlocker,
        Loading,
        GeneralError
    }

    #endregion


    #region Views

    public enum ViewsType
    {
        MainView
    }

    #endregion
    #endregion

    #region General Enums

    public enum TooltipMessageType
    {

    }

    public enum PopupMetaKeys
    {

    }

    public enum Direction
    {
        Down, 
        Left, 
        Up, 
        Right
    }
    public enum TileType
    {
        Empty,
        White,
        Green,
        Red,
        Placeable,
        Placed,
        CantPlace
    }

    public enum PlaceableObjectsType
    {
        Building,
        Conveyor
        
    }


    #endregion

    #region MetaData

    [Serializable]
    public class  MetaData
    {
        public GameMetaConstants GameMetaConstants;
    }

    [Serializable]
    public class GameMetaConstants
    {
        public UnitConstants UnitConstants;

        public Dictionary<PopupMetaKeys, PopupMessagesConstants> PopupMessages = new Dictionary<PopupMetaKeys, PopupMessagesConstants>();
        public Dictionary<TooltipMessageType, string> TooltipMessages = new Dictionary<TooltipMessageType, string>();
    }

    [Serializable]
    public class UnitConstants
    {

    }

    #region PopupMeta
    [Serializable]
    public class PopupMessagesConstants
    {
        public string Title;
        public string Message;

        public PopupMessagesConstants()
        {
            Title = "Title";
            Message = "Message";
        }

        public PopupMessagesConstants(PopupMessagesConstants value)
        {
            Title = value.Title;
            Message = value.Message;
        }

        public PopupMessagesConstants(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }

    

    #endregion
    #region Meta Helper Classes

    [Serializable]
    public class SerializedVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializedVector3()
        {

        }

        public SerializedVector3(SerializedVector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public SerializedVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SerializedVector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }


        public Vector3 GetVector3() => new Vector3(x, y, z);

    }

    [Serializable]
    public class SerializedColor
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color Color { get; set; }

        public SerializedColor()
        {
        }

        public SerializedColor(SerializedColor color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;

            Color = new Color(r, g, b, a);
        }

        public SerializedColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
            Color = new Color(r, g, b, a);
        }

        public SerializedColor(Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
            Color = new Color(r, g, b, a);
        }

        public Color GetColor() => new Color(r, g, b, a);
    }
    #endregion
    #endregion


}