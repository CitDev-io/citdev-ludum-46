using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    [SerializeField] public GameObject moneyPrefab; //2 hours to end of game jam gimme a break
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject moneySpot;
    TextMeshProUGUI textElement;
    public Vector2 offset;

    void Start()
    {
        textElement = GetComponent<TextMeshProUGUI>();
        EventManager.Instance.OnPlayerScoreChanged += HandleScoreChange;
        offset = new Vector2(-0.5f, -1f);
    }

    void HandleScoreChange(long score) {
        StartCoroutine(PayTheManWhatYouOweHim(score));
    }

    IEnumerator PayTheManWhatYouOweHim(long score) {
        GameObject go = Instantiate(moneyPrefab, player.transform.position, Quaternion.identity, Camera.main.transform);
        EventManager.Instance.ReportPayAnimation();
        go.GetComponent<ZoomToMoneyBar>().SetTarget(moneySpot.transform);
        yield return new WaitForSeconds(0.55f);
        textElement.text = score.ToString("#,0");
        Destroy(go);
    }
}


