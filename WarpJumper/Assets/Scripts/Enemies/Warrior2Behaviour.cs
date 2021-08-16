using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior2Behaviour : MonoBehaviour
{
    //movement variables
    [SerializeField] float defaultMovementSpeed = 4f;
    [SerializeField] float attack3MovementSpeed = 8f;
    [SerializeField] float movementSpeed = 4f;
    public int direction;

    public GameObject player;

    //where to be relative to player
    public float defaultHoverX = 1f;
    public float defaultHoverY = 6f;
    public float hoverX = 1f;
    public float hoverY = 6f;
    public Vector3 destination = Vector3.zero;

    public float attackInterval = 12f;

    public bool destinationReached = false;

    public GameObject icePick;
    public GameObject iceCastAnim;
    public GameObject icePickDown;

    public GameObject firePoint;
    public GameObject magicGateL;
    public GameObject magicGateR;

    public GameObject batRow;

    //attack related
    public int currentAttack = -1; //0 = homing ice picks, 1 = magic circle, 2 = horizontal
    public int numAttacks = 3; //total number of attacks

    public int dir = 1;

    public bool canMove = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating("changeAttack", 3f, attackInterval);
        magicGateL.SetActive(false);
        magicGateR.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (player != null && canMove)
        {

            //find position of player
            float xDiff = transform.position.x - player.transform.position.x;
            float yDiff = transform.position.y - player.transform.position.y;

            //flip sprite
            if (xDiff <= 0)
            {
                dir = -1;
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                destination = player.transform.position - new Vector3(hoverX, 0f, 0f) + new Vector3(0f, hoverY, 0f);
            }
            else
            {
                dir = 1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                destination = player.transform.position + new Vector3(hoverX, 0f, 0f) + new Vector3(0f, hoverY, 0f);

            }

            //overwrite destination depending on attack
            if (currentAttack == 2) {
                destination.y = -3;

                if (Vector3.Distance(transform.position, destination) < 0.1) {
                    canMove = false;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);


        }

    }

    void changeAttack(){

        currentAttack++;
        if (currentAttack % numAttacks == 0)
        {
            currentAttack = 0;
        }

        //default values for each attack
        magicGateL.SetActive(false);
        magicGateR.SetActive(false);
        hoverX = defaultHoverX;
        hoverY = defaultHoverY;
        movementSpeed = defaultMovementSpeed;
        canMove = true;

        //set destination for attack 1
        if (currentAttack == 0)
        {
            StartCoroutine(spawnIcePicks());
        }
        else if (currentAttack == 1)
        {
            //enable magic gates
            magicGateR.SetActive(true);
            magicGateL.SetActive(true);

            //make boss move
            hoverX = 4f;

            //start making ice picks fall from magic gates
            StartCoroutine(magicGateAttack());
        }
        else if (currentAttack == 2) {
            hoverX = 10f;
            movementSpeed = attack3MovementSpeed;
            Instantiate(batRow);
            StartCoroutine(icePicksHorizontal());
        }

    }

    IEnumerator spawnIcePicks(){
        for (int i = 0; i < 4; i++)
        {
            Instantiate(iceCastAnim, firePoint.transform.position, Quaternion.identity);
            Instantiate(icePick, firePoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator magicGateAttack()
    {
        for (int i = 0; i < 60; i++)
        {
            GameObject pick1 = Instantiate(icePickDown, magicGateL.transform.position, Quaternion.Euler(0, 0, 270)) as GameObject;
            GameObject pick2 = Instantiate(icePickDown, magicGateR.transform.position, Quaternion.Euler(0, 0, 270)) as GameObject;

            pick1.SendMessage("setDestroyTime", 0.5f);
            pick2.SendMessage("setDestroyTime", 0.5f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator icePicksHorizontal()
    {
        yield return new WaitForSeconds(4f);
        Instantiate(iceCastAnim, firePoint.transform.position, Quaternion.identity);

        Quaternion pickDir = Quaternion.Euler(0, 0, 180);

        if (dir == -1)
        {
            pickDir = Quaternion.identity;
        }
        for (int i = 0; i < 40; i++)
        {
            GameObject pick = Instantiate(icePickDown, firePoint.transform.position, pickDir) as GameObject;
            pick.SendMessage("setDestroyTime", 2.5f);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
