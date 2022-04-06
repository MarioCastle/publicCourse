using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
    
    /// <summary>
    /// /**inside Waypoint.cs **/
    ///     public Vector3[] Points => points;
    ///     public Vector3 Currentposition => _currentPosition ;
    /// </summary>


[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{

    Waypoint Waypoint => target as Waypoint;

    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        for(int i = 0 ;i < Waypoint.Points.Length ; i++)
        {
            EditorGUI.BeginChangeCheck();

            //Create Handles

            Vector3 currentWaypointPoint = Waypoint.CurrentPosition + Waypoint.Points[i];
            //Vector3 potition ,Quatenion,float size,snap ,HandleCap
            Vector3 newWaypointPoint = Handles.FreeMoveHandle(
                currentWaypointPoint, Quaternion.identity,0.7f,
                    new Vector3(0.3f,0.3f,0.3f),Handles.SphereHandleCap);
            
            //Create Text
            GUIStyle textStyle = new GUIStyle();
            textStyle.fontStyle = FontStyle.Bold;
            textStyle.fontSize = 16;
            textStyle.normal.textColor = Color.yellow;
            Vector3 textAlightment = Vector3.down * 0.35f + Vector3.right * 0.35f;
            Handles.Label(Waypoint.CurrentPosition + Waypoint.Points[i] + textAlightment,$"{i+1}",textStyle);

            EditorGUI.EndChangeCheck();

            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target,"Free Move Handle");
                Waypoint.Points[i] = newWaypointPoint - Waypoint.CurrentPosition;
            }

        }
    }
    
    
}
