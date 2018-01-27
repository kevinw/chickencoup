using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ChickenCoup
{
    public class ChickenProperties : MonoBehaviour {

        public int Points { get; private set;}
        public int Speed { get; private set;}
        public int Loudness { get; private set;}
        public int GrumpFactor { get; private set;}
        public int Weight { get; private set;}
        public int CalmFactor { get; private set;}
        public float SizeFactor { get; private set;}
        void Start()
        {
            //on start, assign random properties to this chicken
            //update mesh accordingly
            GenerateChicken();
        }
        
        void GenerateChicken()
        {
            transform.localScale = new Vector3(1,1,1);
            Speed = Random.Range(0,10);
            Loudness = Random.Range(0,10);
            GrumpFactor = Random.Range(0,10);
            Weight = Random.Range(0,10);
            CalmFactor = Random.Range(0,10);
            Points = Loudness * 2 + GrumpFactor * 3 + Weight - CalmFactor - Speed * 2;
            if(Points < 5){Points = 5;}
            if(Points >= 50)
            {
                transform.localScale += new Vector3(1,1,1);
            }
            else if(Points >= 30)
            {
                transform.localScale += new Vector3(.5f,.5f,.5f);
            }
            else if(Points >= 10)
            {
                transform.localScale += new Vector3(.2f,.2f,.2f);
            }
        }
    }
}