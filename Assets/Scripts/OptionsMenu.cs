using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class OptionsMenu : MonoBehaviour
    {
        public Slider PopulationSlider;
        public TextMeshProUGUI PopulationSizeText;

        public Slider LifeTimeSlider;
        public TextMeshProUGUI LifeTimeText;

        public Slider MaxForceSlider;
        public TextMeshProUGUI MaxForceText;

        public Slider MutationRateSlider;
        public TextMeshProUGUI MutationRateText;

        public Toggle ElitismToggle;

        public Toggle ObstacleToggle;

        public Slider NumberOfGenesPreservedSlider;
        public TextMeshProUGUI NumberOfGenesPreservedText;

        public MainMenu MainMenu;

        void Start()
        {
            PopulationSlider.value = PlayerPrefs.GetInt("PopulationSize", 25);

            LifeTimeSlider.value = PlayerPrefs.GetInt("Lifetime", 500);

            MaxForceSlider.value = PlayerPrefs.GetFloat("MaxForce", 0.1f);

            MutationRateSlider.value = PlayerPrefs.GetFloat("MutationRate", 0.03f);

            if (PlayerPrefs.HasKey("Elitism"))
            {
                ElitismToggle.isOn = PlayerPrefs.GetInt("Elitism") == 1;

                NumberOfGenesPreservedSlider.gameObject.SetActive(ElitismToggle.isOn);
            }
            else
            {
                ElitismToggle.isOn = true;
                NumberOfGenesPreservedSlider.gameObject.SetActive(true);
            }

            ObstacleToggle.isOn = PlayerPrefs.GetInt("Obstacle", 0) == 1;

            NumberOfGenesPreservedSlider.value = PlayerPrefs.GetInt("NumberOfGenesPreserved", 5);
        }

        public void SetPopulationSize(float size)
        {
            PopulationSizeText.text = $"{size}";
            PlayerPrefs.SetInt("PopulationSize", (int)size);
        }

        public void SetLifetime(float lifetime)
        {
            LifeTimeText.text = $"{lifetime}";
            PlayerPrefs.SetInt("Lifetime", (int)lifetime);

            NumberOfGenesPreservedSlider.maxValue = lifetime;
        }

        public void SetMaxForce(float force)
        {
            var newForce = (float) Math.Round((decimal) force, 3);

            MaxForceText.text = $"{newForce}";
            PlayerPrefs.SetFloat("MaxForce", newForce);
        }

        public void SetMutationRate(float mutationRate)
        {
            var newRate = (float)Math.Round((decimal)mutationRate, 3);

            MutationRateText.text = $"{newRate}";
            PlayerPrefs.SetFloat("MutationRate", newRate);
        }

        public void SetElitism(bool isElitism)
        {
            PlayerPrefs.SetInt("Elitism", isElitism? 1 : 0);

            if (isElitism)
            {
                NumberOfGenesPreservedSlider.gameObject.SetActive(true);
            }
            else
            {
                NumberOfGenesPreservedSlider.gameObject.SetActive(false);
            }
        }

        public void SetObstacle(bool isObstacle)
        {
            PlayerPrefs.SetInt("Obstacle", isObstacle? 1 : 0);
        }

        public void SetNumberOfGenesPreserved(float number)
        {
            NumberOfGenesPreservedText.text = $"{number}";
            PlayerPrefs.SetInt("NumberOfGenesPreserved", (int)number);
        }

        public void SetDefaultValues()
        {
            PopulationSlider.value = 25;

            LifeTimeSlider.value = 500;

            MaxForceSlider.value = 0.1f;

            MutationRateSlider.value = 0.03f;

            ElitismToggle.isOn = true;
            NumberOfGenesPreservedSlider.gameObject.SetActive(true);

            NumberOfGenesPreservedSlider.value = 5;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                MainMenu.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
