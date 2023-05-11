using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy {

  [RequireComponent(typeof(Waypoints))]
  public class TargetMovement : MonoBehaviour {

    [SerializeField][Range(0.0f, 10.0f)] private float _speed = 1.0f;
		[SerializeField] public bool _loop;
		[SerializeField] private int _startingIndex;
		[SerializeField] private float pauseTime;

		public bool ShouldLoop {
			get { return _loop; }
			set { _loop = value; }
		}

		private int _currentIndex;
		private int _previousIndex;
		private int _moveDirection;
		private bool _pause;
		private bool _moving;

		private Vector3 _localPosition;

		private Waypoints _waypoints;

		public void Awake() {
			_waypoints = GetComponent<Waypoints>();
      _speed = GameObject.FindGameObjectWithTag("Persistent").GetComponent<FloorManager>().GetRandEnemySpeed();
			_currentIndex = _startingIndex;
			_previousIndex = _startingIndex - 1;
			if (_previousIndex < 0) _previousIndex = _waypoints.Length;
			_moveDirection = 1;
			_moving = true;
		}

		public void Start() {
			transform.position = _waypoints[_currentIndex];

			StartCoroutine(MoveToNextTarget(GetNextIndex(_moveDirection)));
		}

		public void CollisionDetected(Collision collision) {
			if (collision.gameObject.CompareTag("Collidable")) {
				StopAllCoroutines();
				_moving = false;
				StartCoroutine(WaitForSecond(pauseTime, MoveToNextTarget(GetNextIndex(-_moveDirection))));
			}
		}

		private IEnumerator MoveToNextTarget(int currentIndex) {
			if (_pause) StartCoroutine(WaitForSecond(pauseTime));
			float localTime = 0.0f;

			while (Vector3.Distance(transform.position, _waypoints[currentIndex]) > 0.01f && _moving) {
				localTime += Time.deltaTime;

				// Required for collision detection
				Vector3 targetPosition;
				if (_previousIndex != -1) {
					targetPosition = Vector3.Lerp(_waypoints[_previousIndex], _waypoints[currentIndex], localTime * _speed / Vector3.Distance(_waypoints[_previousIndex], _waypoints[currentIndex]));
				} else {
					targetPosition = Vector3.Lerp(_localPosition, _waypoints[currentIndex], localTime * _speed / Vector3.Distance(_localPosition, _waypoints[currentIndex]));
				}
				transform.position = targetPosition;
				yield return null;

				if (localTime > 100.0f) break;
			}
			if (_moving) StartCoroutine(MoveToNextTarget(GetNextIndex(_moveDirection)));
		}

		private int GetNextIndex(int direction) {
			int targetIndex = _currentIndex + direction;


			if (_loop && _moving) {
				if (targetIndex < 0) {
					targetIndex = _waypoints.Length + (_currentIndex + direction);
					_pause = true;
				} else if (targetIndex > _waypoints.Length - 1) {
					targetIndex = 0 + (_currentIndex - (_waypoints.Length - 1));
					_pause = true;
				}
				_previousIndex = _currentIndex;
				_currentIndex = targetIndex;
			} else if (!_loop && _moving) {
				if (targetIndex < 0) {
					targetIndex = 1;
					_moveDirection *= -1;
					_pause = true;
				} else if (targetIndex > _waypoints.Length - 1) {
					targetIndex = _waypoints.Length - 2;
					_moveDirection *= -1;
					_pause = true;
				}
				_previousIndex = _currentIndex;
				_currentIndex = targetIndex;
			} else {
				if (targetIndex < 0) {
					targetIndex = _waypoints.Length + (_currentIndex + direction);
					_pause = true;
				} else if (targetIndex > _waypoints.Length - 1) {
					targetIndex = 0 + (_currentIndex - (_waypoints.Length - 1));
					_pause = true;
				}
				_currentIndex = targetIndex;
				_moveDirection *= -1;
				_previousIndex = -1;
				_localPosition = transform.position;
				_moving = true;
			}
			return targetIndex;
		}

		private IEnumerator WaitForSecond(float seconds) {
			_pause = false;
			yield return new WaitForSeconds(seconds);
		}

		private IEnumerator WaitForSecond(float seconds, IEnumerator nextCommand) {
			_pause = false;
			yield return new WaitForSeconds(seconds);
			StartCoroutine(nextCommand);
		} 
	}
}