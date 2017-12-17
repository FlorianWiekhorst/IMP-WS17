﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


	public class PlayerMovement : MonoBehaviour {


        private float horizontalMovement;
        public float moveSpeed = 10;
        
		bool facingRight = true;							// For determining which way the player is currently facing.
		public VirtualJoystick moveJoystick;
		

		[Range(0, 1)]

		bool canMove = true;								// To disable Player Movement
		Animator anim;										// Reference to the player's animator component.
		Rigidbody2D myRigidbody;

		// GameMenu
		//bool inGameMenu = false;							// sets GameMenu by Default to off.
		bool dead = false;	

		//Sound Array
		public AudioClip[] audioclip;						// Creates an Array to store my GameSounds

		[SerializeField] int lifes = 5;						// Number of Lifes left
		[SerializeField] int cells = 0;						// Number of Cells collected



		void Start()
		{
			anim = GetComponent<Animator>();
			myRigidbody = GetComponent<Rigidbody2D> ();
		}

		void FixedUpdate()
		{
			
			if (lifes < 0.5) //The Death
			{
				GetComponent<Rigidbody2D>().velocity = (new Vector2 (0f, 0f));
				Debug.Log ("YOU ARE DEAD");
				canMove = false;
				anim.SetBool ("Dead", true);
				Playsound(3);
				Invoke ("freeze", 7);
			}
		}

		void freeze()
		{
			Time.timeScale = 0f;
		}

		//Enter a Trigger
		void OnTriggerEnter2D(Collider2D other) 
		{
			if (other.tag == "Cell") 
			{
				Playsound(0);
				Destroy(other.gameObject);
				cells++;
			}

			if (other.gameObject.tag == "Hurt") 
			{
				anim.SetBool ("Hurt", true);
				lifes--;
				Playsound(1);
				if(lifes>0)
				{
					GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 50f));
				}
			}
		}

		//Exit a Trigger     
		public void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.tag == "Hurt") 
			{
				anim.SetBool ("Hurt", false);
			}
		}


		public void Move(float move)
		{

			if(canMove)
			{

				horizontalMovement = Input.GetAxis ("Horizontal");
				
				//Move the Player
			    myRigidbody.velocity = new Vector2(horizontalMovement * moveSpeed, myRigidbody.velocity.y);
			    anim.SetFloat ("Speed", horizontalMovement);

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    anim.SetInteger("Direction", 0);
                    
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    anim.SetInteger("Direction", 1);
                   
                }
            }
		}
		//  SOUND
		void Playsound(int clip)
		{
			GetComponent<AudioSource>().clip = audioclip [clip];
			GetComponent<AudioSource>().Play ();
		}


		void OnGUI()
		{
			GUILayout.Label( "Energiezellen = " + cells );
			GUILayout.Label( "Leben = " + lifes );

			if (lifes < 0 || dead) 
			{
				GUILayout.Label("YOU ARE DEAD !!");
			}
		}
	}




