using System;
using UnityEngine;
using UnityEngine.UI;

public class StoveCounterVisual : MonoBehaviour {
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnVisual;
    [SerializeField] private GameObject stove_particles;
    [SerializeField] private Image barImage;
	[SerializeField] private GameObject process_bar_background;

	private void Start() {
		stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
		stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
		barImage.fillAmount = 0f;
		process_bar_background.SetActive(false);
		barImage.gameObject.SetActive(false); // hide bar only
		stoveOnVisual.SetActive(false);       // init off
		stove_particles.SetActive(false);       // init off
	}

	private void StoveCounter_OnStateChanged(object sender, EventArgs e) {
		bool isActive = stoveCounter.IsActive();
		stoveOnVisual.SetActive(isActive);
		process_bar_background.SetActive(isActive);
		barImage.gameObject.SetActive(isActive); // bar follows state
		stove_particles.SetActive(isActive);
	}

	private void StoveCounter_OnProgressChanged(object sender, EventArgs e) {
		StoveCounter.OnProgressChangedEventArgs castedE = (StoveCounter.OnProgressChangedEventArgs)e;
		barImage.fillAmount = castedE.progressNormalized;
	}

    private void Show() { gameObject.SetActive(true); }
    private void Hide() { gameObject.SetActive(false); }
}