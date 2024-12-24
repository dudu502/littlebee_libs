using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

namespace RVO
{
    public class RVOObstacle
    {
        public List<Vector2> obstacleBorders;
        public BoxCollider collider;
    }
    public class ObstacleCollect 
    {
        public static List<RVOObstacle> Collect(BoxCollider[] colliders)
        {
            List<RVOObstacle> result = new List<RVOObstacle>();
            for (int i = 0; i < colliders.Length; i++)
            {
                BoxCollider boxCollider = colliders[i];
                FP minX = boxCollider.transform.position.x -
                             boxCollider.size.x * boxCollider.transform.lossyScale.x * 0.5f;
                FP minZ = boxCollider.transform.position.z -
                             boxCollider.size.z * boxCollider.transform.lossyScale.z * 0.5f;
                FP maxX = boxCollider.transform.position.x +
                             boxCollider.size.x * boxCollider.transform.lossyScale.x * 0.5f;
                FP maxZ = boxCollider.transform.position.z +
                             boxCollider.size.z * boxCollider.transform.lossyScale.z * 0.5f;

                List<Vector2> obstacle = new List<Vector2>();
                obstacle.Add(new Vector2(maxX, maxZ));
                obstacle.Add(new Vector2(minX, maxZ));
                obstacle.Add(new Vector2(minX, minZ));
                obstacle.Add(new Vector2(maxX, minZ));
                result.Add(new RVOObstacle() { obstacleBorders = obstacle, collider = boxCollider});
            }
            return result;
        }
    }
}