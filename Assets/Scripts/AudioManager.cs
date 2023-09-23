using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource music, birds;

    float timeTillNextBird = 6f;

    public List<AudioClip> possibleBirds = new List<AudioClip>();

    // Start is called before the first frame update
    void Start() {
        birds.clip = possibleBirds[Random.Range(0, 6)];
    }

    // Update is called once per frame
    void Update() {
        timeTillNextBird -= Time.deltaTime;
        if(timeTillNextBird < 0) {
            timeTillNextBird = Random.Range(5f, 10f);
            birds.Play();
            birds.clip = possibleBirds[Random.Range(0, (int) possibleBirds.Count)];
            print("Playing");
        }

    }
}
