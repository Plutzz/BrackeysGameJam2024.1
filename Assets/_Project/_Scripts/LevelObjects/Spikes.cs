using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private bool spikesVisible;
    [SerializeField] private GameObject spikes;
    [SerializeField] private float activeSpikesTime;
    [SerializeField] private float deactiveSpikesTime;
    private float currentTimer;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        currentTimer = spikesVisible ? activeSpikesTime : deactiveSpikesTime;
        //spikes.SetActive(spikesVisible);
        animator.SetBool("areSpikesActive", spikesVisible);
    }
    void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer < 0)
        {
            TriggerSpikes();
        }
    }

    private void TriggerSpikes() {
        spikesVisible = !spikesVisible;
        animator.SetBool("areSpikesActive", spikesVisible);
        currentTimer = spikesVisible ? activeSpikesTime : deactiveSpikesTime;
    }

    
}
