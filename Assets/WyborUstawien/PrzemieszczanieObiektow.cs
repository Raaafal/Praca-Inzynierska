﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrzemieszczanieObiektow : MonoBehaviour
{

    //[SerializeField]
    float r = 10f;
    //[SerializeField]
    float f = 2f;

    public Vector2 punktCentralny;

    private void Start()
    {
        Physics2D.maxTranslationSpeed = 10f;
        QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = 60;
    }

    private void FixedUpdate()
    {
        float randomMovement = 0.0f;
        var objects = Physics2D.OverlapCircleAll(transform.position, r);
        foreach(var obj in objects)
        {
            if(obj.gameObject!=gameObject&&obj.TryGetComponent<Rigidbody2D>(out var rb))
            {
                var vec = obj.transform.position - transform.position;
                rb.AddForce(vec.normalized*Sila(vec.magnitude)+ (Vector3)new Vector2(Random.Range(-randomMovement, randomMovement), Random.Range(-randomMovement, randomMovement)), ForceMode2D.Force);
            }
        }
        var rig = GetComponent<Rigidbody2D>();
        float centralnaSila = 2.5f;
        rig.AddForce((punktCentralny-(Vector2)transform.position).normalized*centralnaSila, ForceMode2D.Force);


        //dodanie dodadkowej siły dążącej do ustawienia obiektów na prostej x=0 w celu zachowania symetrii
        rig.AddForce(new Vector2(-Mathf.Sign(transform.position.x)*Mathf.Min( 0.1f,Mathf.Pow( Mathf.Abs(0.5f/transform.position.x),3f)),0f), ForceMode2D.Force);
        //rig.AddForce(Vector2.down, ForceMode2D.Force);
    }
    float Sila(float odleglosc)
    {
        return f / odleglosc / odleglosc;
    }
}
