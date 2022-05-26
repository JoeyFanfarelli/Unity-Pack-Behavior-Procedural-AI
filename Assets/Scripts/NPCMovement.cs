using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;          

public class NPCMovement : MonoBehaviour
{

    [SerializeField]
    private NavMeshAgent agent; 
    [SerializeField]
    private GameObject player;   

    RaycastHit hitInfo;         

  
    static int packSize = 0;        //current size of the pack
    static bool isPack = false;     //Does a pack exist?
    private bool inPack = false;    //Is this gameObject a member of the pack?
    

 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  

    }

   
    void Update()
    {
        NPCWander();        //wander script. agents have randomized destinations and randomized wait time after arriving but before setting new destination
        CheckCollisions();  //checks to see if any players or other agents are in the vicinity. 
    }

    private void CheckCollisions()
    {
        Collider[] otherObjectsInRadius = Physics.OverlapSphere(transform.position, 6); //checks to find all colliders in a sphere that originates at transform.position, and has a radius of 6. Puts them into an array.
        
        foreach (var hitCollider in otherObjectsInRadius)   //loops through all colliders that were found in the sphere.
        {

            if (hitCollider.gameObject.name == "Player")  //If the player is in the the agent's sphere
            {
                if (Physics.Raycast(transform.position, (hitCollider.gameObject.transform.position - transform.position), out hitInfo, 7))  //send out a raycast to see if there's anything between this object and the player.
                {
                    if (hitInfo.collider.gameObject.name == "Player")
                    {
                        if (packSize < 2)       //If alone, or pack is too small to take on the player.
                        {
                            Debug.Log("Fleeing");
                            Flee();             //run away
                        }
                        else if (packSize > 3)  //If pack is big enough to take on the player.
                        {
                            Debug.Log("Chasing");
                            Chase();            //chase the player
                        }
                        else                    //If pack is not small enough to run, but not big enough to chase.
                        {
                            Debug.Log("Maintaining"); //Do Nothing.

                        }

                    }
                }
                

            }
            else if (hitCollider.gameObject.tag == "NPC")         //if another NPC is in the agent's sphere
            {
                if (Physics.Raycast(transform.position, (hitCollider.gameObject.transform.position - transform.position), out hitInfo, 7))      //make sure there's nothing between this object and the NPC
                {
                    if (hitInfo.collider.gameObject.tag == "NPC")
                    {
                        if (isPack == true)     //If a pack exists, the other NPC is in a pack, and this object is not in a pack, it is added to the pack.
                        {
                            if (hitInfo.collider.GetComponent<NPCMovement>().inPack == true)    
                            {
                                if (inPack == false)            
                                {
                                    inPack = true;              
                                    packSize++;                 
                                    Debug.Log(packSize);
                                    gameObject.GetComponent<Renderer>().material.color = Color.blue;    
                                }
                            }
                        }
                        else if (isPack == false)       //Handles formation of a pack. Blue is used to visually identify which NPCs are part of a pack.
                        {
                            Debug.Log("forming pack");
                            inPack = true;          
                            packSize++;             
                            Debug.Log(packSize);
                            gameObject.GetComponent<Renderer>().material.color = Color.blue;    
                            isPack = true;          
                        }

                    }
                }

            }
        }

    }

    //NPC runs away from the player. Runs in the exact opposite direction.
    void Flee()
    {
          Vector3 posDiff = transform.position - player.transform.position;   //Calculates the difference in position between player and NPC
          Vector3 destination = transform.position + posDiff; //Creates a new destination, based on the difference in position. 

          agent.SetDestination(destination);  //Sets the new destination for the NPC, and actually makes it run away.
 
    }

    //Chasing the player
    void Chase()
    {
        agent.SetDestination(player.transform.position); 
    }

    //Wandering behavior, while agents do not have a set destination (i.e., not fleeing or chasing).
    void NPCWander()
    {
        if (!agent.pathPending && !agent.hasPath)  
        {

                if (Random.Range(0, 500) == 1)  //Causes the NPC to pause in between new path generations. Makes the behavior feel more natural. A coroutine could also be used.
                {
                    var randomNPCDestination = Random.insideUnitSphere * 12;    
                    agent.SetDestination(randomNPCDestination);                 
                }


        }

    }

}
