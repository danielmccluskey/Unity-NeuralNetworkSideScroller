using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class SideScrollerAgent : Agent
{
    [SerializeField]
    private GameObject m_goLevelManagerRef;//Reference to the level manager for this particular agent

    [SerializeField]
    private LayerMask m_lmGroundMask;//The mask for the ground check

    [SerializeField]
    private Transform m_tGroundCheck;//The position of the bottom of the agent

    [SerializeField]
    private bool m_bUseVectorObservations = true;//Can the agent use the ray perception script to make observations?

    [SerializeField]
    private float m_fJumpSpeed = 35.0F;//The force given to the agent once they jump

    [SerializeField]
    private float m_fMaxSpeed = 100.0f;//The maximum speed that the agent can move at

    [SerializeField]
    private float m_fSpeed = 15.0F;//The force multiplier given when the agent moves left or right.

    private bool m_bJump = false;//Should the agent jump?
    private float m_fFurthestXPos = 0;//The furthest X position the agent has reached in this iteration
    private RayPerception m_rRayPerception;//Stores a reference to the Rayperception script attached to the agent
    private Vector3 m_v3LastPosition = Vector3.zero;//Stores the last position of the agent from the last frame
    private Vector3 m_v3MoveDirection = Vector3.zero;//Which way the agent should move
    private Vector3 m_v3StartingPos;//The starting position of the agent

    /// <summary>
    /// Collects the (vector, visual, text) observations of the agent.
    /// The agent observation describes the current environment from the
    /// perspective of the agent.
    /// </summary>
    /// <remarks>
    /// Simply, an agents observation is any environment information that helps
    /// the Agent acheive its goal. For example, for a fighting Agent, its
    /// observation could include distances to friends or enemies, or the
    /// current level of ammunition at its disposal.
    /// Recall that an Agent may attach vector, visual or textual observations.
    /// Vector observations are added by calling the provided helper methods:
    /// - <see cref="AddVectorObs(int)" />
    /// - <see cref="AddVectorObs(float)" />
    /// - <see cref="AddVectorObs(Vector3)" />
    /// - <see cref="AddVectorObs(Vector2)" />
    /// - <see cref="AddVectorObs(float[])" />
    /// - <see cref="AddVectorObs(List{float})" />
    /// - <see cref="AddVectorObs(Quaternion)" />
    /// - <see cref="AddVectorObs(bool)" />
    /// - <see cref="AddVectorObs(int, int)" />
    /// Depending on your environment, any combination of these helpers can
    /// be used. They just need to be used in the exact same order each time
    /// this method is called and the resulting size of the vector observation
    /// needs to match the vectorObservationSize attribute of the linked Brain.
    /// Visual observations are implicitly added from the cameras attached to
    /// the Agent.
    /// Lastly, textual observations are added using
    /// <see cref="SetTextObs(string)" />.
    /// </remarks>
    public override void CollectObservations()
    {
        if (m_bUseVectorObservations)//If the agent is allowed to observe
        {
            float fRayDistance = 20.0f;//Distance that they can see.
            float[] fRayAngles = { 0f, 180f };//What angles can I see?
            var sDetectableObjects = new[] { "Enemy", "Goal", "SideScrollerWall", "Hole" };//What tags should I care about seeing?
            AddVectorObs(m_rRayPerception.Perceive(fRayDistance, fRayAngles, sDetectableObjects, 0f, 0f));//Send out the rays and add them to the observation list
        }
    }

    /// <summary>
    /// Starts this instance.
    /// </summary>
    private void Start()
    {
        m_rRayPerception = GetComponent<RayPerception>();//Get the Rayperception script reference
        m_v3StartingPos = transform.position;//Set the starting position
    }

    /// <summary>
    /// Specifies the agent behavior at every step based on the provided
    /// action.
    /// </summary>
    /// <param name="vectorAction">Vector action. Note that for discrete actions, the provided array
    /// will be of length 1.</param>
    /// <param name="textAction">Text action.</param>
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        var Movement = (int)vectorAction[0];//Cast the vector action array as an integer
        m_v3MoveDirection = Vector3.zero;//Reset the move direction vector
        switch (Movement)//Switch statement to find out which action is being used
        {
            //case 0://Sit completely still (Do nothing)
            //    m_v3MoveDirection = new Vector3(0.0f, 0, 0.0f);
            //    break;

            case 0://Move Left
                m_v3MoveDirection = new Vector3(-1.0f, 0, 0.0f);
                break;

            case 1://Move Right
                m_v3MoveDirection = new Vector3(1.0f, 0, 0.0f);
                break;

            case 2://Jump
                if (IsOnGround())
                {
                    m_bJump = true;
                }
                break;

                //case 4://Jump Left
                //    if (IsOnGround())
                //    {
                //        m_v3MoveDirection = new Vector3(-0.7f, 0.0f, 0.0f);
                //        m_bJump = true;
                //    }
                //    break;

                //case 5://Jump Right
                //    if (IsOnGround())
                //    {
                //        m_v3MoveDirection = new Vector3(0.7f, 0.0f, 0.0f);
                //        m_bJump = true;
                //    }
                //    break;
        }

        AddReward(-1f / agentParameters.maxStep);//Subtract from reward, to add incentive to finish quickly
    }

    /// <summary>
    /// Fixed update.
    /// </summary>
    private void FixedUpdate()
    {
        if (m_bJump == true)//If I should Jump
        {
            GetComponent<Rigidbody>().AddForce(0.0f, m_fJumpSpeed, 0.0f);//Add upwards force to simulate jump
            m_bJump = false;//Reset jump flag
        }
        if (m_v3MoveDirection.magnitude > 0)//If the movement vector is not 0
        {
            m_v3MoveDirection.x *= m_fSpeed;//Add the speed multiplier
            GetComponent<Rigidbody>().AddForce(m_v3MoveDirection);//Add that force
            m_v3MoveDirection = Vector3.zero;//Reset the movement vector to zero
        }

        if (GetComponent<Rigidbody>().velocity.magnitude > m_fMaxSpeed)//If the velocity is above the max
        {
            GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(GetComponent<Rigidbody>().velocity, m_fMaxSpeed);//Clamp the velocity to the max velocity
        }

        if (transform.position.x > m_fFurthestXPos)//If the agent has progressed further through the level
        {
            m_fFurthestXPos = transform.position.x;//Set the furthest X pos
            AddReward(0.005f);//Encourage Moving further right
        }

        if (transform.position.y <= -30.0f)//If they fall down a hole or through the map
        {
            AddReward(-1.0f);//Discourage

            Done();//Reset
        }
        if (transform.position.x >= 350)//If they finish the level
        {
            AddReward(1.0f);//Reward them

            Done();//Reset
        }
    }

    /// <summary>
    /// Specifies the agent behavior when being reset, which can be due to
    /// the agent or Academy being done (i.e. completion of local or global
    /// episode).
    /// </summary>
    public override void AgentReset()
    {
        transform.position = m_v3StartingPos;//Go back to start
        m_v3MoveDirection = Vector3.zero;//Reset velocity vector
        m_fFurthestXPos = m_v3StartingPos.x;//Reset progress
        m_goLevelManagerRef.GetComponent<CS_LevelSpawner>().ResetLevel();//Reset enemies
    }

    /// <summary>
    /// Specifies the agent behavior when done and
    /// <see cref="AgentParameters.resetOnDone" /> is false. This method can be
    /// used to remove the agent from the scene.
    /// </summary>
    public override void AgentOnDone()
    {
    }

    /// <summary>
    /// Determines whether agent is on the ground.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if [is on ground]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsOnGround()
    {
        Collider[] cTargetsInViewRadius = Physics.OverlapSphere(m_tGroundCheck.position, 0.2f, m_lmGroundMask);//Check if any ground blocks are at the agents feet
        if (cTargetsInViewRadius.Length > 0)//If there is a ground block at their feet
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Used so that a name gizmo is drawn in scene.
    /// </summary>
    private void OnDrawGizmos()
    {
    }

    /// <summary>
    /// Called when the agent stomps on an enemy
    /// </summary>
    public void EnemyStomped()
    {
        AddReward(0.5f);
    }

    /// <summary>
    /// Calls when the agent is killed
    /// </summary>
    public void Killed()
    {
        AddReward(-2.0f);//Dircourage dieing
        Done();//Reset
    }
}