using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace THPS.CombatSystem
{
    public class GameOverUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _uiPanelObject;
        [SerializeField] private AudioSource _audioSource;

        private Image _panelImage;
        private TextMeshProUGUI _gameOverText;

        private Color _panelEndColor;
        private Color _textEndColor;
        private Color _panelStartColor;
        private Color _textStartColor;

        private void Awake()
        {
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
        }

        private IEnumerator Start()
        {
            // _uiPanelObject.SetActive(false);

            yield return new WaitForSeconds(5f);
            
            // _uiPanelObject.SetActive(true);

            _audioSource.Play();
            
            var timer = 0f;
            var maxTime = 5f;

            while (timer <= maxTime)
            {
                timer += Time.deltaTime;

                var pct = timer / maxTime;

                _panelImage.color = Color.Lerp(_panelStartColor, _panelEndColor, pct);
                _gameOverText.color = Color.Lerp(_textStartColor, _textEndColor, pct);

                yield return null;
            }
        }
    }
}