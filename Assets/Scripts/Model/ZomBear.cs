using UnityEngine;
using System.Collections;

namespace Enemy{

	public class ZomBear {

		Rigidbody rigidbody;
		CapsuleCollider capsuleCollider;
		SphereCollider sphereCollider;
		//NavMeshAgent navMeshAgent;
		AudioSource audioSource;

		GameObject enemy;

		public ZomBear(GameObject gameObj){
			//rigidbody = new Rigidbody ();
			//capsuleCollider = new CapsuleCollider ();
			//sphereCollider = new SphereCollider ();
			//navMeshAgent = new NavMeshAgent ();
			//audioSource = new AudioSource();

			enemy = gameObj;
 			
			//enemy.transform.position.Set (0f,10f,0f);

			rigidbody = enemy.GetComponent<Rigidbody>();
			if (rigidbody == null) {
				rigidbody = enemy.AddComponent<Rigidbody> ();			
			}
			InitRigidbody ();

			capsuleCollider = enemy.GetComponent<CapsuleCollider>();
			if (capsuleCollider == null) {
				capsuleCollider = enemy.AddComponent<CapsuleCollider> ();			
			}
			InitCapsuleCollider ();

			sphereCollider = enemy.GetComponent<SphereCollider>();
			if (sphereCollider == null) {
				sphereCollider = enemy.AddComponent<SphereCollider> ();			
			}
			InitSphereCollider ();

			audioSource = enemy.GetComponent<AudioSource>();
			if (audioSource == null) {
				audioSource = enemy.AddComponent<AudioSource> ();
			}
			InitAudioSource ();

			InitScript ();
		}

		public void InitRigidbody(){
			rigidbody.mass = 1;
			rigidbody.useGravity = true;
			rigidbody.isKinematic = false;
		}

		public void InitCapsuleCollider(){
			capsuleCollider.isTrigger = false;
			capsuleCollider.center = new Vector3(0.0f,0.8f,0.0f);
			capsuleCollider.radius = 0.5f;
			capsuleCollider.height = 1.5f;
			capsuleCollider.direction = 1;
		}

		public void InitSphereCollider(){
			sphereCollider.isTrigger = true;
			sphereCollider.center = new Vector3(0.0f,0.8f,0.0f);
			sphereCollider.radius = 0.8f;
		}

		public void InitNavMeshAgent(){
			
		}

		public void InitAudioSource(){
			audioSource.clip.name = "ZomBear Hurt";
            float[] samples = new float[audioSource.clip.samples * audioSource.clip.channels];
			audioSource.clip.SetData (samples,0);
		}

		public void InitScript()
		{
			EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth> ();
			if(enemyHealth == null){
				enemyHealth = enemy.AddComponent<EnemyHealth> ();
			}
			enemyHealth.startingHealth = 100;
			enemyHealth.sinkSpeed = 2.5f;
			enemyHealth.scoreValue = 10;
			enemyHealth.isDead = true;

			EnemyAttack enemyAttack = enemy.GetComponent<EnemyAttack> ();
			if(enemyAttack == null)
			{
				enemyAttack = enemy.AddComponent<EnemyAttack> ();
			}
			enemyAttack.timeBetweenAttacks = 0.5f;
			enemyAttack.attackDamage = 15;

			EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement> ();
			if(enemyMovement == null)
			{
				enemyMovement = enemy.AddComponent<EnemyMovement> ();
			}
			enemyMovement.movingSpeed = 3f;
		}
			
	}
		
}
