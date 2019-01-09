using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class SideScrollerAgent : Agent
{
    private Vector3 moveDirection = Vector3.zero;
    public float speed = 15.0F;
    public float maxspeed = 100.0f;
    public float jumpSpeed = 35.0F;
    public float gravity = 20.0F;
    private Vector3 m_v3LastPosition = Vector3.zero;
    private float furthestXPos = 0;

    public bool useVectorObs = true;
    private RayPerception rayPer;

    [SerializeField]
    private Transform m_tGroundCheck;

    [SerializeField]
    private LayerMask m_lmGroundMask;

    private Vector3 m_v3StartingPos;

    private bool m_bJump = false;

    public override void CollectObservations()
    {
        if (useVectorObs)
        {
            var rayDistance = 12f;
            float[] rayAngles = { 0f, 180f };
            var detectableObjects = new[] { "Enemy", "Goal", "SideScrollerWall", "Hole" };
            AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
            //AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
        }
    }

    private void Start()
    {
        rayPer = GetComponent<RayPerception>();
        m_v3StartingPos = transform.position;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        var Movement = (int)vectorAction[0];
        moveDirection = Vector3.zero;
        switch (Movement)
        {
            case 0://Sit completely still
                moveDirection = new Vector3(0.0f, 0, 0.0f);
                break;

            case 1://Move Left
                moveDirection = new Vector3(-1.0f, 0, 0.0f);
                break;

            case 2://Move Right
                moveDirection = new Vector3(1.0f, 0, 0.0f);
                break;

            case 3://Jump
                if (IsOnGround())
                {
                    m_bJump = true;
                }
                break;

            case 4://Jump Left
                if (IsOnGround())
                {
                    moveDirection = new Vector3(-0.7f, 0.0f, 0.0f);
                    m_bJump = true;
                }
                break;

            case 5://Jump Right
                if (IsOnGround())
                {
                    moveDirection = new Vector3(0.7f, 0.0f, 0.0f);
                    m_bJump = true;
                }
                break;
        }
        //moveDirection = transform.TransformDirection(moveDirection);
        //Multiply it by speed.
        moveDirection.x *= speed;
        moveDirection.y += 0.0f;
        //moveDirection.y -= gravity * Time.deltaTime;
        //transform.position += moveDirection;
    }

    private void FixedUpdate()
    {
        //GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
        if (m_bJump == true)
        {
            GetComponent<Rigidbody>().AddForce(0.0f, jumpSpeed, 0.0f);
            m_bJump = false;
        }
        if (moveDirection.magnitude > 0)
        {
            GetComponent<Rigidbody>().AddForce(moveDirection);
            moveDirection = Vector3.zero;
        }
        if (GetComponent<Rigidbody>().velocity.magnitude > maxspeed)
        {
            GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, maxspeed);
        }

        if (transform.position.x > furthestXPos)
        {
            furthestXPos = transform.position.x;
            AddReward(0.05f);//Encourage Moving further right
        }

        m_v3LastPosition = transform.position;

        if (transform.position.y <= -30.0f)
        {
            AddReward(-10.0f);

            Done();
        }
        if (transform.position.x >= 350)
        {
            AddReward(10.0f);

            Done();
        }
    }

    public override void AgentReset()
    {
        transform.position = m_v3StartingPos;
        moveDirection = Vector3.zero;
        furthestXPos = 0.0f;
    }

    public override void AgentOnDone()
    {
    }

    private bool IsOnGround()
    {
        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(m_tGroundCheck.position, 0.2f, m_lmGroundMask);
        if (cTargetsInViewRadius.Length > 0)
        {
            return true;
        }
        return false;
    }
}