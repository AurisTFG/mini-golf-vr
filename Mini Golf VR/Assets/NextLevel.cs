using System.Collections;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public Rigidbody ball;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            System.Console.WriteLine("LYGIS PEREITAS");
        }

        
    }

    
}
