using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesController : MonoBehaviour
{

    public Rigidbody rb;
    public float threshold = 4.0f; 
    
    private ParticleSystem particleSystem;

    void Start() {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (rb.velocity.magnitude <= 1) {
            particleSystem.Stop();
            return;
        }

        particleSystem.Play();

        var main = particleSystem.main;
        var emission = particleSystem.emission;

        float magnitude = new Vector2(rb.velocity.x, rb.velocity.y).magnitude;

        //smooth transition between values
        float startSpeed = Mathf.Lerp((float)main.startSpeed.constant, Mathf.Log(magnitude * 2) * 5, Time.deltaTime * 3);
        float rateOverTime = Mathf.Lerp((float)emission.rateOverTime.constant, magnitude * 5, Time.deltaTime * 3);

        main.startSpeed = startSpeed;
        emission.rateOverTime = rateOverTime;
        

    }
}
