using UnityEngine;

namespace Assets
{
    public class Rocket : MonoBehaviour
    {
        private GameObject _target;
        private GameObject _obstacle;
        private GameObject _edges;
        private Rigidbody2D _rb;
        private float _maxForce;
        private int _lifeTime;

        //Internal use
        private Vector3 _lookDirection;
        private bool _hitTarget;
        private bool _hitObstacle;
        private int _completionTime = 0;

        //Fitness and Dna
        public float Fitness;
        public Dna Dna { get; set; }
        private int _geneCounter = 0;

        public void ConstructorWithoutDna(int lifetime, float maxForce)
        {
            _lifeTime = lifetime;
            _maxForce = maxForce;
            Dna = new Dna(_lifeTime, _maxForce);
        }

        public void ConstructorWithDna(Dna dna)
        {
            _geneCounter = 0;
            _completionTime = 0;
            _hitTarget = false;
            _hitObstacle = false;
            Dna = dna;
        }

        // Start is called before the first frame update
        void Start()
        {
            _target = GameObject.Find("Target");
            _obstacle = GameObject.Find("Obstacle");
            _edges = GameObject.Find("Edges");
            _rb = GetComponent<Rigidbody2D>();
            

            _lookDirection = _rb.velocity.normalized;
            transform.right = _lookDirection;
        }

        void FixedUpdate()
        {
            if(!_hitTarget && !_hitObstacle)
            {
                ApplyForce(Dna.Genes[_geneCounter]);
                _geneCounter = (_geneCounter + 1) % Dna.Genes.Length;
            }

            if (!_hitTarget) _completionTime++;

        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name == "Target")
            {
                _rb.velocity = Vector2.zero;
                _hitTarget = true;
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _hitObstacle = true;
            }
            
        }

        public void ApplyForce(Vector2 force)
        {
            _rb.velocity += force;
            _lookDirection = _rb.velocity.normalized;
            transform.right = _lookDirection;
        }

        public float CalculateFitness()
        {
            var distance = Vector2.Distance(transform.position, _target.transform.position);

            if (distance <= 0)
            {
                distance = 0.01f;
            }
            
            Fitness = Mathf.Pow(1 / (_completionTime * distance), 2);

            if (_hitTarget)
            {
                Fitness *= 10;
            }

            if (_hitObstacle)
            {
                Fitness /= 10;
            }

            if (_obstacle != null && transform.position.y > _obstacle.transform.position.y)
            {
                Fitness *= 1.5f;
            }

            return Fitness;
        }
    }
}
