using System;
using System.Collections.Generic;
using Source.Modules.GameLogicModule.Scripts.Clusters.ClusterItem;
using UnityEngine;

namespace Source.Modules.GameLogicModule.Scripts.Clusters
{
    [Serializable]
    public class Cluster<T> : IEquatable<Cluster<T>>
    {
        public Cluster()
        {
            ClusterItems = new List<ClusterItem<T>>();
        }
        public Cluster(List<ClusterItem<T>> clusterItems)
        {
            ClusterItems = clusterItems;
        }

        [field: SerializeField]
        public List<ClusterItem<T>> ClusterItems { get; }

        public void AddItem(T value)
        {
            ClusterItems.Add(new ClusterItem<T>(value));
        }
        
        public bool Equals(Cluster<T> other)
        {
            if (other==null || other.ClusterItems.Count!=ClusterItems.Count)
            {
                return false;
            }
            
            for (var i = 0; i < ClusterItems.Count; i++)
            {
                if (ClusterItems[i].Value.Equals(other.ClusterItems[i].Value) == false)
                {
                    return false;
                }
            }

            return true;
        }
        
        public override int GetHashCode()
        {
            int hash = 0;

            foreach (var item in ClusterItems)
            {
                hash ^= (item.Value?.GetHashCode() ?? 0);
            }

            return hash;
        }
    }
}