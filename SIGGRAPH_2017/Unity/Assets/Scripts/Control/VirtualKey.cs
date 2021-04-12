using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowsInput;
using SIGGRAPH_2018;

public class VirtualKey : MonoBehaviour
{
    public void TestKeyInput()
    {
        // Build an input simulator instance
        InputSimulator inputSimulator = new InputSimulator();
        // Then call the keyboard key down method, pass in the enum virtual key code you want to press
        inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_W);
    }
    // Start is called before the first frame update
    public BioAnimation_Adam Adam;
    int i = 1;
    void Start()
    {
        TestKeyInput();
    }

    // Update is called once per frame
    void Update()
    {
        Adam.move1 = 1;
        
    }
}
