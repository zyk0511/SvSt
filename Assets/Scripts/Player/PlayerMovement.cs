using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6.0f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100.0f;
	AnimatorStateInfo animatorStateInfo;

	void Awake(){
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();

		animatorStateInfo = anim.GetCurrentAnimatorStateInfo (anim.GetLayerIndex ("Base Layer"));
	}

	void FixedUpdate(){
		float h = Input.GetAxisRaw ("Horizontal");

		float v = Input.GetAxisRaw ("Vertical");

		Move (h,v);
		Turning ();
		Animating (h, v);

		//GameObject cameraObj = GameObject.FindWithTag("MainCamera");
		//print(cameraObj.camera.ScreenPointToRay(Input.mousePosition).direction);
	}

	public void Move(float h,float v){
        movement.Set(h, 0.0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning(){
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;

		if(Physics.Raycast(camRay,out floorHit,camRayLength,floorMask)){
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0.0f;
			
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}

	void Animating(float h,float v){
		bool walking = h != 0.0f || v != 0.0f;

		if (!animatorStateInfo.IsName ("Base Layer.Move") && !animatorStateInfo.IsName ("Base Layer.Idle"))
			return;

		//anim.SetBool ("IsWalking",walking);
		anim.SetBool ("Run",walking);
	}
}