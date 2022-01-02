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
    float shotCounter; // счЄтчик выстрелов, через него, видимо, считать будем врем€ до сл. выстрела
    [SerializeField] float minTimeBetweenShots = 0.2f; // дл€ рандомизации стрельбы
    [SerializeField] float maxTimeBetweenShots = 3f; // тож дл€ рандом стрельбы
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Aiming on the player")]
    [SerializeField] bool isSearchingForPlayer = false;
    Transform playerCoordinates; // тут наш летающий танк будет хранить параметр координат игрока
    [SerializeField] float rotationSpeed = 360f;  // скорость поворота "танка" при наведении на игрока
    GameObject playerCaptured; // захват игрока в "систему наведени€" - чтоб "танк" поворачивалс€ на него

   /* [Header("Enemy VFX and SFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1;
    [SerializeField] AudioClip deathSound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f; // [Range(0, 1)]  - бегунок от 0 до 1 - так проще настроить звук
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

        CountDownAndShoot(); // веди отсчЄт и стрел€й. по факту: перезапуск счЄтчика и стрельба
    }


    private void TurnTowardsPlayer()
    {
        Vector3 dir = playerCoordinates.position - transform.position; // это вектор направлени€ на игрока
        dir.Normalize(); // нормализ-й вект. - тоже направление, длина вект. = 1
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90; // угол поворота получен этой формулой 


        Quaternion desiredRotation = Quaternion.Euler(0, 0, zAngle); // перевод угла в кватернион
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime); //наведи курсор на RotateTowards - всЄ очевидно и просто

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
        shotCounter -= Time.deltaTime; // отсчЄт до выстрелов FRAME RATE INDEPENDENT благодар€ исп-ю Time.deltaTime
                                       // по сути это: shotCounter = shotCounter - ¬–≈ћякот.«анимает¬ыполнениеќдного адра кажд. кадр.  огда shotCounter = 0 происх. выстрел
        if (shotCounter <= 0f) // shotCounter дошЄл до нул€ тогда делай выстрел
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


            GameObject shot1 = Instantiate(projectile,     // создана перем. равна€ инстанцированному префабу ракеты
                       transform.position + offset1,     // по€вл€етс€ в позиции игрока
                       transform.rotation) as GameObject;     // €вл€етс€ GameObject'ом


            //laser.GetComponent().velocity = new Vector2(0, -projectileSpeed);     // отриц скорость снар€да дабы 
            // летели вниз, туда где игрок    Quaternion.AngleAxis(180,new Vector3(0, 0, 1) - чтобы развернуть на 180 по оси Z


          //  AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);
        }
        else
        {
            GameObject laser = Instantiate(projectile,     // создана перем. равна€ инстанцированному префабу лазера
                   transform.position,     // по€вл€етс€ в позиции игрока
                   Quaternion.identity) as GameObject;     // €вл€етс€ GameObject'ом
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);     // отриц скорость снар€да дабы 
                                                                                  // летели вниз, туда где игрок


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
        // имеетс€ ввиду: если переменна€ damageDealer пуста€, то прекрати выполнение метода сразу же на этой строке - return
        // т.о. при столкновении с объектом не имеющим DamageDealer.cs не выскочит NullReferenceException т.к. до выполнени€
        // строки ProcessHit(damageDealer) программа не дойдЄт. „еловеческим €зыком: на данное столкновение не будет видимой реакции.
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
