using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TelemetryManager : MonoBehaviour
{
    private const string Url = "https://script.google.com/macros/s/AKfycbw5lRTOC4Gja-RjCr2aPz_rJeR88sFISXuVik9aqTr91WLV6Nt1IPge9lav12slFW4u/exec";
    private const string Token = "j8u54r9mqconwfdg3vth";

    private string sessionId;
    private Dictionary<string, int> attemptsPerLevel = new Dictionary<string, int>();
    private int energyMax;
    private float planningStart;
    private int planningSeconds;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        GameObject go = new GameObject(nameof(TelemetryManager));
        DontDestroyOnLoad(go);
        go.AddComponent<TelemetryManager>();
    }
    private void Awake()
    {
        sessionId = "S-" + System.DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                  + "-" + Random.Range(0, 100000);
    }
    private void OnEnable()
    {
        RoundManager.OnQuotasSet += HandleLevelStarted;
        CardPlacementManager.OnEnergyChange += HandleEnergy;
        RoundManager.OnRoundStarted += HandlePlayPressed;
        RoundManager.OnRoundFinished += HandleRoundFinished;
    }
    private void OnDisable()
    {
        RoundManager.OnQuotasSet -= HandleLevelStarted;
        CardPlacementManager.OnEnergyChange -= HandleEnergy;
        RoundManager.OnRoundStarted -= HandlePlayPressed;
        RoundManager.OnRoundFinished -= HandleRoundFinished;
    }
    private void HandleLevelStarted(IReadOnlyList<ItemQuota> quotas)
    {
        planningStart = Time.realtimeSinceStartup;
    }
    private void HandleEnergy(int used, int max)
    {
        energyMax = max;
    }
    private void HandlePlayPressed()
    {
        planningSeconds = Mathf.RoundToInt(Time.realtimeSinceStartup - planningStart);
    }
    private void HandleRoundFinished(RoundResult result)
    {
        attemptsPerLevel.TryGetValue(result.LevelName, out int previous);
        int attempt = previous + 1;
        attemptsPerLevel[result.LevelName] = attempt;
        RoundTelemetry data = new RoundTelemetry
        {
            token = Token,
            session_id = sessionId,
            version = Application.version,
            level_name = result.LevelName,
            attempt = attempt,
            won = result.Won,
            stars = result.Stars,
            delivered = result.Delivered,
            quota = result.Quota,
            energy_used = result.EnergyUsed,
            energy_max = energyMax,
            belts_used = result.BeltsUsed,
            planning_sec = planningSeconds,
            layout = result.Layout,
            board = result.Board
        };
        StartCoroutine(Send(data));
    }
    private IEnumerator Send(RoundTelemetry data)
    {
        string json = JsonUtility.ToJson(data);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        using (UnityWebRequest request = new UnityWebRequest(Url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 5;
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Telemetry failed: " + request.error);
            }
            else if (request.downloadHandler.text.Contains("\"ok\":false"))
            {
                Debug.LogWarning("Telemetry rejected: " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Telemetry sent: " + request.downloadHandler.text);
            }
        }
    }
}
