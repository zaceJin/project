using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using WindowsInput;
using SIGGRAPH_2018;

public class Navi_Adam : Agent{
    InputSimulator inputSimulator = new InputSimulator();
    /*public Vector3 Position;
    public Vector3 Rotation;
    private Vector3 LastPosition;
    private Quaternion LastRotation ;
    */
    public Controller C;
    //public InputHandler SimulatorInput;
    public BioAnimation_Adam A;

    [SerializeField] private Transform goalTransform;
    // [SerializeField] private Transform targetTransform;

 

    public override void OnEpisodeBegin() {
        goalTransform.position = Vector3.zero;
        //transform.position = Vector3.zero;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goalTransform.position);
    }
    public override void OnActionReceived(ActionBuffers actions) {
        int move = actions.DiscreteActions[0];
        int rotate = actions.DiscreteActions[0];
        Debug.Log("Move" + move);
        Debug.Log("Rotate" + rotate);
        HashSet<KeyCode> state = new HashSet<KeyCode>();
        if (move == 0)
        {
            //inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_W);
            state.Add(KeyCode.W);
        }
        else if (move == 1) {
            //inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_A);
            state.Add(KeyCode.A);
        }
        else if (move == 2)
        {
            //inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_S);
            state.Add(KeyCode.S);
        }
        else if (move == 3)
        {
            //inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_D);
            state.Add(KeyCode.D);
        }
        if (rotate == 0)
        {
            //inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_Q);
            state.Add(KeyCode.Q);
        }
        else if (rotate == 1)
        {
            //inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_E);
            state.Add(KeyCode.E);

        }
        /*float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float rotate = actions.ContinuousActions[2];
        float moveSpeed = 1f;
        */
        InputHandler.Keys.Add(state);
       
    }

 
    private void OtriggerEnter(Collider other) {
        if (other.TryGetComponent<Goal>(out Goal goal)) {
            SetReward(+1f);
            goalTransform.position = Vector3.zero;
            EndEpisode();
        }
    }
}
