using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CustomAgentSeeker : Agent
{
    private Rigidbody2D rb;
    private Transform childSpriteTransform;
    private int episodeCount;
    private int seekCount;
    private int episodeSeekCount;
    private int rangeIncreaseCounter;

    private bool completedEpisode;

    [Header("Standard Attributes")]
    public Transform targetTransform;

    [Header("Seeker Attributes")]
    public GameObject seeker;
    public int successCount;
    

    [Header("Speed Attributes")]
    public float velocityMultiplier;
    public float velocityIncrement;
    public float maxVelocity;
    public int seeksTillVelocityIncrement;

    [Header("Distance observation")]
    public float k;

    [Header("Target Spawning Attributes")]
    public bool resetTargetPosition = true;
    public float targetRange;
    public float tempRangeIncrease;
    public int checksTillIncreaseTempRange;

    public int numChecks;
    /*
     * Starting range is normally set to 3, so a max spawn area of +3 x and -3 x, and +3 y and -3 y.
     * Refer to Tristan if that doesnt make sense. 
     */
    public float startingRange;

    /*
     * Range increment is like 0.05, it just increases the range when a particular number of episodes has elapsed.
     */
    public float rangeIncrement;

    /*
     * This is a really important attribute, since there the only way for an episode to reset is for the agent to get to 
     * the goal it is important to tinker with this attribute, I forgot what I set it to but I think it was like maybe 50 or even 100. IDK, soz 
     */
    public int episodesTillRangeIncrement;

    /*
     * This is set to 10, unless you change the size of the play area, dont change this value.
     */
    public float maxRange;

    private float range;

    //For logging data. Set dataHeadings to whatever you want to record.
    //Make sure to update any values you pass to SaveResults if you change these.
    private static readonly string outputFile = Directory.GetCurrentDirectory() + "/SeekerAgentObservations/obs3.csv";
    private string dataHeadings = "episode,successRate,timeRemaining,seekCount,velocity,range";
    //successRate key: S = success, T = agent timed out



    private void Start()
    {
        rangeIncreaseCounter = 0;
        range = startingRange;
        childSpriteTransform = transform.GetChild(0);
        episodeCount = 0;
        seekCount = 0;
        episodeSeekCount = 0;
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
            //Debug.Log($"{episodeCount},T,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
            SaveResults($"{episodeCount},T,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
        }

        /*
         * So essentially, each time the agent reaches a goal this will increase the episode count,
         * if then agent reaches the goal enough, the goal will be able to spawn into a larger radius around the map,
         * this basically tricks the agent at the start into collecting / realising that going to the goal is good and thus
         * by the time the goal is starts to spawn far away the agent realises that it needs to go to the goal.
         */

        episodeSeekCount = 0;
        episodeCount += 1;

        IncreaseRange();
        if (resetTargetPosition)
            ResetTargetPositionNearSeeker();
        else
            ResetAgentAwayTargetPosition();
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
        sensor.AddObservation((Vector2)(targetTransform.position - transform.position));

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
        if (collision.gameObject.CompareTag("Target"))
        {
            AddReward(1.0f);
            //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
            SaveResults($"{episodeCount},S,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
            completedEpisode = true;
            successCount += 1;

            if (seeker.GetComponent<SlowSeeker>() != null)
            {
                seeker.GetComponent<SlowSeeker>().ReachedTarget();
            }
            else if (seeker.GetComponent<SlowSeekerPadded>() != null)
            {
                seeker.GetComponent<SlowSeekerPadded>().ReachedTarget();
            }

            EndEpisode();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            AddReward(1.0f);
            //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
            SaveResults($"{episodeCount},S,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
            completedEpisode = true;
            successCount += 1;

            if (seeker.GetComponent<SlowSeeker>() != null)
            {
                seeker.GetComponent<SlowSeeker>().ReachedTarget();
            }
            else if (seeker.GetComponent<SlowSeekerPadded>() != null)
            {
                seeker.GetComponent<SlowSeekerPadded>().ReachedTarget();
            }

            EndEpisode();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            AddReward(1.0f);
            //Debug.Log($"{episodeCount},S,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
            SaveResults($"{episodeCount},S,{(MaxStep - StepCount)},{episodeSeekCount},{velocityMultiplier},{range}");
            completedEpisode = true;
            successCount += 1;

            if (seeker.GetComponent<SlowSeeker>() != null)
            {
                seeker.GetComponent<SlowSeeker>().ReachedTarget();
            }
            else if (seeker.GetComponent<SlowSeekerPadded>() != null)
            {
                seeker.GetComponent<SlowSeekerPadded>().ReachedTarget();
            }
            

            EndEpisode();
        }
    }

    public Vector2 GetObservableDistance()
    {
        Vector2 offset = targetTransform.position - transform.position;
        float d = Mathf.Sqrt(offset.x * offset.x + offset.y * offset.y);
        Vector2 unitVector = (1 / d) * offset;
        return Mathf.Exp(-k * d) * unitVector;
    }

    public void Seeked()
    {
        episodeSeekCount += 1;
        seekCount += 1;

        if (seekCount >= seeksTillVelocityIncrement)
        {
            if (velocityMultiplier < maxVelocity)
            {
                velocityMultiplier += velocityIncrement;
            }
        }
        IncreaseRange();
        ResetAgentAwayTargetPosition();
    }

    private void IncreaseRange()
    {
        rangeIncreaseCounter += 1;

        if (rangeIncreaseCounter >= episodesTillRangeIncrement)
        {
            if (range < maxRange)
            {
                range += rangeIncrement;
            }
        }
    }

    public void ResetAgentPosition()
    {
        float tempRange = range;
        int failedCheckCounter = 0;

        for (int i = 0; i < numChecks; i++)
        {
            Vector2 possiblePosition = new Vector2(transform.position.x + Random.Range(-tempRange, tempRange), transform.position.y + Random.Range(-tempRange, tempRange));

            Vector2 transformedPosition = new Vector2(transform.parent.position.x - possiblePosition.x, transform.parent.position.y - possiblePosition.y);

            if (transformedPosition.x < maxRange &&
                transformedPosition.y < maxRange &&
                transformedPosition.x > -maxRange &&
                transformedPosition.y > -maxRange)
            {
                if (!Physics2D.OverlapBox(possiblePosition, new Vector2(1, 1), 0f))
                {
                    transform.position = possiblePosition;
                    break;
                }
            }
            else
            {
                failedCheckCounter += 1;

                if (failedCheckCounter >= checksTillIncreaseTempRange)
                {
                    failedCheckCounter = 0;
                    tempRange += tempRangeIncrease;
                }
            }

        }
    }

    public void ResetAgentAwayTargetPosition()
    {
        float tempRange = range;
        int failedCheckCounter = 0;

        for (int i = 0; i < numChecks; i++)
        {
            Vector2 possiblePosition = new Vector2(transform.position.x + Random.Range(-tempRange, tempRange), transform.position.y + Random.Range(-tempRange, tempRange));

            Vector2 transformedPosition = new Vector2(transform.parent.position.x - possiblePosition.x, transform.parent.position.y - possiblePosition.y);
            Vector2 transformedTargetPosition = new Vector2(possiblePosition.x - targetTransform.position.x, possiblePosition.y - targetTransform.position.y);

            if ((transformedPosition.x < maxRange &&
                transformedPosition.y < maxRange &&
                transformedPosition.x > -maxRange &&
                transformedPosition.y > -maxRange) &&
                (transformedTargetPosition.x > targetRange ||
                transformedTargetPosition.y > targetRange ||
                transformedTargetPosition.x < -targetRange ||
                transformedTargetPosition.y < -targetRange))
            {
                if (!Physics2D.OverlapBox(possiblePosition, new Vector2(1, 1), 0f))
                {
                    transform.position = possiblePosition;
                    break;
                }
            }
            else
            {
                failedCheckCounter += 1;

                if (failedCheckCounter >= checksTillIncreaseTempRange)
                {
                    failedCheckCounter = 0;
                    tempRange += tempRangeIncrease;
                }
            }

        }
    }

    public void ResetTargetPositionNearSeeker()
    {
        Vector2 seekerPosition = seeker.transform.position;
        float tempRange = range;
        int failedCheckCounter = 0;

        for (int i = 0; i < numChecks; i++)
        {
            Vector2 possiblePosition = new Vector2(seekerPosition.x + Random.Range(-tempRange, tempRange), seekerPosition.y + Random.Range(-tempRange, tempRange));

            Vector2 transformedPosition = new Vector2(transform.parent.position.x - possiblePosition.x, transform.parent.position.y - possiblePosition.y);

            if (transformedPosition.x < maxRange &&
                transformedPosition.y < maxRange &&
                transformedPosition.x > -maxRange &&
                transformedPosition.y > -maxRange)
            {
                if (!Physics2D.OverlapBox(possiblePosition, new Vector2(1, 1), 0f))
                {
                    targetTransform.position = possiblePosition;
                    break;
                }
            }
            else
            {
                failedCheckCounter += 1;

                if (failedCheckCounter >= checksTillIncreaseTempRange)
                {
                    failedCheckCounter = 0;
                    tempRange += tempRangeIncrease;
                }
            }

        }
    }

    public void ResetTargetPosition()
    {
        float tempRange = range;
        int failedCheckCounter = 0;

        for (int i = 0; i < numChecks; i++)
        {
            Vector2 possiblePosition = new Vector2(transform.position.x + Random.Range(-tempRange, tempRange), transform.position.y + Random.Range(-tempRange, tempRange));

            Vector2 transformedPosition = new Vector2(transform.parent.position.x - possiblePosition.x, transform.parent.position.y - possiblePosition.y);

            if (transformedPosition.x < maxRange &&
                transformedPosition.y < maxRange &&
                transformedPosition.x > -maxRange &&
                transformedPosition.y > -maxRange)
            {
                if (!Physics2D.OverlapBox(possiblePosition, new Vector2(1, 1), 0f))
                {
                    targetTransform.position = possiblePosition;
                    break;
                }
            }
            else
            {
                failedCheckCounter += 1;

                if (failedCheckCounter >= checksTillIncreaseTempRange)
                {
                    failedCheckCounter = 0;
                    tempRange += tempRangeIncrease;
                }
            }

        }
    }
}
