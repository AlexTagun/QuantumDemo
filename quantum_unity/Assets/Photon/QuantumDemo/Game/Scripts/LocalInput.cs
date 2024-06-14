using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class LocalInput : MonoBehaviour
{
    private void OnEnable()
    {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback)
    {
        var i = new Quantum.Input
        {
            DirectionX = (short)(UnityEngine.Input.GetAxis("Horizontal") * 10),
            DirectionY = (short)(UnityEngine.Input.GetAxis("Vertical") * 10),
            Jump = UnityEngine.Input.GetButton("Jump"),
        };

        callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }
}