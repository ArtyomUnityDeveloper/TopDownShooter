//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] int health = 100;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    float shotCounter; // ������� ���������, ����� ����, ������, ������� ����� ����� �� ��. ��������
    [SerializeField] float minTimeBetweenShots = 0.2f; // ��� ������������ ��������
    [SerializeField] float maxTimeBetweenShots = 3f; // ��� ��� ������ ��������
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Aiming on the player")]
    [SerializeField] bool isSearchingForPlayer = false;
    Transform playerCoordinates; // ��� ��� �������� ���� ����� ������� �������� ��������� ������
    [SerializeField] float rotationSpeed = 360f;  // �������� �������� "�����" ��� ��������� �� ������
    GameObject playerCaptured; // ������ ������ � "������� ���������" - ���� "����" ������������� �� ����

   /* [Header("Enemy VFX and SFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f; // [Range(0, 1)]  - ������� �� 0 �� 1 - ��� ����� ��������� ����
    [SerializeField] AudioClip shootSound;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.25f; */


    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        playerCaptured = GameObject.Find("Player");
    }


    void Update()
    {
        if (isSearchingForPlayer)
        {
            SearchForPlayer();
            TurnTowardsPlayer();
        }

        CountDownAndShoot(); // ���� ������ � �������. �� �����: ���������� �������� � ��������
    }


    private void TurnTowardsPlayer()
    {
        Vector3 dir = playerCoordinates.position - transform.position; // ��� ������ ����������� �� ������
        dir.Normalize(); // ��������-� ����. - ���� �����������, ����� ����. = 1
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90; // ���� �������� ������� ���� �������� 


        Quaternion desiredRotation = Quaternion.Euler(0, 0, zAngle); // ������� ���� � ����������
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime); //������ ������ �� RotateTowards - �� �������� � ������

    }


    private void SearchForPlayer()
    {
        if (playerCaptured == null)
        {
            playerCaptured = GameObject.Find("Player");
        }
        else
        {
            playerCoordinates = playerCaptured.transform;
        }
    }


    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime; // ������ �� ��������� FRAME RATE INDEPENDENT ��������� ���-� Time.deltaTime
                                       // �� ���� ���: shotCounter = shotCounter - ��������.����������������������������� ����. ����. ����� shotCounter = 0 ������. �������
        if (shotCounter <= 0f) // shotCounter ����� �� ���� ����� ����� �������
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }


    private void Fire()
    {
        if (isSearchingForPlayer)
        {
            Vector3 offset1 = transform.rotation * new Vector3(0.224f, 0.627f, 0);


            GameObject shot1 = Instantiate(projectile,     // ������� �����. ������ ����������������� ������� ������
                       transform.position + offset1,     // ���������� � ������� ������
                       transform.rotation) as GameObject;     // �������� GameObject'��


            //laser.GetComponent().velocity = new Vector2(0, -projectileSpeed);     // ����� �������� ������� ���� 
            // ������ ����, ���� ��� �����    Quaternion.AngleAxis(180,new Vector3(0, 0, 1) - ����� ���������� �� 180 �� ��� Z


          //  AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        }
        else
        {
            GameObject laser = Instantiate(projectile,     // ������� �����. ������ ����������������� ������� ������
                   transform.position,     // ���������� � ������� ������
                   Quaternion.identity) as GameObject;     // �������� GameObject'��
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);     // ����� �������� ������� ���� 
                                                                                  // ������ ����, ���� ��� �����


           // AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        /* GameObject explosionHit = Instantiate(hitVFX, transform.position, transform.rotation);
        Destroy(explosionHit, durationOfHit);
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position, hitSoundVolume); */


        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        // ������� �����: ���� ���������� damageDealer ������, �� �������� ���������� ������ ����� �� �� ���� ������ - return
        // �.�. ��� ������������ � �������� �� ������� DamageDealer.cs �� �������� NullReferenceException �.�. �� ����������
        // ������ ProcessHit(damageDealer) ��������� �� �����. ������������ ������: �� ������ ������������ �� ����� ������� �������.
        ProcessHit(damageDealer);
    }


    private void ProcessHit(DamageDealer damageDealer)
    {
        //Debug.Log("Process Hit is ON");
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }


    public void HitFromSplash(int damage)
    {
        //Debug.Log("HitFromSplash is triggered");
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);


        //lootManager.LootSpawn();
        //Debug.Log("After LootSpawn line activated");


        Destroy(gameObject);
       /* GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume); */
    }
}
