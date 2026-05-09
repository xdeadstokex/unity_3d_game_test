using System;
using UnityEngine;

public class ContainerCounter : BaseCounter{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            //spawn new kitchen object and throw to player
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player); 
            //the kitchen object know where its belong
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
