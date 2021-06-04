using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SlowSeekerPadded : Agent
{
    private Rigidbody2D rb;
    private Transform childSpriteTransform;
    private int episodeCount;
    private int seekCount;
    private Vector2 agentPreviousPosition;
    private bool completedEpisode;
    
    [Header("Standard Attributes")]
    public GameObject agent;
    public GameObject target;

    [Header("Padded Attributes")]
    public float observableDistance;
    public float observableDistanceDecrement;
    public float minObservabaleDistance;
    public int seeksTillDistanceDecrement;

    [Header("Speed Attributes")]
    public float velocityMultiplier;

    [Header("Rewards/Punishments")]
    public int seeksTillPunishment;
    public float rewardAmount;
    public float punishAmount;

    [Header("Distance observation")]
    public float k;


    //For logging data. Set dataHeadings to whatever you want to record.
    //Make sure to update any values you pass to SaveResults if you change these.
    private static readonly string outputFile = Directory.GetCurrentDirectory() + "/SlowSeekerPaddedObservations/obs3.csv";
    private string dataHeadings = "episode,successRate,timeRemaining,agentSuccessCount,observableDistance";
    //successRate key: S = success, T = agent timed out



    private void Start()
    {

        //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), agent.GetComponent<Collider2D>(), false);
        agentPreviousPosition = Vector2.zero;
        childSpriteTransform = transform.GetChild(0);
        episodeCount = 0;
        seekCount = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0f;

        SaveResults("");
        SaveResults(dataHeadings);
    }

    public override void OnEpisodeBegin()
    {
        //checks if the target timed out last time, and records it if true
        if (!completedEpisode)
        {
            //Debug.Log($"{episodeCount},T,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
            SaveResults($"{episodeCount},T,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount},{observableDistance}");
        }
        agent.GetComponent<CustomAgentSeeker>().successCount = 0;
        /*
         * So essentially, each time the agent reaches a goal this will increase the episode count,
         * if then agent reaches the goal enough, the goal will be able to spawn into a larger radius around the map,
         * this basically tricks the agent at the start into collecting / realising that going to the goal is good and thus
         * by the time the goal is starts to spawn far away the agent realises that it needs to go to the goal.
         */
        episodeCount += 1;

        if (seekCount >= seeksTillDistanceDecrement)
        {
            if (observableDistance > minObservabaleDistance)
            {
                observableDistance -= observableDistanceDecrement;
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        //sensor.AddObservation(targetTransform.localPosition);
        //sensor.AddObservation(transform.localPosition);

        Vector2 agentCurrentPosition = Vector2.zero;

        //2
        sensor.AddObservation((Vector2)(target.transform.position - transform.position));
        //sensor.AddObservation((Vector2)(target.transform.position - agent.transform.position));

        //4
        //sensor.AddObservation(GetObservableDistance(target.transform.position, transform.position));
        //sensor.AddObservation(GetObservableDistance(agent.transform.position, target.transform.position));

        //6
        if (Vector2.Distance(transform.position, agent.transform.position) < observableDistance)
        {
            agentCurrentPosition = (Vector2)(agent.transform.position - transform.position);
            sensor.AddObservation(agentCurrentPosition);
            sensor.AddObservation(agentPreviousPosition);
            sensor.AddObservation(GetObservableDistance(agent.transform.position, transform.position));
        }
        else
        {
            sensor.AddObservation(Vector2.zero);
            sensor.AddObservation(Vector2.zero);
            sensor.AddObservation(Vector2.zero);
        }
        

        //1
        sensor.AddObservation(StepCount / MaxStep);

        agentPreviousPosition = agentCurrentPosition;


        //15 observations
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector2 movementDirection = Vector2.zero;

        int movement = actionBuffers.DiscreteActions[0];

        if (movement == 0)
        {
            movementDirection.x = 1;
        }
        if (movement == 1)
        {
            movementDirection.x = -1;
        }
        if (movement == 2)
        {
            movementDirection.y = 1;
        }
        if (movement == 3)
        {
            movementDirection.y = -1;
        }

        rb.velocity = movementDirection.normalized * velocityMultiplier * Time.fixedDeltaTime;

        if (movementDirection != Vector2.zero)
        {
            childSpriteTransform.up = rb.velocity;
        }

    }

    public void ReachedTarget()
    {
        //transform.position = target.transform.position;

        if (seekCount >= seeksTillPunishment)
        {
            AddReward(-punishAmount);

            /*
             * NOT USED DURING INFERENCE
             */

            if (agent.GetComponent<CustomAgentSeeker>().resetTargetPosition)
            {
                agent.GetComponent<CustomAgentSeeker>().resetTargetPosition = false;
            }
        }
            
    }

    public void SaveResults(string observation)
    {
        var file = new StreamWriter(outputFile, append: true);
        file.WriteLine(observation);
        file.Close();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * 
         */
        if (collision.collider.CompareTag("Player"))
        {
            if (agent.GetComponent<CustomAgentSeeker>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(rewardAmount);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},T,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount},{observableDistance}");
                agent.GetComponent<CustomAgentSeeker>().Seeked();
                agent.GetComponent<CustomAgentSeeker>().successCount = 0;
                seekCount += 1;
                completedEpisode = true;
                EndEpisode();
            }
            else if (agent.GetComponent<CustomAgentSmart>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(rewardAmount);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},T,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount},{observableDistance}");
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
                seekCount += 1;
                completedEpisode = true;
                EndEpisode();
            }
        }

       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {

            if (agent.GetComponent<CustomAgentSeeker>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(rewardAmount);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},T,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount},{observableDistance}");
                agent.GetComponent<CustomAgentSeeker>().Seeked();
                agent.GetComponent<CustomAgentSeeker>().successCount = 0;
                seekCount += 1;
                completedEpisode = true;
                EndEpisode();
            }
            else if (agent.GetComponent<CustomAgentSmart>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(rewardAmount);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},T,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount},{observableDistance}");
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
                seekCount += 1;
                completedEpisode = true;
                EndEpisode();
            }




        }
    }

    public Vector2 GetObservableDistance(Vector3 a, Vector3 b)
    {
        Vector2 offset = a - b;
        float d = Mathf.Sqrt(offset.x * offset.x + offset.y * offset.y);
        Vector2 unitVector = (1 / d) * offset;
        return Mathf.Exp(-k * d) * unitVector;
    }

}
