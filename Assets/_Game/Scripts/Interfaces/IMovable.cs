using UnityEngine;

namespace Interfaces
{
    public interface IMovable
    {
        void OnMovementStart(Vector3 endPosition);

        void OnMovementEnd();
    }
}
