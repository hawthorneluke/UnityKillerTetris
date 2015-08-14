using UnityEngine;
using System.Collections;

public class FaceType : MonoBehaviour {

	private float counter;
	private float nextTime;

	protected enum State {BeforeStart, Idle, Ready, Activated};

	protected BodyComponent bodyComponent;
	protected int side;
	protected State state;
	protected GameController gameController;
	protected GameObject player;
	protected Renderer renderer;
	protected Material defaultMaterial;
	protected float onHitTimer;

	public float hp;
	public int points; //given after being destroyed
	public float minTimeBeforeReady;
	public float maxTimeBeforeReady;
	public float timeBetweenReadyAndActivate;
	public float timeBetweenActivateAndIdle;
	public Material onHitMaterial;
	public float onHitRenderTime = 0.1f;


	void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		player = GameObject.FindGameObjectWithTag ("Player");

		renderer = this.GetComponent<Renderer>();
		defaultMaterial = renderer.material;

		state = State.BeforeStart;
	}

	protected void Update() {
		if (state != State.BeforeStart) {
			counter += Time.deltaTime;
			if (counter >= nextTime) {
				counter = 0;

				switch(state) {
				case State.Idle:
					OnReadyActivation();
					break;
				case State.Ready:
					OnActivate();
					break;
				case State.Activated:
					OnActivateEnd();
					break;
				}
			}
		}

		if (onHitTimer > 0.0f) {
			onHitTimer -= Time.deltaTime;
			if (onHitTimer <= 0.0f) {
				renderer.material = defaultMaterial;
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "PlayerBullet") {
			PlayerBullet pb = collision.gameObject.GetComponent<PlayerBullet>();
			OnHit(pb.GetDamage());
			pb.OnHit();
		}
	}

	public void Init(BodyComponent bodyComponent, int side) {
		this.bodyComponent = bodyComponent;
		this.side = side;

		//this.transform.position = this.bodyComponent.transform.position;
		this.transform.parent = this.bodyComponent.transform;
		this.transform.localPosition = Vector3.zero;

		Vector3 pos = this.transform.position;
		//float scale = this.bodyComponent.GetScale ();
		//float offset = scale / 2;
		float offset = this.bodyComponent.transform.localScale.x / 2.0f;

		switch (this.side) {
		case 0:
			pos.y -= offset;
			break;
		case 5:
			pos.y += offset;
			break;
		case 1:
			pos.x -= offset;
			break;
		case 4:
			pos.x += offset;
			break;
		case 2:
			pos.z -= offset;
			break;
		case 3:
			pos.z += offset;
			break;
		}

		this.transform.position = pos;
	}

	public BodyComponent GetBodyComponent() {
		return bodyComponent;
	}

	public int GetSide() {
		return side;
	}

	public void OnFightStart() {
		state = State.Idle;
		SetNextActivateTime ();
	}

	public void OnHit(float damage) {
		renderer.material = onHitMaterial;
		onHitTimer = onHitRenderTime;

		hp -= damage;
		if (hp <= 0) {
			Destroy ();
		}
	}

	public void Destroy() {
		Explosive explosive = GetComponent<Explosive> ();
		if (explosive != null) {
			explosive.Explode();
		}

		gameController.AddScore (points);
		Destroy (gameObject);
	}

	public void OnReadyActivation() {
		state = State.Ready;
		nextTime = timeBetweenReadyAndActivate;
		Ready ();
	}
	
	protected virtual void  Ready (){}
	
	void OnActivate() {
		state = State.Activated;
		nextTime = timeBetweenActivateAndIdle;
		Activate ();
	}

	protected virtual void Activate (){
	}

	void OnActivateEnd() {
		state = State.Idle;
		SetNextActivateTime ();
		ActivateEnd ();
	}
	
	protected virtual void ActivateEnd (){}

	public void SetNextActivateTime (){
		nextTime = Random.Range (minTimeBeforeReady, maxTimeBeforeReady);
	}
}
