using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;

public class test2 : MonoBehaviour
{
    float timeStart;
    [SerializeField] bool useJob;


    private void Update()
    {
        
        timeStart = Time.realtimeSinceStartup;

        if (useJob)
        {
            NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(Allocator.Temp);
            for (int j = 0; j < 10; j++)
            {
                JobHandle jobHandle = Job();
                jobHandles.Add(jobHandle);
            }
            JobHandle.CompleteAll(jobHandles);
            jobHandles.Dispose();
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                Task();
            }
        }
        Debug.Log((Time.realtimeSinceStartup - timeStart) * 1000f + "ms");
    }

    private void Task()
    {
        float value = 0f;
        for (int i = 0; i < 80000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }

    private JobHandle Job()
    {
        Job job = new Job();
        return job.Schedule();
    }
}

[BurstCompile]
public struct Job : IJob
{
    public void Execute()
    {
        float value = 0f;
        for (int i = 0; i < 60000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}


