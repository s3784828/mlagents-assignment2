using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Seeker : Agent
{
    private Rigidbody2D rb;
    private Transform childSpriteTransform;
    
    private int episodeCount;
    private bool completedEpisode;

    [Header("Standard Attributes")]
    public GameObject agent;
    
    [Header("Speed Attributes")]
    public float velocityMultiplier;

    [Header("Distance observation")]
    public float k;


    //For logging data. Set dataHeadings to whatever you want to record.
    //Make sure to update any values you pass to SaveResults if you change these.
    private static readonly string outputFile = Directory.GetCurrentDirectory() + "/SeekerObservations/obs3.csv";
    private string dataHeadings = "episode,successRate,timeRemaining,agentSuccessCount";
    //successRate key: S = success, T = agent timed out



    private void Start()
    {
        childSpriteTransform = transform.GetChild(0);
        episodeCount = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = 0f;

        SaveResults("");
        SaveResults(dataHeadings);
    }

    public override void OnEpisodeBegin()
    {
        int successCount = 0;

        if (agent.GetComponent<CustomAgentSeeker>() != null)
        {
            successCount = agent.GetComponent<CustomAgentSeeker>().successCount;
        }
        else if (agent.GetComponent<CustomAgentSmart>() != null)
        {
            successCount = agent.GetComponent<CustomAgentSmart>().successCount;
        }

        //checks if the target timed out last time, and records it if true
        if (!completedEpisode)
        {
            Debug.Log($"{episodeCount},T,{(MaxStep - StepCount)},{successCount}");
            SaveResults($"{episodeCount},T,{(MaxStep - StepCount)},{successCount}");
        }

        if (agent.GetComponent<CustomAgentSeeker>() != null)
        {
            agent.GetComponent<CustomAgentSeeker>().successCount = 0;
        }
        else if (agent.GetComponent<CustomAgentSmart>() != null)
        {
            agent.GetComponent<CustomAgentSmart>().successCount = 0;
        }


        /*
         * So essentially, each time the agent reaches a goal this will increase the episode count,
         * if then agent reaches the goal enough, the goal will be able to spawn into a larger radius around the map,
         * this basically tricks the agent at the start into collecting / realising that going to the goal is good and thus
         * by the time the goal is starts to spawn far away the agent realises that it needs to go to the goal.
         */
        episodeCount += 1;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        //sensor.AddObservation(targetTransform.localPosition);
        //sensor.AddObservation(transform.localPosition);

        //2
        sensor.AddObservation((Vector2)(agent.transform.position - transform.position));

        //2
        sensor.AddObservation(GetObservableDistance());

        //1
        sensor.AddObservation(StepCount / MaxStep);

        //5 observations
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        /*
         * it would be cool to experiment with continous actions.
         */
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

    public void SaveResults(string observation)
    {
        var file = new StreamWriter(outputFile, append: true);
        file.WriteLine(observation);
        file.Close();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            if (agent.GetComponent<CustomAgentSeeker>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(1.0f);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                agent.GetComponent<CustomAgentSeeker>().Seeked();
                agent.GetComponent<CustomAgentSeeker>().successCount = 0;
                completedEpisode = true;
                EndEpisode();
            }
            else if (agent.GetComponent<CustomAgentSmart>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(1.0f);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSmart>().successCount}");
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
                completedEpisode = true;
                EndEpisode();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (agent.GetComponent<CustomAgentSeeker>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(1.0f);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                agent.GetComponent<CustomAgentSeeker>().Seeked();
                agent.GetComponent<CustomAgentSeeker>().successCount = 0;
                completedEpisode = true;
                EndEpisode();
            }
            else if (agent.GetComponent<CustomAgentSmart>() != null)
            {
                //Debug.Log("COLLISION");
                //Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider, false);
                AddReward(1.0f);
                //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSeeker>().successCount}");
                SaveResults($"{episodeCount},S,{(MaxStep - StepCount)},{agent.GetComponent<CustomAgentSmart>().successCount}");
                agent.GetComponent<CustomAgentSmart>().Seeked();
                agent.GetComponent<CustomAgentSmart>().successCount = 0;
                completedEpisode = true;
                EndEpisode();
            }
        }
    }

    public Vector2 GetObservableDistance()
    {
        Vector2 offset = agent.transform.position - transform.position;
        float d = Mathf.Sqrt(offset.x * offset.x + offset.y * offset.y);
        Vector2 unitVector = (1 / d) * offset;
        return Mathf.Exp(-k * d) * unitVector;
    }

}
