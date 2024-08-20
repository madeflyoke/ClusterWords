using System.Collections.Generic;
using Source.Modules.GameLogicModule.Scripts.Clusters;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Utils
{
    public static class WordUtils
    {
        public static List<Cluster<char>> SeparateWordByClusters(string word)
        {
            #region ClustersAlgorithm

            List<string> clusters = new List<string>();
            
            int index = 0;
            int wordLength = word.Length;

            while (index < wordLength)
            {
                int remainingLength = wordLength - index;
               
                if (remainingLength == 1)
                {
                    break;
                }
       
                List<int> validClusterSizes = new List<int>();
                
                for (int i = GameConstants.MIN_CLUSTER_SIZE; i <= GameConstants.MAX_CLUSTER_SIZE; i++)
                {
                    if (remainingLength >=i && wordLength!=i)
                    {
                        validClusterSizes.Add(i);
                    }
                }
            
                if (validClusterSizes.Count > 0)
                {
                    int clusterSize = validClusterSizes[Random.Range(0,validClusterSizes.Count)];

                    if (remainingLength - clusterSize == 1 && validClusterSizes.Count > 1)
                    {
                        validClusterSizes.Remove(clusterSize);
                        clusterSize = validClusterSizes[Random.Range(0,validClusterSizes.Count)];
                    }

                    clusters.Add(word.Substring(index, clusterSize));
                    index += clusterSize;
                }
            }

            if (clusters.Count > 0 && index < wordLength)
            {
                clusters[^1] += word.Substring(index);
            }


            #endregion
            
            List<Cluster<char>> tempWordClusters = new List<Cluster<char>>();
            foreach (var clusterData in clusters)
            {
                Cluster<char> cluster = new Cluster<char>();
                foreach (var value in clusterData)
                {
                    cluster.AddItem(value);
                }
                tempWordClusters.Add(cluster);
            }
            
            return tempWordClusters;
        }
    }
}
