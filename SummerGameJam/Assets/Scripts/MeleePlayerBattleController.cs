﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MeleePlayerBattleController : MonoBehaviour
{
    public int speed = 6;
    public int moved = 0;
    public int dmg   = 10;
    public int hp    = 100;
    public bool isTurn = true;
    public bool isEnemyTurn = false;
    public bool attacked = false;

    public bool enemyAdj = false;

    public Text moveText;
    public Text alert;
    public Text hpText;

    public List<string> enemyDirections = new List<string>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> adjEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        moveText = GameObject.FindGameObjectWithTag("Moves");
        isTurn = true;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("enemy"))
        {
            enemies.Add(obj);
        }
        alert.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetectObjects();
        if (isTurn)
        {
            if (moved < speed)
                Movement();
            
            if (!attacked)
                Attack();
            UpdateUI();
            if (Input.GetKeyDown(KeyCode.Return))
                StartCoroutine(EndTurn());
        }
        if (enemies.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    
    IEnumerator EndTurn()
    {
        isTurn = false;
        isEnemyTurn = true;
        yield return new WaitUntil(() => !isEnemyTurn);
        moved = 0;
        attacked = false;
        isTurn = true;
    }

    void Movement()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!enemyDirections.Contains("L"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!enemyDirections.Contains("R"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!enemyDirections.Contains("U"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1);
                moved++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!enemyDirections.Contains("D"))
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1);
                moved++;
            }
        }
    }

    void UpdateUI()
    {
        moveText.text = "Movement left: " + (speed - moved);
        alert.enabled = enemyAdj;
        hpText.text = "HP: " + hp;
    }

    public void DetectObjects()
    {
        foreach (GameObject e in enemies)
        {
            if (e == null)
                enemies.Remove(e);

            if ((e.transform.position.x + 1 == gameObject.transform.position.x || e.transform.position.x - 1 == gameObject.transform.position.x) && (e.transform.position.y == gameObject.transform.position.y))
            {
                enemyAdj = true;
                
            }
            else if (e.transform.position.x == gameObject.transform.position.x && (e.transform.position.y + 1 == gameObject.transform.position.y || e.transform.position.y - 1 == gameObject.transform.position.y))
            {
                enemyAdj = true;
            }
            else
            {
                enemyAdj = false;
            }
            if (enemyAdj)
            {
                if (!adjEnemies.Contains(e))
                    adjEnemies.Add(e);

                if (gameObject.transform.position.y > e.transform.position.y)
                {
                    if (!enemyDirections.Contains("D"))
                        enemyDirections.Add("D");
                }
                else if (enemyDirections.Contains("D"))
                {
                    enemyDirections.Remove("D");
                }

                if (gameObject.transform.position.y < e.transform.position.y)
                {
                    if (!enemyDirections.Contains("U"))
                        enemyDirections.Add("U");
                }
                else if (enemyDirections.Contains("U"))
                {
                    enemyDirections.Remove("U");
                }

                if (gameObject.transform.position.x > e.transform.position.x)
                {
                    if (!enemyDirections.Contains("L"))
                        enemyDirections.Add("L");
                }
                else if (enemyDirections.Contains("L"))
                {
                    enemyDirections.Remove("L");
                }

                if (gameObject.transform.position.x < e.transform.position.x)
                {
                    if (!enemyDirections.Contains("R"))
                        enemyDirections.Add("R");
                }
                else if (enemyDirections.Contains("R"))
                {
                    enemyDirections.Remove("R");
                }
            }
            
        }
        if (!enemyAdj)
        {
            enemyDirections.Clear();
            adjEnemies.Clear();
        }

                
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (enemyAdj)
            {
                foreach (GameObject e in adjEnemies)
                {
                    e.GetComponent<EnemyBattleScript>().health -= dmg;
                    attacked = true;
                    Debug.Log(e.GetComponent<EnemyBattleScript>().health);
                }
                
            }
        }
    }
}
