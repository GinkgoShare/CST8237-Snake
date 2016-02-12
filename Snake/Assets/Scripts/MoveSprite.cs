using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MoveSprite : MonoBehaviour {

	// Contants used as direction indicators
	private enum Direction { NORTH, EAST, SOUTH, WEST };

	int _score = 0;

	// has the snake eaten?
	bool _ate = false;

	int _highScore = 0;

	// direction of movement initialized to EAST, used to validate a conditional expression
	Direction _direction = Direction.EAST;

	// sections of the snakes body
	List<Transform> _sections = new List<Transform>();


	public Text scoreText;

	public Text highScoreText;

	public Transform borderWest;

	public Transform borderEast;

	public Transform borderSouth;

	public Transform borderNorth;

	public GameObject bodyPrefab;

	public GameObject fruitPrefab;

	public Vector2 movementDirection;

	void Start () {
		SpawnFruit (); // spawn the initial fruit to eat
		SetScoreText(); // set the users score text
		InvokeRepeating ("Move", 0.05f, 0.05f); // Move at a time dependent speed
		movementDirection = new Vector2(1.5f, 0.0f); // set initial direction of translations
	}

	void Update () {
		// snake can only move east and west from a north or south position and vice versa
		if (_direction == Direction.NORTH || _direction == Direction.SOUTH) {
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				_direction = Direction.EAST;
				movementDirection = new Vector2(1.5f, 0);
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				_direction = Direction.WEST;
				movementDirection = new Vector2(-1.5f, 0);
			}
		} else if (_direction == Direction.EAST || _direction == Direction.WEST) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				_direction = Direction.NORTH;
				movementDirection = new Vector2(0, 1.5f);
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				_direction = Direction.SOUTH;
				movementDirection = new Vector2(0, -1.5f);
			}
		}
	}

	void Move() {
		// get postion of head before its moved
		Vector2 position = transform.position;
		transform.Translate (movementDirection);

		if (_sections.Count < 3) SpawnBody (position);

		// if the snake has eaten then add a new body section to the _sections list using the head of the snakes position
		if (_ate) {
			_ate = false;
			SpawnBody (position);
		} else if (_sections.Count > 0) { // otherwise take the last section and move to the front to fill in the movement gap
			_sections.Last ().position = position;
			_sections.Insert (0, _sections.Last ());
			_sections.RemoveAt (_sections.Count - 1);
		}
	}

	void SpawnBody(Vector2 position) {
		GameObject gameObject = Instantiate (bodyPrefab, position, Quaternion.identity) as GameObject;
		_sections.Insert (0, gameObject.transform);
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.CompareTag ("Fruit")) {
			++_score;
			_ate = true; // set _ate var to be checked in Move()
			SetScoreText ();
			// respawn fruit
			int x = (int)Random.Range (borderWest.position.x, borderEast.position.x);
			int y = (int)Random.Range (borderSouth.position.y, borderNorth.position.y);
			collider.gameObject.transform.position = new Vector3 (x, y, 0.0f);
		} else { // means snake collided with boundary
			// remove body sections from snake
			foreach (Transform section in _sections.ToList()) Destroy (section.gameObject);
			_sections.Clear ();
			gameObject.transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
			if (_score > _highScore) _highScore = _score;
			_score = 0;
			SetScoreText ();
			SetHighScoreText ();
		}
	}

	void SetScoreText() {
		scoreText.text = "Score " + _score.ToString();
	}

	void SetHighScoreText() {
		highScoreText.text = "High Score " + _highScore.ToString();
	}

	void SpawnFruit() {
		// initialize 5 fruits
		for (int i = 0; i < 5; ++i) {
			int x = (int)Random.Range (borderWest.position.x, borderEast.position.x);
			int y = (int)Random.Range (borderSouth.position.y, borderNorth.position.y);
			Instantiate (fruitPrefab, new Vector2 (x, y), Quaternion.identity);
		}
	}
}
