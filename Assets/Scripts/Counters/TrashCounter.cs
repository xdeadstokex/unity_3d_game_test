using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        // Do nothing
        if(player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
    }
}
