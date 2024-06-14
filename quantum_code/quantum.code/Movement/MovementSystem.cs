using Photon.Deterministic;

namespace Quantum;

public unsafe class MovementSystem : SystemMainThreadFilter<MovementSystem.Filter>, ISignalOnPlayerDataSet
{
    public struct Filter
    {
        public EntityRef Entity;
        public CharacterController3D* CharacterController;
        public Transform3D* Transform;
        public PlayerLink* Link;
    }

    public override void Update(Frame f, ref Filter filter)
    {
        var input = f.GetPlayerInput(filter.Link->Player);
        var inputVector = new FPVector2((FP)input->DirectionX / 10, (FP)input->DirectionY / 10);
        
        //Anti cheat
        if (inputVector.SqrMagnitude > 1)
        {
            inputVector = inputVector.Normalized;
        }
        
        //Move
        filter.CharacterController->Move(f, filter.Entity, inputVector.XOY);
        
        //Jump
        if (input->Jump.WasPressed)
            filter.CharacterController->Jump(f);
    }

    public void OnPlayerDataSet(Frame f, PlayerRef player)
    {
        var data = f.GetPlayerData(player);

        var entityPrototype = f.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);
        var createdEntity = f.Create(entityPrototype);

        if (f.Unsafe.TryGetPointer<PlayerLink>(createdEntity, out var playerLink))
        {
            playerLink->Player = player;
        }
        
        if (f.Unsafe.TryGetPointer<Transform3D>(createdEntity, out var transform))
        {
            transform->Position = GetSpawnPosition(player);
        }
    }

    private FPVector3 GetSpawnPosition(int playerNumber)
    {
        return new FPVector3(-4 + playerNumber * 2, 0, 0);
    }
}