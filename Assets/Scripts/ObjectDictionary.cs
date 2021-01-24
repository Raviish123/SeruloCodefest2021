using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDictionary : MonoBehaviour
{

    public static int GetObjectCost(string objectName)
    {
        var cost = 0;
        switch (objectName)
        {
            case "Vaccine":
                cost = 500;
                break;
            case "Carrot":
                cost = 300;
                break;
            case "Apple":
                cost = 375;
                break;
            default:
                break;
        }
        return cost;
    }

    public static void UseObject(string objectName, PlayerResistance pr)
    {
        switch (objectName)
        {
            case "Vaccine":
                UseVaccine(pr);
                break;
            case "Carrot":
                UseCarrot(pr);
                break;
            case "Apple":
                UseApple(pr);
                break;
            default:
                break;
        }
        
    }


    static void UseVaccine(PlayerResistance pr)
    {
        pr.Disinfect();
    }
    static void UseCarrot(PlayerResistance pr)
    {
        pr.IncreaseResistance(30);
    }
    static void UseApple(PlayerResistance pr)
    {
        pr.StartPausedRoutine(10);
    }
}
