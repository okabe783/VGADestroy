using UnityEngine;

namespace Talk
{
    public interface ITalkable
    {
        Transform Transform { get; }
        void Talk();
    }
}