using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "DataDriven/Game/Objective/Go To", fileName = "Go To")]
[Serializable]
public class Data_MissionObjective_GoTo : Data_MissionObjective
{

    private Vector2 Destination ;
    private float DestinationBoundaries;

}