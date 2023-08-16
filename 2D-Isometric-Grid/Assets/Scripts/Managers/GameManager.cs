using core.architecture;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace core.managers.game
{
    public class GameManager : Singleton<GameManager>
    {
        public bool inGame;
        [Header("Game Logs")]
        public GameObject InGameDebugConsole;
        private GameObject rootCanvas;
        public bool EnableLogs;


        private void Start()
        {
            Debug.unityLogger.logEnabled = EnableLogs;
            var _rootCanvas = Resources.Load<GameObject>(GameConstants.RootCanvasPath);
            rootCanvas = Instantiate(_rootCanvas);
            InGameDebugConsole.SetActive(EnableLogs);
        }
        private void OnEnable()
        {
            
        }


        private void OnDisable()
        {
            base.DeInitialize();
        }
        private void OnDestroy()
        {
            base.DeInitialize();
        }
    }
}


