using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterThings : MonoBehaviour
{
    float speed;
    float range = 50f;
    Rigidbody mnstrBody;
    Vector3 mnstrPosition;
    Vector3 targetPosition;
    GameObject target;
    void Start()
    {
        mnstrBody = transform.GetComponent<Rigidbody>();
        mnstrBody.freezeRotation = true; // Prevents falling over! (Thank you ChatGPT :] ) 
        speed = UnityEngine.Random.Range(6, 36);
        if (speed < 10)
        {
            range += 10;
        }
        int Max = UnityEngine.Random.Range(6, 12);
        for (int mnstrNumb = 0; Max <= mnstrNumb; mnstrNumb++)
        {
            Instantiate(transform);
        }
    }

    bool onGround = true;
    void LateUpdate()
    {

        float jumpForce = 600f;


        Rigidbody mnstrBody = transform.GetComponent<Rigidbody>();
        mnstrPosition = mnstrBody.transform.position;

        if (GameObject.FindWithTag("Player"))
        {
            target = GameObject.FindWithTag("Player");
            targetPosition = target.transform.position;
        }




        Vector3 moveVector = -(mnstrPosition - targetPosition) / 10;
        moveVector.Normalize();

        float moveToX = moveVector.x;
        float moveToZ = moveVector.z;

        if ((targetPosition - mnstrPosition).x <= range || (targetPosition - mnstrPosition).z <= range && GameObject.FindWithTag("Player"))
        {
            mnstrBody.AddForce(moveToX * speed, 0, moveToZ * speed, ForceMode.Impulse);
        }


        if (moveVector.y > 0.33 && onGround)
        {
            mnstrBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            onGround = false;
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            onGround = true;
        }
    }
}
