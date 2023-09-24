using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource music, birds;

    float timeTillNextBird = 6f;
    float intensity = 0;

    StatManager stats;

    public List<AudioClip> possibleBirds = new List<AudioClip>();

    // Start is called before the first frame update
    void Start() {
        birds.clip = possibleBirds[Random.Range(0, possibleBirds.Count)];
    }

    // Update is called once per frame
    void Update() {
        if(!stats) {
            stats = FindObjectOfType<StatManager>();
        }

        intensity = stats.forestsDestroyed / 150f;
        intensity = Mathf.Min(intensity, 1);

        if(intensity < 0.9f) {
            timeTillNextBird -= Time.deltaTime;
            if(timeTillNextBird < 0) {
                timeTillNextBird = Random.Range(5f + intensity * 30, 10f + intensity * 30);
                birds.Play();
                birds.clip = possibleBirds[Random.Range(0, possibleBirds.Count)];
            }
        }
    }
}
