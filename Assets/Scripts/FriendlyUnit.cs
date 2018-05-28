using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyUnit : MonoBehaviour
{
    public bool Selected;
    public float speed;
    public float MaxDistance;
    public float AttackingDistance;
    public float RateOfFire;
    public GameObject Bullet;

    Renderer myRenderer;
    public Material UnselectedMaterial;
    public Material SelectedMaterial;


    Vector3 positionToMoveTo = Vector3.zero;
    bool moving, attacking;
    float fireCoolDown;
    GameObject enemy;

    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        fireCoolDown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (positionToMoveTo != Vector3.zero && Vector3.Distance(transform.position, positionToMoveTo) >= MaxDistance)
            {
                transform.Translate((positionToMoveTo - transform.position).normalized * speed * Time.deltaTime);
            }
        }
        if (attacking && Vector3.Distance(transform.position, positionToMoveTo) > AttackingDistance)
        {
                transform.Translate((positionToMoveTo - transform.position).normalized * speed * Time.deltaTime);
        }

        if (attacking && Vector3.Distance(transform.position, positionToMoveTo) <= AttackingDistance)
        {
            transform.LookAt(positionToMoveTo);

            if (fireCoolDown >= RateOfFire)
            {
                fireCoolDown = 0;
                Instantiate(Bullet, transform.position, transform.rotation);
            }
            if (enemy == null)
            {
                attacking = false;
            }
        fireCoolDown += Time.deltaTime;
        }

    }

    public void Select()
    {
        Selected = true;
        myRenderer.material = SelectedMaterial;
    }

    public void Deselect()
    {
        Selected = false;
        myRenderer.material = UnselectedMaterial;
    }

    public void MoveTo(Vector3 newPosition)
    {
        positionToMoveTo = newPosition;
        moving = true;
    }

    public void Attack(GameObject enemy)
    {
        attacking = true;
        this.enemy = enemy;
        positionToMoveTo = enemy.transform.position;
    }

    //IEnumerator Lerp(Vector3 newPosition)
    //{
    //    float i = 0;
    //    while (i <= 1f)
    //    {
    //        i += Time.deltaTime / Vector3.Distance(newPosition, transform.position);
    //        transform.position = Vector3.Lerp(transform.position, newPosition, i);
    //        yield return null;
    //    }
    //}

}
