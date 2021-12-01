using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [Header("Begibbon")]
    public AudioSource begibbonAttack;
    public AudioSource begibbonDeath;
    [Header("Neanderthrow")]
    public AudioSource neanderthrowAttack;
    public AudioSource neanderthrowDeath;
    [Header("Growrilla")]
    public AudioSource growrillaDeath;
    [Header("Dashtard")]
    public AudioSource dashtardAttack;
    public AudioSource dashtardDeath;

    Dictionary<Type, AudioSource> attacks;
    Dictionary<Type, AudioSource> deaths;

    void Start()
    {
        attacks = new Dictionary<Type, AudioSource>();
        attacks.Add(typeof(Begibbon), begibbonAttack);
        attacks.Add(typeof(Neanderthrow), neanderthrowAttack);
        attacks.Add(typeof(Dashtard), dashtardAttack);

        deaths = new Dictionary<Type, AudioSource>();
        deaths.Add(typeof(Begibbon), begibbonDeath);
        deaths.Add(typeof(Neanderthrow), neanderthrowDeath);
        deaths.Add(typeof(Growrilla), growrillaDeath);
        deaths.Add(typeof(Dashtard), dashtardDeath);
    }


    public void Attack(Enemy enemy)
    {
        attacks[enemy.GetType()].PlayAtPosition(enemy.transform.position);
    }

    public void Death(Enemy enemy)
    {
        deaths[enemy.GetType()].PlayAtPosition(enemy.transform.position);
    }
}
