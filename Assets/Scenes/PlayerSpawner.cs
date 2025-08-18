using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Inspector에서 프리팹 등록
    public GameObject uiPrefab;     // Inspector에서 프리팹 등록
    private GameObject currentPlayer;
    private GameObject currentUI;

    void Awake()
    {
        // 씬 로딩 완료 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayerAndUI();
    }

    private void SpawnPlayerAndUI()
    {
        // Player 스폰 위치 찾기
        Transform spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;
        if (spawnPoint == null)
        {
            Debug.LogWarning("SpawnPoint가 씬에 없음! (Player는 Vector3.zero에 스폰됨)");
            spawnPoint = new GameObject("TempSpawn").transform;
            spawnPoint.position = Vector3.zero;
        }

        // 이전 플레이어 삭제
        if (currentPlayer != null) Destroy(currentPlayer);
        // 새 플레이어 생성
        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        // 이전 UI 삭제
        if (currentUI != null) Destroy(currentUI);
        // 새 UI 생성
        currentUI = Instantiate(uiPrefab);
    }
}