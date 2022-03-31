using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBallAnimation : MonoBehaviour
{
    [SerializeField]
    Rigidbody myRB;
    [SerializeField]
    float speed,yOffset;
    [SerializeField]
    bool upwards;
    [SerializeField]
    GameObject splashPrefab;
    [SerializeField]
    SpriteRenderer mySpriteRenderer;
    private float targetY;
    private void Start()
    {
        //if upwards is true, shoot sprite upwards
        if (upwards)
        {
            myRB.AddForce(new Vector3(0, speed, 0),ForceMode.VelocityChange);
            Destroy(this.gameObject, 10f);
        }
    }
    private void FixedUpdate()
    {
        //if targety is reached spawn a splash effect and destroy projectile
        if (!upwards && myRB.transform.position.y <= targetY)
        {
            GameObject.Instantiate(splashPrefab, myRB.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// start shooting a projectile down towards a destination
    /// </summary>
    /// <param name="targetPosition"></param>
    public void ShootDownwardsTowards(Vector3 targetPosition)
    {
        targetY = targetPosition.y+0.5f;
        myRB.transform.position = new Vector3(targetPosition.x, targetPosition.y+yOffset, 0);
        myRB.AddForce(new Vector3(0, -speed, 0), ForceMode.VelocityChange);

    }
}
