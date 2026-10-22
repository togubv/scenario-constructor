using UnityEngine;

namespace SC
{
    public enum ExecutionProcedureType
    {
        [InspectorName("Свободный")]
        Free = 0,

        [InspectorName("По очереди")]
        InRotation = 1,
    }
}
