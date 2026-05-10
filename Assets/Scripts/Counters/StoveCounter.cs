using System;
using UnityEngine;

public class StoveCounter : BaseCounter
{   
    private enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    
    public event EventHandler OnStateChanged;
    public event EventHandler OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs {
        public float progressNormalized;
    }
    
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start() { state = State.Idle; }

	private void Update() {
		switch (state) {
			case State.Idle:
				break;
			case State.Frying:
				fryingTimer += Time.deltaTime;
				if (fryingRecipeSO != null) {
					OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
						progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
					});
					if (fryingTimer >= fryingRecipeSO.fryingTimerMax) {
						fryingTimer = 0f;
						GetKitchenObject().DestroySelf();
						KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
						state = State.Fried;
						OnStateChanged?.Invoke(this, EventArgs.Empty);
						burningTimer = 0f;
						burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
					}
				}
				break;
			case State.Fried:
				burningTimer += Time.deltaTime;
				if (burningRecipeSO != null) {
					OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
						progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
					});
					if (burningTimer >= burningRecipeSO.burningTimerMax) {
						GetKitchenObject().DestroySelf();
						KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
						state = State.Burned;
						OnStateChanged?.Invoke(this, EventArgs.Empty);
					}
				}
				break;
			case State.Burned:
				break;
		}
	}

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                state = State.Frying;
                fryingTimer = 0f;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
        } 
        else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        return GetFryingRecipeSOWithInput(inputKitchenObjectSO) != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
    
    public bool IsActive() {
        return state == State.Frying || state == State.Fried;
    }
}