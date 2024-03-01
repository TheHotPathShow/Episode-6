using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace THPS.CombatSystem
{
    public class GameOverUIController : MonoBehaviour
    {
        public static GameOverUIController Instance;
        
        [SerializeField] private GameObject _uiPanelObject;
        [SerializeField] private AudioSource _audioSource;
        
        private Image _panelImage;
        private TextMeshProUGUI _gameOverText;

        private Color _panelEndColor;
        private Color _textEndColor;
        private Color _panelStartColor;
        private Color _textStartColor;
        
        private float _endScale;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            _panelImage = _uiPanelObject.GetComponent<Image>();
            _gameOverText = _uiPanelObject.GetComponentInChildren<TextMeshProUGUI>();

            _panelStartColor = _panelImage.color;
            _panelEndColor = _panelStartColor;
            _panelStartColor.a = 0f;
            _panelImage.color = _panelStartColor;

            _textStartColor = _gameOverText.color;
            _textEndColor = _textStartColor;
            _textStartColor.a = 0f;
            _gameOverText.color = _textStartColor;

            _endScale = _uiPanelObject.transform.localScale.x;
            _uiPanelObject.SetActive(false);
        }

        public IEnumerator ShowGameOverUI()
        {
            _uiPanelObject.SetActive(true);
            _audioSource.Play();
            
            var timer = 0f;
            var maxTime = 5f;

            while (timer <= maxTime)
            {
                timer += Time.deltaTime;

                var pct = timer / maxTime;

                _panelImage.color = Color.Lerp(_panelStartColor, _panelEndColor, pct);
                _gameOverText.color = Color.Lerp(_textStartColor, _textEndColor, pct);
                _uiPanelObject.transform.localScale = Vector3.one * Mathf.Lerp(0.75f, _endScale, pct);
                yield return null;
            }
        }
    }
}