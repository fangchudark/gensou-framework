using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GensouLib.Unity.Core;

public class SaveLoad : MonoBehaviour
{
    public int MaxSlots = 20;
    public GameObject SaveSlotPrefab;
    public Button CloseButton;
    public TextMeshProUGUI PanelTitle;
    public RectTransform SaveSlotContainer;
    public string TimestampGameObjectName = "Timestamp";
    public string DialogueGameObjectName = "Dialogue";
    public string ScreenshotGameObjectName = "Screenshot";
    private void Awake() {
        SaveLoadGame.Init(
            SaveSlotPrefab,
            CloseButton,
            PanelTitle,
            SaveSlotContainer,
            MaxSlots,
            TimestampGameObjectName,
            DialogueGameObjectName,
            ScreenshotGameObjectName
        );
    }

    // Start is called before the first frame update
    async void Start()
    {
       await SaveLoadGame.CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
