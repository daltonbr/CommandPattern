using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem
{
    
    public class AnimTask
    {
        public Transform TargetTransform;
        public Vector3 TargetPosition;
        public float Speed;
    }

    public bool IsAnimating => m_Task.Count > 0;

    private List<AnimTask> m_Task = new List<AnimTask>();

    public void NewAnim(Transform targetTranform, Vector3 target, float speed)
    {
        //check if we already have a task for that transform, if we do, this replace that task
        for (int i = 0; i < m_Task.Count; ++i)
        {
            if (m_Task[i].TargetTransform == targetTranform)
            {
                m_Task[i].TargetPosition = target;
                m_Task[i].Speed = speed;
                return;
            }
        }
        
        m_Task.Add(new AnimTask()
        {
            TargetPosition = target,
            TargetTransform = targetTranform,
            Speed = speed
        });
    }

    public void Update()
    {
        for (int i = 0; i < m_Task.Count; ++i)
        {
            var task = m_Task[i];
            var t = task.TargetTransform;
            t.position = Vector3.MoveTowards(t.position, task.TargetPosition, task.Speed * Time.deltaTime);

            //Sidenote: == on Vector3 handle floating point impression already! 
            if (t.position == task.TargetPosition)
            {
                m_Task.RemoveAt(i);
                i--;
            }
        }
    }
}
