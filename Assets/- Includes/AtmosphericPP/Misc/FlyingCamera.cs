using UnityEngine;
using System.Collections;


public class FlyingCamera : MonoBehaviour {
	
	public float mainSpeed = 40.0f; //regular speed

	public float acceleration = 0.3f;
	public float deceleration = 0.9f;

	public float accelerationShift = 2.0f;

	public float Rotation_Smooth = 0.3f;

	public float camSens = 0.25f; //How sensitive it with mouse
	
	private Vector3 lastMouse = new Vector3(255, 255, 255); 

	private Vector3 lastDeltaP; 
	private float lastDeltaT; 
	private Vector3 prev_rotate; 

	void Start(){
		lastMouse = Input.mousePosition;
	}

	void  LateUpdate (){

		float dt = Time.deltaTime;

		if (Input.GetMouseButton(1) ){ 

			if (Input.GetMouseButtonDown(1))
				lastMouse = Input.mousePosition;

			float rot = 0;
			if ( Input.GetKey(KeyCode.E) ){ 
				rot = -10.0f*dt;
			}
			if ( Input.GetKey(KeyCode.Q) ){ 
				rot = 10.0f*dt;
			}

		    lastMouse = Vector3.Lerp(prev_rotate, Input.mousePosition - lastMouse, Rotation_Smooth ); 
		
			prev_rotate = lastMouse;
		
		    lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 ); 
		
			lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, transform.eulerAngles.z + rot); 
		
			transform.eulerAngles = lastMouse;

			float speedChange = Input.GetAxis("Mouse ScrollWheel");

			if ( speedChange != 0){ 
				mainSpeed += speedChange*mainSpeed*0.1f;
				mainSpeed = Mathf.Max(mainSpeed, 1.0f);
			}
		}


		lastMouse =  Input.mousePosition;
		
	    //Mouse & camera angle done.  
	
	    //Keyboard commands

		Vector3 baseInput = GetBaseInput(); 
		float accel = acceleration;

		if (Input.GetKey (KeyCode.LeftShift))
		{ 
			accel *= accelerationShift;
		}

		Vector3 desiredVelocity = baseInput*mainSpeed*dt;

		Vector3 deltaPosition = Vector3.Lerp( lastDeltaP, desiredVelocity, ( (baseInput.sqrMagnitude == 0)?deceleration:accel )*dt );

		lastDeltaP = deltaPosition;
		lastDeltaT = dt;

		transform.Translate( deltaPosition ); 
	}
	
	 
	private Vector3 GetBaseInput (){ //returns the basic values, if it's 0 than it's not active.
	
	    Vector3 p_Velocity = new Vector3();
	
	    if (Input.GetKey (KeyCode.W)){
	
	        p_Velocity += new Vector3(0, 0 , 1);
	
	    }
	
	    if (Input.GetKey (KeyCode.S)){
	
	        p_Velocity += new Vector3(0, 0 , -1);
	
	    }
	
	    if (Input.GetKey (KeyCode.A)){
	
	        p_Velocity += new Vector3(-1, 0 , 0);
	
	    }
	
	    if (Input.GetKey (KeyCode.D)){
	
	        p_Velocity += new Vector3(1, 0 , 0);
	
	    }
		if (Input.GetKey(KeyCode.Space) /*|| Input.GetKey(KeyCode.E)*/){ 
	        
	        //Move UP
	        p_Velocity += new Vector3(0, 1 , 0);
	
	    }
	    
	    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C) /*|| Input.GetKey(KeyCode.Q)*/){ 
	    
	        //Move DOWN
	        p_Velocity += new Vector3(0, -1 , 0);
	    }
	
	    return p_Velocity;	
	}

}
