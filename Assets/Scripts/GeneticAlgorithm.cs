using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
    public class GeneticAlgorithm : MonoBehaviour
    {
        public GameObject Spawnee;
        public Text LifeTimeText;
        public Text BestFitnessText;
        public Text GenerationText;
    
        public List<Rocket> Population { get; set; }

        public bool UseMemoryValues = true;
        public int PopulationSize;
        public int LifeTime;
        public float MaxForce;
        public float MutationRate;
        public bool Elitism;
        public bool Obstacle;
        public int NumberOfGenesPreserved;

        private int _lifeCounter;
        private int _generation;
        private System.Random _random;
        private float _bestFitness;
        private float _fitnessSum;

        // Start is called before the first frame update
        void Start()
        {
            if (UseMemoryValues)
            {
                GetMemoryValues();
            }

            _generation = 1;
            SetGenerationText();
            _random = new System.Random();
            Population = new List<Rocket>(PopulationSize);
            _lifeCounter = LifeTime;

            CreateInitialPopulation();
        }

        // Calling on every frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }

        void FixedUpdate()
        {
            DecreaseLifeTimeText();
            if (_lifeCounter <= 0)
            {
                CalculateFitness();
                NaturalSelection();
                _lifeCounter = LifeTime;

                SetBestFitnessText();
                _generation++;
                SetGenerationText();
            }
            _lifeCounter--;
        }

        private void CalculateFitness()
        {
            _fitnessSum = 0;
            _bestFitness = 0;

            for (int i = 0; i < PopulationSize; i++)
            {
                _fitnessSum += Population[i].CalculateFitness();
            }

            for (int i = 0; i < PopulationSize; i++)
            {
                Population[i].Fitness /= _fitnessSum;

                if (Population[i].Fitness > _bestFitness)
                {
                    _bestFitness = Population[i].Fitness;
                }
            }
        }

        private void NaturalSelection()
        {
            Population.Sort(Comparer);

            var newRocketDna = new List<Dna>();

            for (int i = 0; i < PopulationSize; i++)
            {
                if (Elitism && i < NumberOfGenesPreserved)
                {
                    newRocketDna.Add(Population[i].Dna);
                    continue;
                }

                var parent1 = ChooseParent().Dna;

                Dna parent2;
                do
                {
                    parent2 = ChooseParent().Dna;

                } while (parent2 == parent1);

                var childDna = parent1.Crossover(parent2);
                childDna.Mutation(MutationRate, MaxForce);

                newRocketDna.Add(childDna);
            }

            GenerateNewPopulation(newRocketDna);
        }

        public int Comparer(Rocket a, Rocket b)
        {
            if (a.Fitness > b.Fitness)
            {
                return -1;
            }
            else if (a.Fitness < b.Fitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private Rocket ChooseParent()
        {
            var index = -1;
            var randomNumber = _random.NextDouble();

            while (randomNumber > 0)
            {
                randomNumber -= Population[++index].Fitness;
            }

            return Population[index];
        }

        private void DecreaseLifeTimeText()
        {
            LifeTimeText.text = $"Life time: {_lifeCounter}";
        }

        private void SetBestFitnessText()
        {
            BestFitnessText.text = $"Best fitness: {_bestFitness}";
        }

        private void SetGenerationText()
        {
            GenerationText.text = $"Generation: {_generation}";
        }

        private void CreateInitialPopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Instantiate(Spawnee, new Vector3(0, -4), Quaternion.identity);
            }

            Population = FindObjectsOfType<Rocket>().ToList();
            Population.ForEach(r => r.ConstructorWithoutDna(LifeTime, MaxForce));
        }

        private void GenerateNewPopulation(List<Dna> newDna)
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Population[i].transform.position = new Vector3(0, -4);
                Population[i].ConstructorWithDna(newDna[i]);
            }
        }

        private void GetMemoryValues()
        {
            if (PlayerPrefs.HasKey("PopulationSize"))
            {
                PopulationSize = PlayerPrefs.GetInt("PopulationSize");
            }
            
            if (PlayerPrefs.HasKey("Lifetime"))
            {
                LifeTime = PlayerPrefs.GetInt("Lifetime");
            }

            if (PlayerPrefs.HasKey("MaxForce"))
            {
                MaxForce = PlayerPrefs.GetFloat("MaxForce");
            }

            if (PlayerPrefs.HasKey("MutationRate"))
            {
                MutationRate = PlayerPrefs.GetFloat("MutationRate");
            }

            if (PlayerPrefs.HasKey("Elitism"))
            {
                Elitism = PlayerPrefs.GetInt("Elitism") == 1;
            }

            if (PlayerPrefs.HasKey("Obstacle"))
            {
                Obstacle = PlayerPrefs.GetInt("Obstacle") == 1;

                GameObject.Find("Obstacle").SetActive(Obstacle);
            }

            if (PlayerPrefs.HasKey("NumberOfGenesPreserved"))
            {
                NumberOfGenesPreserved = PlayerPrefs.GetInt("NumberOfGenesPreserved");
            }
        }
    }
}
