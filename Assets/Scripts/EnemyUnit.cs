using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour {


    public int hp;
    public Image hpImage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        hp -= damage;
        hpImage.transform.localScale = new Vector3((float)hp / 100, 1,1);
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
