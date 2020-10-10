using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class Dna
    {
        public Vector2[] Genes { get; set; }
        private float _maxforce;
        private int _lifetime;

        //constructor to use for the initial population
        public Dna(int lifetime, float maxforce)
        {
            _lifetime = lifetime;
            _maxforce = maxforce;

            GenerateInitialGenes();
        }

        //constructor to use when creating bebe
        public Dna(int lifetime, Vector2[] newGenes)
        {
            _lifetime = lifetime;
            Genes = newGenes;
        }

        private void GenerateInitialGenes()
        {
            Genes = new Vector2[_lifetime];

            for (int i = 0; i < Genes.Length; i++)
            {
                var angle = Random.Range(0.0f, (2 * Mathf.PI));
                var randomForce = Random.Range(0.0f, _maxforce);

                Genes[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Genes[i] *= randomForce;
            }
        }

        public Dna Crossover(Dna otherParent)
        {
            var childGenes = new Vector2[Genes.Length];

            //for (int i = 0; i < Genes.Length; i++)
            //{
            //    childGenes[i] = Random.Range(0.0f, 1.0f) < 0.5f ? Genes[i] : otherParent.Genes[i];
            //}

            //return new Dna(_lifetime, childGenes);

            int midPoint = Random.Range(0, Genes.Length);

            for (int i = 0; i < Genes.Length; i++)
            {
                if (i < midPoint)
                {
                    childGenes[i] = Genes[i];
                }
                else
                {
                    childGenes[i] = otherParent.Genes[i];
                }
            }

            return new Dna(_lifetime, childGenes);
        }

        public void Mutation(float mutationRate, float maxforce)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                if (Random.Range(0.0f, 1.0f) < mutationRate)
                {
                    var angle = Random.Range(0.0f, (2 * Mathf.PI));
                    var randomForce = Random.Range(0.0f, _maxforce);

                    Genes[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                    Genes[i] *= randomForce;
                }
            }
        }
    }
}