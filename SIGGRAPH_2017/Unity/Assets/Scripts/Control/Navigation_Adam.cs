using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using WindowsInput;
using SIGGRAPH_2018;

public class Navigation_Adam : Agent
{

    int count = 0;
    int pre_move = 0;
    int pre_rotate = 0;
    int step = 0;
    int change_goal = 0;
    int episodes = 0;
    [SerializeField] private Transform goalTransform;

    [SerializeField] private Transform Wall1Transform;
    [SerializeField] private Transform Wall2Transform;
    [SerializeField] private Transform Wall3Transform;
    [SerializeField] private Transform Wall4Transform;

    [SerializeField] private Transform Obs1Transform;
    [SerializeField] private Transform Obs2Transform;
    [SerializeField] private Transform Obs3Transform;
    [SerializeField] private Transform Obs4Transform;

    // [SerializeField] private Transform targetTransform;
    private Rigidbody AdamRigidBody;
    private void Awake()
    {
        AdamRigidBody = GetComponent<Rigidbody>();
        InputSimulator inputSimulator = new InputSimulator();
        inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_W);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
    }
    public BioAnimation_Adam Adam;
    

    public override void OnEpisodeBegin()
    {
        //Debug.Log("Episode " + episodes+ "begins");
        
        if (change_goal % 2 == 0)
        {
            goalTransform.position = new Vector3(Random.Range(-15f, -17f), 0.5f, Random.Range(9,11));
        }
        else {
            goalTransform.position = new Vector3(Random.Range(15.0f, 18f), 0.5f, Random.Range(-6, -9));
        }
        step = 0;
        episodes++;
        change_goal++;
        //Adam.Awake();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);

        sensor.AddObservation(goalTransform.position);

        sensor.AddObservation(Wall1Transform.position);
        sensor.AddObservation(Wall1Transform.localScale);
        sensor.AddObservation(Wall2Transform.position);
        sensor.AddObservation(Wall2Transform.localScale);
        sensor.AddObservation(Wall3Transform.position);
        sensor.AddObservation(Wall3Transform.localScale);
        sensor.AddObservation(Wall4Transform.position);
        sensor.AddObservation(Wall4Transform.localScale);

        sensor.AddObservation(Obs1Transform.position);
        sensor.AddObservation(Obs1Transform.localScale);
        sensor.AddObservation(Obs1Transform.rotation);
        sensor.AddObservation(Obs2Transform.position);
        sensor.AddObservation(Obs2Transform.localScale);
        sensor.AddObservation(Obs2Transform.rotation);
        sensor.AddObservation(Obs3Transform.position);
        sensor.AddObservation(Obs3Transform.localScale);
        sensor.AddObservation(Obs3Transform.rotation);
        sensor.AddObservation(Obs4Transform.position);
        sensor.AddObservation(Obs4Transform.localScale);
        sensor.AddObservation(Obs4Transform.rotation);
        //Debug.Log("Position of Goal" + goalTransform.position);
    }
    
    public void CheckRange() {
        if ((transform.position.x <= -25 && transform.position.x >= 25) || (transform.position.z <= -25 && transform.position.z >= 25))
        {
            AddReward(-2.0f / 500);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int move = actions.DiscreteActions[0];
        int rotate= actions.DiscreteActions[1];
        if (move == 5) {
            move = 0;
        }
        if (rotate == 3)
        {
            rotate = 0;
        }
        Adam.move1 = move;
        Adam.rotate1 = rotate;
        CheckRange();
        /*if (move == 1) {
            AddReward(1.0f / 1000);
        }*/
        /*if (count == 0)
        {
            Adam.move1 = move;
            Adam.rotate1 = rotate;
            pre_move = move;
            pre_rotate = rotate;
            CheckRange();
            //AddReward(1.0f / 500);
            count++;
        }
       /*else if (count == 10)
        {
            count = 1;
            Adam.move1 = move;
            Adam.rotate1 = rotate;
            pre_move = move;
            pre_rotate = rotate;
            CheckRange();
            //AddReward(1.0f / 500);
        }
        else {
            Adam.move1 = pre_move;
            Adam.rotate1 = pre_rotate;
            count++;
        }*/

        step++;
        //Debug.Log("Move    " + Adam.move1);
        //Debug.Log("Rotate   " + Adam.rotate1);
  
        if (step == MaxStep-1) {
            Debug.Log("Max step on Episode:" + episodes );
            Debug.Log("Current Reward:    " + GetCumulativeReward());
        }
        Adam.Update();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            AddReward(+3f);
            /*if (step <= 1000) {
                AddReward(3.0f);
            }*/
            Debug.Log("Reach Goal on:   " + step);
            Debug.Log("Current Reward:    " + GetCumulativeReward());
            EndEpisode();
        }
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-5f);
            Debug.Log("collide on Wall:   " + step);
            Debug.Log("Current Reward:    " + GetCumulativeReward());
            EndEpisode();
        }
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            AddReward(-5f);
            Debug.Log("collide on Obstacle:   " + step);
            Debug.Log("Current Reward:    " + GetCumulativeReward());
            EndEpisode();
        }
        /*if (other.TryGetComponent<Moving_Obstacles>(out Moving_Obstacles moveob))
        {
            AddReward(-1f);
            Debug.Log("collide on eve:   " + step);
            Debug.Log("Current Reward:    " + GetCumulativeReward());
            //EndEpisode();
        }*/
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int move_input = 10;
        int turn_input = 10;
        //float startTime = 0f;
        //float holdTime = 1.0f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            /*startTime = Time.time;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (startTime + holdTime >= Time.time)
                {
                    move_input = 1;
                }
            }*/
            move_input = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            /*startTime = Time.time;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (startTime + holdTime >= Time.time)
                {
                    move_input = 2;
                }
            }*/
            move_input = 2;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            /*startTime = Time.time;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (startTime + holdTime >= Time.time)
                {
                    move_input = 3;
                }

            }*/
            move_input = 3;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            /*startTime = Time.time;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (startTime + holdTime >= Time.time)
                {
                    move_input = 4;
                }
            }*/
            move_input = 4;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow)&& Input.GetKeyUp(KeyCode.DownArrow) && Input.GetKeyUp(KeyCode.RightArrow) && Input.GetKeyUp(KeyCode.LeftArrow)) {
            move_input = 5;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            /*startTime = Time.time;
            if (Input.GetKey(KeyCode.Q))
            {
                if (startTime + holdTime >= Time.time)
                {
                    turn_input = 1;
                }
            }*/
            turn_input = 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            /*startTime = Time.time;
            if (Input.GetKey(KeyCode.E))
            {
                if (startTime + holdTime >= Time.time)
                {
                    turn_input = 2;
                }
            }*/
            turn_input = 2;
        }
        if (Input.GetKeyUp(KeyCode.Q) && Input.GetKeyUp(KeyCode.E)) {
            turn_input = 3;
        }
        actionsOut.DiscreteActions.Array[0] = move_input;
        actionsOut.DiscreteActions.Array[1] = turn_input;
        if (move_input == 5)
        {
            move_input = 0;
        }
        if (turn_input == 3)
        {
            turn_input = 0;
        }
        Adam.move1 = move_input;
        Adam.rotate1 = turn_input;
        Adam.Update();
    }
}
