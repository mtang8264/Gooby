using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*  This script is designed to simplify the reading of inputs by removing the need to write out the input commands in each script.
 *  Instead, this script reads all the necesary inputs and stores them in static variables which other classes can read.
 */

public class InputHandler : MonoBehaviour
{
    // Joysticks
    public static Vector2 leftStick;
    public static Vector2 leftStickLastFrame;
    public static Vector2 rightStick;
    public static Vector2 rightStickLastFrame;
    // Triggers
    public static float leftTrigger;
    public static float leftTriggerLastFrame;
    public static float rightTrigger;
    public static float rightTriggerLastFrame;
    // Bumpers
    public static bool leftBumper;
    public static bool rightBumper;

    // Face buttons
    public static bool triangle;
    public static bool square;
    public static bool circle;
    public static bool x;
    // D pad
    public static Vector2 dPad;

    /* In update we will read all of our inputs.
     * For refernce, I am listing the axis inputs that Unity is reading these inputs on just in case something terrible happens.
     * I am also listing if I inverted the values.
     * Joysticks read as 0 by default and follow Unity's direction signs so up is positive and down is negative.
     * Triggers read as -1 by default and read as 1 when fully pressed. 0 is half way between them.
     */
    void Update()
    {
        // Joysticks
        leftStickLastFrame.x = leftStick.x;
        leftStick.x = Input.GetAxis("Left_Stick_Horizontal");   // X axis
        leftStickLastFrame.y = leftStick.y;
        leftStick.y = Input.GetAxis("Left_Stick_Vertical");     // Y axis - inverted
        rightStickLastFrame.x = rightStick.x;
        rightStick.x = Input.GetAxis("Right_Stick_Horizontal"); // 3rd axis
        rightStickLastFrame.y = rightStick.y;
        rightStick.y = Input.GetAxis("Right_Stick_Vertical");   // 6th axis - inverted
        // Triggers
        leftTriggerLastFrame = leftTrigger;
        leftTrigger = Input.GetAxis("Left_Trigger");            // 4th axis - inverted
        rightTriggerLastFrame = rightTrigger;
        rightTrigger = Input.GetAxis("Right_Trigger");          // 5th axis - inverted
        // Bumpers
        leftBumper = Input.GetButton("Left_Bumper");            // joystick button 4
        rightBumper = Input.GetButton("Right_Bumper");          // joystick button 5

        // Face Buttons
        triangle = Input.GetButton("Triangle");                 // joystick button 3
        square = Input.GetButton("Square");                     // joystick button 0
        circle = Input.GetButton("Circle");                     // joystick button 2
        x = Input.GetButton("X");                               // joystick button 1
        // D Pad
        dPad.x = Input.GetAxis("D_Pad_Horizontal");             // 7th axis
        dPad.y = Input.GetAxis("D_Pad_Vertical");               // 8th axis
    }
}

#if UNITY_EDITOR
//  This custom editor is for ease of use so that I can check to make sure inputs are being properly read and stored.
//  It is not compiled or run when not in the Unity Editor.
[CustomEditor(typeof(InputHandler))]
public class InputHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draws the origional GUI which is just the file containing the class because all other values are static.
        base.OnInspectorGUI();

        // Joysticks
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Joysticks", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Vector2Field("Left Stick", InputHandler.leftStick);
        EditorGUILayout.Vector2Field("Right Stick", InputHandler.rightStick);
        EditorGUI.EndDisabledGroup();

        // Triggers
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Triggers", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("Left Trigger", InputHandler.leftTrigger);
        EditorGUILayout.FloatField("Right Trigger", InputHandler.rightTrigger);
        EditorGUI.EndDisabledGroup();

        // Bumpers
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Bumpers", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Toggle("Left Bumper", InputHandler.leftBumper);
        EditorGUILayout.Toggle("Right Bumper", InputHandler.rightBumper);
        EditorGUI.EndDisabledGroup();

        // Face buttons
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Face Buttons", EditorStyles.boldLabel);
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Toggle("Triangle Button", InputHandler.triangle);
        EditorGUILayout.Toggle("Square Button", InputHandler.square);
        EditorGUILayout.Toggle("Circle Button", InputHandler.circle);
        EditorGUILayout.Toggle("X Button", InputHandler.x);
        EditorGUI.EndDisabledGroup();

        // D Pad
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Vector2Field("D-Pad ", InputHandler.dPad);
        EditorGUI.EndDisabledGroup();

        Repaint();
    }
}
#endif